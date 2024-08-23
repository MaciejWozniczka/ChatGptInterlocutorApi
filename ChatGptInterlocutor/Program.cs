var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var services = builder.Services;

services.AddCors(o => o.AddPolicy("default", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

services.AddControllers();

services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions
        .ReferenceHandler = ReferenceHandler.IgnoreCycles);

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatGptInterlocutor", Version = "v1" });
});

services.AddMediatR(typeof(Program));

services.AddScoped<IOpenAiService, OpenAiService>();

services.Configure<ChatGptOptions>(configuration.GetSection("ChatGptOptions"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("default");
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CaseWareStatus v1"));

app.Run();