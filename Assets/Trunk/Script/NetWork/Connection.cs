using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
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
    /// <summary>
    /// 主机Socket
    /// </summary>
    TcpHost host;
    /// <summary>
    /// 从机Socket
    /// </summary>
    TcpClient client;
    //UDP端口
    int udpPort;
    //IP广播端口
    int broadCastPort= 7887;
    //TCP端口
    int tcpPort;
    //是否主机
    bool isHose = false;
    string localIP;
    //广播IP数据
    byte[] broadcastSelfData;
    //是否广播IP
    bool broadcastSelf = false;
    //当前人数
    int curPlayer = 0;
    //最大人连接数
    int maxPlayer = 2;
    //内网组
    string netGroup ="-";
    //IP广播间隙
    int broadCastIntervalRate = 300;
    //IP广播计数器
    int curBroadCastFrame = 0;
    //序列化工具
    SerializeUtil serializeUtil;
    /// 包头
    const byte PACKER_HEAD = 255;



    public void Init()
    {
        serializeUtil = new SerializeUtil();
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
        multidataConnection = new UdpBase(udpPort);
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
                BroadCastHostIp();
            }
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
    public void SendData()
    {

    }
    //解析
     void DeserializeMsg(byte[] data)
    {
        if (data != null && data.Length>2)
        {
            byte head = data[0];
            if (head == PACKER_HEAD)
            {
                byte protoID = data[1];
                int offset = 2;
                 object proto = serializeUtil.Deserialize(data, offset, data.Length - offset);
            
            }
        }

    }
 
    void HandleMutiDataMsg()
    {
        byte[] data = multidataConnection.GetMsg();
            DeserializeMsg(data);
    }
    void HandleHostDataMsg()
    {
        byte[] data = host.GetRecv();
        DeserializeMsg(data);
    }
    void HandleClientDataMsg()
    {
        byte[] data = client.GetRecv();
        DeserializeMsg(data);
    }
    /// <summary>
    /// 同步主机IP到内网
    /// </summary>
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

    /// <summary>
    /// 从机等待主机IP信号
    /// </summary>
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
