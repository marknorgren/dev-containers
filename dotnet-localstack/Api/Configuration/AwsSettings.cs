namespace Api.Configuration;

public class AwsSettings
{
	public const string ConfigSection = "AWS";
	public string? ServiceUrl { get; set; }
	public string? Region { get; set; }
	public string? QueueName { get; set; }
	public string? AccessKey { get; set; }
	public string? SecretKey { get; set; }
}
