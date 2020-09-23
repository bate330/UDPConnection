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
            string[] Bytes = hex.Split(',');
            int[] bytes = new int[Bytes.Length];
            byte[] byte1 = new byte[bytes.Length];
            for (int i=0;i<Bytes.Length;i++)
            {
                bytes[i] = Convert.ToInt16(Bytes[i], 16);
                byte1[i] = (byte)bytes[i];
            }
            
            return byte1;
        }

        static void Main(string[] args)
        {
            
           // Byte[] sendBytes = { 02, 00, 01, 10 ,48 ,00, 48 ,48 ,48 ,48 ,00, 00 ,00 ,00 ,05, 03 };// print on
            try
            {
                Console.WriteLine("Input ip: ");
                string ipaddress = Console.ReadLine();
                Console.WriteLine("Input port: ");
                string PrinterPort = Console.ReadLine();
                int port = Int32.Parse(PrinterPort);
                Console.WriteLine("Input Frame (as a separator use: ','): ");
                string text = Console.ReadLine();
                Byte[] sendBytes = StringToByteArray(text);
                UdpClient udpClient = new UdpClient(ipaddress, port);
                udpClient.Send(sendBytes, sendBytes.Length);
                int[] SendBytesint = new int[sendBytes.Length];
                string[] SendHexValue = new string[sendBytes.Length];

                Console.WriteLine("You have sent: ");
                for (int i = 0; i < sendBytes.Length; i++)
                {
                    SendBytesint[i] = Convert.ToInt32(sendBytes[i]);
                    SendHexValue[i] = SendBytesint[i].ToString("X");
                    Console.Write(SendHexValue[i] + ", ");
                }
                Console.WriteLine();
            
            //-----------------------------------------------------------------------------------------------
                System.Threading.Thread.Sleep(1000);
            //----------------------------------------------------------------------------------------------------------

            //Creates an IPEndPoint to record the IP Address and port number of the sender.
            // The IPEndPoint will allow you to read datagrams sent from any source.

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any , 3000);

            // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveACK = udpClient.Receive(ref RemoteIpEndPoint);
                int[] ACKint = new int[receiveACK.Length];
                string[] HexValue = new string[receiveACK.Length];

                Console.WriteLine("Received ACK: ");
                for (int i=0 ; i<receiveACK.Length ; i++)
                {
                    ACKint[i]= Convert.ToInt32(receiveACK[i]);
                    HexValue[i] = ACKint[i].ToString("X");
                    Console.Write(HexValue[i] + " ");
                }
                Console.WriteLine();
                Console.WriteLine("Received Frame: ");

                Byte[] receiveFrame = udpClient.Receive(ref RemoteIpEndPoint);
                ACKint = new int[receiveFrame.Length];
                HexValue = new string[receiveFrame.Length];

                for (int i=0; i < receiveFrame.Length; i++)
                {
                    ACKint[i] = Convert.ToInt32(receiveFrame[i]);
                    HexValue[i] = ACKint[i].ToString("X");
                    Console.Write(HexValue[i] + ", ");
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
