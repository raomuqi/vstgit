using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class Connection 
{
    bool autoConnect=false;
    UdpBase udpBase;
    int udpPort;
    int tcpPort;
    bool tcpConnected = false;
    bool isHose = false;
    string localIP;
    byte[] broadcastSelfData;
    bool broadcastSelf = false;
    int curPlayer = 0;
    int maxPlayer = 2;
    public void Init()
    {
        autoConnect = AppCfg.expose.AutoConnect;
        udpPort = int.Parse(AppCfg.expose.UdpPort);
        tcpPort= int.Parse(AppCfg.expose.TcpPort);
        isHose = AppCfg.expose.IsHost;
        curPlayer = 0;
        maxPlayer = AppCfg.expose.MaxPlayer;
        //作为主机
        if (autoConnect == true && isHose == true)
        {
            //获取本机内网IP
            string hostname = Dns.GetHostName();//
            IPHostEntry localhost = Dns.GetHostEntry(hostname);
            IPAddress localaddr = localhost.AddressList[0];
            localIP = localaddr.ToString();
            udpBase = new UdpBase(udpPort,false);
            broadcastSelfData = Encoding.UTF8.GetBytes(localIP.ToString() + "|" + SystemInfo.deviceUniqueIdentifier);
            SetUpTcp();
            broadcastSelf = true;
        }
        else if(autoConnect == false && isHose == true)
        {
            SetUpTcp();
        }

    }
    public void OnUpdate()
    {
        if (broadcastSelf == true)
        {
            udpBase.BroadCast(broadcastSelfData);
        }
    }

    void SetUpTcp()
    {

    }
}
