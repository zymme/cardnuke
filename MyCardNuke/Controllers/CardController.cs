using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;

using MyCardNuke.Domain;
using MyCardNuke.Models;
using MyCardNuke.Commands;

using MediatR;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using Newtonsoft.Json;


namespace MyCardNuke.Controllers
{
    [Route("api/card")]
    public class CardController : Controller
    {
        private readonly ILogger<CardController> _logger;
        private readonly IMediator _mediator;

        public CardController(ILogger<CardController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("AddCard")]
        public async Task<IActionResult> AddCard([FromBody]AddCard card)
        {
            try 
            {
                _logger.LogInformation("sending call to handle add card command ...");

                bool addcard = await _mediator.Send(card);

                if (addcard)
                    return Ok("Card Added");

                return BadRequest("Card Not Added");

                //// lets add an event to the event store for now here for testing purposes
                //if(await _eventStoreCard.Connect())
                //{
                //    var writeResult = await _eventStoreCard.WriteCardToStream(card);

                //    _eventStoreCard.Close();

                //    if (writeResult)
                //        _logger.LogInformation("Event writtent to stream");
                //}
                //else 
                //{
                //    _logger.LogInformation("Problem in connecting to event store");
                //}



            }
            catch(ApplicationException ae)
            {
                return BadRequest(ae.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Exception in AddCard {e.Message}");
                _logger.LogError($"Exception in AccCard {e.Message}");
                return StatusCode(500, e.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> ChargeCard(ChargeCreditCard charge)
        {
            try 
            {
                _logger.LogInformation($"Entered ChargeCard processing : {JsonConvert.SerializeObject(charge)}");


            }
            catch(Exception e)
            {
                
            }
            throw new NotImplementedException();
        }



    }
}
