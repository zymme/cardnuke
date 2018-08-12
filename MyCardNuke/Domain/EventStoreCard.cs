using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using EventStore.ClientAPI;

using System.Net;
using MyCardNuke.Entities;
using Newtonsoft.Json;
using EventStore.ClientAPI.SystemData;
using MyCardNuke.Commands;

namespace MyCardNuke.Domain
{
    public class EventStoreCard : IEventStoreCard
    {
        private readonly ILogger<EventStoreCard> _logger;
        private readonly IConfiguration _configuration;
        private IEventStoreConnection connection;
        private string _stream;

        public EventStoreCard(ILogger<EventStoreCard> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _stream = _configuration["EventStoreSettings:Stream"];
        }

        public async Task<bool> Connect()
        {
            _logger.LogInformation("Attempting connection to EventStore ...");

            try
            {
                var connString = _configuration["EventStoreSettings:Server"];
                Uri eventStoreUri = new Uri(connString);

                ConnectionSettings connectionSettings = ConnectionSettings
                    .Create()
                    .SetReconnectionDelayTo(TimeSpan.FromMilliseconds(1000))
                    .SetHeartbeatTimeout(TimeSpan.FromMilliseconds(500))
                    .SetDefaultUserCredentials(new UserCredentials("admin", "changeit"))
                    .EnableVerboseLogging()
                    .UseConsoleLogger()
                    .Build();

                IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Parse(_configuration["EventStoreSettings:ServerIP"]),
                                                       int.Parse(_configuration["EventStoreSettings:Port"]));
                
                 connection = EventStoreConnection.Create(connectionSettings,
                                                          ipendpoint,
                                                          _stream);

                await connection.ConnectAsync();

                return true;
            }
            catch(Exception e)
            {
                _logger.LogError($"Error in connecting to EventStore ... {e.Message}");
                Console.WriteLine($"Error in connecting to EventStore ... {e.Message}");
                throw e;
            }
        }


        public async Task<bool> WriteNewCardToStream(Card card)
        {
            try
            {
                //ConnectionSettings connectionSettings = ConnectionSettings
                //    .Create()
                //    .SetReconnectionDelayTo(TimeSpan.FromMilliseconds(1000))
                //    .SetHeartbeatTimeout(TimeSpan.FromMilliseconds(500))
                //    .SetDefaultUserCredentials(new UserCredentials("admin", "changeit"))
                //    .EnableVerboseLogging()
                //    .UseConsoleLogger()
                //    .Build();

                //using(var conn = EventStoreConnection.Create(connectionSettings, new IPEndPoint(IPAddress.Loopback, 1113),
                //                                          _stream))
                //{
                //    await conn.ConnectAsync();
                //    //connection.AppendToStreamAsync("CardNukeStream")
                //    Guid newCardGuid = Guid.NewGuid();

                //    byte[] cardBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(card));

                //    var eventData = new EventData(
                //        newCardGuid, "newCardAdded", true, cardBytes, cardBytes);

                //    conn.AppendToStreamAsync(
                //        _stream, ExpectedVersion.Any, eventData).Wait();


                //}

                Guid newCardGuid = Guid.NewGuid();

                byte[] cardBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(card));

                var eventData = new EventData(
                    newCardGuid, "newCardAdded", true, cardBytes, cardBytes);

                await this.connection.AppendToStreamAsync(_stream, 
                                                          ExpectedVersion.Any, 
                                                          eventData);
               
                return true;
                
            }
            catch(Exception e)
            {
                _logger.LogError($"Error occured in write to stream : {e.Message}");
                throw e;
            }

        }


        public bool Close() 
        {
            try
            {
                if (this.connection != null)
                    connection.Close();
            }
            catch(Exception e)
            {
                _logger.LogError($"Error in Close(): {e.Message}");
            }

            return true;
        }

        public Task<bool> WritePayCardToStream(PayCreditCard card)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> WriteChargeCardToStream(ChargeCard card)
        {

            try 
            {
                Guid chargeEventStoreGuid = Guid.NewGuid();

                byte[] cardBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(card));

                var eventData = new EventData(chargeEventStoreGuid, "chargeCreditCard", true,
                                              cardBytes, cardBytes);

                var writeResult = await this.connection.AppendToStreamAsync("cardtx-"+card.guid_card.ToString(),
                                                                            ExpectedVersion.Any, eventData);
                
                return true;
            }
            catch(Exception e)
            {
                _logger.LogError($"Error in WriteChargeCardToStream: {e.Message}");
                throw e;
            }


        }
    }
}
