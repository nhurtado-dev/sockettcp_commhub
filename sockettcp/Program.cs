using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

[StructLayout(LayoutKind.Sequential, Pack=8)]
struct HeartbeatPacket
{
    public char cSynchByte;
    public int nPacketType;
    public int nSize;
}


namespace sockettcp
{
    class Program
    {

        
        private const int port = 44965;
        private const string ip = "10.20.66.3";
        static void Main(string[] args)
        {
            /*var client = new TcpClient(ip, prot);

            NetworkStream ns = client.GetStream();

            byte[] bytes = new byte[1024];

            int bytesRead = ns.Read(bytes, 0, bytes.Length);

            Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRead).ToString());
            Console.WriteLine(bytesRead);

            client.Close();*/
            void PrintByteArray(byte[] bytes)
            {
                var sb = new StringBuilder("new byte[] { ");
                foreach (var b in bytes)
                {
                    sb.Append(b/* + ", "*/);
                }
                sb.Append("}");
                Console.WriteLine(sb.ToString());
            }
            // http://stackoverflow.com/a/829994/346561
            byte[] UnsignedBytesFromSignedBytes(byte[] signed)
            {
                var unsigned = new byte[signed.Length];
                Buffer.BlockCopy(signed, 0, unsigned, 0, signed.Length);
                return unsigned;
            }

            byte[] data = new byte[1024];
            var unsignedBytes = UnsignedBytesFromSignedBytes(data);

            
            /*string stringByte= BitConverter.ToString(data);*/

            bool status = false;

            /*IPHostEntry iphostInfo = Dns.GetHostEntry("GPSADI1.GPSGS.COM.PE");*/
            IPAddress ipAddress = IPAddress.Parse("10.20.66.3"); /*iphostInfo.AddressList[0];*/
            IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, 44965);

            Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //status = true;
                client.Connect(ip, port);

                Console.WriteLine("Socket created to {0}", client.RemoteEndPoint.ToString());

                //byte[] sendmsg = Encoding.ASCII.GetBytes("This is from Client\n");

                //int n = client.Send(sendmsg);
                while (!status)
                {
                    unsafe
                    {
                        int m = client.Receive(data);
                        Console.WriteLine("===============================  ");
                        PrintByteArray(unsignedBytes);
                        //Console.WriteLine("" + Encoding.ASCII.GetString(data));
                        Console.WriteLine("===============================  ");
                        Console.WriteLine(Convert.ToBase64String(data));
                        /*Console.WriteLine(Encoding.Default.GetString(data));*/
                        /*Console.WriteLine(Encoding.BigEndianUnicode.GetString(data));*/
                        /*HeartbeatPacket ex = new HeartbeatPacket();
                        byte* addr = (byte*)&ex;
                        Console.WriteLine("Size: {0}", sizeof(HeartbeatPacket));
                        Console.WriteLine("SYNCH BYTE: {0}", ex.cSynchByte);
                        Console.WriteLine("PACKET TYPE: {0}", (byte*) &ex.nPacketType - addr);
                        Console.WriteLine("SIZE: {0}", (byte*)&ex.nSize - addr);*/
                    }
                }
                //client.Shutdown(SocketShutdown.Both);
                //client.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Transmission end.");
            Console.ReadKey();


        }
    }
}
