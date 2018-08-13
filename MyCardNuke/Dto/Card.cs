using System;

namespace MyCardNuke.Dto
{
    public class Card
    {
        public int Id { get; set; }
        public Guid GuidCard { get; set; }
        public string LastFour { get; set; }
        public DateTime CreateDate { get; set; }
        public double Total { get; set; }

    }
}
