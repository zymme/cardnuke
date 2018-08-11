using System;
namespace MyCardNuke.Models
{
    public class Card
    {
        public Card()
        {
        }

        public int Id { get; set; }
        public string GuidCard { get; set; }
        public string LastFour { get; set; }

        public float total { get; set; }

    }
}
