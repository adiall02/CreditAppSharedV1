using System;
using System.Text;
using System.Text.Json;
using Azure.Storage.Queues.Models;
using CreditAppSharedV1.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CreditAppSharedV1.Functions.Functions;

public class PersistSnapshotFunction
{
    private readonly ILogger<PersistSnapshotFunction> _logger;

    public PersistSnapshotFunction(ILogger<PersistSnapshotFunction> logger)
    {
        _logger = logger;
    }

    [Function("Fn_PersistSnapshot")]
    public Task Run(
        [QueueTrigger("%DocumentGeneratedQueue%", Connection = "AzureWebJobsStorage")] string message)
    {
        var doc = JsonSerializer.Deserialize<DocumentGeneratedMessage>(message);

        if (doc != null)
            _logger.LogInformation("Persist snapshot placeholder for ApplicationId {ApplicationId}, Blob {BlobName}", doc.ApplicationId, doc.BlobName);

        return Task.CompletedTask;
    }
}