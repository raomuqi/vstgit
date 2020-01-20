

public static class ProtoParse
{
    public static byte GetProtoID<T>(T t)
    {
        byte type=1;
        if (t is ProtoString)
        {
            type= 1;
        }
        return type;
    }
}
