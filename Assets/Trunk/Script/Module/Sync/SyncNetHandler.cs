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
#if UNITY_EDITOR
        updateSyncSampler = UnityEngine.Profiling.CustomSampler.Create("OnSyncObjectList");
        uploadSyncSampler= UnityEngine.Profiling.CustomSampler.Create("SendSyncObjectList");
#endif
        model = SyncController.instance.GetModel<SyncModel>(SyncModel.name);
        if (model.IsUploader())
        {
            udpWarp = new ProtoUdpWarp();
            Global.instance.AddUpdateFunction(SendSyncObjectList);
        }
        else
            RegisterListenProto(ProtoIDCfg.SYNC_OBJECTS, OnSyncObjectList);
    }
    protected override void OnClear()
    {
        Global.instance.RemoveUpdateFunction(SendSyncObjectList);
    }

    void SendSyncObjectList()
    {
#if UNITY_EDITOR
        uploadSyncSampler.Begin();
#endif
        if (model.syncList.Count > 0)
        {
            udpWarp.objList = model.syncList;
            Send(ProtoIDCfg.SYNC_OBJECTS, udpWarp, ProtoType.Unimportance);
        }
#if UNITY_EDITOR
        uploadSyncSampler.End();
#endif

    }
    void OnSyncObjectList(object protoObj)
    {
#if UNITY_EDITOR
        updateSyncSampler.Begin();
#endif
        ProtoUdpWarp warp = protoObj as ProtoUdpWarp;
        List<SyncObject> protoList = warp.objList;
        model.UpdateSyncData(protoList);
#if UNITY_EDITOR
        updateSyncSampler.End();
#endif
    }
}
