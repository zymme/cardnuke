using System;
using System.Net;
using System.Text;
using System.Threading;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using MyCardNukeDataLib.Entities;
using MyCardNukeDataLib.Repository;
using MyCardNukeSub.Options;
using Newtonsoft.Json;

namespace MyCardNukeSub.Subscriptions
{
    public class PersistentSubcription
    {
        protected const string EVENTSTORE_IP = "127.0.0.1";
        protected const int EVENSTORE_PORT = 1113;
        protected const string STREAM = "cardtx";
        protected const string STREAM_GROUP_CARDTXGROUP = "cardtxgroup";

        private IEventStoreConnection _connection;
        private readonly ICardRepository _cardRepository;
       
        public PersistentSubcription(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;

            try
            {
                ConnectionSettings connectionSettings =
                    ConnectionSettings.Create()
                                      .SetDefaultUserCredentials(new UserCredentials("admin", "changeit"))
                                      //.EnableVerboseLogging()
                                      .UseConsoleLogger()
                                      .Build();
                IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Parse(EVENTSTORE_IP), EVENSTORE_PORT);

                _connection = EventStoreConnection.Create(connectionSettings,
                                                          ipendpoint, STREAM);

                _connection.ConnectAsync().Wait();

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in ReadFromEventStore() ctor: {e.Message}");
                throw e;
            }
        }


        public void Start()
        {
            Console.WriteLine("In Start() PersistentSubscription ...");
            ProcessChargeEvents();
        }

        private void ProcessChargeEvents()
        {
            try 
            {
                _connection.ConnectToPersistentSubscription(STREAM, STREAM_GROUP_CARDTXGROUP, async (_, x) =>
                {
                    
                    Console.WriteLine("Received an event!");
                    var data = Encoding.ASCII.GetString(x.Event.Data);

                    ChargeCard chargeCard = JsonConvert.DeserializeObject<ChargeCard>(data);

                    Console.WriteLine($"event received : {JsonConvert.SerializeObject(chargeCard)}");

                    var binsert = await _cardRepository.InsertCharge(chargeCard);
                    if(!binsert)
                    {
                        Console.WriteLine("Insert Charge on card unsuccessful");
                        throw new Exception("Insert Charge on card unsuccessful");
                    }

                    Console.WriteLine($"Insert Charge on card successful: cardid={chargeCard.id}");

                }, subscriptionDropped: null, userCredentials: null, bufferSize: 10, autoAck: true);

                Console.WriteLine("waiting for events. press enter to exit");
                Console.ReadLine();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error in ProcessEvents: {e.Message}");
                throw e;
            }

        }

        public void Close()
        {
            if (_connection != null)
                _connection.Close();
        }


    }
}
