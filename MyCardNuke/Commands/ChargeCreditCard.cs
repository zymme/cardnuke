using System;
using MediatR;

namespace MyCardNuke.Commands
{
    public class ChargeCreditCard : IRequest<bool>
    {
        public Guid guid_card { get; set; }
        public double amount { get; set; }
        public string subject { get; set; }
        public DateTime charge_date { get; set; }
    }
}
