using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class UdpBase  {
    public class UdpRecver
    {
        public int index = 0;
        public IPEndPoint ip;
        public UdpRecver(int index, string ip,int port)
        {
            this.index = index;
            this.ip = new IPEndPoint(IPAddress.Parse(ip), port);
        }
    }
    private UdpClient udp;
    private bool isConnect = true;
    private IPEndPoint recvEndPoint = null;
     Queue<byte[]> recvMsgs=new Queue<byte[]>();
    private Thread recvThread;
    IPEndPoint broadCastIP;
    List<UdpRecver> recverList;
    int connectPort = -1;
    bool canRecv = true;
    string name;
    // Use this for initialization
    public UdpBase(int port, string name, bool canRecv=true)
    {
        try
        {
            this.name = name;
            connectPort = port;
            IPEndPoint broadcastIP = new IPEndPoint(IPAddress.Broadcast, port);
            udp = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            this.canRecv = canRecv;
            if (canRecv)
            {
                recvThread = new Thread(Listen);
                recvThread.Name = "recv_" + this.name;
                recvThread.Start();
            }
        }  
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public byte[] GetMsg()
    {
        if (recvMsgs.Count > 0)
        {
            return recvMsgs.Dequeue();
        }
        return null;

    }
    public int GetRecverID(string ip)
    {
        if (recverList == null)
            recverList = new List<UdpRecver>();
        recverList.Add(new UdpRecver(recverList.Count, ip,connectPort));
        return recverList.Count - 1;
    }
    /// <summary>
    /// 重连
    /// </summary>
    /// <param name="port"></param>
    public void Reset(int port, bool canRecv = true)
    {
        connectPort = port;
        Dispose();
        udp = new UdpClient(new IPEndPoint(IPAddress.Any, port));
        this.canRecv = canRecv;
        if (canRecv)
        {
            recvThread = new Thread(Listen);
            recvThread.Name = "recv_" + this.name;
            recvThread.Start();
        }
    }

    void Listen()
    {
        isConnect = true;
        while (isConnect)
        {
            try
            {
                byte[] recvdata = udp.Receive(ref recvEndPoint);
                if (recvMsgs.Count >= 2048)
                {
                    recvMsgs.Dequeue();
                }
                recvMsgs.Enqueue(recvdata);
            }
            catch (Exception e)
            {

            }

        }
    }
    public void BroadCast(byte[] data)
    {
        if (broadCastIP == null)
        {
            broadCastIP = new IPEndPoint(IPAddress.Broadcast, connectPort);
        }
        if (udp != null)
        {
            udp.Send(data, data.Length, broadCastIP);
        }
    }

    /// <summary>
    /// 发送数据
    /// 先用GetRecverID获得ID
    /// </summary>
    public void SendTo(byte[] data, int recverID)
    {
        if (udp != null)
        {
            udp.Send(data, data.Length, recverList[recverID].ip);
        }
    }
    /// <summary>
    /// 关闭连接释放资源
    /// </summary>
    public void Dispose()
    {
        isConnect = false;
        try
        {
             if (udp != null)
             {
                 udp.Close();
                 udp = null;
             }
             if (recvThread != null)
             {
                 recvThread.Abort();
             }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
