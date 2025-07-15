using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Application.IPublisher;
using InterfaceAdapters.Controllers;

namespace InterfaceAdapters.Tests;

public class IntegrationTestsWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
       .WithDatabase("testdb")
       .WithUsername("testuser")
       .WithPassword("testpass")
       .WithImage("postgres:15-alpine")
       .WithCleanUp(true)
       .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<SubjectCMDContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            // Register AbsanteeContext with container's connection string
            services.AddDbContext<SubjectCMDContext>(options =>
                options.UseNpgsql(_postgres.GetConnectionString()));

            // Ensure database is created
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SubjectCMDContext>();
            db.Database.EnsureCreated();

            // Replace the real publisher with a fake on for testing
            services.RemoveAll<IMassTransitPublisher>();
            services.AddSingleton<IMassTransitPublisher, FakeSubjectCreatedPublisher>();
        });
    }

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _postgres.StopAsync();
    }
}
