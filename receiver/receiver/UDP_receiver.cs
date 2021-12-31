using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace receiver
{
    class UDP_receiver
    {
        static void Main(string[] args)
        {

            Console.Write("숫자를 입력하여 통신 방법을 선택하세요\n");
            Console.Write("1.유니캐스트 \n2.브로드캐스트 \n3.멀티캐스트\n");

            int input;
            try
            {
                input = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return;
            }

            Console.WriteLine("선택한 통신 방법 번호는 " + input);
            

            if (input == 1)
            {

                UdpClient receiver = new UdpClient(12000);              
                IPEndPoint src_ip = new IPEndPoint(IPAddress.Any, 12000);
                
                while (true)
                {
              

                    byte[] data = receiver.Receive(ref src_ip);
                    string strData = Encoding.Unicode.GetString(data);
                    Console.WriteLine("received data : {0}", strData);

                    if (strData == "quit")      
                        break;                  
                }

                receiver.Close();
            }


            else if (input == 2)
            {
                UdpClient receiver = new UdpClient(12000);
                IPEndPoint src_ip = new IPEndPoint(0, 0);
                BinaryFormatter formatter = new BinaryFormatter();
               
                
                while (true)
                {
        
                    byte[] data = receiver.Receive(ref src_ip);
                    string strData = Encoding.Unicode.GetString(data);
                    Console.WriteLine("received data : {0}", strData);
                    
                    if (strData == "quit")
                        break;
        
                }
               
                receiver.Close();
            }

            else if (input == 3)
            {
                UdpClient receiver = new UdpClient();

                receiver.ExclusiveAddressUse = false;
                IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 12000);

                receiver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                receiver.ExclusiveAddressUse = false;

                receiver.Client.Bind(localEp);

                IPAddress multicastaddress = IPAddress.Parse("239.0.0.222");
                receiver.JoinMulticastGroup(multicastaddress);

                Console.WriteLine("Listening this will quit");

                while (true)
                {
                    byte[] data = receiver.Receive(ref localEp);
                    string strData = Encoding.Unicode.GetString(data);
                    Console.WriteLine("received data : {0}", strData);
                    

                    if (strData == "quit")
                        break;
                }

                Console.WriteLine("통신이 종료 되었습니다. Press ENTER to quit");
                Console.ReadLine();
            }

            else
                Console.WriteLine("지원되지 않는 번호입니다.");
                return;
           
        }
    }
}
