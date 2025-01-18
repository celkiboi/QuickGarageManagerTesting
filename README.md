# README

## Automated Testing Framework for GitHub Pages Web Application

This project is an automated testing framework built with **C#**, **NUnit**, and **Playwright** to test a webpage hosted on GitHub Pages. The framework includes several test cases and demonstrates techniques such as browser automation, page navigation, and element validation.

### Prerequisites

Before setting up the project, ensure the following tools are installed on your machine:

1. **.NET SDK** (version 9.0) - [Download here](https://dotnet.microsoft.com/download)
2. **Visual Studio 2022** or later with the following workloads:
   - ASP.NET and web development
   - .NET desktop development
3. **Microsoft.Playwright CLI**

### Installation Steps

Follow these steps to set up the project:

1. **Clone the Repository**
   ```bash
   git clone https://github.com/your-username/your-repository.git
   cd your-repository
   ```

2. **Install NuGet Packages**
   Open the project in Visual Studio and install the following packages via the NuGet Package Manager:
   - **Microsoft.Playwright**
   - **NUnit**
   - **NUnit3TestAdapter**

   Alternatively, run the following commands in the **Package Manager Console**:
   ```bash
   Install-Package Microsoft.Playwright
   Install-Package NUnit
   Install-Package NUnit3TestAdapter
   ```

3. **Install Playwright Browsers**
   In the **Package Manager Console** or terminal, execute:
   ```bash
   dotnet tool install --global Microsoft.Playwright.CLI
   dotnet playwright install
   ```

   This command downloads the required browsers (Chromium, Firefox, WebKit).

4. **Configure the .gitignore File**
   Ensure the `.gitignore` file excludes unnecessary files like `bin/`, `obj/`, and `.playwright/`. The file is included in the repository for your convenience.

5. **Run the Tests**
   Open **Test Explorer** in Visual Studio:
   - Go to `Test > Test Explorer`.
   - Build the project (`Ctrl + Shift + B`) to discover the tests.
   - Run all tests by selecting them and clicking `Run`.
