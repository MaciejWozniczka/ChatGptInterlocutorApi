namespace ChatGptInterlocutor.Services;

public interface IOpenAiService
{
    Task<string?> SendChatQuery(string prompt);
}
public class OpenAiService : IOpenAiService
{
    private readonly string _url;
    private readonly string _key;
    public OpenAiService(IConfiguration configuration)
    {
        _url = configuration.GetSection("ChatGptAuth:Endpoint").Value;
        _key = configuration.GetSection("ChatGptAuth:Key").Value;
    }

    async Task<string?> IOpenAiService.SendChatQuery(string prompt)
    {
        var requestData = new OpenAiRequest
        {
            Model = "gpt-4o",
            Input = prompt
        };

        var response = await "https://api.openai.com/v1/chat/completions"
            .WithOAuthBearerToken(_key)
            .PostJsonAsync(new
            {
                model = "gpt-4o",
                messages = new[] {
                    new { role = "user", content = prompt }
                }
            })
            .ReceiveJson<OpenAiResponse>();
            
        return response.Choices.FirstOrDefault()?.Message.Content;
    }
}