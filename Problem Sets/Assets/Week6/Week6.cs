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
public class Week6 : MonoBehaviour
{
    /*
     * I've written a server that will serve you a high score list as a JSON object.  Create the functions
     * that will give you the highest scoring name, and the number of people with a score above a score from
     * the JSON you get from the server.
     *
     * NOTE: This is the same problem as last week, but instead of getting the data from a file, you're getting
     * the data from a URL.
     *
     * I've included a utility class, so end your Coroutine with "yield return [VALUE];" and it'll get the value
     * you meant to return.  This is a super useful extension of Unity Coroutines so I would definitely recommend
     * you add check out that code!
     *
     * JSON.Parse(someJSONObjectAsString)["someKey"] will return either a string value, or a Dictionary of strings to
     * JSON objects.
     *
     * NOTES:
     *     - If it hangs, you may need to press play to make the coroutine run.
     *     - You may want to start by solving the homework with just the JSON file provided, then do internet connectivity
     */

    public IEnumerator NumberAboveScore(string URL, int score)
    {
        yield return 0;
    }
    
    public IEnumerator GetHighScoreName(string URL)
    {
        
        yield return "Name";
    }
    
    /*
     * The second problem set is making a function that can handle callbacks.  Imagine you're checking a master server
     * to see whether there's a game server available or not.  Make a coroutine that takes in a URL and two Actions.
     * If it receives the string "available", it invokes the Action for success, if it's not available, it invokes the
     * Action for failure.
     */

    public IEnumerator CheckServerAvailability(string URL, Action onSuccess, Action onFailure)
    {
        yield return null;
    }
    
    /*
     * The third problem set is making functions that return a lambda expression.
     *
     * Make a function that returns a lambda expression that returns a function that takes in a string and reverses it,
     * and a function that takes in a TextMeshProUGUI and adds a line about successfully adding a line to the text mesh pro.
     */

    public Func<string, string> StringReverser()
    {
        return s => "";
    }

    public Action<TextMeshProUGUI> AddSuccess()
    {
        return tmp => { };
    }
    
    // =========================== DON'T EDIT BELOW THIS LINE =========================== //

    public TextMeshProUGUI networkTest, callbackTest, lambdaTest;
    private Coroutine checkingScores, callbacks;
    
    private void Update()
    {
        if (checkingScores == null)
            checkingScores = StartCoroutine(CheckHighScores());

        if (callbacks == null)
        {
            callbacks = StartCoroutine(CheckCallbacks());
        }

        lambdaTest.text = "Lambdas\n<align=left>\n";
        lambdaTest.text += Success(StringReverser()("TEST") == "TSET") + " string reverser returned correctly.\n";
        AddSuccess()(lambdaTest);
    }

    private IEnumerator CheckCallbacks()
    {
        callbackTest.text = "Callbacks\n<align=left>\n";

        yield return CheckServerAvailability("http://example-server.herokuapp.com/available",
            () => { callbackTest.text += "<color=\"green\">PASS</color> \"onSuccess\" callback\n"; },
            () => { callbackTest.text += "<color=\"red\">FAIL</color> \"onSuccess\" callback\n"; });
        
        yield return CheckServerAvailability("http://example-server.herokuapp.com/notavailable",
            () => { callbackTest.text += "<color=\"red\">FAIL</color> \"onFailure\" callback\n"; },
            () => { callbackTest.text += "<color=\"green\">PASS</color> \"onFailure\" callback\n"; });
        
        yield return CheckServerAvailability("http://example-server.herokuapp.com/error",
            () => { callbackTest.text += "<color=\"red\">FAIL</color> Calls \"onFailure\" on error.\n"; },
            () => { callbackTest.text += "<color=\"green\">PASS</color> Calls \"onFailure\" on error.\n"; });
    }

    private IEnumerator CheckHighScores()
    {
        networkTest.text = "Network Data\n<align=left>\nConnecting To Internet.";
        var numberAboveScore = new CoroutineWithData(this, NumberAboveScore("https://example-server.herokuapp.com/exampleJSON", 10));
        yield return numberAboveScore.coroutine;
        
        var highScoreName = new CoroutineWithData(this, GetHighScoreName("https://example-server.herokuapp.com/exampleJSON"));
        yield return highScoreName.coroutine;
        
        networkTest.text = "Network Data\n<align=left>\n";
        networkTest.text += Success((int) numberAboveScore.result == 6) + " number above score worked correctly.\n";
        networkTest.text += Success((string) highScoreName.result == "GUW") + " get high score name worked correctly.\n";
    }

    private string Success(bool test)
    {
        return test ? "<color=\"green\">PASS</color>" : "<color=\"red\">FAIL</color>";
    }
    
    
    public class CoroutineWithData {
        public Coroutine coroutine { get; private set; }
        public object result;
        private IEnumerator target;
        public CoroutineWithData(MonoBehaviour owner, IEnumerator target) {
            this.target = target;
            this.coroutine = owner.StartCoroutine(Run());
        }
 
        private IEnumerator Run() {
            while(target.MoveNext()) {
                result = target.Current;
                yield return result;
            }
        }
    }
}