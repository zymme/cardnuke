using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MyCardNuke.Commands;
using MyCardNuke.Domain;

using MyCardNukeDataLib.Repository;
using MyCardNuke.Dto;

using Newtonsoft.Json;

namespace MyCardNuke.Handlers
{
    public class AddCardHandler : IRequestHandler<AddCard, Card>
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

        public async Task<Card> Handle(AddCard request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Handle AddCard : {JsonConvert.SerializeObject(request)}");

                // check if card (last_four) exists already - if so throw ApplicationException
                if (_cardRepository.GetByLastFour(request.LastFour))
                    throw new ApplicationException($"The card [{request.LastFour}] already exists ");
                
                var newCard = new MyCardNukeDataLib.Entities.Card
                {
                    guid_card = Guid.NewGuid(),
                    create_date = DateTime.Now,
                    last_four = request.LastFour,
                    total = request.Total
                    
                };

                if(!await _cardRepository.Insert(newCard))
                {
                    _logger.LogError("Error saving card information");
                    throw new Exception("Error saving card information");
                }

                // lets add an event to the event store for now here for testing purposes
                if (await _eventStoreCard.Connect())
                {
                    var writeResult = await _eventStoreCard.WriteNewCardToStream(newCard);                   

                    if (writeResult)
                        _logger.LogInformation("Event written to stream");
                }
                else
                {
                    _logger.LogInformation("Problem in connecting to event store");
                }
                _eventStoreCard.Close();

                return new Card
                {
                    CreateDate = newCard.create_date,
                    GuidCard = newCard.guid_card, 
                    Id = newCard.id,
                    LastFour = newCard.last_four,
                    Total = newCard.total
                };

            }
            catch(Exception e)
            {
                _logger.LogError($"Error in Handle AddCardHandler: {e.Message}");
                _eventStoreCard.Close();
                throw e;
            }


        }
    }
}
