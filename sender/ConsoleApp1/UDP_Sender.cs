using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleApp1
{
    class UDP_Sender
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("숫자를 입력하여 통신 방법을 선택하세요");
            Console.Write("1.유니캐스트 \n2.브로드캐스트 \n3.멀티캐스트\n");
            Console.WriteLine("채팅 입력 창에서 quit을 입력하시면 통신이 종료됩니다.");

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

            Console.WriteLine("선택한 통신 방법은 " + input + "번 입니다.");


            if(input == 1)
            {
                //UdpClient 객체 생성
                UdpClient sender = new UdpClient();        
                IPEndPoint des_ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12000); //루프백 아이피 "127.0.0.1"(자기자신)

                Console.Write("ID 입력 : ");
                string id = Console.ReadLine();
                for (; ; )
                {
                    //보낼데이터를 저장한 문자열변수 선언 및 초기화
                    Console.Write("채팅 입력 : ");
                    String message = Console.ReadLine();
                    if (message == "quit")
                        break;

                    string data = string.Format("{0} : {1}", id, message);
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream stream = new MemoryStream();
                    //네트워크에 UDP소켓으로 송신
                    formatter.Serialize(stream, data);     
                    byte[] send_data = Encoding.Unicode.GetBytes(data.ToString());

                    sender.Send(send_data, send_data.Length, des_ip);

                    //Console.WriteLine("보낸 목적 ip주소는: {0}", des_ip); //주소확인 test
                    stream.Close();
                }
                byte[] buffer = null;
                buffer = Encoding.Unicode.GetBytes("quit");
                sender.Send(buffer, buffer.Length, des_ip);
                Console.WriteLine("통신이 종료 되었습니다. Press ENTER to quit.");
                sender.Close();

               
            }

            else if(input == 2)
            {
                //UdpClient 객체 생성
                UdpClient sender = new UdpClient();
                //sender.EnableBroadcast : 해당 UDP소켓이 브로드캐스트를 사용하는지 설정하는 변수
                // true (기본값) : 브로드캐스트 허용.
                // false : 브로드캐스트로 송신하거나 수신하지 못하도록 설정
                //IPEndPoint 객체 생성 - 브로드캐스트설정 및 12000번포트 설정
                //전송할 대상의 IP를 255.255.255.255로 설정하면 브로드캐스트로 송신
                IPEndPoint des_ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 12000);

                Console.Write("ID 입력 : ");
                string id = Console.ReadLine();
                for (; ; )
                {
                    //보낼데이터를 저장한 문자열변수 선언 및 초기화
                    Console.Write("채팅 입력 : ");
                    String message = Console.ReadLine();
                    if (message == "quit")
                        break;

                    string data = string.Format("{0} : {1}", id, message);
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream stream = new MemoryStream();
                    //네트워크에 UDP소켓으로 송신
                    formatter.Serialize(stream, data);                   
                    byte[] send_data = Encoding.Unicode.GetBytes(data.ToString());

                    sender.Send(send_data, send_data.Length, des_ip);
                    stream.Close();

                }
                byte[] buffer = null;
                buffer = Encoding.Unicode.GetBytes("quit");
                sender.Send(buffer, buffer.Length, des_ip);
                Console.WriteLine("통신이 종료 되었습니다. Press ENTER to quit.");
                sender.Close();
            }

            else if(input == 3)
            {
                UdpClient sender = new UdpClient();

                IPAddress multicastaddress = IPAddress.Parse("239.0.0.222");
                sender.JoinMulticastGroup(multicastaddress);
                IPEndPoint remoteEP = new IPEndPoint(multicastaddress, 12000);

               

                Console.Write("ID 입력 : ");
                

                string id = Console.ReadLine();
                for (; ; )
                {                  
                    Console.Write("채팅 입력 : ");
                    String message = Console.ReadLine();
                    if(message == "quit")
                        break;               
                    
                    string data = string.Format("{0} : {1}", id, message);
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream stream = new MemoryStream();
                    //네트워크에 UDP소켓으로 송신
                    formatter.Serialize(stream, data);
                   
                    byte[] send_data = Encoding.Unicode.GetBytes(data.ToString());

                    sender.Send(send_data, send_data.Length, remoteEP);
                    stream.Close();
                   
                }
                byte[] buffer = null;
                buffer = Encoding.Unicode.GetBytes("quit");
                sender.Send(buffer, buffer.Length, remoteEP);

                sender.Close();

                Console.WriteLine("통신이 종료 되었습니다. Press ENTER to quit.");
                Console.ReadLine();
            }

            else
                Console.WriteLine("지원되지 않는 번호입니다.");
                return;
           
        }

    }

}



