using Amazon.SQS;
using Amazon.SQS.Model;
using Api.Configuration;
using Microsoft.Extensions.Options;

namespace Api.Services;

public interface ISqsService
{
    Task<string> SendMessageAsync(string messageBody);
    Task<List<Message>> ReceiveMessagesAsync(int maxMessages = 10);
    Task DeleteMessageAsync(string receiptHandle);
}

public class SqsService : ISqsService
{
    private readonly IAmazonSQS _sqsClient;
    private readonly string _queueUrl;

    public SqsService(IAmazonSQS sqsClient, IOptions<AwsSettings> settings)
    {
        _sqsClient = sqsClient;
        // For LocalStack, we need to use the correct queue URL format
        _queueUrl = $"http://sqs.us-west-2.localhost:4566/000000000000/{settings.Value.QueueName}";
    }

    public async Task<string> SendMessageAsync(string messageBody)
    {
        var request = new SendMessageRequest
        {
            QueueUrl = _queueUrl,
            MessageBody = messageBody
        };

        var response = await _sqsClient.SendMessageAsync(request);
        return response.MessageId;
    }

    public async Task<List<Message>> ReceiveMessagesAsync(int maxMessages = 10)
    {
        var request = new ReceiveMessageRequest
        {
            QueueUrl = _queueUrl,
            MaxNumberOfMessages = maxMessages,
            WaitTimeSeconds = 5 // Enable long polling
        };

        var response = await _sqsClient.ReceiveMessageAsync(request);
        return response.Messages;
    }

    public async Task DeleteMessageAsync(string receiptHandle)
    {
        var request = new DeleteMessageRequest
        {
            QueueUrl = _queueUrl,
            ReceiptHandle = receiptHandle
        };

        await _sqsClient.DeleteMessageAsync(request);
    }
}
