using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class VRChairSDK : MonoBehaviour {
     static VRChairSDK instance = null;
    public static bool debugMsg = false;
    public static VRChairSDK GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("[VRChairSDK]");
            instance = go.AddComponent<VRChairSDK>();
            GameObject.DontDestroyOnLoad(instance);
            instance.Init();
        }
        return instance;
    } 
    UdpBase recvUdp;
    UdpBase sendUdp;
    int recverID;
    const string CMD_SETATTITUTDE = "SetAttitude:";
    const string CMD_SETDY= "SetDy:";
    const string CMD_SETRX = "SetRx:";
    const string CMD_SETRZ = "SetRz:";
    const string CMD_SETFAN= "SetFan:";
    const string CMD_SETFAX = "SetAx:";
    const string CMD_SETEFF = "SetEff:";
    const string CMD_SPLIT = ",";
    StringBuilder sendString = new StringBuilder();
     System.Action<byte, byte> onBtnChange;
    byte[] btnStatus = new byte[20];
    public void Init()
    {
      
            if (sendUdp == null)
            {
                sendUdp = new UdpBase(33002, false);
                recverID = sendUdp.GetRecverID("127.0.0.1");
            }
            if (recvUdp == null)
            {
                recvUdp = new UdpBase(33001, true);
            }
      
    }
    /// <summary>
    /// index,status
    /// </summary>
    public void RegisterBtnChangeCallback(System.Action<byte, byte> callback)
    {
        onBtnChange += callback;

    }

     void OnBtnChange(byte index,byte stauts)
    {
        if (onBtnChange != null)
            onBtnChange(index, stauts);
    }

    public void Dispose()
    {
        onBtnChange = null;
        VRChairSDK.instance = null;
        GameObject.DestroyImmediate(gameObject);
    }
    void OnDestroy()
    {
        onBtnChange = null;
        if (sendUdp != null)
        {
            sendUdp.Dispose();
            sendUdp = null;
        }
        if (recvUdp != null)
        {
            recvUdp.Dispose();
            recvUdp = null;
        }
    }
    void Update()
    {
        byte[] udpMsg = sendUdp.GetMsg();
        if (udpMsg != null)
        {
            string recvString = System.Text.ASCIIEncoding.ASCII.GetString(udpMsg);

            if (debugMsg)
                Debug.Log(recvString);

            if (recvString.StartsWith("Btn:"))
            {
                string btnResult= recvString.Replace("Btn:", "");
                string[] btnSplit = btnResult.Split(',');
                for (byte i=0;i< btnSplit.Length;i++)
                {
                    byte lastStatus = btnStatus[i];
                    byte inputStatus;
                    if (byte.TryParse(btnSplit[i], out inputStatus))
                    {
                        if (lastStatus != inputStatus)
                        {
                            btnStatus[i] = inputStatus;
                            OnBtnChange(i, btnStatus[i]);
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// XYZ一起发送
    /// </summary>
    public void SetAttitude(float x,float y,float z)
    {
        sendString.Remove(0, sendString.Length);
        sendString.Append(CMD_SETATTITUTDE);
        sendString.Append(x);
        sendString.Append(CMD_SPLIT);
        sendString.Append(y);
        sendString.Append(CMD_SPLIT);
        sendString.Append(z);
        sendUdp.SendTo(ASCIIEncoding.ASCII.GetBytes(sendString.ToString()), recverID);
    }
    /// <summary>
    /// 轴向旋转
    /// </summary>
    public void SetRX(float x)
    {
        sendString.Remove(0, sendString.Length);
        sendString.Append(CMD_SETRX);
        sendString.Append(x);
        sendUdp.SendTo(ASCIIEncoding.ASCII.GetBytes(sendString.ToString()), recverID);
    }
    /// <summary>
    /// 高度
    /// </summary>
    public void SetDY( float y)
    {
        sendString.Remove(0, sendString.Length);
        sendString.Append(CMD_SETDY);
        sendString.Append(y);
        sendUdp.SendTo(ASCIIEncoding.ASCII.GetBytes(sendString.ToString()), recverID);
    }
    /// <summary>
    /// 水平旋转
    /// </summary>
    public void SetRZ(float z)
    {
        sendString.Remove(0, sendString.Length);
        sendString.Append(CMD_SETRZ);
        sendString.Append(z);
        sendUdp.SendTo(ASCIIEncoding.ASCII.GetBytes(sendString.ToString()), recverID);
    }
    /// <summary>
    /// 风扇
    /// </summary>
    public void SetFan(bool isOpen)
    {
        sendString.Remove(0, sendString.Length);
        sendString.Append(CMD_SETFAN);
        sendString.Append(isOpen.ToString().ToLower());
        sendUdp.SendTo(ASCIIEncoding.ASCII.GetBytes(sendString.ToString()), recverID);
    }

    public void SetAx(int index,float height)
    {
        sendString.Remove(0, sendString.Length);
        sendString.Append(CMD_SETFAX);
        sendString.Append(index);
        sendString.Append(CMD_SPLIT);
        sendString.Append(height);
        sendUdp.SendTo(ASCIIEncoding.ASCII.GetBytes(sendString.ToString()), recverID);
    }
    public void SetEff(int index,bool isOpen)
    {
        sendString.Remove(0, sendString.Length);
        sendString.Append(CMD_SETEFF);
        sendString.Append(index);
        sendString.Append(CMD_SPLIT);
        sendString.Append(isOpen.ToString().ToLower());
        sendUdp.SendTo(ASCIIEncoding.ASCII.GetBytes(sendString.ToString()), recverID);
    }
}
