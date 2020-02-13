
/// <summary>
/// 生成Json暴露给应用外配置
/// </summary>
[System.Serializable]
public class ExposeCfg 
{
 
    public bool AutoConnect = true;

    public bool IsHost = true;

    public string TcpPort = "7416";

    public string UdpPort = "2548";


    public string IpAddress = "127.0.0.1";

    public int MaxPlayer = 2;

    public string NetGroup = "0";

    public int WaitConnectTime = 5;

    public int IsDebuger = 0;

    public int pos = 1;
}
