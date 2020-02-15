using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        // ProtoUdpWarp pl = new ProtoUdpWarp();
        // pl.objList = new List<SyncObject>();
         k = new SyncObject();
        k.objectID = 0;
        k.posX = 0;
        // pl.objList.Add(k);
        // SyncObject k2 = new SyncObject();
        // k2.objectID = 1;
        // k2.posZ = 1;
        // pl.objList.Add(k2);
        // SyncObject k3 = new SyncObject();
        // k3.objectID = 3;
        // k3.rotW = 3;
        // pl.objList.Add(k3);
        // ProtoBase dd = pl as ProtoBase;

       
        // Debug.Log(src.Length);        //    byte[] t= BitConverter.GetBytes()

        //np = new ProtoUdpWarp();
        //Debug.Log(np.objList[0].posX);
        //Debug.Log(np.objList[1].objectID);
        //Debug.Log(np.objList[1].posZ);
        //Debug.Log(np.objList[2].rotW);




    }
    SyncObject k;
    void Update()
    {
        src = k.Serialize();

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
