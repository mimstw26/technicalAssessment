using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GeneralKnowledge.Test.App.Tests
{
    /// <summary>
    /// Basic string manipulation exercises
    /// </summary>
    public class StringTests : ITest
    {
        public void Run()
        {
            // TODO:
            // Complete the methods below

            AnagramTest();
            GetUniqueCharsAndCount();
        }

        private void AnagramTest()
        {
            var word = "stop";
            var possibleAnagrams = new string[] { "test", "tops", "spin", "post", "mist", "step" };

            foreach (var possibleAnagram in possibleAnagrams)
            {
                Console.WriteLine(string.Format("{0} > {1}: {2}", word, possibleAnagram, possibleAnagram.IsAnagram(word)));
            }
        }

        private void GetUniqueCharsAndCount()
        {
            var word = "xxzwxzyzzyxwxzyxyzyxzyxzyzyxzzz";

            // TODO:
            // Write an algoritm that gets the unique characters of the word below 
            // and counts the number of occurences for each character found

            // dictionary for characters and their count to be stored, SortedDictionary to make it easier to read in print out.
            SortedDictionary<char, int> countCharacters = new SortedDictionary<char, int>();

            // go through the word and put individual characters into char character
            foreach (char character in word)
            {
                // if this is the first time we have seen the character, create new dictionary entry and
                // set its count to 1
                if (!countCharacters.ContainsKey(character))
                    countCharacters.Add(character, 1);

                // if we have seen this character before, find it in dictionary and up the count by 1
                else
                    countCharacters[character]++;
            }

            foreach (var character in countCharacters)
            {
                Console.WriteLine("{0} : {1}", character.Key, character.Value);
            }

        }
    }

    public static class StringExtensions
    {
        public static bool IsAnagram(this string a, string b)
        {
            // TODO: 
            // Write logic to determine whether a is an anagram of b

            // convert the strings to char arrays so they can be sorted.  
            // used ToUpper to ensure they would each be sorted the same.
            char[] word1Char = a.ToUpper().ToCharArray();
            char[] word2Char = b.ToUpper().ToCharArray();

            // sort the arrays
            Array.Sort(word1Char);
            Array.Sort(word2Char);

            // store arrays in new strings that will be compared
            string word1New = new string(word1Char);
            string word2New = new string(word2Char);

            // compare strings, will return true if same or skip over if and return false if not.
            // no need for an else.
            if (word1New == word2New)
                return true;

            return false;
        }
    }
}
