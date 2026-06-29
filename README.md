# Probability Calculator

A web app that calculates the probability of two independent events. Built with a .NET 10 backend API and a React frontend.


## What It Does

You enter two probability values (between 0 and 1) and pick one of two calculations:

  Combined With — the chance that both events happen: P(A) x P(B)
  Either — the chance that at least one event happens: P(A) + P(B) - P(A) x P(B)

The result is shown on screen and saved to a log file on the server.


## Requirements

  .NET 10 SDK
  Node.js 20 or later
  npm 10 or later


## How to Run

### Step 1 — Start the backend

Open a terminal and run:

  cd backend
  dotnet run --project src/ProbabilityCalculator.Api

The API will start at http://localhost:5246

To test the API in the browser, open: http://localhost:5246/scalar/v1

To test manually, send a POST request to http://localhost:5246/api/calculate with this JSON body:

  {
    "probabilityA": 0.5,
    "probabilityB": 0.5,
    "operation": "CombinedWith"
  }

Valid values for operation: CombinedWith, Either
Valid range for probabilityA and probabilityB: 0 to 1 inclusive

Expected response:

  {
    "operation": "CombinedWith",
    "probabilityA": 0.5,
    "probabilityB": 0.5,
    "result": 0.25
  }

### Step 2 — Start the frontend

Open a second terminal and run:

  cd frontend
  npm install
  npm run dev

The app will open at http://localhost:5173

### Step 3 — Run the backend tests

  cd backend
  dotnet test

All 24 tests should pass.

### Step 4 — Run the frontend tests

  cd frontend
  npm test

All 14 tests should pass.


