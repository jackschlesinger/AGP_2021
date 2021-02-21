using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class Week4 : MonoBehaviour
{
    /*
     * Create a function that helps takes in four bytes (a, b, c, d), and returns an integer that represents those four
     * bytes in order (abcd).
     *
     * For example:
     *     - If you got in (1, 1, 1, 1), you would return 00000001 00000001 00000001 00000001, or 16843009.
     *     - (2, 1, 1, 1)     =>     00000010 00000001 00000001 00000001     =>    33620225 
     *     - (0, 0, 1, 0)     =>     00000000 00000000 00000001 00000000     =>    256
     * 
     */

    private int BytesToInt(byte a, byte b, byte c, byte d)
    {
        return 0;
    }

    private int PowerOfTwo(int power)
    {
        return 0;
    }

    /*
     * Define two functions - one that finds the smallest prime factor of a number (SmallestPrimeFactor()), and one that
     * returns the number of digits (NumberOfDigits()).  Then, write an changing function (ChangingFunction) that returns
     * the result of NumberOfDigits, unless the answer is three, and then after will always return the smallest prime factor.
     *
     * Use the function "Initialize()" if you have anything that needs to be reset - treat it like a start function.
     *
     * Assume number of digits always gets a positive number.
     * 
     */

    delegate int MathFunction(int input);
    private MathFunction currentFunction;

    public int SmallestPrimeFactor(int input)
    {
        return 0;
    }

    public int NumberOfDigits(int input)
    {
        return 0;
    }

    // Imagine this is your "Start()" function
    public void Initialize()
    {
        
    }

    public int ChangingFunction(int input)
    {
        return 0;
    }
    
    
    // =========================== DON'T EDIT BELOW THIS LINE =========================== //

    public TextMeshProUGUI bytesToIntTest, delegateTest;

    private void Update()
    {
        bytesToIntTest.text = "Bytes to Int\n<align=left>\n";
        
        bytesToIntTest.text += Success(BytesToInt(0, 0, 1, 0) == 256) + " Correct for (0, 0, 1, 0).\n";
        bytesToIntTest.text += Success(BytesToInt(0, 42, 1, 0) == 2752768) + " Correct for (0, 42, 1, 0).\n";
        bytesToIntTest.text += Success(BytesToInt(1, 1, 1, 1) == 16843009) + " Correct for (1, 1, 1, 1).\n";
        bytesToIntTest.text += Success(BytesToInt(2, 1, 1, 1) == 33620225) + " Correct for (2, 1, 1, 1).\n";
        
        delegateTest.text = "Delegate Test\n<align=left>\n";

        delegateTest.text +=
            Success(NumberOfDigits(10) == 2 && NumberOfDigits(4431) == 4 && NumberOfDigits(123842) == 6 &&
                    NumberOfDigits(100) == 3) + " Number of digits works.\n";
        delegateTest.text +=
            Success(SmallestPrimeFactor(4) == 2 && SmallestPrimeFactor(17) == 17 && SmallestPrimeFactor(95) == 5) +
            " Smallest prime number is correct.\n";
        
        Initialize();
        delegateTest.text +=
            Success(ChangingFunction(12) == 2 && ChangingFunction(39243) == 5 &&
                    ChangingFunction(2313) == 4 && ChangingFunction(333) == 3 && ChangingFunction(5) == 5) + " Changes when input is 3.\n";

        delegateTest.text += Success(ChangingFunction(9) == 3 && ChangingFunction(4) == 2) + " Doesn't change back when the input is 3 again.";

    }

    private string Success(bool test)
    {
        return test ? "<color=\"green\">PASS</color>" : "<color=\"red\">FAIL</color>";
    }
}