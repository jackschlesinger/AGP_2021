using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using SimpleJSON;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class Week7 : MonoBehaviour
{
    /*
     * Below are a series of problems to solve with recursion.  You may need to make additional functions to do so.
     * Do not solve these problems with loops.
     */

    // Return the reversed version of the input.
    public string ReverseString(string toReverse)
    {
        return ReverseStringRecursive(toReverse);
    }
    
    string ReverseStringRecursive(string toReverse, string reversedString = "")
    {
        if (toReverse.Length == 0)
        {
            return reversedString;
        }

        return ReverseStringRecursive(toReverse.Substring(0, toReverse.Length - 1),
            reversedString + toReverse[toReverse.Length - 1]);
    }
    
    // CAT    ""
    // CA     "T"
    // C       "TA"
    // ""     "TAC"
    
    // Return whether or not the string is a palindrome
    public bool IsPalindrome(string toCheck)
    {
        return IsPalindromeRecursive(toCheck);
    }

    public bool IsPalindromeRecursive(string toCheck, int index = 0)
    {
        if (index >= toCheck.Length / 2)
            return true;

        if (char.ToUpper(toCheck[index]) != char.ToUpper(toCheck[toCheck.Length - 1 - index]))
            return false;
        
        return IsPalindromeRecursive(toCheck, index + 1);
    }
    
    // 0123456
    // RACECAR
    // R == R ?
    // A == A ?
    // C == C ?
    // get to e, return true (midddle)
    
    // DOOD
    // D == D?
    // O == O?

    // Return all strings that can be made from the set characters using all characters.
    
    // AllStringsFromCharacters('a', 'b', 'c', 'd'); => AllStringsFromCharacters(new char[] { 'a', 'b', 'c', 'd'});
    
    public string[] AllStringsFromCharacters(params char[] characters)
    {
        return RecurseASFC(characters.ToList(), new HashSet<string>(), new List<Tuple<char[], string>>()).ToArray();
    }

    HashSet<string> RecurseASFC(List<char> characters, HashSet<string> toReturn, List<Tuple<char[], string>> alreadyTraversed, string current = "")
    {
        if (alreadyTraversed.Any((k) =>
        {
            var (collectionOfCharacters, cachedString) = k;
            return UnorderedSequencesEqual(collectionOfCharacters, characters) && cachedString == current;
        }));
        
        alreadyTraversed.Add(new Tuple<char[], string>(characters.ToArray(), current));

        if (characters.Count == 0)
        {
            toReturn.Add(current);
            return toReturn;
        }

        foreach (var letter in characters)
        {
            var charactersWithoutLetter = new List<char>();
            charactersWithoutLetter.AddRange(characters);
            charactersWithoutLetter.Remove(letter);

            RecurseASFC(charactersWithoutLetter, toReturn, alreadyTraversed, current + letter);
        }

        return toReturn;
    }

    bool UnorderedSequencesEqual(IEnumerable<char> a, IEnumerable<char> b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

        if (a.Count() != b.Count()) return false;

        // doesn't test duplicates correctly
        // return a.All(i => b.Contains(i));

        var newList = new List<char>();
        newList.AddRange(b);

        foreach (var value in a)
        {
            var index = newList.IndexOf(value);

            if (index < 0) return false;

            newList.RemoveAt(index);
        }

        return true;
    }

    
    
    // ['A', 'B', 'C']
    // ['B', 'C'] "A"
    // ['B'] "AC"
    // [] "ACB"
    // ['C'] "AB"
    // [] "ABC"
    // ['A', 'C'] "B"
    // ['A', 'B'] "C"

    public int SumOfAllNumbers(params int[] numbers)
    {
        return RecursiveSOAN(numbers.ToList());
    }

    public int RecursiveSOAN(List<int> numbers, int total = 0)
    {
        if (numbers.Count == 0) return total;

        total += numbers[0];
        numbers.RemoveAt(0);

        return RecursiveSOAN(numbers, total);
    }
    

    /*
     * Solve the following problem with recursion:
     *
     * A new soda company is doing a promotion - they'll buy back cans.  But they're not sure how much to charge per can,
     * or how much to pay out for cans.  Write a function that can determine how many cans someone can purchase for
     * a given amount of money, assuming they always return all the cans and then buy as much soda as they can.
     */

    public int TotalCansPurchasable(float money, float price, float refundForCan)
    {
        if (refundForCan >= price) return int.MaxValue;
        if (money < price) return 0;

        var purchasedCans = Mathf.FloorToInt(money / price);
        var change = money - purchasedCans * price;
        var returnFromRefund = purchasedCans * refundForCan;
        var totalFundsRemaining = change + returnFromRefund;
        
        return purchasedCans + TotalCansPurchasable(totalFundsRemaining, price, refundForCan);
    }
    
    // =========================== DON'T EDIT BELOW THIS LINE =========================== //

    public TextMeshProUGUI recursionTest, sodaTest;
    
    private void Update()
    {
        recursionTest.text =  "Recursion Problems\n<align=left>\n";
        recursionTest.text += Success(ReverseString("TEST") == "TSET") + " string reverser worked correctly.\n";
        recursionTest.text += Success(!IsPalindrome("TEST") && IsPalindrome("ASDFDSA") && IsPalindrome("ASDFFDSA")) + " palindrome test worked correctly.\n";
        recursionTest.text += Success(AllStringsFromCharacters('A', 'B').Length == 2 && AllStringsFromCharacters('A').Length == 1 && AllStringsFromCharacters('A', 'B', 'C').Length == 6) + " all strings test worked correctly.\n";
        recursionTest.text += Success(SumOfAllNumbers(1, 2, 3, 4, 5) == 15 && SumOfAllNumbers(1, 2, 3, 4, 5, 6, 7) == 28) + " sum test worked correctly.\n";

        sodaTest.text = "Soda Problem\n<align=left>\n";
        
        sodaTest.text += Success(TotalCansPurchasable(4, 2, 1) == 3) + " soda test works correctly w/out change.\n";
        sodaTest.text += Success(TotalCansPurchasable(5, 2, 1) == 4) + " soda test works correctly w/change.\n";
    }

    private string Success(bool test)
    {
        return test ? "<color=\"green\">PASS</color>" : "<color=\"red\">FAIL</color>";
    }
}