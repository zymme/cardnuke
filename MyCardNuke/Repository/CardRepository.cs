using System;
using System.Threading.Tasks;

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


        public async Task<bool> Insert(Card card)
        {
            _cardContext.Add(card);
            return await Save();
        }

        private async Task<bool> Save()
        {
            return (await _cardContext.SaveChangesAsync() > 0);
        }
    }
}
