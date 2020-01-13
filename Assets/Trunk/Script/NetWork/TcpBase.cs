
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
public class TcpBase 
{
    public class SocketAccept
    {
       public Socket socket;
        public int id;
        public bool send;
        public bool recv;
        public Thread sendThread;
        public Thread recvThread;
        public Queue<byte[]> sendQueue = new Queue<byte[]>();
        public SocketAccept(Socket socket,int id)
        {
            this.socket = socket;
            this.id = id;
            send = true;
            recv = true;
        }
    }
    Thread acceptThread;
    private Socket host;
    bool isListen = false;
    List<SocketAccept> clientList;
    private IPAddress iPAddress;
    int maxClient;
    int port;
    bool waitClient = false;
    public TcpBase( int port,int maxClient)
    {
        iPAddress = IPAddress.Parse("127.0.0.1");
        this.port = port;
        host = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        host.Bind(new IPEndPoint(iPAddress, port));
        host.Listen(maxClient+2);
    }

    private void WaitClientConnection()
    {
        int index = 0;
        while (waitClient)
        {
            Socket accept = host.Accept();
            if (accept != null)
            {
                SocketAccept client = new SocketAccept(accept, index);
                clientList.Add(client);
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
    void RecvMessage(object parameter)
    {
        SocketAccept client = parameter as SocketAccept;
        Socket clientsocket = client.socket;
        while (client.recv)
        {
            try
            {
                byte[] strbyte = new byte[2048];
                int count = clientsocket.Receive(strbyte);
            }
            catch (Exception)
            {
                OnClientException(client);
            }
        }

    }
    void SendMessage(object parameter)
    {
        SocketAccept client = parameter as SocketAccept;
        Socket clientsocket = client.socket;
        while (client.send)
        {
            try
            {
                while (client.sendQueue.Count > 0)
                {
                     clientsocket.Send(client.sendQueue.Dequeue());
                }
            }
            catch (Exception)
            {
                OnClientException(client);
            }
            Thread.Sleep(10);
        }
    }

    void OnClientException(SocketAccept client)
    {
        client.recv = false;
        client.send = false;
        //客户端离去时终止线程
        Debug.LogError("断开连接" + client.id);
        client.sendThread.Abort();
        client.recvThread.Abort();
    }
}
