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
    SocketAccept[] clientList;
    Queue<int> loginClient = new Queue<int>();
    Queue<int> logoutClient = new Queue<int>();
    Queue<int> freeID = new Queue<int>();
    int maxClient=2;
    bool waitClient = false;
    int loginNegative = -1;
    int curClient = 0;
    int clientIndex=0;
    bool isDispose = false;
    protected override void OnInit()
    {
        iPAddress = IPAddress.Parse("127.0.0.1");
    }
    public void SetHost(int port, int maxClient)
    {
        try
        {
            clientList = new SocketAccept[maxClient];
            this.maxClient = maxClient;
            this.port = port;
            waitClient = true;
            tcpSocket.Bind(new IPEndPoint(iPAddress, port));
            tcpSocket.Listen(maxClient + 2);
            acceptThread = new Thread(WaitClientConnection);
            acceptThread.IsBackground = true;
            acceptThread.Start();
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
            for (int i = 0; i < clientList.Length; i++)
            {
                SocketAccept client = clientList[i];
                if (client != null)
                {
                    client.sendQueue.Enqueue(data);
                }
            }
        }
    }
    public SocketAccept[] GetClientList()
    {
        return clientList;
    }

    public void SendMsg(int index, byte[] data)
    {
        if (clientList != null && index< clientList.Length)
        {
          SocketAccept client = clientList[index];
          if (client != null)
          {
              client.sendQueue.Enqueue(data);
          }
        }
    }
    public int GetClientCount()
    {
      return curClient;
    }
    public Queue<int> GetLogOutClient()
    {
        return logoutClient;
    }
    public Queue<int> GetLoginClient()
    {
        return loginClient;
    
    }
    /// <summary>
    /// client加入
    /// </summary>
    private void WaitClientConnection()
    {
        while (waitClient)
        {
            Socket accept = tcpSocket.Accept();

            if (accept != null && GetClientCount()<maxClient)
            {
                int id = 0;
                if (freeID.Count > 0)
                {
                    id = freeID.Dequeue();
                }
                else
                {
                    id = clientIndex;
                    clientIndex++;
                }
                
                SocketAccept client = new SocketAccept(accept, id);
                clientList[id] =client;
                loginClient.Enqueue(id);
                client.id = id;
                //创建接受客户端消息的线程，并将其启动
                client.recvThread = new Thread(RecvMessage);
               client.recvThread.IsBackground = true;
                client.recvThread.Name = id + "recvTr";
                client.recvThread.Start(client);
                client.sendThread = new Thread(SendMessage);
                client.sendThread.Name = id + "sendTr";
                client.sendThread.Start(client);
                client.sendThread.IsBackground = true;
                curClient++;
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
                if (count >= Connection.PACKER_OFFSET)
                {
                    byte[] result = new byte[count];
                    Array.Copy(strbyte, result, count);
                    if (client.recvQueue.Count < 2048)
                    {
                        client.recvQueue.Enqueue(result);
                    }
                    else
                    {
                        Debug.LogError("丢失数据包");
                    }
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
    /// <summary>
    /// 踢出Clietn
    /// </summary>
    public void RemoveClient(int id)
    {
        if (clientList != null)
        {
            for (int i = 0; i < clientList.Length; i++)
            {
                SocketAccept client = clientList[i];
                if (client != null && client.id == id)
                {
                    OnClientException(client);
                }
            }
        }

    }

    //client异常
    void OnClientException(SocketAccept client)
    {
        if (isDispose == true)
            return;
        int id = client.id;
        freeID.Enqueue(id);
        logoutClient.Enqueue(id);
        curClient--;
        Debug.Log("断开连接" + client.id);
        client.Dispose();
        clientList = null;
       
    }

    protected override void OnDispose()
    {
        isDispose = true;
        waitClient = false;
        if (acceptThread != null)
            acceptThread.Abort();
        if (clientList != null)
        {
            for (int i = 0; i < clientList.Length; i++)
            {
                SocketAccept client = clientList[i];
                if (client != null)
                {
                    client.Dispose();
                }
            }
        }
        clientIndex = 0;
        curClient = 0;
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
        public Queue<byte[]> recvQueue = new Queue<byte[]>(2048);
        public SocketAccept(Socket socket, int id)
        {
            this.socket = socket;
            this.id = id;
            send = true;
            recv = true;
        }
        public void Dispose()
        {
            recv = false;
            send = false;
            if (recvThread != null)
                recvThread.Abort();
            if (sendThread != null)
                sendThread.Abort();
        }
    }
}
