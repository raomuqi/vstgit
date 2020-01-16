using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class TcpClient : TcpBase
{
    bool recvMsg = false;
    bool sendMsg = false;
    bool reConnect = false;
    Thread recvThread;
    Thread sendThread;
    Thread reConnectThread;
    Queue<byte[]> sendQueue = new Queue<byte[]>();
    int reConnectTime;
    protected override void OnInit()
    {
        tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }
    protected override void OnDispose()
    {
       
        recvMsg = false;
         sendMsg = false;
        reConnect = false;
        if (recvThread != null)
        {
            recvThread.Abort();
            recvThread = null;
        }
        if (sendThread != null)
        {
            sendThread.Abort();
            sendThread = null;
        }
        if (reConnectThread != null)
        {
            reConnectThread.Abort();
            reConnectThread = null;
        }
    }
    public bool Connect(string ip, int port)
    {
        bool result = false;
        try
        {
            this.iPAddress = IPAddress.Parse(ip);
            this.port = port;
            tcpSocket.Connect(iPAddress, port);
            recvMsg = true;
            result = true;
            sendMsg = true;
            sendThread = new Thread(SendMessage);
            recvThread = new Thread(RecvMessage);
            connectStatus = 1;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
        return result;
    }

    public void SendMsg(byte[] data)
    {
       sendQueue.Enqueue(data);
    }
    public void TryConnect(string ip, int port)
    {
        if (reConnectThread == null)
        {
            reConnect = true;
            reConnectTime = 100;
            reConnectThread = new Thread(TryConnectThread);
        }
    }
    void RecvMessage()
    {
        byte[] strbyte = new byte[2048];
        while (recvMsg)
        {
            try
            {
                int count = tcpSocket.Receive(strbyte);
                byte[] result = new byte[count];
                Array.Copy(strbyte, result, count);
                if (revceDataList.Count < 2048)
                {
                    revceDataList.Enqueue(result);
                }
                else
                {
                    Debug.LogError("丢失数据包");
                }
            }
            catch (Exception e)
            {
                OnSocketException(e);
            }
        }

    }
    //client独立发送线程
    void SendMessage()
    {
        while (sendMsg)
        {
            try
            {
                if (sendQueue.Count > 0)
                {
                    tcpSocket.Send(sendQueue.Dequeue());
                }
            }
            catch (Exception e)
            {
                OnSocketException(e);

            }
            Thread.Sleep(10);
        }
    }
    //连接
    void TryConnectThread()
    {
        while (reConnect)
        {
            try
            {
                Thread.Sleep(reConnectTime);
                if (reConnectTime < 10000)
                {
                    reConnectTime = reConnectTime * 2;
                }
                tcpSocket.Connect(iPAddress, port);
                recvMsg = true;
                sendMsg = true;
                sendThread = new Thread(SendMessage);
                recvThread = new Thread(RecvMessage);
                reConnect = true;
                connectStatus = 1;

            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }
    /// <summary>
    /// 异常处理
    /// </summary>
    /// <param name="e"></param>
    void OnSocketException(Exception e)
    {
        connectStatus = 0;
         Debug.LogError(e.Message);
        sendQueue.Clear();
        revceDataList.Clear();
        tcpSocket.Dispose();
        tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        recvMsg = false;
        sendMsg = false;
        if (recvThread != null)
        {
            recvThread.Abort();
            recvThread = null;
        }
        if (sendThread != null)
        {
            sendThread.Abort();
            sendThread = null;
        }
        if (reConnectThread == null)
        {
            reConnect = true;
            reConnectTime = 100;
            reConnectThread = new Thread(TryConnectThread);
        }

    }
}
