using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EventTutorial
{
    class HandleDataClass
    {
        private string lanCode = "C35EBB59";
        private string rawData = "";
        private List<String> dataBlocks = new List<string>();  
        private string data = "";
        private string alarmID = "";
        private string decoded = "";
        private int counter = 0;
        private string finalText = "";

        public void SubscribeToEvent(Server server, String lanCode)
        {
            server.DataReceivedEvent += server_DataReceivedEvent;
            this.lanCode = lanCode;
        }

        private void server_DataReceivedEvent(object sender, ReceivedDataArgs args)
        {
            dataBlocks.Clear();
            data = "";
            //Get byte and convert to hex values
            for (int i = 0; i <= args.ReceivedBytes.Length - 1; i++)
            {
                rawData += args.ReceivedBytes[i].ToString("X2") + " ";

                data += args.ReceivedBytes[i].ToString("X2");
                if ((i + 1) % 4 == 0)
                {
                    dataBlocks.Add(data);
                    counter++;
                    data = "";
                }
            }

            long newKey = Convert.ToInt64(dataBlocks[1], 16);
            long lanCodeToInt = Convert.ToInt64(lanCode, 16);
            newKey = newKey ^ lanCodeToInt;

            decoded = dataBlocks[0].ToString() + dataBlocks[1].ToString();
            var decoded_part = "";

            for (int i = 2; i <= dataBlocks.Count - 1; i++)
            {
                decoded_part = "";
                long value = Convert.ToInt64(dataBlocks[i], 16);
                value = value ^ newKey;
                //alarmID += Encoding.Default.GetString(BitConverter.GetBytes(value).Reverse().ToArray());
                decoded_part = value.ToString("X");
                if (decoded_part.Length != 8)
                {
                    var numOfZeros = 8 - decoded_part.Length;
                    for (int j = 0; j < numOfZeros; j++)
                    {
                        decoded += "0";
                    }
                }
                decoded += decoded_part;
            }

            //Console.WriteLine(alarmID);

            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine("RawData: " + rawData);

            var decodedForPrinting = "";

            for (int i = 0; i <= decoded.Length - 1; i++)
            {
                if (i == 0 || i % 2 == 1)
                {
                    decodedForPrinting += decoded[i].ToString();
                }
                else
                {
                    decodedForPrinting += " " + decoded[i].ToString();
                }
            }

            Console.WriteLine("Decode:  " + decodedForPrinting);

            var hexaNumbToText = "";
            for (int i = 16; i <= decoded.Length - 1; i++)
            {
                hexaNumbToText = "";
                byte[] dataBytes = FromHex(decoded[i].ToString() + decoded[i+1].ToString());
                hexaNumbToText = Encoding.ASCII.GetString(dataBytes);
                if (hexaNumbToText.All(c => Char.IsLetterOrDigit(c) || c.Equals('.')))
                {
                    finalText += Encoding.ASCII.GetString(dataBytes);
                }
                else
                {
                    finalText += "<" + decoded[i].ToString() + decoded[i + 1].ToString() + ">";
                }
                i++;
            }

            Console.WriteLine("Text:    " + finalText);

            //INFO : printing in blocks
            //var dataBlocks_string = "";
            //for (int i = 0; i <= dataBlocks.Count - 1; i++)
            //{
            //    dataBlocks_string += dataBlocks[i].ToString() + " ";
            //}
            //Console.WriteLine(dataBlocks_string);

            rawData = "";
            counter = 0;
            alarmID = "";
            decoded = "";
            finalText = "";
        }

        public static byte[] FromHex(string hex)
        {
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }
        //Till unused!
        //public static byte[] StringToByteArray(string hex)
        //{
        //    return Enumerable.Range(0, hex.Length)
        //                     .Where(x => x % 2 == 0)
        //                     .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
        //                     .ToArray();
        //}
    }
}
