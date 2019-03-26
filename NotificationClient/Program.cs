using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string protocole = "https";
            string host = "localhost:44357";

            var connection = new HubConnectionBuilder()
                .WithUrl($"{protocole}://{host}/api/notification").Build();

            connection.Closed += ex =>
            {
                Console.WriteLine($"Connection Closed: {ex?.Message}");
                return Task.CompletedTask;
            };

            connection.On<ContactNotificationDTO>("ContactModified", notif =>
            {
                Console.WriteLine($"Received notification {notif}");
            });

            Console.WriteLine("Connecting...");
            await connection.StartAsync();
            Console.WriteLine("Connection waiting");

            Console.ReadLine();
        }
    }


    public class NotificationDTO
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public int RecipientId { get; set; }
        /// <summary>
        /// The person who caused the notification to occure
        /// </summary>
        public int SenderId { get; set; }
    }

    public enum ContactNotificationType
    {
        ContactModified,
        ContactDeleted
    }

    public class ContactNotificationDTO : NotificationDTO
    {
        public ContactNotificationType Type { get; set; }
        public int ContactId { get; set; }
        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }
}
