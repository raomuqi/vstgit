
using System.Collections.Generic;


public abstract class BaseNetHandler
{
    NotiLib<byte> sendEvets;
    public void SendProto(byte cmd, EventArgs args)
    {
        if (sendEvets != null)
            sendEvets.FireEvent(cmd, args);
    }
    /// <summary>
    /// 发送Proto
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="cb"></param>
    protected void RegisterSendProto(byte cmd, EventCallBack cb)
    {
        if (sendEvets == null)
            sendEvets = new NotiLib<byte>();
        sendEvets.AddEvent(cmd, cb);

    }
    /// <summary>
    /// 监听Proto
    /// </summary>
    /// <param name="protoID"></param>
    /// <param name="cb"></param>
    protected void RegisterListenProto(byte protoID, ProtoCallBack cb)
    {
        Connection.GetInstance().AddLisener(protoID, cb);
    }


    protected void RemoveSendProto(byte cmd, EventCallBack cb)
    {
        if (sendEvets != null)
            sendEvets.RemoveEvent(cmd, cb);
    }
    protected void Send(byte protoID, ProtoBase obj, ProtoType msgType)
    {
        Connection.GetInstance().SendData(protoID, obj, msgType);
    }

    protected void Send(byte protoID, byte[] obj, ProtoType msgType)
    {
        Connection.GetInstance().SendBytes(protoID, obj, msgType);
    }
    public void Init()
    {
        OnInit();
    }
    public void Clear()
    {
        sendEvets = null;
        OnClear();
    }

    protected virtual void OnInit() { }
    protected virtual void OnClear() { }
}
