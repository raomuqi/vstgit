using UnityEngine;


public static class OjectPool  {
    static PoolForGameObject _goPool;
    static PoolForGameObject goPool
    {
        get
        {
            if (_goPool == null)
                _goPool = new PoolForGameObject();
            return _goPool;
        }
    }

   
    public enum PoolType
    {
        GameObject,
        Bullet
    }
    static public void Recycle<T>(T obj, PoolType poolType = PoolType.GameObject)
    {
        switch (poolType)
        {
            case PoolType.GameObject:
                goPool.Recycle(obj as GameObject);
            break;
          
        }
    }
}
