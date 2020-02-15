

using System.Collections.Generic;

public class SyncModel : BaseModel
{
    public const string name = "SyncModel";
    public List<SyncObject> syncList = new List<SyncObject>();
    Dictionary<int, SyncObject> syncDic = new Dictionary<int, SyncObject>();
    /// <summary>
    /// 是否上传者
    /// </summary>
    public static bool isUploader = false;
    protected override void OnInit()
    {
        isUploader = Connection.GetInstance().isHost;
    }

    protected override void OnClear() { }


    public bool IsUploader()
    {
        return isUploader;
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
            if (syncDic.TryGetValue(updateObj.objectID, out srcObj))
            {
                srcObj.posX = updateObj.posX;
                srcObj.posY = updateObj.posY;
                srcObj.posZ = updateObj.posZ;
                srcObj.rotX = updateObj.rotX;
                srcObj.rotY = updateObj.rotY;
                srcObj.rotZ = updateObj.rotZ;
                srcObj.rotW = updateObj.rotW;
            }
        }

    } 
    /// <summary>
    /// 添加同步物体
    /// </summary>
    public void AddSyncObj(SyncObject sync)
    {
        if (!syncList.Contains(sync))
        {
            syncList.Add(sync);
            syncDic.Add(sync.objectID, sync);
        }
    }

    /// <summary>
    /// 移除同步物体
    /// </summary>
    public void RemoveSyncObj(SyncObject sync)
    {
        if (syncList.Contains(sync))
        {
            syncList.Remove(sync);
            syncDic.Remove(sync.objectID);
        }
    }


}
