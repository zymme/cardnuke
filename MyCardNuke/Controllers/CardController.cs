using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;

using MyCardNuke.Domain;
using MyCardNuke.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyCardNuke.Controllers
{
    [Route("api/card")]
    public class CardController : Controller
    {
        private readonly IEventStoreCard _eventStoreCard;
        private readonly ILogger<CardController> _logger;

        public CardController(IEventStoreCard eventStoreCard, ILogger<CardController> logger)
        {
            _eventStoreCard = eventStoreCard;
            _logger = logger;
        }

        [HttpPost("AddCard")]
        public async Task<IActionResult> AddCard(Card card)
        {
            try 
            {
                _logger.LogInformation("Made connection to EventStore ...");

                // lets add an event to the event store for now here for testing purposes
                if(await _eventStoreCard.Connect())
                {
                    var writeResult = await _eventStoreCard.WriteCardToStream(card);

                    _eventStoreCard.Close();

                    if (writeResult)
                        _logger.LogInformation("Event writtent to stream");
                }
                else 
                {
                    _logger.LogInformation("Problem in connecting to event store");
                }



            }
            catch(Exception e)
            {
                Console.WriteLine($"Exception in AddCard {e.Message}");
                _logger.LogError($"Exception in AccCard {e.Message}");
            }
            return Ok("Not Implemented Yet");
        }



    }
}
