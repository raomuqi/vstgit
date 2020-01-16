using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class Connection 
{
    /// <summary>
    /// 开启自动连接
    /// </summary>
    bool autoConnect =false;
    /// <summary>
    /// 用于主机同步内网IP的Socket
    /// </summary>
    UdpBase syncIpConnection;
    /// <summary>
    /// 用于大量数据同步
    /// </summary>
    UdpBase multidataConnection;
    TcpHost host;
    TcpClient client;
    int udpPort;

    int broadCastPort= 7887;
    int tcpPort;
    bool isHose = false;
    string localIP;
    byte[] broadcastSelfData;
    bool broadcastSelf = false;
    int curPlayer = 0;
    int maxPlayer = 2;
    string netGroup ="-";
    int broadCastIntervalRate = 300;
    int curBroadCastFrame = 0;
    public void Init()
    {
        if (int.TryParse(AppCfg.expose.UdpPort, out udpPort) == false)
        {
            Debug.LogError("UDP端口配置错误");
        };
        if (int.TryParse(AppCfg.expose.TcpPort, out tcpPort) == false)
        {
            Debug.LogError("Tcp端口配置错误");
        };
        autoConnect = AppCfg.expose.AutoConnect;
        netGroup = AppCfg.expose.NetGroup;
        isHose = AppCfg.expose.IsHost;
        curPlayer = 0;
        maxPlayer = AppCfg.expose.MaxPlayer;
        //作为主机
        if (isHose)
        {
            if (autoConnect)
            {
                string hostname = Dns.GetHostName();//
                IPHostEntry localhost = Dns.GetHostEntry(hostname);
                IPAddress localaddr = localhost.AddressList[0];
                localIP = localaddr.ToString();
                syncIpConnection = new UdpBase(broadCastPort, false);
                multidataConnection = new UdpBase(udpPort);
                broadcastSelfData = Encoding.UTF8.GetBytes(Application.version + "|" + localIP.ToString()+"|"+ netGroup);
                broadcastSelf = true;
            }

            SetUpTcp();
        }
        else  //从机
        {
            if (autoConnect)
            {
                syncIpConnection = new UdpBase(broadCastPort);
            }
            else
            {
                ConenctTcp(AppCfg.expose.IpAddress);
            }

        }

    }
    public void OnUpdate()
    {
        if (syncIpConnection != null)
        {
            if (broadcastSelf == true  )
            {
                //同步主机IP到内网
                BroadCastHostIp();
            }
            // 从机等待主机IP信号
            WaitSyncHostIp();
        }

        HandleMutiDataMsg();
        if (isHose)
        {
            HandleHostDataMsg();
        }
        else
        {
            HandleClientDataMsg();
        }
    }


    void HandleMutiDataMsg()
    {
        byte[] data = multidataConnection.GetMsg();
        if (data != null)
        {


        }
    }
    void HandleHostDataMsg()
    {
        byte[] data = host.GetRecv();
        if (data != null)
        {


        }
    }
    void HandleClientDataMsg()
    {
        byte[] data = host.GetRecv();
        if (data != null)
        {


        }
    }
    void BroadCastHostIp()
    {
        int broadCastInterval = broadCastIntervalRate * (curPlayer + 1);
        if (curBroadCastFrame >= broadCastInterval)
        {
            curBroadCastFrame = 0;
            syncIpConnection.BroadCast(broadcastSelfData);
        }
        else
        {
            curBroadCastFrame++;
        }

    }
    void WaitSyncHostIp()
    {
       
        byte[] ipData = syncIpConnection.GetMsg();
        if (ipData != null)
        {
            string recvIpString = Encoding.UTF8.GetString(ipData);
            string[] strSplit = recvIpString.Split('|');
            if (strSplit.Length == 3)
            {
                string version = strSplit[0];
                string ip = strSplit[1];
                string group = strSplit[2];
                if (version == Application.version && netGroup == group)
                {
                    ConenctTcp(ip);
                }
            }
        }

    }

    /// <summary>
    /// 连接主机
    /// </summary>
    void ConenctTcp(string ip)
    {
        client = new TcpClient();
        if (client.Connect(ip, tcpPort))
        {

        }
    }
    /// <summary>
    /// 建立主机
    /// </summary>
    void SetUpTcp()
    {
        host = new TcpHost();
        host.SetHost(tcpPort, maxPlayer);
    }
}
