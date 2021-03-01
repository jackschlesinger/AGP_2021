using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class bArray
{
    public byte[] content;
}

public class ByteSerializer
{
    private static BinaryFormatter _bf = new BinaryFormatter ();

    private bArray _data;
    private bool _valid = true;
    
    public ByteSerializer(byte[] b)
    {
        _data = new bArray();
        
        _data.content = new byte[b.Length];
        b.CopyTo(_data.content, 0);
    }

    public ByteSerializer(string s)
    {
        _data = new bArray();
        
        var dataStream = new MemoryStream(System.Convert.FromBase64String(s));
        try
        {
            _data = (bArray) _bf.Deserialize(dataStream);
        }
        catch (SerializationException e)
        {
            Debug.Log("failed to deserialize data");
            _valid = false;
        }
    }

    public byte[] GetAsBytes()
    {
        if (!_valid) return null;
        
        return _data.content;
    }

    public string GetAsString()
    {
        if (!_valid) return "";
        
        var memoryStream = new MemoryStream ();
        _bf.Serialize (memoryStream, _data);
        return System.Convert.ToBase64String (memoryStream.ToArray ());
    }
}
