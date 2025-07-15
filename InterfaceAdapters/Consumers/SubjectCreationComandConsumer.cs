using Application.DTO;
using Application.IService;
using Domain.Messages;
using MassTransit;

namespace InterfaceAdapters.Consumers;

public class SubjectCreationComandConsumer : IConsumer<SubjectCreationCommand>
{
    private readonly ISubjectService _subjectService;

    public SubjectCreationComandConsumer(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    public async Task Consume(ConsumeContext<SubjectCreationCommand> context)
    {
        var msg = context.Message;
        CreateSubjectDTO createDTO = new CreateSubjectDTO { Description = msg.Description, Details = msg.Details };
        await _subjectService.Create(createDTO);
    }
}
