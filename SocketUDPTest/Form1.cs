using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketUDPTest
{
    public partial class Form1 : Form
    {

        const int listenPort = 3456;
        IPAddress[] ips = Dns.GetHostAddresses("");

        Socket server;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(new IPEndPoint(IPAddress.Parse(ips[3].ToString()), listenPort));//绑定端口号和IP
            sendMsg();
            Console.WriteLine("UDP Service has been turned on.");
            labIP.Text = ips[3].ToString();
            new Thread(new ThreadStart(StartListener)).Start();
        }

        private void StartListener()
        {
            while (true)
            {
                Console.WriteLine("Waiting...");
                EndPoint point = new IPEndPoint(IPAddress.Any, listenPort);//用来保存发送方的ip和端口号
                byte[] buffer = new byte[1024];
                int length = server.ReceiveFrom(buffer, ref point);//接收数据报
                string message = Encoding.UTF8.GetString(buffer, 0, length);
                Console.WriteLine(point.ToString() + " :" + message);
            }
        }

        private void sendMsg()
        {
            EndPoint point = new IPEndPoint(IPAddress.Parse("192.168.43.1"), listenPort);
            Console.WriteLine("I am ready to send message.");
            string msg = txtSendMessage.Text;
            server.SendTo(Encoding.UTF8.GetBytes(msg), point);
            Console.WriteLine("I have been sent message.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendMsg();
        }
    }

}
