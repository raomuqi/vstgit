using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncNetHandler : BaseNetHandler
{
    SyncModel model;
    ProtoUdpWarp upLoadWarp;
    ProtoUdpWarp upDateWarp;
    ProtoPlayerInfo playerInfo;
    InputModel inputModel;
    byte[] selfInputData = new byte[3];
    protected override void OnInit()
    {
        upLoadWarp = new ProtoUdpWarp();
        upDateWarp = new ProtoUdpWarp();

        model = SyncController.instance.GetModel<SyncModel>(SyncModel.name);
        Global.instance.AddUpdateFunction(SendSyncObjectList);
        inputModel = InputController.instance.GetModel<InputModel>(InputModel.name);

        RegisterSendProto(ProtoIDCfg.SYNC_INPUT, SendSyncInput);
        RegisterListenProto(ProtoIDCfg.SYNC_INPUT, OnSyncInput);
        RegisterListenProto(ProtoIDCfg.SYNC_OBJECTS, OnSyncObjectList);
    }
    protected override void OnClear()
    {
        Global.instance.RemoveUpdateFunction(SendSyncObjectList);
    }

    /// <summary>
    /// 发送上传输入
    /// </summary>
    void SendSyncInput(EventArgs args)
    {
        if (playerInfo == null)
        {
            playerInfo = PlayerController.instance.GetModel<PlayerModel>(PlayerModel.name).GetPlayerInfo();
        }
        if (playerInfo == null)
            return;
        selfInputData[0] = playerInfo.pos;
        selfInputData[1] = inputModel.selfFires[0];
        selfInputData[2] = inputModel.selfFires[1];
        Send(ProtoIDCfg.SYNC_INPUT, selfInputData, ProtoType.Importance);
    }
    /// <summary>
    /// 更新其他玩家输入
    /// </summary>
    void OnSyncInput(byte[] data)
    {
        model.SetPosInput(data[0], data[1], data[2]);
    }

    /// <summary>
    /// 发送上传数据
    /// </summary>
    void SendSyncObjectList()
    {
        if (model.uploadList.Count > 0)
        {
            upLoadWarp.objList = new SyncObject[model.uploadList.Count];
            for (int i = 0; i < model.uploadList.Count; i++)
            {
                upLoadWarp.objList[i] = model.uploadList[i];

            }
            Send(ProtoIDCfg.SYNC_OBJECTS, upLoadWarp, ProtoType.Unimportance);
        }
    }
    void OnSyncObjectList(byte[] protoObj)
    {

         Util.DeSerializeUDPProto(protoObj,ref upDateWarp);

        if (upDateWarp != null)
        {
            SyncObject[] protoList = upDateWarp.objList;
            model.UpdateSyncData(protoList);
        }

    }
}
