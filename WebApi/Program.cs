using Agents;
using Agents.CampaignAgent;
using Agents.UserAgent;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with in-memory provider
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("DemoDb"));

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<ISimulationService, SimulationService>();

// Register Semantic Kernel with OpenAI
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var apiKey = config["OpenAI:ApiKey"] ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY");
    var kernelBuilder = Kernel.CreateBuilder();

    kernelBuilder.AddOpenAIChatCompletion("gpt-4.1-nano", apiKey!);

    return kernelBuilder.Build();
});

// Register agents
builder.Services.AddScoped<UserAgent>();
builder.Services.AddScoped<CampaignAgent>();

// Add controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO: Configure LangChain and OpenAI API key from environment
// builder.Services.Configure<LangChainOptions>(options =>
// {
//     options.OpenAIApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
// });

var app = builder.Build();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
