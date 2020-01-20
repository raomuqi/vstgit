using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class Connection 
{
    static Connection instance = null;
    public static Connection GetInstance()
    {
        if (instance == null)
        {
            instance = new Connection();
        }
        return instance;
    }
    /// <summary>
    /// 开启自动连接
    /// </summary>
    public bool autoConnect =false;
 
    /// <summary>
    /// 用于大量数据同步
    /// </summary>
    UdpBase multidataConnection;
    /// <summary>
    /// 从机Socket
    /// </summary>
    TcpClient client;
    //UDP端口
    int udpPort;
 
    //TCP端口
    int tcpPort;
    //是否主机
    bool isHost = false;
    //IP广播端口
    int broadCastPort = 7887;
    //接受广播IP
    bool recvBroadCastIP = false;
    //内网组
    string netGroup ="-";

    // 包头
    public const byte PACKER_HEAD = 255;
    // 包头
    public const byte PACKER_OFFSET = 5;
    //事件库
    NotiLib<byte> eventLib;
    /// <summary>
    /// 服务器
    /// </summary>
     GameServer server;
    /// <summary>
    /// 用于主机同步内网IP的Socket
    /// </summary>
    UdpBase syncIpConnection;
    System.Action onReConnectSuccesss;
    System.Action onDisConnect;
    public void Init()
    {
        if (int.TryParse(AppCfg.expose.UdpPort, out udpPort) == false)
            Debug.LogError("UDP端口配置错误");
        if (int.TryParse(AppCfg.expose.TcpPort, out tcpPort) == false)
            Debug.LogError("Tcp端口配置错误");
    
        eventLib = new NotiLib<byte>();
        autoConnect = AppCfg.expose.AutoConnect;
        netGroup = AppCfg.expose.NetGroup;
        isHost = AppCfg.expose.IsHost;
  
        multidataConnection = new UdpBase(udpPort);
        //作为主机
        if (isHost)
        {
            server = new GameServer(autoConnect,tcpPort, broadCastPort, netGroup);
        }
        if (autoConnect)
        {
            if (isHost)
            {
                ConenctTcp("127.0.0.1");
            }
            else
            {
                recvBroadCastIP = true;
                syncIpConnection = new UdpBase(broadCastPort);
            }
        }
        else
        {
            ConenctTcp(AppCfg.expose.IpAddress);
        }
    }

    public void OnUpdate()
    {
        //同步主机IP
        UpdateSyncIpConnect();

        if (isHost)
            server.OnUpdate();

        HandleMutiDataMsg();
        HandleClientDataMsg();
        HandleConnectStatus();
    }
 
    /// <summary>
    /// 发送数据
    /// </summary>
    public void SendData(byte protoID,object obj, ProtoType msgType)
    {
        byte[] data= Util.SerializeProtoData(protoID, obj);
        switch (msgType)
        {
            case ProtoType.Importance:
                 client.SendMsg(data);
            break;
            case ProtoType.Unimportance:
                multidataConnection.BroadCast(data);
              //  DeserializeMsg(data);
            break;
        }
    }
 

    /// <summary>
    /// 解析数据
    /// </summary>
    /// <param name="data"></param>
    public void DeserializeMsg(byte[] data)
    {
        if (data != null && data.Length>PACKER_OFFSET)
        {
            byte head = data[0];
            if (head == PACKER_HEAD)
            {
                byte protoID = data[1];
                int offset = PACKER_OFFSET;
                 object proto = SerializeUtil.Deserialize(data, offset, data.Length - offset);
                FireEvent(protoID, proto);
            }
        }
    }
    /// <summary>
    /// 处理重连 断线
    /// </summary>
    void HandleConnectStatus()
    {
        int connectStatus = client.GetConnectStatus();
        if (connectStatus == 2)
        {
            if (onDisConnect != null)
                onDisConnect();
        }
        else if (connectStatus == 1)
        {
            if (onReConnectSuccesss != null)
                onReConnectSuccesss();
        }
    }
    /// <summary>
    /// 处理多数据消息
    /// </summary>
    void HandleMutiDataMsg()
    {
        byte[] data = multidataConnection.GetMsg();
            DeserializeMsg(data);
    }
    /// <summary>
    /// 客户端处理消息
    /// </summary>
    void HandleClientDataMsg()
    {
        byte[] data = client.GetRecv();
        DeserializeMsg(data);
    }
 
    /// <summary>
    /// 连接主机
    /// </summary>
    void ConenctTcp(string ip)
    {
        client = new TcpClient();
        client.TryConnect(ip, tcpPort);
    }
    //同步IP
    void UpdateSyncIpConnect()
    {
        byte[] ipData = null;
        if (syncIpConnection != null)
        {
            ipData = syncIpConnection.GetMsg();
        }
        //接受同步IP
        if (recvBroadCastIP == true)
        {
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
                        recvBroadCastIP = false;
                        ConenctTcp(ip);
                    }
                }
            }
        }
    }
    /// <summary>
    /// 添加监听
    /// </summary>
    public void AddLisener(byte protoID, EventCallBack cb)
    {
        eventLib.AddEvent(protoID, cb);
    }
    /// <summary>
    /// 移除监听
    /// </summary>
    public void RemoveLisener(byte protoID, EventCallBack cb)
    {
        eventLib.RemoveEvent(protoID, cb);
    }
    /// <summary>
    /// 清除所有监听
    /// </summary>
    public void ClearLisener()
    {
        eventLib.Clear();
    }
    /// <summary>
    /// 通知逻辑层
    /// </summary>
    void FireEvent(byte protoID, object proto)
    {
        EventObjectArgs arg = new EventObjectArgs();
        arg.t = proto;
        eventLib.FireEvent(protoID, arg);
    }
    public void DisposeClient()
    {
        if (client != null)
            client.Dispose();
    }
    public void Close()
    {
        if (client != null)
            client.Dispose();
        if (server != null)
            server.Close();
        if (multidataConnection != null)
            multidataConnection.Dispose();
        if (syncIpConnection != null)
            syncIpConnection.Dispose();

    }
 
}
