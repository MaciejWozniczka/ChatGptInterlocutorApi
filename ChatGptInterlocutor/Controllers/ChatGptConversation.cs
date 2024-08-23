namespace ChatGptInterlocutor.Controllers;

[ApiController]
public class GetAnswer : ControllerBase
{
    private readonly IMediator _mediator;
    public GetAnswer(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/api/chat/{text}")]
    public async Task<GetAnswerDto> GetAnswerAsync(string text)
    {
        return await _mediator.Send(new GetAnswerQuery(text));
    }

    public class GetAnswerQuery : IRequest<GetAnswerDto>
    {
        public string Text { get; set; }
        public GetAnswerQuery(string text)
        {
            Text = text;
        }
    }

    public class GetAnswerDto
    {
        public string Answer { get; set; }
    }

    public class GetLegalFormDtoQueryHandler : IRequestHandler<GetAnswerQuery, GetAnswerDto>
    {
        private readonly IOpenAiService _openAiService;
        public GetLegalFormDtoQueryHandler(IOpenAiService openAiService)
        {
            _openAiService = openAiService;
        }

        public async Task<GetAnswerDto> Handle(GetAnswerQuery request, CancellationToken cancellationToken)
        {
            var result = await _openAiService.SendChatQuery(request.Text);
            return new GetAnswerDto() { Answer = result };
        }
    }
}