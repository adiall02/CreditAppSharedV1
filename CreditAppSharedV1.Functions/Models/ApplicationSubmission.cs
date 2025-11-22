namespace CreditAppSharedV1.Functions.Models;

public class ApplicationSubmission
{
    public string ApplicationId { get; set; } = string.Empty;
    public string ApplicantId { get; set; } = string.Empty;
    public decimal RequestedAmount { get; set; }
    public int TermMonths { get; set; }
    public decimal AnnualIncome { get; set; }
    public decimal MonthlyDebt { get; set; }
    public decimal FinalMonthlyRate { get; set; }
    public DateTime SubmittedUtc { get; set; } = DateTime.UtcNow;
}