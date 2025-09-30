# 🏡 Funda Smoke Test Suite

This project contains automated smoke tests for the **Funda website** using **C#, .NET, Playwright, xUnit, and Allure**.

---

## 🎯 Objective
Validate critical user flows on the Funda website to ensure confidence during releases.  
These tests are **fast, reliable, and scalable** (target: up to ~200 tests).

---

## 🧪 Smoke Tests Included
- **Login Test** – Verifies login and logout functionality.
- **Landing Page Test** – Validates UI elements and navigation on the landing page.
- **Property Details Test** – Ensures property card and details page show consistent information.
- **Search Test** – Performs a city-based search and validates results.
- **Sort Test** – Sorts search results by price and verifies correct ordering.

---

## 🛡️ Robot Detection Handling
To bypass robot detection, a special **user agent** is used (stored in `.env`).  

---

## 🛠 Tech Stack
- **Language:** C#
- **Framework:** .NET 7
- **Automation:** Playwright
- **Testing:** xUnit
- **Reporting:** Allure Reports
- **CI/CD:** GitHub Actions

---

## 📂 Project Structure
```text
FUNDA
│
├── funda_web_automation
│ ├── .config
│ ├── .github
│ ├── allure-report
│ ├── bin
│ ├── Fixtures
│ │ └── PlaywrightTestBase.cs
│ ├── obj
│ ├── Pages
│ │ ├── HeaderPage.cs
│ │ ├── LandingPage.cs
│ │ ├── LoginPage.cs
│ │ ├── PropertyDetailsPage.cs
│ │ └── SearchResultsPage.cs
│ └── Tests
│   ├── LandingPageTests.cs
│   ├── LoginTests.cs
│   ├── PropertyDetailsTests.cs
│   ├── SearchTests.cs
│   └── SortingTests.cs
│
├── .env
├── .gitignore
├── allureConfig.json
├── PlaywrightTests.csproj
├── run-latest-report.sh
├── TestData.cs
├── xunit.runner.json
├── Fun_Playwright.sln.old
└── funda.sln
```
---

## ⚠️ Known Flaky Tests
- `LoginTests.LoginAndLogout` → Occasionally times out because the "Inloggen" button is not clickable

## 👤 Author
**Mahesh Pra** – C# / Automation Engineer  
GitHub: [maheshPra/funda_web_automation](https://github.com/maheshPra/funda_web_automation)
