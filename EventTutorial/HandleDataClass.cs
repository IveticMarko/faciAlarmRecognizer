﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace EventTutorial
{
    class HandleDataClass
    {
        private string lanCode = "";
        private string rawData = "";
        private List<String> dataBlocks = new List<string>();  
        private string data = "";
        private string alarmID = "";
        private string decoded = "";
        private int counter = 0;
        private string finalText = "";

        private const string CONNECTION_STRING = "Server=.\\SQLEXPRESS;Database=FaciDB;Integrated Security=true";
        private const string RECORD_SEPARATOR = @"<1E>";
        private const string UNIT_SEPARATOR = @"<1F>";
        private const string STATUS_CODE_STAND_BY = "sb";

        public void SubscribeToEvent(Server server, String lanCode)
        {
            server.DataReceivedEvent += server_DataReceivedEvent;
            this.lanCode = lanCode;
        }

        private void server_DataReceivedEvent(object sender, ReceivedDataArgs args)
        {
            //TODO: put this processing in separate function maybe.
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
            //TODO: put this in try or do the checks there.
            DataProcessing(finalText);
            LoggDataToDB(finalText);

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

        private void LoggDataToDB(string data)
        {
            SqlConnection conn = GetDBConnection();

            const string SQL_LOGG_DATA = 
                @"INSERT INTO loggingtable(Data, EditDate)
                  VALUES(@Data, GETUTCDATE())";
            
            using (SqlCommand cmd = new SqlCommand(SQL_LOGG_DATA, conn))
            {
                cmd.Parameters.Add("@Data", SqlDbType.NVarChar, 500).Value = data;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private SqlConnection GetDBConnection()
        {
            SqlConnection conn = new SqlConnection(CONNECTION_STRING);
            return conn;
        }

        private void DataProcessing(string data)
        {
            string[] dataBlocks = Regex.Split(data, RECORD_SEPARATOR);
            foreach (var block in dataBlocks)
            {
                if (block.Length > 0)
                {
                    var startIndex = 0;
                    //In case when we have <00> at the beggining of the data string.
                    if (block.Substring(0, 4) == "<00>")
                    {
                        if (block.Length == 4)
                        {
                            continue;
                        }
                        startIndex = 4;
                    }
                    var roomID_1 = Int32.Parse(block.Substring(startIndex, 4).Replace("<", "").Replace(">",""));
                    var roomID_2 = Int32.Parse(block.Substring(startIndex + 4, 4).Replace("<", "").Replace(">", ""));
                    var roomID = Int32.Parse(roomID_1.ToString() + roomID_2.ToString());
                    var groupController = block.Substring(startIndex + 8, 2);
                    var group = block.Substring(startIndex + 10, 1);
                    var roomName = block.Substring(startIndex + 11, 4);
                    
                    int status = -1;
                    try
                    {
                        status = Int32.Parse(block.Substring(startIndex + 15, 4).Replace("<", "").Replace(">", ""));
                    }
                    catch (Exception)
                    {
                        //TODO: Sometimes here breaks!!! TEST IT
                    }

                    if (status != -1 && status != (int) RoomStatuses.sb)
                    {
                        String suplementData = null;
                        String additionalData = null;

                        try
                        {
                            suplementData = block.Substring(startIndex + 19, 2);
                        }
                        catch (Exception)
                        {
                            //just continue the process of data processing
                        }

                        //If length of data is greater than 21, that mean that we have some additional data
                        if (block.Length > startIndex + 21)
                        {
                            // <20> == space 
                            additionalData = block.Substring(startIndex + 21, block.Length - (startIndex + 21)).Replace("<20>", " ");
                        }

                        if (status != (int)RoomStatuses.aw && status != (int)RoomStatuses.aw2)
                        {
                            EnterData(roomID, groupController, Int32.Parse(group), roomName, status, suplementData, additionalData);
                        }
                        else if (status == (int)RoomStatuses.aw) 
                        {
                            loadCurrentStateAndSendNotification(groupController, Int32.Parse(group));
                        }
                        else if (status == (int)RoomStatuses.aw2)
                        {
                            //TODO: Disable sending, and check if that is correct.
                        }
                    }
                }
            }
        }

        private void EnterData(int roomID, String groupController, int group, String roomName, int statusID, String suplementData, String additionalData)
        {
            try
            {
                SqlConnection conn = GetDBConnection();

                const string SQL_ENTERED_DATA =
                    @"IF NOT EXISTS (SELECT 1
			                         FROM entereddatatable
			                         WHERE RoomID = @RoomID
			                               AND GroupController = @GroupController
					                       AND [Group] = @Group
					                       AND SuplementData = @SuplementData
					                       AND ActualStatusID = @ActualStatusID)
                      BEGIN
                      INSERT INTO entereddatatable
                      ([RoomID], [GroupController], [Group], [RoomName], [StatusID], [SuplementData], [AdditionalData], [ActualStatusID], [CreationDate])
                      VALUES
                      (@RoomID, @GroupController, @Group, @RoomName, @StatusID, @SuplementData, @AdditionalData, @ActualStatusID, GETUTCDATE())
                      END";

                using (SqlCommand cmd = new SqlCommand(SQL_ENTERED_DATA, conn))
                {
                    cmd.Parameters.Add("@RoomID", SqlDbType.Int).Value = roomID;
                    cmd.Parameters.Add("@GroupController", SqlDbType.NVarChar, 2).Value = groupController;
                    cmd.Parameters.Add("@Group", SqlDbType.TinyInt).Value = group;
                    cmd.Parameters.Add("@RoomName", SqlDbType.NVarChar, 4).Value = roomName;
                    cmd.Parameters.Add("@StatusID", SqlDbType.TinyInt).Value = statusID;
                    cmd.Parameters.Add("@SuplementData", SqlDbType.NVarChar, 2).Value = (String.IsNullOrEmpty(suplementData) ? "" : suplementData);
                    cmd.Parameters.Add("@AdditionalData", SqlDbType.NVarChar, 100).Value = (String.IsNullOrEmpty(additionalData) ? "" : additionalData);
                    cmd.Parameters.Add("@ActualStatusID", SqlDbType.Int).Value = ActualStatus.Active;

                    conn.Open();

                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void loadCurrentStateAndSendNotification(String groupController, int group)
        {
            //TODO: think about try. How many of them we need here and what to do with errors.
            try
            {
                SqlConnection conn = GetDBConnection();

                const string SQL_LOAD_CURRENT_STATE =
                    @"UPDATE entereddatatable
                      SET ActualStatusID = @ProcessedActualStatus, 
                          EditDate = GETUTCDATE()
                      OUTPUT inserted.RoomID, inserted.RoomName, inserted.StatusID, inserted.SuplementData, inserted.AdditionalData
                      WHERE GroupController = @GroupController
                            AND [Group] = @Group
                            AND ActualStatusID = @ActiveActualStatus";
                
                using (SqlCommand cmd = new SqlCommand(SQL_LOAD_CURRENT_STATE, conn))
                {
                    cmd.Parameters.Add("@GroupController", SqlDbType.NVarChar, 2).Value = groupController;
                    cmd.Parameters.Add("@Group", SqlDbType.TinyInt).Value = group;
                    cmd.Parameters.Add("@ProcessedActualStatus", SqlDbType.Int).Value = ActualStatus.Processed;
                    cmd.Parameters.Add("@ActiveActualStatus", SqlDbType.Int).Value = ActualStatus.Active;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                SendingData data = new SendingData
                                {
                                    roomID = Int32.Parse((reader["RoomID"].ToString())),
                                    roomName = (reader["RoomName"].ToString()),
                                    statusID = Int32.Parse((reader["StatusID"].ToString())),
                                    suplementData = (reader["SuplementData"].ToString()),
                                    additionalData = (reader["AdditionalData"].ToString())
                                };

                                var json = ToJSON(data);

                                List<string> listOfTokens;
                                Cache.devices.TryGetValue(group, out listOfTokens);

                                //TODO: this is done just for the test. Try to send message to every device.
                                var token = listOfTokens[0];

                                //TEST: this is token of mine device.
                                //"ct19Qyuc5PI:APA91bH8Otgu1I5kuLLnK0NIpO9ZWJ5bjpA5_iR4nEhWPLl779yx0n1Sm_5WkEXdGkP1oYmacdio97VqcF6L6PKED00b68YSgLd0XpN405IjahUTlrj8spwkHFuRLEV8ZYcrrjrsnfwa"

                                //Start NotificationSender Thread
                                Thread notificationSender = new Thread(() =>
                                    NotificationSender.sendNotification(json, token)); 

                                notificationSender.Start();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string ToJSON(object obj)
        {
            //TODO: try to find dll for this bellow.
            //Namespace: System.Web.Script.Serialization
            //Assembly: System.Web.Extensions.dll

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //return serializer.Serialize(obj);

            return JsonConvert.SerializeObject(obj);
        }

    }
}
