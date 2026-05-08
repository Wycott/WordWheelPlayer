---
inclusion: manual
---

# Game Rules Reference

## Core Mechanics

1. A random 9-letter word is selected from the dictionary as the seed
2. One letter is chosen as the **key letter** (central letter) using Scrabble-weighted probability favouring common letters (A, E, I, O, U, L, N, S, T, R)
3. Letters are displayed in a diamond/wheel pattern with the key letter in the centre
4. The player enters words to score points

## Word Validation Rules

A word is accepted if ALL of the following are true:

| Rule | Detail |
|------|--------|
| Minimum length | 3 letters |
| Maximum length | 9 letters |
| Contains key letter | The central letter must appear in the word |
| Valid letters only | Only letters from the wheel may be used |
| Each letter used once | A letter cannot be used more times than it appears in the wheel |
| No trailing S | Words ending in S are rejected (words ending in SS are allowed) |
| In dictionary | The word must exist in `words.txt` |
| Not already found | Duplicate entries are rejected |

## Scoring

Points are awarded using the Fibonacci sequence indexed by `(word.Length - 3)`:

| Word Length | Points |
|-------------|--------|
| 3 letters | 1 |
| 4 letters | 2 |
| 5 letters | 3 |
| 6 letters | 5 |
| 7 letters | 8 |
| 8 letters | 13 |
| 9 letters | 21 |

## Best Score

- The best score is persisted to `BestGame.json`
- If the current score exceeds the stored best, it's updated immediately
- The date is shown as "Just now" if achieved in the current session

## Letter Display

Letters are arranged in a diamond pattern across 5 lines:

```
        A
      B   C
    D   E   F
      G   H
        I
```

The centre position (E in the example) is the key letter. The key letter is marked internally with a `*` suffix in the letter string.

## Commands

| Command | Short | Action |
|---------|-------|--------|
| :LETTERS | :L | Redisplay the letter wheel |
| :WORDS | :W | List all words found so far |
| :MIX | :M | Shuffle the surrounding letters (key stays central) |
| :SCORE | :S | Show current score and best score |
| :RESTART | :R | Start a new game with fresh letters |
| :VERSION | :V | Show build/version info |
| :HELP | :H | Show instructions |
| :EXIT | :X | Quit the game |

### Easter Eggs

| Command | Action |
|---------|--------|
| :EGG / :EASTEREGG | Displays a teaser message |
| :PEEK | Reveals the 9-letter solution word |
