using System;

using MediatR;

namespace MyCardNuke.Commands
{
    public class AddCard : IRequest<bool>
    {
        public AddCard()
        {
        }

        public int Id { get; set; }
        public Guid GuidCard { get; set; }
        public string LastFour { get; set; }

        public double Total { get; set; }

    }
}
