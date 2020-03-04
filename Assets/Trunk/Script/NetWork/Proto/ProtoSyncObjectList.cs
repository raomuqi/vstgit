using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoSyncObjectList : ProtoBase
{
    public SyncObject[] objList;

    protected override byte[] OnSerialize()
    {
        if (objList == null || objList.Length < 1)
            return null;
        return ArraySerialize<SyncObject>(objList);
    }

    protected override void OnParse(byte[] data)
    {
        if (objList != null)
        {
            for (int i = 0; i < objList.Length; i++)
            {
                objList[i].Recycle();
            }
        }
        objList = ArrayDeSerializem<SyncObject>(data, ProtoPool.ProtoRecycleType.SyncObject);
    }
    protected override void OnRecycle()
    {
        ObjectPool.protoPool.Recycle(ProtoPool.ProtoRecycleType.CreateObjects, this);
    }
}
