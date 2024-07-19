using PEzBus.EventBus.Events;
using PEzBus.Infrastructure.Enums;
using PEzBus.Types;
using PEzBus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PEzBus.Infrastructure.SocketClient
{
    public class SocketClient : ISocketClient<IMessage> 
    {
        private Socket m_Socket;
        public ConcurrentPriorityQueue<IMessage,MessagePriority> m_Queue = new ();

        private CancellationTokenSource cancellationTokenSource;

        private Thread _SendThread;
        private Thread _ReceiveThread;
        private Action<ArraySegment<byte>> OnMessageArrived;
        private readonly object m_Lock = new object ();

        public SocketClient(string address, int port,Action<ArraySegment<byte>> onMessageArrived, CancellationTokenSource? cts)
        {
            OnMessageArrived = onMessageArrived;
            cancellationTokenSource = cts;

            IPHostEntry localhost = Dns.GetHostEntry(address);   
            IPAddress ipAddress = localhost.AddressList[0];
            m_Socket = new (AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
            Connect(ipAddress,port).Wait();
        }

        private void StartSending()
        {
            while(!cancellationTokenSource.IsCancellationRequested)
            {
                if(m_Queue.TryDequeue(out var message))
                {
                    lock (m_Lock)
                    {
                        SendNextAsync(message.Key);
                    }
                }
            }
        }

        private void StartReceiving()
        {
            while(!cancellationTokenSource.IsCancellationRequested)
            {
                lock (m_Lock)
                {
                    var recivedBytes = new ArraySegment<byte>();
                    m_Socket.ReceiveAsync(recivedBytes).Wait();
                    OnMessageArrived(recivedBytes);
                }
            }
        }

        private async Task Connect(IPAddress ipAddress, int port)
        {
            try
            {
                await m_Socket.ConnectAsync(ipAddress, port);
                _ReceiveThread = BackgroundThread.Start(() => StartReceiving());
                _SendThread = BackgroundThread.Start(() => StartSending());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }       
        }

        private Task SendNextAsync(IMessage message)
        {
            m_Socket.SendAsync(message.ToBytes());

            return Task.CompletedTask;
        }
    }
}
