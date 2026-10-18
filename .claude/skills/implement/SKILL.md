---
name: implement
description: "Use when I want you to implement new code, let it be a feature, unit test or a fix."
---

# Development & Verification Workflow

Execute the following sequential steps when asked to implement, build or code a new feature or test in this project. Do 
not skip any phase.


## 1. Preparation

Look at the markdown files in /docs/UserGuide directory to understand the project. Load the solution and get an overview 
of the project structure with projects and classes. Check the /docs/Architecture.md to understand the architecture and 
design principles.


## 2. Planning

Before starting the implementation, plan the approach to solve the task. Identify the components that need to be modified 
or created, consider the impact on existing code, and determine the best way to structure the new code. Break down the 
task into smaller, manageable steps and think about potential edge cases and error handling.

/docs/Architecture.md will give you more details on how specific areas of the project are supposed to be implemented and 
also about the process to follow. Use it as a reference to ensure that your implementation aligns with the overall 
architecture and design principles.

When you're not sure about the implementation details, ask for clarification or additional information. Make sure you have 
a clear understanding of the requirements and the expected outcome before proceeding.

Always include unit tests. Plan the test cases to cover various scenarios, including normal cases, edge cases, and error 
conditions. Ensure that the tests are comprehensive and validate the correctness of the implementation. You might find
some more details on how to write unit tests for specific areas in /docs/Architecture.md.


## 3. Implementation

Implement the requested task according to your plan. Adhere to /docs/Architecture.md and /docs/CodingGuidelines.md. Try 
to stick to the existing coding style and naming conventions. Use the existing code as a reference.


## 4. Verification

- Compile the code and try to fix errors. Keep in mind that we are designing an API here. So if you have compilation errors I might also need to change the API.
- Only compile once. If you cannot fix errors in the first try, ask me for help. Do not try to fix errors in multiple iterations.
- If compilation is successful, run only the unit tests affected by the current implementation (not the full suite, it 
  takes too long) and ensure they pass. Affected tests are the new/changed test classes plus existing test classes 
  covering the changed code. Use a filter, for all providers when shared code changed: 
  `dotnet test Quib.slnx --filter "FullyQualifiedName~Quib.UnitTests.QueryTests.LimitTest|FullyQualifiedName~Quib.UnitTests.QueryTests.OrderTest"`.
- If any test fails, investigate the cause and fix the issue.
- Again, if you cannot fix the issue in the first try, ask me for help. Do not try to fix issues in multiple iterations.
- Run `dotnet format` only on the files you changed or added, never on the whole solution: 
  `dotnet format Quib.slnx --include <file1> <file2> ...`.