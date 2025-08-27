# Wednesday Morning Lab

We need another service for our Issue Tracker app. This service will be an HTTP API.

## Purpose

This API will "own" the Help Desk Employee VIPs.

### What is a "VIP"?

These are employees that, for reasons determined by the manager of the Help Desk, deserve priority service when reporting a problem to the help desk.

### Setup Before Lab

> I will step you through this, this is for reference.

#### Containers (Backing Services)

Make sure Docker Desktop is running.

- Using the Containers extension in VSCode, under the "Containers" section of the UI (at the top), right-click on any docker-compose files (probably `dev-environment`) and click "Compose Down".
- In the same UI, navigate further down to "Volumes" and right-click and remove any persistent volumes (this will reset all of our databases)
- In the Explorer ui in VSCode, navigate to your `src/dev-environment/db` folder. Add a new file named `vips.sql`.
- In this file add the following: `create database vips;`
- Save and close the file `vips.sql` file.
- Right-click on the `docker-compose.yml` file and select `Compose Up`.

#### Project

Make sure Visual Studio is closed during this, please.

1. In Visual Studio Code, open a terminal (Ctrl+BackTick). 
2. Make sure you are in the `~\class\web-api-200` directory.
3. Run the following command:

```bash
 npx gitpick -o JeffryGonzalez/web-api-200-aug-2025 instructor
```

This will bring my current code into the `instructor` directory in your repo.

In **your* `/src/` directory create a new folder called "Lab1".

In the VS Code Explorer, Right-Click on the `instructor/src/HelpDesk` folder and select "Copy".

Right click on your `/src/Lab1` folder in the Explorer and select "Paste".

Open Visual Studio, and select "Open Project" and navigate to `~/class/web-api-200/src/Lab1/HelpDesk` and select the `HelpDeskSolution.sln`.

There are two projects - the `HelpDesk.Api` and the `HelpDesk.Vip.Api`. Your work for this lab will be in the `HelpDesk.Vip.Api` project.

This will serve on `http://localhost:1338`. Make sure Visual Studio is using that VIP Project to start.



### Functionality Required

#### Adding a VIP

The user should be able to POST a request to the VIP endpoint.

Example:

```http
POST http://localhost:1338/vips
Content-Type: application/json

{
    "sub": "Bob@aol.com",
    "description": "This is Bob Smith, the lead of the band The Cure, definitely a VIP"
}
```

Properties:

- `sub` This is the `sub`ject claim from the identity token. 
    - Required
    - Must be unique. You can't add the same VIP twice. If the same `sub` is posted more than once, return an Http Status Code of 409 - Conflict.
- `description` - a brief description of why they are considered a VIP. 
    - Required
    - Should be at least 10 characters and less than 500 characters.

A successful POST should return:

```http
201 Created
Location: /vips/{id}
Content-Type: application/json

{
    "id": "Id as Guid",
    "sub": "Bob@aol.com",
    "description": "description from request",
    "addedOn": "ISO 8601 of the date this was added"
}
```

### Getting a VIP

Implement the `GET /vips/{id}` endpoint.

### Getting All Vips

Implement `GET /vips` endpoint.

This should return a list of each of the VIPs.

### Removing a VIP

Implement a `DELETE /vips/{id}` endpoint.

This should always return a `204 No Content` response.

- Rules:
    - And existing VIP that is removed (deleted) should *never* be removed from the database.
    - If the VIP does exist, simply mark it somehow as "removed". 
        - Note, this means the POST above may actually "reactivate" a "removed" VIP.

## Bonus (If Time Allows) Steps

- Implement Open Telemetry For this API. (Note, the Nuget packages have already been added to the starter project)
- Create a custom Otel meter of a counter for each time a VIP is added or removed.
- Add System Tests

## Some "Hints"

You probably can do most of this by looking at our previous work in the `HelpDesk.Api` project and in the Web API 100's `SoftwareCenter.Api` projects in the `src` directory.

- If you do validation, you can use either the validation attributes "built in" in (like `[Required]`, etc.) or you can install and use the FluentValidations library (see the SoftwareCenter.Api for a reference)
- You can use either controllers or functional "minimal APIs" - again references to this in our other projects.
- If you use controllers, remember to add `builder.Services.AddControllers()` and `app.MapControllers()` in your `Program.cs`.
- `DELETE` requests do not take an `body`. Everything has to be on the URL or headers.
- The biggest hint - **do not stay "stuck" for more than 10 minutes.** Ask me for help!