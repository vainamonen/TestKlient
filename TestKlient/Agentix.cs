using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace TestKlient
{
        
    class Agentix
    {
        StreamReader read = null;
        StreamWriter write = null;
        TcpClient serwer = null;
        NetworkStream netstream=null;
        bool isConnected = true;
        


        public Agentix(TcpClient tcpk)
        {
            serwer = tcpk;
            netstream = serwer.GetStream();
            read = new StreamReader(netstream);
            write = new StreamWriter(netstream);
        }
        //Funkcja odpowiedzialna za odbieraie danych od serwera
        //wykonywana w osobnym watąku
        private void reader()
        {
            
            String odp;
            Char[] delimitter = { ' ' };
            String[] slowa;
            while(isConnected)
            {
                odp = read.ReadLine();
                Console.WriteLine("Odczytano: "+odp);
                slowa = odp.Split(delimitter,StringSplitOptions.RemoveEmptyEntries);
                if(slowa[0]=="ADD")//dodawanie wpisu
                {
                    Console.WriteLine("Wykryto komendę ADD");
                    int p1, vc1, vp1, p2, vc2, vp2;
                    if(slowa.Length != 7)
                    {
                        Console.WriteLine("Zła liczba parametrów w ADD: " + slowa.Length);
                    }
                    else
                    {
                        
                        p1 = int.Parse(slowa[1]);
                        vp1 = int.Parse(slowa[2]);
                        vc1 = int.Parse(slowa[3]);
                        p2 = int.Parse(slowa[4]);
                        vp2 = int.Parse(slowa[5]);
                        vc2 = int.Parse(slowa[6]);
                        Console.WriteLine("Wykryte parametry w ADD: " + p1 + " " + vp1 + " " + vc1 + " " +p2+" " + vp2 + " " +vc2);
                    }
                }
                else if(slowa[0] == "DELETE")//usuwanie jednego wpisu
                {
                    Console.WriteLine("Wykryto komendę DELETE");
                    int p1, vc1, vp1, p2, vc2, vp2;
                    if (slowa.Length != 7)
                    {
                        Console.WriteLine("Zła liczba parametrów w DELETE: " + slowa.Length);
                    }
                    else
                    {

                        p1 = int.Parse(slowa[1]);
                        vp1 = int.Parse(slowa[2]);
                        vc1 = int.Parse(slowa[3]);
                        p2 = int.Parse(slowa[4]);
                        vp2 = int.Parse(slowa[5]);
                        vc2 = int.Parse(slowa[6]);
                        Console.WriteLine("Wykryte parametry w DELETE: " + p1 + " " + vp1 + " " + vc1 + " " +p2+" " + vp2 + " " +vc2);
                    
                    }
                }
                else if(slowa[0] == "CLEAR")//usuwanie wszystkich wpisów
                {
                    Console.WriteLine("Wykryto komendę CLEAR");
                }
                else if(slowa[0] == "LOGGED")//udane logowanie
                {
                    Console.WriteLine("Wykryto komendę LOGGED");
                }
                else if (slowa[0] == "ESTABLISHED")//udane zestawienie połączenia
                {
                    Console.WriteLine("Wykryto komendę ESTABLISHED");
                }
                else if (slowa[0] == "CLIENTS")//zwrócono listę klientów
                {
                    List<String> listakl = new List<string>();
                    Console.WriteLine("Wykryto komendę CLIENTS");
                    for (int i = 1; i < slowa.Length;i++ )
                    {
                        listakl.Add(slowa[i]);
                    }
                    Console.WriteLine("Wykryto klientów: " + (slowa.Length - 1));
                    //Console.WriteLine(listakl.ToString());
                    foreach(String s in listakl)
                    {
                        Console.Write(s + ", ");
                    }
                    Console.Write("\n");
                }
                else if(slowa[0] == "MSG" || slowa[0] == "ERROR" || slowa[0] == "DONE")
                {
                    Console.WriteLine("Wykryto komunikat o treści:");
                    Console.WriteLine(odp);
                }
            }
        }
 
        //Funkcja przesyłająca dane do serwera
        //Wykonywana w osobnym watku
        private void writer()
        {
            while (isConnected)
            {
                String myString = Console.ReadLine();
                write.WriteLine(myString);
                write.Flush();
            }
        }
        public void SendSMS(String sms)
        {
            if (isConnected)
            {
                write.WriteLine(sms);
                write.Flush();
            }
            else
                Console.WriteLine("Nie da się wysłać, brak połączenia");
        }
        public void start()
        {
            if (serwer.Client.Connected)
            {
                Console.WriteLine("Połączono z :" + serwer.ToString());

                //watek wysyłający dane metoda writer
                new Thread(writer).Start();

                //wątek odbierjący dane metoda reader
                new Thread(reader).Start();

            }
            else
                Console.WriteLine("Nie połączono ze zdalnym hostem");
        }
        public void connect()
        {
            //serwer.Connect(hostname, port);
        }
        public void SendLoginC()
        {
            SendSMS("LOGINC\n" + "kurwadupachuj");
        }
        public void SendLoginT()
        {
            SendSMS("LOGINT\n" + "kurwadupachuj");
        }
        public void SendCall(String name)
        {
            SendSMS("CALL\n" + name);
        }
        public void SendDisconnect(String name)
        {
            SendSMS("DISCONNECT\n" + name);
        }
        public void SendGetClients()
        {
            SendSMS("GET_CLIENTS");
        }
        static void Main(string[] args)
        {
            try
            {
             
                String host = "127.0.0.1";
                int port = 2000;
 
               TcpClient serwer2 = new TcpClient(host, port);
               Agentix agent = new Agentix(serwer2);

               agent.start();
               Thread.Sleep(2000);
               agent.SendLoginT();
               agent.SendSMS("___________________________________________________");
               agent.SendLoginC();
               agent.SendSMS("___________________________________________________");
               agent.SendGetClients();
               agent.SendSMS("___________________________________________________");
               agent.SendCall("szczypawka");
               agent.SendSMS("___________________________________________________");
               agent.SendDisconnect("psiakrew");
               
              
            }
            catch (SocketException se)
            {
                Console.WriteLine("Błąd : "+ se.Message);
            }
        }
    }
}
    