## Project Structure

  backend/src/ProbabilityCalculator.Api       The API (C#)
  backend/tests/ProbabilityCalculator.Tests   Backend tests (24 total)
  frontend/                                   The web app (React + TypeScript)
  frontend/src/test/                          Frontend tests (14 total)
  .vscode/                                    VS Code settings


## How It Is Designed

### Backend

The backend is a .NET 10 Minimal API with one endpoint: POST /api/calculate

Validation happens first. Both probabilities must be between 0 and 1. If the input is invalid, the API returns an error message and the calculation never runs.

The Strategy Pattern is used for the two operations. Each operation is its own class (CombinedWithOperation, EitherOperation). The service looks up the right one by name and runs it. To add a new operation in the future, you just add a new class — nothing else needs to change.

Every successful calculation is written to a plain text file (calculationlog.txt) with the date, inputs, and result.

### Frontend

The frontend is built with React 19 and TypeScript using Vite.

  The form collects the two probabilities and the selected operation
  React Hook Form handles form validation on the client side before anything is sent to the API
  The result is displayed below the form
  If the API returns an error, it is shown in red and the previous result is cleared


## Design Decisions and Trade-offs

The Strategy Pattern was chosen for the calculations because the brief mentioned future extensibility — more operations may be added later. With this approach, adding a new calculation is a single new class with no changes to existing code. A simple if/else approach would have been shorter but harder to extend cleanly.

FluentValidation was chosen over basic data annotations because validation rules live in a dedicated class that can be unit tested independently. The validator also derives valid operation names directly from the registered operations, so it stays in sync automatically when new operations are added.

The frontend uses React Hook Form rather than plain React state because it avoids re-rendering the component on every keystroke and keeps validation logic separate from display logic. For two fields this is a minor gain, but it reflects the right approach for a form that may grow.

The API documentation uses Scalar instead of Swagger. In .NET 10, Microsoft replaced Swashbuckle (the traditional Swagger package) with built-in OpenAPI support. Scalar is the recommended UI for this — it provides the same ability to browse and test endpoints but works natively with .NET 10 without additional packages. The API docs can be viewed at http://localhost:5246/scalar/v1 when running locally.

The audit log is a plain text file as specified. In a production system this would be replaced with structured logging (such as Azure Application Insights or a logging service) to support searching, filtering, and alerting. The ICalculationLogger interface already abstracts this — swapping the implementation requires no changes to the service.

The API is stateless. Each request carries everything it needs. This was a deliberate choice to make the service easy to scale horizontally — multiple instances can run behind a load balancer without needing to share session state.


## Cloud, Deployment and Scaling

### How to Deploy

The backend would be packaged as a Docker container and deployed to a container platform such as Azure App Service or Azure Kubernetes Service (AKS). The frontend would be built with npm run build and deployed as static files to Azure Static Web Apps or a CDN such as Azure Front Door.

A typical deployment pipeline would look like this:

  1. Developer pushes code to GitHub
  2. GitHub Actions runs the tests automatically
  3. If tests pass, it builds the Docker image and pushes it to a container registry
  4. The container is deployed to the cloud environment

### How to Scale

Because the API is stateless, scaling is straightforward. Under high load, the container platform automatically starts more instances of the API behind a load balancer. Azure App Service and AKS both support auto-scaling based on CPU usage or request volume.

For global usage, the frontend static files would be served from a CDN with edge locations in each region, so users get fast load times regardless of where they are. The API could be deployed to multiple regions with Azure Front Door routing requests to the nearest one.

The text file log would need to be replaced with a centralised logging service (such as Azure Application Insights) before scaling to multiple instances, since each instance would otherwise write to its own separate file.

### High Availability

To achieve 99.99% uptime:

  Run at least two instances of the API at all times so that if one fails, the other continues serving requests
  Use a health check endpoint so the load balancer can detect and remove unhealthy instances automatically
  Deploy across two or more availability zones so a data centre outage does not take the service down
  Use blue-green deployments — the new version is deployed alongside the old one, traffic is switched over gradually, and the old version is kept running until the new one is confirmed stable

### CI/CD

A GitHub Actions pipeline would handle the full lifecycle:

  On every pull request — run all tests, block merging if any fail
  On merge to main — build and push the Docker image, deploy to a staging environment
  On release — deploy to production with approval gate


## Monitoring and Troubleshooting in Production

### Monitoring

Azure Application Insights would be used to track request volume, response times, error rates, and dependency health. A dashboard would show the key metrics at a glance, and alerts would fire if error rates exceed a threshold or response times degrade.

### Logging

Each API request would be logged with a correlation ID so that a single user journey can be traced across log entries. Structured logging (key-value pairs rather than plain text) makes it easy to filter and query logs. The current text file log would be replaced with this in production.

### Scaling

Auto-scaling rules would be set based on CPU usage and request volume. During peak hours, additional instances spin up automatically. During quiet periods, they scale back down to reduce cost.

### Troubleshooting

If an issue is reported in production, the steps would be:

  1. Check the Application Insights error logs and filter by the time the issue was reported
  2. Find the correlation ID from the affected request
  3. Trace all log entries for that request to identify where it failed
  4. If needed, reproduce in the staging environment using the same inputs
  5. Deploy a fix through the normal CI/CD pipeline — no manual changes to production


## Configuration

### Backend — Log File Path

The audit log file path can be changed in:
  backend/src/ProbabilityCalculator.Api/appsettings.json

Look for this setting:

  CalculationLog
    FilePath: calculationlog.txt

Change the value to any file path you want. The file is created automatically.

### Backend — API Port

The port is set in:
  backend/src/ProbabilityCalculator.Api/Properties/launchSettings.json

The default port is 5246. If you change it, update the frontend .env file to match.

### Frontend — API URL

A .env.example file is provided in the frontend folder. Copy it and rename it to .env:

  cp frontend/.env.example frontend/.env

Then update the value if your API is running on a different port. Restart the frontend after any changes. If no .env file is present, the frontend defaults to http://localhost:5000.


## VS Code

Press F5 to launch both the API and the frontend at the same time with debuggers attached.

You can also run individual tasks from Terminal > Run Task:

  build: api      builds the backend
  test: all       runs all 24 backend tests
  frontend: dev   starts the frontend

To run the frontend tests, open a terminal in the frontend folder and run npm test. This runs 14 tests covering the API client, form behaviour, and result display.
