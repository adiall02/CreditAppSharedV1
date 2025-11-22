using System;
using System.Text;
using System.Text.Json;
using Azure.Storage.Queues.Models;
using CreditAppSharedV1.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CreditAppSharedV1.Functions.Functions;

public class SendDecisionEmailFunction
{
    private readonly ILogger<SendDecisionEmailFunction> _logger;

    public SendDecisionEmailFunction(ILogger<SendDecisionEmailFunction> logger)
    {
        _logger = logger;
    }

    [Function("Fn_SendDecisionEmail")]
    public Task Run(
        [QueueTrigger("%DecisionMadeQueue%", Connection = "AzureWebJobsStorage")] string message)
    {
        var decisionJson = Encoding.UTF8.GetString(Convert.FromBase64String(message));
        var decision = JsonSerializer.Deserialize<DecisionMessage>(decisionJson);

        if (decision != null)
            _logger.LogInformation("Send decision email placeholder for ApplicationId {ApplicationId}, Decision {Decision}", decision.ApplicationId, decision.Decision);

        return Task.CompletedTask;
    }
}