using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MyCardNuke.Commands;
using MyCardNuke.Domain;
using MyCardNuke.Repository;
using MyCardNuke.Entities;

using Newtonsoft.Json;

namespace MyCardNuke.Handlers
{
    public class ChargeCardHandler : IRequestHandler<ChargeCreditCard, bool>
    {

        private readonly IEventStoreCard _eventStoreCard;
        private readonly ILogger<ChargeCardHandler> _logger;
        private readonly ICardRepository _cardRepository;

        public ChargeCardHandler(IEventStoreCard eventStoreCard, ILogger<ChargeCardHandler> logger,
                              ICardRepository cardRepository)
        {
            _eventStoreCard = eventStoreCard;
            _logger = logger;
            _cardRepository = cardRepository;
        }


        public async Task<bool> Handle(ChargeCreditCard request, CancellationToken token)
        {
            try
            {
                _logger.LogInformation($"Handle ChargeCreditCard: {JsonConvert.SerializeObject(request)}");

                var chargeCard = new ChargeCard
                {
                    charge_amount = request.amount,
                    charge_date = DateTime.Now,
                    guid_card = request.guid_card
                };

                // persist to repository
                if(!await _cardRepository.InsertCharge(chargeCard))
                {
                    _logger.LogError("Error saving charge card information");
                    throw new Exception("Error saving charge card information");
                }

                // record event in eventstore


            }
            catch(Exception e)
            {
                _logger.LogError($"Error occured in Handle: {e.Message}");
            }
            throw new NotImplementedException();
        }

    }
}
