using Newtonsoft.Json;

namespace HelpDesk.Api.Employee.Issues;


public enum ProblemImpact { Inconvenience, WorkStoppage }
public enum ProblemImpactRadius { Personal, Customer }

public enum ProblemContactPreference { Phone, Email }

public enum SubmittedIssueStatus { AwaitingTechAssignment, UnsupportedSoftwareAwaitingReview }
public record SubmitIssueRequest
{
    public Guid SoftwareId { get; set; }
    public string Description { get; set; } = string.Empty;

    public ProblemImpact Impact { get; set; } = ProblemImpact.Inconvenience;
    public ProblemImpactRadius ImpactRadius { get; set; } = ProblemImpactRadius.Personal;
    public ProblemContactPreference ContactPreference { get; set; } = ProblemContactPreference.Phone;

    public Dictionary<ProblemContactPreference, string> ContactMechanisms { get; set; } = [];

}

/*{
  "id": 99,
  "reportedProblem": {
    "softwareId": "33",
    "description": "Text description of issue",
    "impact": "Inconvenience",
    "impactRadius": "Personal",
    "contactPreference": "Phone",
    "contactMechanisms": {
      "Phone": "555-1212",
      "Email": "bob@company.com"
    }
},
  "status": "AwaitingTechAssignment",
  "reportedAt": "DateTimeIso",
  "reportedBy": "bob@company.com"
}*/

public record SubmitIssueResponse
{
    public Guid Id { get; set; }
    public SubmitIssueRequest? ReportedProblem { get; set; }
    public SubmittedIssueStatus Status { get; set; } = SubmittedIssueStatus.AwaitingTechAssignment;

    public DateTimeOffset ReportedAt { get; set; }
    public string ReportedBy { get; set; } = string.Empty;

    public EmployeeProblemEntity MapToEntity()
    {
        return new EmployeeProblemEntity
        {
            Id = Id,
            ReportedAt = ReportedAt,
            ReportedBy = ReportedBy,
            Status = Status,
            ReportedProblem = ReportedProblem,

        };
    }

}