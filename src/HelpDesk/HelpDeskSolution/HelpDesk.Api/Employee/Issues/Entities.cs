namespace HelpDesk.Api.Employee.Issues;

public class EmployeeProblemEntity
{
    public Guid Id { get; set; }
    public SubmitIssueRequest? ReportedProblem { get; set; }
    public SubmittedIssueStatus Status { get; set; } = SubmittedIssueStatus.AwaitingTechAssignment;

    public DateTimeOffset ReportedAt { get; set; }
    public string ReportedBy { get; set; } = string.Empty;

    public SubmitIssueResponse MapToResponse()
    {
        return new SubmitIssueResponse
        {
            Id = Id,
            Status = Status,
            ReportedAt = ReportedAt,
            ReportedBy = ReportedBy,
            ReportedProblem = ReportedProblem,
        };
    }
}
