using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Function
{
    public class FunctionHandler
    {
        private string FcmAuthKey;
        private string[] RegistrationIds;

        public FunctionHandler()
        {
            FcmAuthKey = Environment.GetEnvironmentVariable("fcm_auth_key");

            var registrationIds = Environment.GetEnvironmentVariable("registration_ids");
            if (!string.IsNullOrWhiteSpace(registrationIds))
            {
                RegistrationIds = registrationIds.Split(',');
            }
        }

        public void Handle(string input)
        {
            if (string.IsNullOrWhiteSpace(FcmAuthKey))
            {
                Console.WriteLine("FCM auth key is invalid");
                return;
            }

            if (!string.IsNullOrWhiteSpace(input))
            {
                dynamic parsedBody = JsonConvert.DeserializeObject(input);

                var postData = BuildMessageData(parsedBody);
                var topicResponse = SendMessageToTopic("messages", postData);
                var idsResponse = SendMessageToRegistrationIds(postData);

                Console.WriteLine(new
                {
                    topic_response = JsonConvert.DeserializeObject(topicResponse),
                    ids_response = JsonConvert.DeserializeObject(idsResponse)
                });
            }
            else
            {
                Console.WriteLine("Please send some data");
            }
        }

        private FcmMessageData BuildMessageData(dynamic parsedBody)
        {
            var messageData = new FcmMessageData
            {
                To = (string)parsedBody.to,
                Message = (string)parsedBody.message,
                Title = (string)parsedBody.title,
                Type = (string)parsedBody.type,
                Task = (string)parsedBody.task,
                TaskData = (string)parsedBody.task_data,
                FromDevice = string.IsNullOrWhiteSpace((string)parsedBody.from_device)
                    ? "default"
                    : (string)parsedBody.from_device
            };

            return messageData;
        }

        private string SendMessageToTopic(string topic, FcmMessageData messageData)
        {
            var fcmMessage = new FcmMessage
            {
                To = $"/topics/{topic}",
                Data = messageData
            };

            return SendMessageToFcm(fcmMessage);
        }

        private string SendMessageToRegistrationIds(FcmMessageData messageData)
        {
            if (RegistrationIds.Length == 0)
            {
                return "No registration IDs";
            }

            var fcmMessage = new FcmMessage
            {
                RegistrationIds = RegistrationIds,
                Data = messageData
            };

            return SendMessageToFcm(fcmMessage);
        }

        private string SendMessageToFcm(FcmMessage message)
        {
            var serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var jsonToPost = JsonConvert.SerializeObject(message, serializerSettings);

            var webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/json");
            webClient.Headers.Add("Authorization", $"key={FcmAuthKey}");

            var postData = Encoding.ASCII.GetBytes(jsonToPost);
            var response = webClient.UploadData("https://fcm.googleapis.com/fcm/send", "POST", postData);
            return Encoding.ASCII.GetString(response);
        }
    }
}
