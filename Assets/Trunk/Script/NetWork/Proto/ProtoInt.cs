
public class ProtoInt :ProtoBase
{

    public int context=0;
    protected override byte[] OnSerialize()
    {
        byte[] temp = null;
        temp = System.BitConverter.GetBytes(context);
        return temp;
    }

    protected override void OnParse(byte[] data)
    {
        context = System.BitConverter.ToInt32(data,0);
    }

}
