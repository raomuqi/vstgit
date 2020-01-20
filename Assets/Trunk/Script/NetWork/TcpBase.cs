
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public abstract class TcpBase 
{
    protected Socket tcpSocket;
    protected Queue<byte[]> revceDataList;
    public int connectStatus = 0;

    public TcpBase()
    {
        tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        OnInit();
    }
    protected IPAddress iPAddress;
    protected int port;
    protected virtual void OnInit()
    {
        revceDataList = new Queue<byte[]>(2048);
    }
    protected virtual void OnDispose() { }
    public void Dispose()
    {
        OnDispose();
        if (tcpSocket != null)
        {
            tcpSocket.Close();
            tcpSocket = null;
        }
        connectStatus = 0;
    }

    public virtual byte[] GetRecv()
    {
        byte[] result = null;
        if (revceDataList.Count > 0)
        {
            result = revceDataList.Dequeue();
        }
        return result;
    }


  

}
