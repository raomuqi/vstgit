using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class GameServer
{
    /// 开启自动连接
    bool autoConnect = false;
    /// 用于主机同步内网IP的Socket
    UdpBase syncIpConnection;
    string localIP;
    //广播IP数据
    byte[] broadcastSelfData;
    //是否广播IP
    bool broadcastSelf = false;
    //连接器
    Connection connect;
    /// 主机Socket
    TcpHost host;
    //当前人数
    int curPlayer = 0;
    //最大人连接数
    int maxPlayer = 2;
    //IP广播间隙
    int broadCastIntervalRate = 500;
    //IP广播计数器
    int curBroadCastFrame = 0;
    /// 最大心跳时间
    float MAX_HEARTBEAT_TIME = Connection.HEART_BEAT_TIME * 2;
    //部署时间
    float startUpTime = 0;
    //等待连接时间
    float waitConnectTime = 0;
    //进入场景时间
    float waitEnterSceneTime;
    //最大进入场景时间
    float maxEnterSceneTime;
    //地图ID
    int MapID = 0;
    //游戏状态
    GameStatus gameStatus = GameStatus.WaitConnect;
    public System.Action<int,int> onClientChange = null;

    public Dictionary<byte,ProtoPlayerInfo> playerInfos = new Dictionary<byte, ProtoPlayerInfo>();

    public Dictionary<int, SyncObject> sceneObjes = new Dictionary<int, SyncObject>();


    public GameServer(bool autoConnect, int port,int broadCastPort,string netGroup)
    {
        maxPlayer = AppCfg.expose.MaxPlayer;
        waitConnectTime = AppCfg.expose.WaitConnectTime;
        maxEnterSceneTime = ServerCfg.MaxEnterSceneTime;
        host = new TcpHost();
        curPlayer = 1;
        host.SetHost(port, maxPlayer);
       
        this.autoConnect = autoConnect;
        if (autoConnect)
        {
            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ipa in IpEntry.AddressList)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ipa.ToString();
                    break;
                }
            }
            syncIpConnection = new UdpBase(broadCastPort,"SyncIP_Host", false);
            broadcastSelfData = Encoding.UTF8.GetBytes(Application.version + "|" + localIP.ToString() + "|" + netGroup);
            broadcastSelf = true;
        }
        gameStatus = GameStatus.WaitConnect;
        startUpTime = Time.time;
        Debug.LogWarning("部署服务器成功");
    }

   
    public void OnUpdate()
    {
        switch (gameStatus)
        {
            case GameStatus.WaitConnect:
                CheckPlayerConnetTime();
                break;
            case GameStatus.EnterScene:
                CheckEnterScene();
                break;
            case GameStatus.Running:
                break;
        }

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
    void ParseMsg(TcpHost.SocketAccept socket, byte protoID, byte[] protoData,byte[] srcData)
    {
       
        switch (protoID)
        {
            //心跳
            case ProtoIDCfg.HEARTBEAT:
                socket.heartbeatTime =Time.time;
                socket.heartbeatStatus = 0;
                break;
            //登入
            case ProtoIDCfg.LOGIN:
                OnMsgLogin(socket,protoData);
                break;
            //进入场景
            case ProtoIDCfg.ENTER_SCENE:
                OnMsgEnterScene(socket, protoData);
              break;
             //创建对象
            case ProtoIDCfg.CREATE_OBJECTS:
                OnMsgCreateObject(socket, protoData);
                break;
            //同步输入
            case ProtoIDCfg.SYNC_INPUT:
                Broadcast(srcData);
                break;
        }
    }
    void OnMsgLogin(TcpHost.SocketAccept socket,  byte[] protoData)
    {
        ProtoPlayerInfo p;
        byte id = (byte)socket.id;
        if (playerInfos.TryGetValue((byte)id, out p))
        {
            p.connectStatus = 1;
            host.RemoveClient(id);
        }
        else
        {
            byte loginPos = protoData[0];
            bool[] inPosPlayers = new bool[10];
            bool canUsePos = true;
            foreach (var player in playerInfos)
            {
                if (player.Value.pos == loginPos)
                {
                    canUsePos = false;
                }
                inPosPlayers[player.Value.pos] = true;
            }
            if (!canUsePos)
            {
                for (int i = 1; i < inPosPlayers.Length; i++)
                {
                    if (inPosPlayers[i] == false)
                    {
                        loginPos = (byte)i;
                        break;
                    }
                }
            }
            p = new ProtoPlayerInfo();
            p.hp = PlayerCfg.HP;
            p.score = PlayerCfg.score;
            p.connectStatus = 1;
            p.id = id;
            p.pos = loginPos;

            playerInfos.Add(id, p);
            Debug.LogWarning("玩家加入" + id);
            SyncPlayerList();
        }
        //返回角色数据
        SendMsg(socket.id, ProtoIDCfg.LOGIN, p);

    }

    void OnMsgEnterScene(TcpHost.SocketAccept socket,  byte[] protoData)
    {
        ProtoPlayerInfo p;
        ProtoInt intP = new ProtoInt();
        intP.Parse(protoData);
        int playerMapID = intP.context;
        intP.Recycle();
        if (playerInfos.TryGetValue((byte)socket.id, out p))
        {
            p.mapID = playerMapID;
            Debug.LogWarning(socket.id + "进入场景完成");
        }
    }

    void OnMsgCreateObject(TcpHost.SocketAccept socket, byte[] protoData)
    {
        SyncObject syncObj = new SyncObject();
        if (syncObj.Parse(protoData))
        {
            CreateSceneObject(syncObj);
        }
    }
    /// <summary>
    /// 创建创建对象
    /// </summary>
    void CreateSceneObject(SyncObject obj)
    {
        int serverId = sceneObjes.Count;
        obj.serverID = serverId;
        if (!sceneObjes.ContainsKey(obj.serverID))
        {
            sceneObjes.Add(obj.serverID, obj);
            Broadcast(ProtoIDCfg.CREATE_OBJECTS, obj);
            Debug.LogWarning("创建对象：" + obj.objectIndex);
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
        SyncPlayerList();

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
        
    }
    ProtoPlayerList playerListProto;
    /// <summary>
    /// 同步玩家信息
    /// </summary>
    void SyncPlayerList()
    {
        if (playerInfos.Count > 0)
        {
            if (playerListProto == null)
            {
                playerListProto = new ProtoPlayerList();
            }
            playerListProto.players = new ProtoPlayerInfo[playerInfos.Count];
            foreach (var key in playerInfos)
            {
                playerListProto.players[(int)key.Key] = key.Value;
            }
            Broadcast(ProtoIDCfg.S_PLAYERS, playerListProto);
        }
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

    /// <summary>
    /// 检查准备时间，时间到了进入场景
    /// </summary>
    void CheckPlayerConnetTime()
    {
        if (MapID == 0 && Time.time - startUpTime > waitConnectTime && curPlayer > 0)
        {
            MapID = 1;
            ProtoInt proto = new ProtoInt();
            proto.context = MapID;
            Broadcast(ProtoIDCfg.ENTER_SCENE, proto);
            waitEnterSceneTime = Time.time;
            gameStatus = GameStatus.EnterScene;
        }
    }
    /// <summary>
    /// 检查开始游戏
    /// </summary>
    void CheckEnterScene()
    {
        int enterCount = 0;
        foreach (var player in playerInfos)
        {
            if (player.Value.mapID == MapID)
                enterCount++;
        }
        if (Time.time-waitEnterSceneTime >= maxEnterSceneTime  && enterCount>0)
        {
            Broadcast(ProtoIDCfg.S_STARTGAME,null);
            gameStatus = GameStatus.Running;
        }

    }
 

    #region 收发消息实现函数
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
                        byte[] proto = data.Length==offset?null:SerializeUtil.GetContextData(data, offset, data.Length - offset);
                        ParseMsg(client, data[1], proto,data);
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

    

    public void Broadcast(byte protoID, ProtoBase obj)
    {
        byte[] data = Util.SerializeProtoData(protoID, obj);
        if (data == null)
            return;
        host.SendMsg(data);
    }
    public void Broadcast(byte[] data)
    {
        host.SendMsg(data);
    }
    public void SendMsg(int clientID, byte protoID, ProtoBase obj)
    {
        byte[] data = Util.SerializeProtoData(protoID, obj);
        if (data == null)
            return;
        host.SendMsg(clientID, data);
    }
    public void SendMsg(int clientID, byte[] data)
    {
        host.SendMsg(clientID, data);
    }
    public void Close()
    {
        Debug.LogWarning("关闭服务器");
        host.Dispose();
        host = null;
        if (syncIpConnection != null)
            syncIpConnection.Dispose();
    }
    #endregion
}
