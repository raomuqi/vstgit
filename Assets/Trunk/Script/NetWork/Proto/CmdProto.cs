
using System;

[Serializable]
public class ProtoBase
{
    public byte ProtoID = 3;

}

[Serializable]
public class CmdProto : ProtoBase
{
    public string name;

   
  
}
