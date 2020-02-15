


public class ProtoString : ProtoBase
{
    public string context;
    protected override byte[] OnSerialize()
    {
        byte[] temp = null;
        temp= System.Text.Encoding.UTF8.GetBytes(context);
        return temp;
    }

    protected override void OnParse(byte[] data)
    {
        context = System.Text.Encoding.UTF8.GetString(data);
    }

}
