# 🚀 Improvements & Roadmap

---
- Enhance wait strategies to reduce test flakiness.
- Add retry logic for intermittent/flaky tests.
- Organize smoke tests into a proper test suite class.
- Cross-browser support.
- Use [TestPriority] or [ITestCaseOrderer] to control test execution order.
- Scale the suite to 200+ tests with tagging and filtering capabilities.
- Implement API-level validations for faster and more reliable feedback.
- Utilize environment-specific configurations for staging vs production.
- Monitor and verify performance for page load times.

---
## ⚠️ Assumptions
- Robot detection bypass is allowed using the provided user agent.
- Tests do not trigger "contact agent" or "viewing request" flows.
- Tests run in a controlled environment (local dev machines or CI).

