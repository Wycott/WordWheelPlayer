# Untested Modules — WordWheelPlayer

## Summary

14 source files were checked (excluding `Program.cs` which is the entry point and `GameConsole.cs` which is marked `[ExcludeFromCodeCoverage]`). Of the remaining 12 files, 2 have no corresponding unit test coverage: `GameConfig.cs` and `Helpers/BestScoreHelper.cs`. Overall test coverage is good — 10 out of 12 testable classes are exercised.

---

## Untested Files

| # | File | Class/Interface | Reason |
|---|------|-----------------|--------|
| 1 | `WordWheelPlayer/GameConfig.cs` | GameConfig | No test file references this class |
| 2 | `WordWheelPlayer/Helpers/BestScoreHelper.cs` | BestScoreHelper | No test file references this class |

---

## Tested Files

| # | File | Class/Interface | Test File(s) |
|---|------|-----------------|--------------|
| 1 | `WordWheelPlayer/GameEngine.cs` | GameEngine | `GameEngineTests.cs`, `GameEngineAdditionalTests.cs`, `GameEngineWordValidationTests.cs` |
| 2 | `WordWheelPlayer/Display.cs` | GameEngine (partial) | `DisplayTests.cs` |
| 3 | `WordWheelPlayer/EnglishDictionary.cs` | EnglishDictionary | `EnglishDictionaryTests.cs`, `EnglishDictionaryAdditionalTests.cs` |
| 4 | `WordWheelPlayer/IGameConsole.cs` | IGameConsole | Referenced via mocks in `GameEngineTests.cs`, `DisplayTests.cs`, etc. |
| 5 | `WordWheelPlayer/GameLetter.cs` | GameLetter | `GameLetterTests.cs`, `GameLetterAdditionalTests.cs` |
| 6 | `WordWheelPlayer/LongestWordCandidate.cs` | LongestWordCandidate | `LongestWordCandidateTest.cs`, `LongestWordCandidateAdditionalTests.cs` |
| 7 | `WordWheelPlayer/BestScore.cs` | BestGame | `BestScoreTests.cs`, `BestGameAdditionalTests.cs` |
| 8 | `WordWheelPlayer/Helpers/DisplayHelper.cs` | DisplayHelper | `Helpers/DisplayHelperTests.cs` |
| 9 | `WordWheelPlayer/Helpers/LetterHelper.cs` | LetterHelper | `Helpers/LetterHelperTests.cs`, `Helpers/LetterHelperAdditionalTests.cs` |
| 10 | `WordWheelPlayer/Helpers/ScoreHelper.cs` | ScoreHelper | `Helpers/ScoreHelperTests.cs` |
