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
    bool isDispose = false;
     bool isBreakFlag = false;
    bool isConnectFlag = false;
    protected override void OnInit()
    {
        base.OnInit();
    }
    protected override void OnDispose()
    {
        isDispose = true;
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
        Debug.Log("关闭client");

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
            sendThread.Start();
            recvThread = new Thread(RecvMessage);
            recvThread.Start();
            connectStatus = 1;
        }
        catch (SocketException e)
        {
            Debug.LogError(e.Message);
        }
        return result;
    }

    public void SendMsg(byte[] data)
    {
       sendQueue.Enqueue(data);
    }
    public void TryConnect(string ip, int port)
    {
        this.iPAddress = IPAddress.Parse(ip);
        this.port = port;
        if (reConnectThread != null)
        {
            reConnectThread.Abort();
            reConnectThread = null;
        }
        reConnect = true;
        reConnectTime = 100;
        reConnectThread = new Thread(TryConnectThread);
        reConnectThread.Start();
    }
    void RecvMessage()
    {
        byte[] strbyte = new byte[2048];
        while (recvMsg)
        {
            try
            {
                int count = tcpSocket.Receive(strbyte);
                if (count > 0)
                {
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
            }
            catch (SocketException e)
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
            catch (SocketException e)
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
                Debug.Log("尝试连接");
                tcpSocket.Connect(iPAddress, port);
                recvMsg = true;
                sendMsg = true;
                sendThread = new Thread(SendMessage);
                sendThread.Start();
                recvThread = new Thread(RecvMessage);
                recvThread.Start();
                reConnect = false;
                connectStatus = 1;
                Debug.Log("连接服务器成功");

            }
            catch (SocketException e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }
    /// <summary>
    /// -1无需重连,1重连成功,2掉线
    /// </summary>
    /// <returns></returns>
    public int  GetConnectStatus()
    {
        if (isBreakFlag && connectStatus == 1)
        {
            isBreakFlag = false;
            return 1;
        }
        else if (isConnectFlag && connectStatus == 0)
        {
            isConnectFlag = false;
            return 2;
        }
        return -1;
    }
    /// <summary>
    /// 异常处理
    /// </summary>
    /// <param name="e"></param>
    void OnSocketException(Exception e)
    {
        if (isDispose)
            return;
        connectStatus = 0;
         Debug.LogError(e.Message);
        sendQueue.Clear();
        isBreakFlag = true;
        isConnectFlag = true;
        revceDataList.Clear();
        tcpSocket.Dispose();
        tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        recvMsg = false;
        sendMsg = false;
        if (recvThread != null)
        {
           // recvThread.Abort();
            recvThread = null;
        }
        if (sendThread != null)
        {
          //  sendThread.Abort();
            sendThread = null;
        }
        if (reConnectThread != null)
        {
           // reConnectThread.Abort();
            reConnectThread = null;
        }
        reConnect = true;
        reConnectTime = 100;
        reConnectThread = new Thread(TryConnectThread);
        reConnectThread.Start();
    }
}
