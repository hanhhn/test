using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firebase
{
    class Program
    {
        static void Main(string[] args)
        {
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("D:/Projects/note/wewe-7711d-0931806c8bec.json"),
            });

            while (true)
            {
                Console.WriteLine(defaultApp.Name);
                push(defaultApp).Wait();
                Console.ReadKey();
                Console.WriteLine("retry send message");
            }
        }

        static async Task push(FirebaseApp app)
        {
            // See documentation on defining a message payload.
            var message = new Message()
            {
                Token = "eoeQnUGWvw9I_urbGBA1GN:APA91bHFVkYaTvqEGtUNEXp-8R-VPjRhauOuNz9x-DaOJ5wWwNefQcaQLZt4GWlUdbSnPGoZ546Rt0Aw006SMLgS5-NH3yQ-q1ydgJTErLfSfCd9g5F-D969471C13S6iC2TOU3_-yRZ",
                Notification = new Notification
                {
                    Title = "Test title " + Guid.NewGuid().ToString(),
                    Body = "body test content",
                },
                Data = new Dictionary<string, string>
                {
                    { "1", Guid.NewGuid().ToString() },
                    { "2", Guid.NewGuid().ToString() },
                }
            };

            var firebaseMessaging = FirebaseMessaging.GetMessaging(app);
            string response = await firebaseMessaging.SendAsync(message);
            Console.WriteLine("Successfully sent message: " + response);
        }
    }
}
