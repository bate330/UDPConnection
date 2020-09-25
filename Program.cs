using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApp15
{
    class Program
    {
        public static byte[] StringToByteArray(string hex)
        {
            string[] Bytes = hex.Split(' ');
            int[] bytes = new int[Bytes.Length];
            byte[] byte1 = new byte[bytes.Length];
            for (int i=0;i<Bytes.Length;i++)
            {
                bytes[i] = Convert.ToInt16(Bytes[i], 16);
                byte1[i] = (byte)bytes[i];
            }
            
            return byte1;
        }

        public static void UdpSender(byte[] sendBytes, UdpClient udpClient)
        {
            // 02 00 01 a 30 00 30 30 30 30 00 00 00 00 05 03 === print on
            udpClient.Send(sendBytes, sendBytes.Length);
            int[] SendBytesint = new int[sendBytes.Length];
            string[] SendHexValue = new string[sendBytes.Length];
            Console.WriteLine();
            Console.WriteLine("You have sent: ");
            for (int i = 0; i < sendBytes.Length; i++)
            {
                SendBytesint[i] = Convert.ToInt32(sendBytes[i]);
                SendHexValue[i] = SendBytesint[i].ToString("X");
                Console.Write(SendHexValue[i] + " ");
            }
            Console.WriteLine();
        }

        public static bool UdpReceiveAck(UdpClient udpClient, IPEndPoint RemoteIpEndPoint)
        {
            Byte[] receiveACK = udpClient.Receive(ref RemoteIpEndPoint);
            int[] ACKint = new int[receiveACK.Length];
            string[] HexValue = new string[receiveACK.Length];

            Console.WriteLine("Received ACK: ");
            for (int i = 0; i < receiveACK.Length; i++)
            {
                ACKint[i] = Convert.ToInt32(receiveACK[i]);
                HexValue[i] = ACKint[i].ToString("X");
                Console.Write(HexValue[i] + " ");
            }

            if (receiveACK[0] == 6)
            {
                return true;
            }
            else
                return false;
        }


        public static Byte[] UdpReceiver(IPEndPoint RemoteIpEndPoint, UdpClient udpClient, int port)
        {
                Console.WriteLine();
                Console.WriteLine("Received Frame: ");

                Byte[] receiveFrame = udpClient.Receive(ref RemoteIpEndPoint);
                int[] ACKframe = new int[receiveFrame.Length];
                string[] HexValue = new string[receiveFrame.Length];

                for (int i = 0; i < receiveFrame.Length; i++)
                {
                    ACKframe[i] = Convert.ToInt32(receiveFrame[i]);
                    HexValue[i] = ACKframe[i].ToString("X");
                    Console.Write(HexValue[i] + " ");
                }
            return receiveFrame;
        }

        static void Main(string[] args)
        {
            byte[] ack = { 06 };
            try
            {
                Console.WriteLine("Input ip: ");
                string ipaddress = Console.ReadLine();
                Console.WriteLine("Input port: ");
                string PrinterPort = Console.ReadLine();
                int port = Int32.Parse(PrinterPort);
                UdpClient udpClient = new UdpClient(ipaddress, port);
                Console.WriteLine("Input Frame: ");
                string text = Console.ReadLine();
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, port);
                Byte[] sendBytes = StringToByteArray(text);
                UdpSender(sendBytes, udpClient);
                //----------------------------------
                System.Threading.Thread.Sleep(1000);
                //----------------------------------
                bool ReceiveAck = UdpReceiveAck(udpClient, RemoteIpEndPoint);
                if (ReceiveAck == true)
                {
                    Byte[] receiveBytes1 = UdpReceiver(RemoteIpEndPoint, udpClient, port);
                    UdpSender(ack, udpClient);
                } 

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to ESC... ");
            Console.ReadKey();
        }
    }
}
