using System;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;



namespace MyCardNuke.Entities
{
    [Table("card")]
    public class Card
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime create_date { get; set; }
        public string last_four { get; set; }
        public Guid guid_card { get; set; }
        public double total { get; set; }
    }
}
