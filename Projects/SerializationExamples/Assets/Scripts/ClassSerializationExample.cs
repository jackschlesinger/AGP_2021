using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

public class ClassSerializationExample : MonoBehaviour
{
    public TextMeshPro display;

    [Serializable]
    public struct SerializableVector2Int // Because "Vector2Int" isn't serializable
    {
        public SerializableVector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x, y;
    }
    
    [Serializable]
    public class LevelData
    {
        public string levelDescription;
        public SerializableVector2Int[] walls;
        public Item[] items; // must also be [Serializable]
        public uint levelScore;
        public Player player; // must also be [Serializable]

        public int width, height;
    }

    [Serializable]
    public class Item
    {
        public Item(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
        
        public string name;
        public string description;
        public SerializableVector2Int location;
        
        public int[] statChanges;
    }

    [Serializable]
    public class Player
    {
        public Player(string name, Item[] inventory, SerializableVector2Int location)
        {
            this.name = name;
            this.inventory = inventory;
            this.location = location;
        }
        
        public string name;
        public Item[] inventory;
        public SerializableVector2Int location;
    }

    private static LevelData currentLevel;

    public void Awake()
    {
        currentLevel = new LevelData();
        currentLevel.walls = new[] {new SerializableVector2Int(0, 0), new SerializableVector2Int(1, 0), new SerializableVector2Int(2, 0), new SerializableVector2Int(3, 0), 
                                    new SerializableVector2Int(0, 1), new SerializableVector2Int(3, 1), 
                                    new SerializableVector2Int(0, 2), new SerializableVector2Int(3, 2), 
                                    new SerializableVector2Int(0, 3), new SerializableVector2Int(1, 3), new SerializableVector2Int(2, 3), new SerializableVector2Int(3, 3), };
        currentLevel.items = new[] {new Item("Sword of Truth", "Causes truth to be a sword")};
        currentLevel.items[0].location = new SerializableVector2Int(2, 2);
        currentLevel.levelScore = 203;
        currentLevel.player = new Player("Geoffry", new []{new Item("Torch", "Causes light.") }, new SerializableVector2Int(1, 1));
        currentLevel.width = 4;
        currentLevel.height = 4;
        
        // WriteLevelData("Level0");
    }

    public void Start()
    {
        ReadLevelData("Level0");
        ShowLevelData();
    }

    public void ShowLevelData()
    {
        display.text = "";
        var representation = new char[currentLevel.width, currentLevel.height];
        
        for (var i = 0; i < currentLevel.width; i++)
        for (var j = 0; j < currentLevel.height; j++)
            representation[i, j] = ' ';
        
        foreach (var wall in currentLevel.walls)
        {
            Debug.Log("Adding (" + wall.x + ", " + wall.y + ")");
            representation[wall.x, wall.y] = '#';
        }

        var itemList = new List<string>();
        foreach (var item in currentLevel.items)
        {
            representation[item.location.x, item.location.y] = item.name[0];
            itemList.Add(item.name + ": " + item.description);
        }

        representation[currentLevel.player.location.x, currentLevel.player.location.y] = 'P';

        display.text += "Map:\n";
        
        for (var y = 0; y < currentLevel.height; y++)
        {
            for (var x = 0; x < currentLevel.width; x++)
            {
                display.text += representation[x, y];
            }

            display.text += "\n";
        }
        
        Debug.Log("got to here.");

        display.text += "\n\nItems:\n";

        foreach (var item in itemList)
        {
            display.text += item + "\n";
        }

        display.text += "\n\nPlayer: " + currentLevel.player.name;
    }

    public void WriteLevelData(string fileName) {
        var formatter = new BinaryFormatter();  
        var stream = new FileStream(fileName + ".bin", FileMode.Create, FileAccess.Write, FileShare.None);  
        formatter.Serialize(stream, currentLevel);  
        stream.Close();  
    }
    
    public void WriteObject<T>(string fileName, T toWrite) where T : ISerializable {
        IFormatter formatter = new BinaryFormatter();  
        Stream stream = new FileStream(fileName + ".bin", FileMode.Create, FileAccess.Write, FileShare.None);  
        formatter.Serialize(stream, toWrite);  
        stream.Close();  
    }

    public void ReadLevelData(string fileName)
    {
        var formatter = new BinaryFormatter();  
        var stream = new FileStream(fileName + ".bin", FileMode.Open, FileAccess.Read, FileShare.Read);  
        currentLevel = (LevelData) formatter.Deserialize(stream);  
        stream.Close();  
    }
    
    //  currentLevel = ReadData<LevelData>("FileName");
    
    public T ReadData<T>(string fileName) where T : ISerializable
    {
        var formatter = new BinaryFormatter();  
        var stream = new FileStream(fileName + ".bin", FileMode.Open, FileAccess.Read, FileShare.Read);  
        var toReturn = (T) formatter.Deserialize(stream);  
        stream.Close();

        return toReturn;
    }
    
}
