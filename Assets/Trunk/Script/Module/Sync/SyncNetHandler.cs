using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncNetHandler : BaseNetHandler
{
    SyncModel model;
    ProtoUdpWarp udpWarp;
#if UNITY_EDITOR
    UnityEngine.Profiling.CustomSampler updateSyncSampler;
    UnityEngine.Profiling.CustomSampler uploadSyncSampler;
#endif
    protected override void OnInit()
    {

        udpWarp = new ProtoUdpWarp();
#if UNITY_EDITOR
        updateSyncSampler = UnityEngine.Profiling.CustomSampler.Create("OnSyncObjectList");
        uploadSyncSampler= UnityEngine.Profiling.CustomSampler.Create("SendSyncObjectList");
#endif
        model = SyncController.instance.GetModel<SyncModel>(SyncModel.name);
        if (model.IsUploader())
        {
            Global.instance.AddUpdateFunction(SendSyncObjectList);
        }
        else
            RegisterListenProto(ProtoIDCfg.SYNC_OBJECTS, OnSyncObjectList);
    }
    protected override void OnClear()
    {
        Global.instance.RemoveUpdateFunction(SendSyncObjectList);
    }
    /// <summary>
    /// 发送同步数据
    /// </summary>
    void SendSyncObjectList()
    {
#if UNITY_EDITOR
        uploadSyncSampler.Begin();
#endif
        if (model.syncList.Count > 0)
        {
            udpWarp.objList = new SyncObject[model.syncList.Count];
            for (int i = 0; i < model.syncList.Count; i++)
            {
                udpWarp.objList[i] = model.syncList[i];
            }
            Send(ProtoIDCfg.SYNC_OBJECTS, udpWarp, ProtoType.Unimportance);
        }
#if UNITY_EDITOR
        uploadSyncSampler.End();
#endif

    }
    void OnSyncObjectList(byte[] protoObj)
    {
#if UNITY_EDITOR
        updateSyncSampler.Begin();
#endif
         Util.DeSerializeUDPProto(protoObj,ref udpWarp);
#if UNITY_EDITOR
        updateSyncSampler.End();
#endif
        if (udpWarp != null)
        {
            SyncObject[] protoList = udpWarp.objList;
            model.UpdateSyncData(protoList);
        }

    }
}
