using Application.IPublisher;
using Application.IService;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Resolvers;
using InterfaceAdapters.Consumers;
using InterfaceAdapters.Publisher;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<SubjectCMDContext>(opt =>
    opt.UseNpgsql(connectionString));

//Services
builder.Services.AddTransient<ISubjectService, SubjectService>();

//Repositories
builder.Services.AddTransient<ISubjectRepository, SubjectRepository>();

//Factories
builder.Services.AddTransient<ISubjectFactory, SubjectFactory>();

//Mappers
builder.Services.AddTransient<SubjectDataModelConverter>();
builder.Services.AddAutoMapper(cfg =>
{
    //DataModels
    cfg.AddProfile<DataModelMappingProfile>();
});

builder.Services.AddScoped<IMassTransitPublisher, SubjectCreatedPublisher>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SubjectCreatedConsumer>();
    x.AddConsumer<SubjectCreationComandConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        var random = Guid.NewGuid();

        cfg.ReceiveEndpoint($"subjectCMD-{random}", e =>
        {
            e.ConfigureConsumer<SubjectCreatedConsumer>(context);
            e.ConfigureConsumer<SubjectCreationComandConsumer>(context);
        });
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

// read env variables for connection string
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

    if (!env.IsEnvironment("Testing"))
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<SubjectCMDContext>();
        dbContext.Database.Migrate();
    }
}

app.Run();

public partial class Program { }