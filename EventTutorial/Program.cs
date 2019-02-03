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
        static void Main(string[] args)
        {
            Console.WriteLine("Please insert LANCODE and press enter (for example: C35EBB59): ");
            var lanCode = Console.ReadLine();
            //Note: here we don't have any checker for the input (for the lan code)!

            ////Just for testing
            //var lanCode = "C35EBB59";

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
    }
}
