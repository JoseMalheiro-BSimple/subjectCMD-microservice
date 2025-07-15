using Application.IService;
using Domain.Messages;
using MassTransit;

namespace InterfaceAdapters.Consumers;

public class SubjectCreatedConsumer : IConsumer<SubjectCreatedMessage>
{
    private readonly ISubjectService _subjectService;

    public SubjectCreatedConsumer(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    public async Task Consume(ConsumeContext<SubjectCreatedMessage> context)
    {
        var msg = context.Message;
        await _subjectService.CreateWithNoValidation(msg.Id, msg.Description, msg.Details);
    }
}
