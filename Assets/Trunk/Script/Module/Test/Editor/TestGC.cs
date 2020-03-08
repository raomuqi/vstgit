using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;
public static class TestGC 
{
    [MenuItem("Test/Proto")]
    public static void TestProto()
    {
        ProtoIntArray tt = new ProtoIntArray();
        tt.context = new int[5] { 2, 1,4,100,5656 };
        byte[] b = tt.Serialize();
        tt.Parse(b);
        Debug.Log(tt.context[0]);
        Debug.Log(tt.context[1]);
        Debug.Log(tt.context[2]);
        Debug.Log(tt.context[3]);
        Debug.Log(tt.context[4]);
    }


    [MenuItem("Test/Pos")]
    public static void TestPos()
    {
      
    }
 
}
