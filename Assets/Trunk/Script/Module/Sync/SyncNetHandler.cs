using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncNetHandler : BaseNetHandler
{
    SyncModel model;
    ProtoUdpWarp upLoadWarp;
    ProtoUdpWarp upDateWarp;
#if UNITY_EDITOR
    UnityEngine.Profiling.CustomSampler updateSyncSampler;
    UnityEngine.Profiling.CustomSampler uploadSyncSampler;
#endif
    protected override void OnInit()
    {
        upLoadWarp = new ProtoUdpWarp();
        upDateWarp = new ProtoUdpWarp();
#if UNITY_EDITOR
        updateSyncSampler = UnityEngine.Profiling.CustomSampler.Create("OnSyncObjectList");
        uploadSyncSampler= UnityEngine.Profiling.CustomSampler.Create("SendSyncObjectList");
#endif
        model = SyncController.instance.GetModel<SyncModel>(SyncModel.name);
        Global.instance.AddUpdateFunction(SendSyncObjectList);
        RegisterListenProto(ProtoIDCfg.SYNC_OBJECTS, OnSyncObjectList);
    }
    protected override void OnClear()
    {
        Global.instance.RemoveUpdateFunction(SendSyncObjectList);
    }
    /// <summary>
    /// 发送上传数据
    /// </summary>
    void SendSyncObjectList()
    {
//#if UNITY_EDITOR
//        uploadSyncSampler.Begin();
//#endif
        if (model.uploadList.Count > 0)
        {
            upLoadWarp.objList = new SyncObject[model.uploadList.Count];
            for (int i = 0; i < model.uploadList.Count; i++)
            {
                upLoadWarp.objList[i] = model.uploadList[i];

            }
            Send(ProtoIDCfg.SYNC_OBJECTS, upLoadWarp, ProtoType.Unimportance);
        }

//#if UNITY_EDITOR
//        uploadSyncSampler.End();
//#endif

    }
    void OnSyncObjectList(byte[] protoObj)
    {
#if UNITY_EDITOR
        updateSyncSampler.Begin();
#endif
         Util.DeSerializeUDPProto(protoObj,ref upDateWarp);
#if UNITY_EDITOR
        updateSyncSampler.End();
#endif
        if (upDateWarp != null)
        {
            SyncObject[] protoList = upDateWarp.objList;
            //for (int i = 0; i < upDateWarp.objList.Length; i++)
            //    Debug.Log(upDateWarp.objList[i].serverID);
            model.UpdateSyncData(protoList);
        }

    }
}
