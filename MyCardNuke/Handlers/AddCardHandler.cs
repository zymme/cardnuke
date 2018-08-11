using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MyCardNuke.Commands;
using MyCardNuke.Domain;
using MyCardNuke.Entities;
using MyCardNuke.Repository;

using Newtonsoft.Json;

namespace MyCardNuke.Handlers
{
    public class AddCardHandler : IRequestHandler<AddCard, bool>
    {
        private readonly IEventStoreCard _eventStoreCard;
        private readonly ILogger<AddCardHandler> _logger;
        private readonly ICardRepository _cardRepository;

        public AddCardHandler(IEventStoreCard eventStoreCard, ILogger<AddCardHandler> logger, 
                              ICardRepository cardRepository)
        {
            _eventStoreCard = eventStoreCard;
            _logger = logger;
            _cardRepository = cardRepository;
        }

        public async Task<bool> Handle(AddCard request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Handle AddCard : {JsonConvert.SerializeObject(request)}");

                //TODO: check if card (last_four) exists already - if so throw ApplicationException stating so

                var newCard = new Card
                {
                    guid_card = Guid.NewGuid(),
                    last_four = request.LastFour,
                    total = request.Total
                    
                };

                if(!await _cardRepository.Insert(newCard))
                {
                    _logger.LogError("Error saving card information");
                }



                // lets add an event to the event store for now here for testing purposes
                //if (await _eventStoreCard.Connect())
                //{
                //    var writeResult = await _eventStoreCard.WriteNewCardToStream(request);                   

                //    if (writeResult)
                //        _logger.LogInformation("Event written to stream");
                //}
                //else
                //{
                //    _logger.LogInformation("Problem in connecting to event store");
                //}
                //_eventStoreCard.Close();

                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error in Handle AddCardHandler: {e.Message}");
                _eventStoreCard.Close();
            }

            return false;
        }
    }
}
