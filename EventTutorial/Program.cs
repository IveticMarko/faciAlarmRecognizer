using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventTutorial
{
    class Program
    {
        public static Boolean isFirebaseTriggered = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Please insert LANCODE and press enter (for example: 59BB5EC3): ");
            var lanCode = Console.ReadLine();
            //TODO: Note: here we don't have any checker for the input (for the lan code)!

            lanCode = Reverse(lanCode);

            //Load all devices that we have in the system. We will send data to those devices.
            Cache.GetAllDevice();

            Server server = new Server();
            HandleDataClass hdc = new HandleDataClass();

            //Start server Thread
            Thread serverThread = new Thread(() => server.Listen());
            serverThread.Start();

            //Start Handler Thread
            Thread dataHandlerThread = new Thread(() =>
                hdc.SubscribeToEvent(server, lanCode));

            dataHandlerThread.Start();

            //Do other things
            while (true)
            {
                Thread.Sleep(100);
            }

        }
        
        public static string Reverse(string text)
        {
            char[] cArray = text.ToCharArray();
            string reverse = String.Empty;
            List<String> tempList = new List<string>();
            List<String> reverseList = new List<string>();

            var temp = "";
            for (int i = 1; i <= cArray.Length; i++)
            {
                if ((i % 2) == 0)
                {
                    temp += cArray[i-1].ToString();
                    tempList.Add(temp);
                    temp = "";
                }
                temp = cArray[i-1].ToString();
            }
            
            for (int i = tempList.Count - 1; i >= 0; i--)
            {
                reverseList.Add(tempList[i]);
            }

            reverse = String.Join("", reverseList);

            return reverse;
        }
    }
}
