using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class GameServer
{
    /// <summary>
    /// 开启自动连接
    /// </summary>
    bool autoConnect = false;
    /// <summary>
    /// 用于主机同步内网IP的Socket
    /// </summary>
    UdpBase syncIpConnection;
    string localIP;
    //广播IP数据
    byte[] broadcastSelfData;
    //是否广播IP
    bool broadcastSelf = false;
    Connection connect;
    /// <summary>
    /// 主机Socket
    /// </summary>
    TcpHost host;
    //当前人数
    int curPlayer = 0;
    //最大人连接数
    int maxPlayer = 2;
    //IP广播间隙
    int broadCastIntervalRate = 500;
    //IP广播计数器
    int curBroadCastFrame = 0;
    /// <summary>
    /// 最大心跳时间
    /// </summary>
    float MAX_HEARTBEAT_TIME = Connection.HEART_BEAT_TIME * 2;

    public System.Action<int,int> onClientChange = null;

    public Dictionary<byte,ProtoPlayerInfo> playerInfos = new Dictionary<byte, ProtoPlayerInfo>();

    public GameServer(bool autoConnect, int port,int broadCastPort,string netGroup)
    {
        maxPlayer = AppCfg.expose.MaxPlayer;
        host = new TcpHost();
        curPlayer = 1;
        host.SetHost(port, maxPlayer);
        this.autoConnect = autoConnect;
        if (autoConnect)
        {
            string hostname = Dns.GetHostName();//
            IPHostEntry localhost = Dns.GetHostEntry(hostname);
            IPAddress localaddr = localhost.AddressList[0];
            localIP = localaddr.ToString();
            syncIpConnection = new UdpBase(broadCastPort,"SyncIP_Host", false);
            broadcastSelfData = Encoding.UTF8.GetBytes(Application.version + "|" + localIP.ToString() + "|" + netGroup);
            broadcastSelf = true;
        }
        Debug.Log("部署服务器成功");
    }
    public void OnUpdate()
    {
        //处理接收消息
        HandleHostDataMsg();
        //检测client状态变化
        UpdateClientStatus();

        //发送同步IP
        if (broadcastSelf == true)
        {
            int broadCastInterval = broadCastIntervalRate;
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
    }
    /// <summary>
    /// 处理Proto
    /// </summary>
    void ParseMsg(TcpHost.SocketAccept socket, byte protoID, object protoData)
    {
        switch (protoID)
        {
            case ProtoIDCfg.HEARTBEAT:
                socket.heartbeatTime =Time.time;
                socket.heartbeatStatus = 0;
                break;
            case ProtoIDCfg.LOGIN:
                byte id = (byte)socket.id;
                ProtoPlayerInfo p;
                if (playerInfos.TryGetValue((byte)id, out p))
                {
                    p.connectStatus = 1;
                    host.RemoveClient(id);
                }
                else
                {
                    p = new ProtoPlayerInfo();
                    p.hp = PlayerCfg.HP;
                    p.score = PlayerCfg.score;
                    p.connectStatus = 1;
                    p.id = id;
                    playerInfos.Add(id, p);
                }
                //返回角色数据
                SendMsg(socket.id, ProtoIDCfg.LOGIN, p);
                break;

        }
    }

    /// <summary>
    /// 用户加入
    /// </summary>
    void OnClientLogin(int clientID){}
    /// <summary>
    /// 用户登出
    /// </summary>
    void OnClientLogOut(int clientID)
    {
        ProtoPlayerInfo p;
        if (playerInfos.TryGetValue((byte)clientID, out p))
        {
            p.connectStatus = 2;
        }
    }
    /// <summary>
    /// 用户数改变
    /// </summary>
    void OnClientChange(int curCount)
    {
        if (curCount >= maxPlayer)
            broadcastSelf = false;
        else
            broadcastSelf = true;
        SyncPlayerList();
        
    }

   
    /// <summary>
    /// 主机处理消息
    /// </summary>
    void HandleHostDataMsg()
    {
       TcpHost.SocketAccept[] clients= host.GetClientList();
        for (int i = 0; i < clients.Length; i++)
        {
            TcpHost.SocketAccept client = clients[i];
            if (client != null)
            {
                 while (client.recvQueue.Count > 0)
                {
                    byte[] data = client.recvQueue.Dequeue();
                    if (data != null && data.Length > 0 && data[0]==Connection.PACKER_HEAD)
                    {
                        int offset =Connection.PACKER_OFFSET;
                        object proto = data.Length==offset?null:SerializeUtil.Deserialize(data, offset, data.Length - offset);
                        ParseMsg(client, data[1], proto);
                    }
                }
                 //心跳
                float heartbeatInterval = Time.time - client.heartbeatTime;
                if (heartbeatInterval > Connection.HEART_BEAT_TIME)
                {
                    if (client.heartbeatStatus > 0)
                    {
                        Debug.LogWarning(client.id + "心跳超时");
                        host.RemoveClient(client.id);
                    }
                    else
                    {
                        SendMsg(client.id, ProtoIDCfg.HEARTBEAT, null);
                        client.heartbeatStatus++;
                    }
                    client.heartbeatTime = Time.time;
                }
            }
        }
    }

    /// <summary>
    /// 同步玩家信息
    /// </summary>
    void SyncPlayerList()
    {
        ProtoPlayerList proto = new ProtoPlayerList();
        proto.players = new ProtoPlayerInfo[playerInfos.Count];
        foreach (var key in playerInfos)
        {
            proto.players[(int)key.Key] = key.Value;
        }
        Broadcast(ProtoIDCfg.S_PLAYERS, proto);
    }
    /// <summary>
    /// 更新用户状态
    /// </summary>
    void UpdateClientStatus()
    {
        curPlayer = host.GetClientCount();
        Queue<int> loginClient = host.GetLoginClient();
        bool updatePlayerInfo = false;
        while (loginClient.Count > 0)
        {
            OnClientLogin(loginClient.Dequeue());
            updatePlayerInfo = true;
        }
        Queue<int> logoutClient = host.GetLogOutClient();
        while (logoutClient.Count > 0)
        {
            OnClientLogOut(logoutClient.Dequeue());
            updatePlayerInfo = true;
        }
        if (updatePlayerInfo)
        {
            OnClientChange(curPlayer);
        }
    }

    public void Broadcast(byte protoID, object obj)
    {
        byte[] data = Util.SerializeProtoData(protoID, obj);
        host.SendMsg(data);
    }
    public void Broadcast(byte[] data)
    {
        host.SendMsg(data);
    }
    public void SendMsg(int clientID, byte protoID, object obj)
    {
        byte[] data = Util.SerializeProtoData(protoID, obj);
        host.SendMsg(clientID, data);
    }
    public void SendMsg(int clientID, byte[] data)
    {
        host.SendMsg(clientID, data);
    }
    public void Close()
    {
        Debug.Log("关闭服务器");
        host.Dispose();
        host = null;
        if (syncIpConnection != null)
            syncIpConnection.Dispose();
    }
}
