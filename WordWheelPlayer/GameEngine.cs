using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace WordWheelPlayer
{
    public class GameEngine
    {
        private const int minLength = 3;

        private List<string> WordsFound = new List<string>();

        private List<GameLetter> GameLetters = new List<GameLetter>();

        private string keyLetter = String.Empty;

        private List<string> EnglishDictionary = new List<string>();

        //private List<string> LettersToUse = new List<string>()
        //{
        //    "I", // By convention, all words must include this letter
        //    "E",
        //    "D",
        //    "F",
        //    "T",
        //    "C",
        //    "E",
        //    "G",
        //    "N"
        //};

        private List<string> LettersToUse = new List<string>()
        {
            "A", // By convention, all words must include this letter
            "C",
            "E",
            "L",
            "R",
            "W",
            "T",
            "A",
            "U"
        };

        public GameEngine()
        {
            Init();
        }

        public void Start()
        {
            string word = String.Empty;

            while (word != "-")
            {
                word = Console.ReadLine();
                Console.WriteLine();

                if (word != null)
                {
                    word = word.ToUpper();

                    if (WordsFound.Contains(word) || !word.Contains(keyLetter))
                    {
                        continue;
                    }

                    int letterCount = 0;

                    foreach (var guessLetter in word)
                    {

                        var gl = GameLetters.FirstOrDefault(x => x.Letter == guessLetter.ToString() && x.Used == false);

                        if (gl != null)
                        {
                            gl.Used = true;
                            letterCount++;
                        }
                    }

                    if (letterCount == word.Length)
                    {
                        if (WordIsInDictionary(word) && word.Length >= minLength)
                        {
                            WordsFound.Add(word);
                        }
                    }
                    ResetLetters();
                }

                int wordsFound = 0;

                WordsFound.Sort();
                Console.Clear();
                foreach (var foundWord in WordsFound)
                {
                    Console.WriteLine(foundWord);
                    wordsFound++;
                }

                Console.WriteLine();
                Console.WriteLine($"Total:{wordsFound}");
                Console.WriteLine();
            }

        }

        private void ResetLetters()
        {
            foreach (var letter in GameLetters)
            {
                letter.Used = false;
            }
        }

        private void Init()
        {
            InitDictionary();

            foreach (var letter in LettersToUse)
            {
                var gameLetter = new GameLetter()
                {
                    Letter = letter,
                    Used = false
                };

                Console.Write(letter.ToString());

                if (GameLetters.Count == 0)
                {
                    gameLetter.MustInclude = true;
                    keyLetter = letter;
                    Console.Write("*");
                }

                GameLetters.Add(gameLetter);
            }
            Console.WriteLine();
        }


        private void InitDictionary()
        {
            using (var sr = new StreamReader("words.txt"))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    var candidate = line.ToUpper();

                    string regex = "^[a-zA-Z]+$";


                    if (candidate.Length <= LettersToUse.Count &&
                        candidate.Length >= minLength &&
                        Regex.IsMatch(candidate, regex)
                       )
                    {
                        EnglishDictionary.Add(candidate);
                    }
                }
            }
        }

        private bool WordIsInDictionary(string wordToCheck)
        {
            return EnglishDictionary.Contains(wordToCheck);
        }
    }
}
