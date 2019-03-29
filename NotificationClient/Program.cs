using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
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
            string host = "localhost:44359";

            var connection = new HubConnectionBuilder()
                .WithUrl($"{protocole}://{host}/api/notification").Build();

            connection.Closed += ex =>
            {
                Console.WriteLine($"Connection Closed: {ex?.Message}");
                return Task.CompletedTask;
            };

            connection.On<NotificationDto>("Notify", notif =>
            {
                Console.WriteLine($"Received notification:::");
                Console.WriteLine(JsonConvert.SerializeObject(notif, Formatting.Indented).ToString());
            });

            Console.WriteLine("Connecting...");
            await connection.StartAsync();
            Console.WriteLine("Connection waiting");

            Console.ReadLine();
        }
    }

    public class NotificationDto
    {
        public Guid Id { get; set; }
        public NotificationType Type { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Url { get; set; }
        public LightUserDto User { get; set; }
        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }

    public enum NotificationType
    {
        ContactModified,
        ContactDeleted,
        ContactShared,
        ContactAddedToGroup,
        EntiteUserDeleted,
        EventAdded,
        InviteeStatusModified,
        UserAddedToEntite,
    }

    public class LightUserDto
    {
        public Guid Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mail { get; set; }
    }
}