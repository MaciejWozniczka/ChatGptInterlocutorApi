using OpenAI.Chat;

namespace ChatGptInterlocutor.Services;

public interface IOpenAiService
{
    Task<string> SendChatQuery(string prompt);
}
public class OpenAiService : IOpenAiService
{
    private readonly ChatGptOptions _configuration;
    private AzureOpenAIClient? _client;
    public OpenAiService(IOptions<ChatGptOptions> configuration) : base()
    {
        _configuration = configuration.Value;
    }

    private AzureOpenAIClient GetClient()
    {
        if (_client == null)
        {
            _client ??= new AzureOpenAIClient(new Uri(_configuration.Endpoint),
                new AzureKeyCredential(_configuration.Key));
        }
        return _client;
    }

    async Task<string> IOpenAiService.SendChatQuery(string prompt)
    {
        var client = GetClient();
        var chatGpt = client.GetChatClient("FS4o");

        var chatResponse =
            await chatGpt.CompleteChatAsync(
                new UserChatMessage(prompt));

        return chatResponse.Value.Content.FirstOrDefault()?.Text!;
    }
}