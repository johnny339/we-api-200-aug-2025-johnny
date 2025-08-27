# Employee Submits Problem

## Request

We need to know:

- Who: Employee - in Authorization Header
- softwareId: A reference to the software for which the problem is related.
- description: A narrative description of the problem
- impact: "Inconvenience" | "WorkStoppage"
- impactRadius: "Personal" | "Customer"
- contactPreference: How the customer prefers to be contacted. Either phone or email.
- contactMechanisms: A map of phone and email.

```http
POST http://localhost:1337/employee/problems
Authorization: Bearer JWT
Content-Type: application/json

{
  "softwareId": "bc3f26b0-4f07-41ad-8370-e73733afbe83",
  "description": "Text description of issue",
  "impact": "Inconvenience",
  "impactRadius": 0,
  "contactPreference": 0,
  "contactMechanisms": {
    "Phone": "555-1212",
    "Email": "jeff@company.com"
  }
}

```

```http
GET http://localhost:1337/employee/problems/c72640f6-06ae-4de3-a920-c23e1ccc278c
```

## Possible Manager Submitting an Issue on Behalf of the Employee

```http
POST /employees/{idOfTheEmployee}/problems
Authorization: Bearer JWT (role = manager)
Content-Type: application/json

{
  "softwareId": "33",
  "description": "Text description of issue",
  "impact": "Inconvenience",
  "impactRadius": "Personal",
  "contactPreference": "Phone",
  "contactMechanisms": {
    "Phone": "555-1212",
    "Email": "bob@company.com"
  }
}
```


## Response

We send:
- id: A generated Id for this problem.
- reportedProblem: A copy of the submitted problem
- status:
  - `AwaitingTechAssignment` - default, when logged.
  - `AssignedToTech` - after a tech has been assigned
  - `CustomerContacted` - after the tech has contacted the customer
  - `ProblemResolved` - if the tech is able to resolve the problem during contact.
  - ?? Elevated? Etc.
- reportedAt: The ISO 8601 of when the problem was reported
- reportedBy: The `sub` claim of the person that reported the problem

We need to validate the request - 

- softwareId: must be the id of a supported piece of software. Or does it?

```http
201 Created
Location: /employee/problems/99
Content-Type: application/json

{
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
}
```

