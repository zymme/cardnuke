using System;
namespace MyCardNuke.Models
{
    public class Card
    {
        public Card()
        {
        }

        public string guid_card { get; set; }
        public string LastFour { get; set; }


        public void CardPaid(Guid cardId, float amount) 
        {
            
        }
    }
}
