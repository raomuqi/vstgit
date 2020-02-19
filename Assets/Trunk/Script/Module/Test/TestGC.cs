using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class TestGC : MonoBehaviour
    
{
    byte[] temp;
     BinaryFormatter bf;
     MemoryStream memory;
    ProtoInt pw = new ProtoInt();
    byte[] src;

    // Start is called before the first frame update
    void Awake()
    {
       var udp = new UdpClient(1222);
        var udp2 = new UdpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1222));
    }
    SyncObject k;
    void Update()
    {
     //   src = k.Serialize();

    }

    public  object Deserialize(byte[] data, int index, int length)
    {
        if (bf == null)
            bf = new BinaryFormatter();
        if (memory == null)
            memory = new MemoryStream();
        memory.Flush();
        // memory.Position = 0;
        memory.Write(data, index, length);
        memory.Position = 0;
        object obj = null;
        try
        {
            obj = bf.Deserialize(memory);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return obj;
    }
}
