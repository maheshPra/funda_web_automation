🖥 Local

✅ Prerequisites

  1) .NET SDK installed.
  2) Chromium, Firefox, or WebKit browsers available.
  3) Allure CLI available (for reporting).
  4) Playwright dependencies available.

🔧 Setup & Execution
  1) Clone the repository: git clone https://github.com/maheshPra/funda_web_automation.git.
  2) cd funda_web_automation.
  3) Replace <FUNDA_USER_AGENT> with your user agent value in the following files:
    .env file: <FUNDA_USER_AGENT>
    run-latest-report.sh script: "<FUNDA_USER_AGENT>" (enclosed in double quotes).

▶️ Run tests and reports, execute the following in the terminal

NOTE: By default, the tests run in headless mode.To execute them in headless mode, refer to step 4 below.

  1) To run all tests and open the latest Allure report.
    ./run-latest-report.sh
  2) Run all tests:
    dotnet test
  3) Run a specific test:
    dotnet test --filter <TestName>
  4) Run in headed(with browser) mode:
      Open PlaywrightTestBase.cs
      Set Headless = false
      Re-run tests as above 

☁️ CI (GitHub Actions)

  1) Open repository: funda_web_automation
  2) Click Actions from the top menu
  3) Select "Manual Playwright Test with Allure" workflow
  4) Click the Run workflow dropdown
  5) Enter the "FUNDA_USER_AGENT" value and click Run workflow
     
     👉 "Due to test flakiness, the pipeline occasionally fails to run, which prevents the report from being generated.
     This issue needs further investigation, but given the time constraints, I focused on completing the configuration for now.
     Sample success report: https://maheshpra.github.io/funda_web_automation/
