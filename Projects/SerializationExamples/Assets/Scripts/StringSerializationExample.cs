using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TMPro;
using UnityEngine;

public class StringSerializationExample : MonoBehaviour
{
    public TextMeshPro display;

    public interface ISaveable
    {
        int Version { get; }
        string GetString();
        object SetFromString(string textData);
        int NumberOfBytes();
    }
    
    public class LevelData : ISaveable {
        public Vector2Int[] walls;
        public Item[] items; // must also be [Serializable]
        public uint levelScore;
        public Player player;

        public int width, height;

        public int NumberOfBytes()
        {
            return 24 + 
                   walls.Length * 8 +
                   items.Aggregate(0, (current, item) => current + item.NumberOfBytes()) +
                   4 + player.NumberOfBytes() + 8;
        }
        
        public int Version => 1;

        public string GetString()
        {
            var ms = new MemoryStream(NumberOfBytes());
            var bw = new BinaryWriter(ms);
            
            bw.Write(Version);

            // Version 1 data
            bw.Write(walls.Length);
            foreach (var wall in walls)
            {
                bw.Write(wall.x);
                bw.Write(wall.y);
            }

            bw.Write(items.Length);

            foreach (var item in items)
            {
                bw.Write(item.GetString());
            }
            
            bw.Write(levelScore);
            
            bw.Write(player.GetString());

            bw.Write(width);
            bw.Write(height);

            var saveData = new ByteSerializer(ms.GetBuffer());

            bw.Close();
            ms.Close();

            return saveData.GetAsString();
        }

        public object SetFromString(string textData)
        {
            var dataLoader = new ByteSerializer(textData);
            var data = dataLoader.GetAsBytes();
            
            if (data == null || data.Length <= 1) return this;
            var ms = new MemoryStream(data);
            var br = new BinaryReader(ms);

            var version = br.ReadInt32();

            // Version 1 data
            walls = new Vector2Int[br.ReadInt32()];
            for (var i = 0; i < walls.Length; i++)
            {
                walls[i] = new Vector2Int(br.ReadInt32(), br.ReadInt32());
            }
            
            items = new Item[br.ReadInt32()];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = (Item) new Item().SetFromString(br.ReadString());
            }

            levelScore = br.ReadUInt32();
            
            player = (Player) new Player().SetFromString(br.ReadString());

            width = br.ReadInt32();
            height = br.ReadInt32();

            br.Close();
            ms.Close();

            return this;
        }
    }
    
    public class Item : ISaveable
    {
        public Item() { }
        
        public Item(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
        
        public string name;
        public string description;
        public Vector2Int location;
        
        // V2 Data
        public int[] statChanges;
        
        public int Version => 1;

        public string GetString()
        {
            var ms = new MemoryStream(NumberOfBytes());
            var bw = new BinaryWriter(ms);
            
            bw.Write(Version);

            // Version 1 data
            bw.Write(name);
            bw.Write(description);
            bw.Write(location.x);
            bw.Write(location.y);

            var saveData = new ByteSerializer(ms.GetBuffer());

            bw.Close();
            ms.Close();

            return saveData.GetAsString();
        }

        public object SetFromString(string textData)
        {
            var dataLoader = new ByteSerializer(textData);
            var data = dataLoader.GetAsBytes();
            
            if (data == null || data.Length <= 1) return this;
            var ms = new MemoryStream(data);
            var br = new BinaryReader(ms);

            var version = br.ReadInt32();

            name = br.ReadString();
            description = br.ReadString();
            location = new Vector2Int(br.ReadInt32(), br.ReadInt32());

            br.Close();
            ms.Close();

            return this;
        }

        public int NumberOfBytes()
        {
            return 24 + (name.Length + description.Length) * sizeof(char);
        }
    }

    public class Player : ISaveable
    {
        public Player() { }
        public Player(string name, Item[] inventory, Vector2Int location)
        {
            this.name = name;
            this.inventory = inventory;
            this.location = location;
        }
        
        public string name;
        public Item[] inventory;
        public Vector2Int location;

        public int Version => 1;

        public string GetString()
        {
            var ms = new MemoryStream(NumberOfBytes());
            var bw = new BinaryWriter(ms);
            
            bw.Write(Version);

            // Version 1 data
            bw.Write(name);
            
            bw.Write(inventory.Length);
            foreach (var item in inventory)
            {
                bw.Write(item.GetString());
            }

            bw.Write(location.x);
            bw.Write(location.y);

            var saveData = new ByteSerializer(ms.GetBuffer());

            bw.Close();
            ms.Close();

            return saveData.GetAsString();
        }

        public object SetFromString(string textData)
        {
            var dataLoader = new ByteSerializer(textData);
            var data = dataLoader.GetAsBytes();
            
            if (data == null || data.Length <= 1) return this;
            var ms = new MemoryStream(data);
            var br = new BinaryReader(ms);

            var version = br.ReadInt32();

            name = br.ReadString();
            
            inventory = new Item[br.ReadInt32()];

            for (var i = 0; i < inventory.Length; i++)
            {
                inventory[i] = (Item) new Item().SetFromString(br.ReadString());
            }

            location = new Vector2Int(br.ReadInt32(), br.ReadInt32());

            br.Close();
            ms.Close();

            return this;
        }

        public int NumberOfBytes()
        {
            return 36 + name.Length * sizeof(char) + inventory.Aggregate(0, (total, current) => total + current.NumberOfBytes());
        }
    }

    private static LevelData currentLevel;

    public void Awake()
    {
        currentLevel = new LevelData();
        currentLevel.walls = new[] {new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0), 
                                    new Vector2Int(0, 1), new Vector2Int(3, 1), 
                                    new Vector2Int(0, 2), new Vector2Int(3, 2), 
                                    new Vector2Int(0, 3), new Vector2Int(1, 3), new Vector2Int(2, 3), new Vector2Int(3, 3), };
        currentLevel.items = new[] {new Item("Sword of Truth", "Causes truth to be a sword")};
        currentLevel.items[0].location = new Vector2Int(2, 2);
        currentLevel.levelScore = 203;
        currentLevel.player = new Player("Geoffry", new []{new Item("Torch", "Causes light.") }, new Vector2Int(1, 1));
        currentLevel.width = 4;
        currentLevel.height = 4;
        
        WriteString("Level1.txt", currentLevel.GetString());
    }

    public void Start()
    {
        currentLevel = new LevelData();
        
        currentLevel.SetFromString(ReadTextFile("", "Level1.txt"));
        ShowLevelData();
    }

    private void WriteString(string fileName, string data)
    {
        using (var outputFile = new StreamWriter(Path.Combine(Application.dataPath, fileName)))
        {
            outputFile.Write(data);
        }
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

    public string ReadTextFile(string filePath, string fileName) {
        var fileReader = new StreamReader(Application.dataPath + filePath + "/" + fileName);
        var toReturn = "";
        using (fileReader)
        {
            string line;
            do
            {
                line = fileReader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    toReturn += line + '\n';
                }
            } while (line != null);

            fileReader.Close();
        }

        return toReturn;
    }
    
}
