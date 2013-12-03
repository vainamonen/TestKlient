using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace TestKlient
{
        
            class Program
    {
        static StreamReader read = null;
        static StreamWriter write = null;
        static TcpClient serwer = null;
        static Byte[] bufor = null;
        static NetworkStream netstream=null;
        static bool isConnected = false;
 
        //Funkcja odpowiedzialna za odbieraie danych od serwera
        //wykonywana w osobnym watąku
        private static void reader()
        {
            read = new StreamReader(netstream);
            String odp;
            Char[] delimitter = { ' ' };
            String[] slowa;
            while(true)
            {
                odp = read.ReadLine();
                Console.WriteLine("Odczytano: "+odp);
                slowa = odp.Split(delimitter);
                //Console.Write("Po splitcie: ");
                //for(int i=0;i<slowa.Length;i++)
                //{
                //    Console.Write("Slowo[x] gdzie x= " + i + " wartość: " + slowa[i] + ", " );
                //}
                //Console.Write("\n");
                if(slowa[0]=="ADD")
                {
                    Console.WriteLine("Wykryto komendę ADD");
                }
                else if(slowa[0] == "DELETE")
                {
                    Console.WriteLine("Wykryto komendę DELETE");
                }
                else if(slowa[0] == "CLEAR")
                {
                    Console.WriteLine("Wykryto komendę CLEAR");
                }
                else if(slowa[0] == "LOGGED")
                {
                    Console.WriteLine("Wykryto komendę LOGGED");
                }
                else if (slowa[0] == "ESTABLISHED")
                {
                    Console.WriteLine("Wykryto komendę ESTABLISHED");
                }
                else if (slowa[0] == "CLIENTS")
                {
                    Console.WriteLine("Wykryto komendę CLIENTS");
                }
            }
        }
 
        //Funkcja przesyłająca dane do serwera
        //Wykonywana w osobnym watku
        private static void writer()
        {
            write = new StreamWriter(netstream);
           
            while (true)
            {
                String myString = Console.ReadLine();
                write.WriteLine(myString);
                write.Flush();
            }
        }
 
 
        static void Main(string[] args)
        {
            try
            {
             
                String host = "127.0.0.1";
                int port = 2000;
 
                serwer = new TcpClient(host, port);
               
                //Sprawdzamy czy nastąpiło połączenie
                if (serwer.Client.Connected)
                {
                    Console.WriteLine("Połączono z :" + host);
 
                    netstream = serwer.GetStream();
                  
     //bufor na dane
                    bufor = new byte[serwer.Client.ReceiveBufferSize];
 
                    //watek wysyłający dane metoda writer
                    new Thread(writer).Start();
 
                    //wątek odbierjący dane metoda reader
                    new Thread(reader).Start();
 
                                    }
                else
                    Console.WriteLine("Nie połączono ze zdalnym hostem");
            }
            catch (SocketException se)
            {
                Console.WriteLine("Błąd : "+ se.Message);
            }
        }
    }
        }
    

