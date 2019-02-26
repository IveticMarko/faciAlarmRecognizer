using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace EventTutorial
{
    public class FCMResponse
    {
        public long multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<FCMResult> results { get; set; }
    }
    public class FCMResult
    {
        public string message_id { get; set; }
    }
    class NotificationSender
    {
        private const string AUTHORIZATION_KEY = "AAAAIng5XHY:APA91bFdwSSZ6T9K1nLo1xELEzsIgdNcBaj3-xxJQNHP4KjHxIrbsDkaNiPdIxR9Dc6YU03T9otFGPpRELxPA1OIBT5mDwkFO53q2F7VGuNzvuvO9ezCWxJpt6nzZ1UKbJHFIfvnW6cJ";
        private const string SENDER_ID = "148045913206";

        public static void sendNotification(string message, string clientToken)
        {
            //TODO: !MAYBE! rework this to receive list of client tokens.
            //      Think about that.
            //      It's not maybe necessarily.
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";
            var objNotification = new
            {
                to = clientToken,
                notification = new
                {
                    title = "",
                    body = message
                }
            };
            string jsonNotificationFormat = JsonConvert.SerializeObject(objNotification);

            Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
            tRequest.Headers.Add(string.Format("Authorization:key={0}", AUTHORIZATION_KEY));
            tRequest.Headers.Add(string.Format("Sender:id={0}", SENDER_ID));
            tRequest.ContentLength = byteArray.Length;
            tRequest.ContentType = "application/json";
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String responseFromFirebaseServer = tReader.ReadToEnd();

                            FCMResponse response = JsonConvert.DeserializeObject<FCMResponse>(responseFromFirebaseServer);
                            //if (response.success == 1)
                            //else if (response.failure == 1)
                        }
                    }

                }
            }
        }
    }
}
