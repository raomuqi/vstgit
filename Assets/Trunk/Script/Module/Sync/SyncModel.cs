

using System.Collections.Generic;
using UnityEngine;

public class SyncModel : BaseModel
{
    public const string name = "SyncModel";
    public List<SyncObject> uploadList = new List<SyncObject>();
    Dictionary<int, SyncObject> updataList = new Dictionary<int, SyncObject>();
    public byte[][] playerInput = new byte[10][];
    protected override void OnInit()
    {
        for (byte i = 0; i < playerInput.Length; i++)
        {
            playerInput[i] = new byte[2];
        }
    }

    protected override void OnClear() { }


    public void SetPosInput(byte pos,byte fire1,byte fire2)
    {
        byte[] fires = playerInput[pos];
        fires[0] = fire1;
        fires[1] = fire2;
    }
    public byte[] GetPosInput(byte pos)
    {
       return playerInput[pos];
      
    }
    /// <summary>
    /// 刷新同步数据
    /// </summary>
    public void UpdateSyncData(SyncObject[] updateList)
    {

        for (int i = 0; i < updateList.Length; i++)
        {
            SyncObject updateObj = updateList[i];
            SyncObject srcObj;
            if (updataList.TryGetValue(updateObj.serverID, out srcObj))
            {
                srcObj.posX = updateObj.posX;
                srcObj.posY = updateObj.posY;
                srcObj.posZ = updateObj.posZ;
                srcObj.rotX = updateObj.rotX;
                srcObj.rotY = updateObj.rotY;
                srcObj.rotZ = updateObj.rotZ;
                srcObj.rotW = updateObj.rotW;
            }
            else
            {
                if (SyncCreater.instance != null)
                    SyncCreater.instance.CreateObject(updateObj.objectIndex, updateObj.serverID, updateObj);
            }
            updateObj.Recycle();
        }

    } 
    /// <summary>
    /// 添加同步物体到上传列表
    /// </summary>
    public void AddUpLoadList(SyncObject sync)
    {
        if (!uploadList.Contains(sync))
        {
            Debug.Log("添加上传物体"+sync.serverID);
            uploadList.Add(sync);
        }
    }
    /// <summary>
    /// 添加同步物体到更新列表
    /// </summary>
    public void AddUpUpdateList(SyncObject sync)
    {
        if (!updataList.ContainsKey(sync.serverID))
        {
            updataList.Add(sync.serverID, sync);
        }
    }

    /// <summary>
    /// 移除上传物体
    /// </summary>
    public void RemoveUpLoadObj(SyncObject sync)
    {
        if (uploadList.Contains(sync))
        {
            uploadList.Remove(sync);
        }
    }
    /// <summary>
    /// 移除更新物体
    /// </summary>
    public void RemoveUpDataObj(SyncObject sync)
    {
        if (updataList.ContainsKey(sync.serverID))
        {
            updataList.Remove(sync.serverID);
        }
    }

}
