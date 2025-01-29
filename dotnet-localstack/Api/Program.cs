using Amazon.SQS;
using Api.Configuration;
using Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure AWS services
builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AWS"));
builder.Services.AddSingleton<IAmazonSQS>(sp =>
{
	var clientConfig = new AmazonSQSConfig
	{
		ServiceURL = "http://localhost:4566",
		AuthenticationRegion = "us-west-2",
	};
	return new AmazonSQSClient("test", "test", clientConfig);
});

// Register application services
builder.Services.AddScoped<ISqsService, SqsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var summaries = new[]
{
	"Freezing",
	"Bracing",
	"Chilly",
	"Cool",
	"Mild",
	"Warm",
	"Balmy",
	"Hot",
	"Sweltering",
	"Scorching",
};

app.MapGet(
		"/weatherforecast",
		() =>
		{
			var forecast = Enumerable
				.Range(1, 5)
				.Select(index => new WeatherForecast(
					DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
					Random.Shared.Next(-20, 55),
					summaries[Random.Shared.Next(summaries.Length)]
				))
				.ToArray();
			return forecast;
		}
	)
	.WithName("GetWeatherForecast")
	.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
