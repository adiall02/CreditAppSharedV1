namespace CreditAppSharedV1.Functions.Models;

public class DecisionMessage
{
    public string ApplicationId { get; set; } = string.Empty;
    public string ApplicantId { get; set; } = string.Empty;
    public string Decision { get; set; } = string.Empty;
    public DateTime DecidedAtUtc { get; set; }
}