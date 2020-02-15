using UnityEngine;


public static class ObjectPool  {
    static PoolForGameObject _goPool;
    public static PoolForGameObject goPool
    {
        get
        {
            if (_goPool == null)
                _goPool = new PoolForGameObject();
            return _goPool;
        }
    }
    static ProtoPool _protoPool;
    public static ProtoPool protoPool
    {
        get
        {
            if (_protoPool == null)
                _protoPool = new ProtoPool();
            return _protoPool;
        }
    }

   
}
