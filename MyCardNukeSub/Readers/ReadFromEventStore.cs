using System;
using System.Collections.Generic;
using System.Net;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace MyCardNukeSub.Readers
{
    public class ReadFromEventStore
    {

        protected const string EVENTSTORE_IP = "127.0.0.1";
        protected const int EVENSTORE_PORT = 1113;
        protected const string STREAM = "CardNukeStream";

        private IEventStoreConnection _connection;

        public ReadFromEventStore()
        {
            try
            {
                ConnectionSettings connectionSettings = 
                    ConnectionSettings.Create()
                                      .SetDefaultUserCredentials(new UserCredentials("admin", "changeit"))
                                      .EnableVerboseLogging()
                                      .UseConsoleLogger()
                                      .Build();
                IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Parse(EVENTSTORE_IP),EVENSTORE_PORT);

                _connection = EventStoreConnection.Create(connectionSettings,
                                                          ipendpoint, STREAM);

                _connection.ConnectAsync().Wait();               

            }
            catch(Exception e)
            {
                Console.WriteLine($"Error in ReadFromEventStore() ctor: {e.Message}");
                throw e;
            }
        }


        public void ReadFromBeginning()
        {
            Console.WriteLine("About to read from beginning");

            try 
            {
                List<ResolvedEvent> streamEvents = new List<ResolvedEvent>();
                StreamEventsSlice currentSlice;
                long nextSliceStart = StreamPosition.Start;

                do
                {
                    currentSlice = _connection.ReadStreamEventsForwardAsync(
                        STREAM, nextSliceStart, 200, false).Result;

                    nextSliceStart = currentSlice.NextEventNumber;

                    streamEvents.AddRange(currentSlice.Events);

                } while (!currentSlice.IsEndOfStream);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error in read from beginning: {e.Message}");
            }

        }

    }
}
