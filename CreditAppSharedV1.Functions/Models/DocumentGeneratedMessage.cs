namespace CreditAppSharedV1.Functions.Models;

public class DocumentGeneratedMessage
{
    public string ApplicationId { get; set; } = string.Empty;
    public string ApplicantId { get; set; } = string.Empty;
    public string BlobName { get; set; } = string.Empty;
    public string BlobContainer { get; set; } = string.Empty;
}