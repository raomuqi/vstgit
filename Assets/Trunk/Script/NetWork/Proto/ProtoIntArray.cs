using System;


public class ProtoIntArray : ProtoBase
{
    public int[] context;
    protected override byte[] OnSerialize()
    {
        if (context == null || context.Length<1) return null;
        byte[] temp = new byte[context.Length*4];
        for (int i = 0; i < context.Length; i++)
        {
            Array.Copy(System.BitConverter.GetBytes(context[i]), 0, temp,  4 * i, 4);
        }
        return temp;
    }

    protected override void OnParse(byte[] data)
    {
        int length = data.Length / 4;
        if (length > 0)
        {
            context = new int[length];
            for (int i = 0; i < length; i++)
            {
                context[i] = System.BitConverter.ToInt32(data, i * 4);
            }
        }
    }
    protected override void OnRecycle()
    {
        context = null;
        ObjectPool.protoPool.Recycle(ProtoPool.ProtoRecycleType.IntArray, this);
    }
}
