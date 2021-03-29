using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PrefixTrieExample : MonoBehaviour
{
    public TextMeshProUGUI display;
    public TMP_InputField input;

    public TextAsset wordlist;

    private Node root;

    private void Awake()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        root = CreateTree();
        
        stopwatch.Stop();
        Debug.Log("Time creating prefix trie: " + stopwatch.Elapsed.ToString("g"));
    }
    
    private void Update()
    {
        if (string.IsNullOrWhiteSpace(input.text))
        {
            display.text = "Enter text to see if it's a word in English.";
        }
        else
        {
            display.text = root.IsWord(input.text) ? "<color=green>Valid" : "<color=red>Invalid";
        }
    }

    private Node CreateTree()
    {
        var toReturn = new Node();

        foreach (var word in wordlist.text.Split('\n'))
        {
            if (string.IsNullOrEmpty(word)) continue;

            toReturn.AddWord(word);
        }

        return toReturn;
    }
}

public class Node
{
    private List<Node> children = new List<Node>();
    private char letter;
    private bool endsValidWord;

    public void AddWord(string word)
    {
        if (word.Length == 0)
        {
            endsValidWord = true;
            return;
        }

        GetOrAddChild(word[0]).AddWord(word.Substring(1, word.Length - 1));
    }

    public bool HasChild(char letterToCheck)
    {
        /*
        foreach (var n in children)
        {
            if (n.letter == char.ToUpper(letterToCheck))
                return true;
        }

        return false;
        */
        
        return children.Any(n => n.letter == char.ToUpper(letterToCheck));
    }

    public bool AllChildrenAreValidEndOfWord()
    {
        /*
        foreach (var n in children)
        {
            if (!n.endsValidWord)
                return false;
        }

        return true;
        */
        
        return children.All(n => n.endsValidWord);
    }

    public Node GetChild(char letterToGet)
    {
        // if (!HasChild(letterToGet)) return null;
        
        return children.FirstOrDefault(n => n.letter == char.ToUpper(letterToGet));
    }

    public Node GetOrAddChild(char letterToAdd, bool isEndOfWord = false)
    {
        if (HasChild(letterToAdd))
        {
            return GetChild(letterToAdd);
        }

        /*
        var toAdd = new Node();
        toAdd.letter = char.ToUpper(letterToAdd);
        endsValidWord = isEndOfWord;
        */
        
        var toAdd = new Node { letter = char.ToUpper(letterToAdd), endsValidWord = isEndOfWord };
        
        children.Add(toAdd);

        return toAdd;
    }
    
    public bool IsWord(string word)
    {
        if (word.Length == 0)
        {
            return endsValidWord;
        }

        if (!HasChild(word[0])) 
            return false;

        return GetChild(word[0]).IsWord(word.Substring(1, word.Length - 1));
    }
}

