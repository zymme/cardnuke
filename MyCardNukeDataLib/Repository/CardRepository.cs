using System;
using System.Threading.Tasks;

using System.Linq;
using Microsoft.Extensions.Logging;
using MyCardNuke.Entities;


namespace MyCardNuke.Repository
{
    public class CardRepository : ICardRepository
    {
        private readonly ILogger<CardRepository> _logger;
        private readonly CardContext _cardContext;

        public CardRepository(ILogger<CardRepository> logger, CardContext cardContext)
        {
            _logger = logger;
            _cardContext = cardContext;
        }

        public bool GetByLastFour(string lastfour)
        {
            var card = _cardContext.Cards.Where(x => x.last_four == lastfour)
                                   .FirstOrDefault();
            if (card != null)
                return true;


            return false;
        }


        public async Task<bool> Insert(Card card)
        {
            try 
            {
                _cardContext.Add(card);
                return await Save();
            }
            catch(Exception e)
            {
                _logger.LogError($"Error in Insert: {e.Message}");
                return false;
            }
           
        }

        public async Task<bool> InsertCharge(ChargeCard charge)
        {
            try 
            {
                _cardContext.Add(charge);
                return await Save();
            }
            catch(Exception e)
            {
                _logger.LogError($"Error in Insert: {e.Message}");
                return false;
            }
        }

        private async Task<bool> Save()
        {
            return (await _cardContext.SaveChangesAsync() > 0);
        }
    }
}
