﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace EventTutorial
{
    class Server
    {

        public void Listen()
        {
            UdpClient listener = new UdpClient(11099);

            IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, 11099);

            while (true)
            {
                byte[] data = listener.Receive(ref serverEP);
                RaiseDataReceived(new ReceivedDataArgs(serverEP.Address,
                    serverEP.Port, data));
            }
        }

        public delegate void DataReceived(object sender, ReceivedDataArgs args);

        public event DataReceived DataReceivedEvent;

        private void RaiseDataReceived(ReceivedDataArgs args)
        {
            if (DataReceivedEvent != null)
            {
                DataReceivedEvent(this, args);
            }

            //DataReceivedEvent?.Invoke(this, args);
        }

    }

    public class ReceivedDataArgs
    {
        public IPAddress IpAdress { get; set; }
        public int Port { get; set; }
        public byte[]ReceivedBytes;

        public ReceivedDataArgs(IPAddress ip, int port, byte[]data)
        {
            this.IpAdress = ip;
            this.Port = port;
            this.ReceivedBytes = data;
        }
    }
}
