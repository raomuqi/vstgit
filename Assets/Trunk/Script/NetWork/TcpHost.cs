using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class TcpHost : TcpBase
{
    Thread acceptThread;
    List<SocketAccept> clientList;
    int maxClient;
    bool waitClient = false;
    public static object lockObject = new object();
    protected override void OnInit()
    {
        iPAddress = IPAddress.Parse("127.0.0.1");
        tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }
    public void SetHost(int port, int maxClient)
    {
        try
        {
            clientList = new List<SocketAccept>(maxClient + 2);
            this.maxClient = maxClient;
            this.port = port;
            tcpSocket.Bind(new IPEndPoint(iPAddress, port));
            tcpSocket.Listen(maxClient + 2);
            acceptThread = new Thread(WaitClientConnection);
            connectStatus = 1;
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }

    }
    public void SendMsg(byte[] data)
    {
        if (clientList != null)
        {
            for (int i = 0; i < clientList.Count; i++)
            {

                SocketAccept client = clientList[i];
                if (client != null)
                {
                    client.sendQueue.Enqueue(data);
                }
            }
        }
    }


    public void SendMsg(int index, byte[] data)
    {
        if (clientList != null)
        {
            for (int i = 0; i < clientList.Count; i++)
            {
                SocketAccept client = clientList[i];
                if (client != null && client.id==index)
                {
                    client.sendQueue.Enqueue(data);
                }
            }
        }
    }
    public int GetClientCount()
    {
        if (clientList != null)
        {
            return clientList.Count;
        }
        return 0;
    }
    /// <summary>
    /// client加入
    /// </summary>
    private void WaitClientConnection()
    {
        int index = 0;
        while (waitClient)
        {
            Socket accept = tcpSocket.Accept();
            if (accept != null)
            {
                SocketAccept client = new SocketAccept(accept, index);
                clientList.Add(client);
                client.id = index;
                Debug.Log("client登入" + index);
                //创建接受客户端消息的线程，并将其启动
                client.recvThread = new Thread(RecvMessage);
                client.recvThread.Start(client);
                client.sendThread.Name = index + "recvTr";
                client.sendThread = new Thread(SendMessage);
                client.sendThread.Start(client);
                client.sendThread.Name = index + "sendTr";
                index++;
            }
        }
    }

    //接收线程
    void RecvMessage(object parameter)
    {
        SocketAccept client = parameter as SocketAccept;
        Socket clientsocket = client.socket;
        byte[] strbyte = new byte[2048];
        while (client.recv)
        {
            try
            {
                int count = clientsocket.Receive(strbyte);
                byte[] result = new byte[count];
                Array.Copy(strbyte, result, count);
                if (revceDataList.Count < 2048)
                {
                    lock (lockObject)
                    {
                        revceDataList.Enqueue(result);
                    }
                }
                else
                {
                    Debug.LogError("丢失数据包");
                }
            }
            catch (Exception e)
            {
                OnClientException(client);
                Debug.LogError(e.Message);
            }
        }

    }

    //client独立发送线程
    void SendMessage(object parameter)
    {
        SocketAccept client = parameter as SocketAccept;
        Socket clientsocket = client.socket;
        while (client.send)
        {
            try
            {
                if (client.sendQueue.Count > 0)
                {
                    clientsocket.Send(client.sendQueue.Dequeue());
                }
            }
            catch (Exception e)
            {
                OnClientException(client);
                Debug.LogError(e.Message);

            }
            Thread.Sleep(10);
        }
    }
    //client异常
    void OnClientException(SocketAccept client)
    {
        client.recv = false;
        client.send = false;
        Debug.Log("断开连接" + client.id);
        if (client.sendThread != null)
        {
            client.sendThread.Abort();
            client.sendThread = null;
        }
        if (client.recvThread != null)
        {
            client.recvThread.Abort();
            client.recvThread = null;
        }
        if (clientList!=null && clientList.Contains(client))
        {
            clientList.Remove(client);
        }
    }

    protected override void OnDispose()
    {
        waitClient = false;
        if (acceptThread != null)
            acceptThread.Abort();
        if (clientList != null)
        {
            for (int i = 0; i < clientList.Count; i++)
            {
                SocketAccept client = clientList[i];
                client.recv = false;
                client.send = false;
                if(client.recvThread!=null)
                  client.recvThread.Abort();
                if(client.sendThread != null)
                 client.sendThread.Abort();
            }
        }
        clientList = null;
    }
    public class SocketAccept
    {
        public Socket socket;
        public int id;
        public bool send;
        public bool recv;
        public Thread sendThread;
        public Thread recvThread;
        public Queue<byte[]> sendQueue = new Queue<byte[]>();
        public SocketAccept(Socket socket, int id)
        {
            this.socket = socket;
            this.id = id;
            send = true;
            recv = true;
        }
    }
}
