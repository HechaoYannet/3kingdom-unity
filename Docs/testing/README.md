# Test Infrastructure

**Engine**: Unity 2022 LTS (Tuanjie Engine 1.5.3 compatible)
**Test Framework**: Unity Test Framework / NUnit
**CI**: `.github/workflows/tests.yml`
**Setup date**: 2026-04-27

## Directory Layout

```text
tests/
  EditMode/       # Pure logic and fast validation tests
  PlayMode/       # Scene/coroutine/integration tests
  smoke/          # Critical path smoke checks and gate notes
  evidence/       # Manual screenshots and walkthrough notes
```

## Running Tests

- Unity Editor: `Window -> General -> Test Runner`
- CI: GitHub Actions via `game-ci/unity-test-runner@v4`

## Test Naming

- **Files**: `[system]_[feature]_test.cs`
- **Functions**: `test_[scenario]_[expected]`
- **Example**: `card_round_flow_test.cs` -> `test_round_state_sequence_is_valid()`

## Story Type to Test Evidence

| Story Type | Required Evidence | Location |
|---|---|---|
| Logic | Automated EditMode test | `tests/EditMode/` |
| Integration | PlayMode test or documented manual walkthrough | `tests/PlayMode/` |
| Visual/Feel | Screenshot or clip with reviewer sign-off | `tests/evidence/` |
| UI | Manual walkthrough or interaction test | `tests/evidence/` |
| Config/Data | Smoke check pass plus spot-check notes | `tests/smoke/` |

## CI Notes

The Unity workflow requires a repository secret named `UNITY_LICENSE`. Without that secret, the workflow file can exist but CI will not complete successfully.
