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
        ProtoActiveObjects a = new ProtoActiveObjects();
        a.objectIndexs = new int[4] { 8, 6, 4, 2 };
        a.serverIDs = new int[4] { 4, 3, 2, 1 };

        ProtoActiveObjects b = new ProtoActiveObjects();
        b.Parse(a.Serialize());
    }


    [MenuItem("Test/Pos")]
    public static void TestPos()
    {
      
    }
 
}
