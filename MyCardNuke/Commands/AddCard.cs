using System;

using MyCardNuke.Dto;

using MediatR;

namespace MyCardNuke.Commands
{
    public class AddCard : IRequest<Card>
    {
        public AddCard()
        {
        }
               
        public string LastFour { get; set; }

        public double Total { get; set; }

    }
}
