using System.Text;
using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using CreditAppSharedV1.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CreditAppSharedV1.Functions.Functions;

public class GeneratePdfFunction(ILogger<GeneratePdfFunction> logger)
{
    [Function("Fn_GeneratePdf")]
    public async Task Run(
        [QueueTrigger("q-application-submitted", Connection = "AzureWebJobsStorage")]
        string message)
    {
        try
        {
            var submission = JsonSerializer.Deserialize<ApplicationSubmission>(message);

            if (submission == null)
            {
                logger.LogWarning("Submission was null. Raw message: {Message}", message);
                return;
            }

            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            var containerName = Environment.GetEnvironmentVariable("DocumentsContainer") ?? "mortgage-documents";

            var blobContainerClient = new BlobContainerClient(connectionString, containerName);
            await blobContainerClient.CreateIfNotExistsAsync();

            var blobName = $"{submission.ApplicationId}-{Guid.NewGuid():N}.pdf";
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            var bytes = Encoding.UTF8.GetBytes($"PDF for application {submission.ApplicationId}");
            using var stream = new MemoryStream(bytes);

            await blobClient.UploadAsync(stream, overwrite: true);

            var docMessage = new DocumentGeneratedMessage
            {
                ApplicationId = submission.ApplicationId,
                ApplicantId = submission.ApplicantId,
                BlobName = blobName,
                BlobContainer = containerName
            };

            var docQueueName = Environment.GetEnvironmentVariable("DocumentGeneratedQueue") ?? "q-document-generated";
            var docQueueClient = new QueueClient(connectionString, docQueueName);
            await docQueueClient.CreateIfNotExistsAsync();

            var docJson = JsonSerializer.Serialize(docMessage);
            var docBytes = Encoding.UTF8.GetBytes(docJson);
            var docBase64 = Convert.ToBase64String(docBytes);

            await docQueueClient.SendMessageAsync(docBase64);

            logger.LogInformation("Generated PDF {BlobName} for ApplicationId {ApplicationId}", blobName, submission.ApplicationId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing message: {Message}", message);
            throw;
        }
    }
}
