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
public class Week5 : MonoBehaviour
{
    /*
     * Write a CSV parser - that takes in a CSV file of players and returns a list of those players as class objects.
     * To help you out, I've defined the player class below.  An example save file is in the folder "CSV Examples".
     *
     * NOTES:
     *     - the first row of the file has the column headings: don't include those!
     *     - location is tricky - because the location has a comma in it!!
     */

    private class Player
    {
        public enum Class : byte
        {
            Undefined = 0,
            Monk,
            Wizard,
            Druid,
            Thief,
            Sorcerer
        }
        
        public Class classType;
        public string name;
        public uint maxHealth;
        public int[] stats;
        public bool alive;
        public Vector2 location;
    }

    private List<Player> CSVParser(TextAsset toParse)
    {
        var toReturn = new List<Player>();

        var lines = toParse.text.Split('\n');

        for (var i = 1; i < lines.Length; i++)
        {
            var splitLine = lines[i].Split(',');
            if (splitLine.Length != 11)
            {
                Debug.LogWarning("You have an incorrect number of fields for " + splitLine);
                continue;
            }

            var toAdd = new Player();

            toAdd.name = splitLine[0];

            switch (splitLine[1].ToUpper().Trim(' '))
            {
                case "MONK":
                    toAdd.classType = Player.Class.Monk;
                    break;
                case "WIZARD":
                    toAdd.classType = Player.Class.Wizard;
                    break;
                case "DRUID":
                    toAdd.classType = Player.Class.Druid;
                    break;
                case "THIEF":
                    toAdd.classType = Player.Class.Thief;
                    break;
                case "SORCERER":
                    toAdd.classType = Player.Class.Sorcerer;
                    break;
            }

            uint value;
            if (uint.TryParse(splitLine[2], out value))
                toAdd.maxHealth = value;

            toAdd.stats = new[]
            {
                int.Parse(splitLine[3]),
                int.Parse(splitLine[4]),
                int.Parse(splitLine[5]),
                int.Parse(splitLine[6]),
                int.Parse(splitLine[7]),
            };

            toAdd.alive = splitLine[8].ToUpper() == "TRUE"
                          || splitLine[8] == "YES"
                          || splitLine[8] == "T";

            var x = float.Parse(splitLine[9].Remove(0, 1));
            var y = float.Parse(splitLine[10].Remove(splitLine[10].Length - 2, 2));

            for (var j = 0; j < splitLine[10].Length; j++)
            {
                Debug.Log(j + ": " + splitLine[10][j]);
            }
            
            toAdd.location = new Vector2(x, y);
            
            toReturn.Add(toAdd);
        }
        
        return toReturn;
    }

    /*
     * Provided is a high score list as a JSON file.  Create the functions that will find the highest scoring name, and
     * the number of people with a score above a score.
     *
     * I've included a library "SimpleJSON", which parses JSON into a dictionary of strings to strings or dictionaries
     * of strings.
     *
     * JSON.Parse(someJSONString)["someKey"] will return either a string value, or a Dictionary of strings to
     * JSON objects.
     */

    public int NumberAboveScore(TextAsset jsonFile, int score)
    {
        var parsed = JSON.Parse(jsonFile.text);

        var toReturn = 0;
        
        foreach (JSONNode item in parsed["highScores"])
        {
            if (item["score"] > score)
                toReturn++;
        }

        return toReturn;
    }

    public string GetHighScoreName(TextAsset jsonFile)
    {
        var parsed = JSON.Parse(jsonFile.text);

        if (parsed["highScores"].Count == 0) return "";

        var toReturn = parsed["highScores"][0];
        
        foreach (JSONNode item in parsed["highScores"])
        {
            if (item["score"] > toReturn["score"])
                toReturn = item;
        }

        return toReturn["player"];
    }
    
    // =========================== DON'T EDIT BELOW THIS LINE =========================== //

    public TextMeshProUGUI csvTest, networkTest;
    public TextAsset csvExample, jsonExample;
    private Coroutine checkingScores;
    
    private void Update()
    {
        csvTest.text = "CSV Parser\n<align=left>\n";

        var parsedPlayers1 = CSVParser(csvExample);

        if (parsedPlayers1.Count == 0)
        {
            csvTest.text += "Need to return some players.";
        }
        else
        {
            csvTest.text += Success(parsedPlayers1.Any(p => p.name == "Jeff") &&
                                    parsedPlayers1.Any(p => p.name == "Stonks")
                            ) + " read in player names correctly.\n";
            csvTest.text +=
                Success(parsedPlayers1.First(p => p.name == "Jeff").alive &&
                        !parsedPlayers1.First(p => p.name == "Stonks").alive) + " Correctly read 'alive'.\n";
            csvTest.text +=
                Success(parsedPlayers1.First(p => p.name == "Stonks").classType == Player.Class.Wizard &&
                        parsedPlayers1.First(p => p.name == "Twergle").classType == Player.Class.Thief) +
                " Correctly read player class.\n";
            csvTest.text +=
                Success(parsedPlayers1.First(p => p.name == "Fortune").location == new Vector2(12.322f, 42f)) +
                " Correctly read in location.\n";
            csvTest.text += Success(parsedPlayers1.First(p => p.name == "Jeff").maxHealth == 23) +
                            " Correctly read in max health.\n";
            csvTest.text +=
                Success(parsedPlayers1.First(p => p.name == "Fortune").location == new Vector2(12.322f, 42f)) +
                " Correctly read in location.\n";
        }
        
        networkTest.text = "JSON Data\n<align=left>\n";
        networkTest.text += Success(NumberAboveScore(jsonExample, 10) == 6) + " number above score worked correctly.\n";
        networkTest.text += Success(GetHighScoreName(jsonExample) == "GUW") + " get high score name worked correctly.\n";
    }
    
    private string Success(bool test)
    {
        return test ? "<color=\"green\">PASS</color>" : "<color=\"red\">FAIL</color>";
    }
}