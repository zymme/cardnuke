using System;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyCardNukeDataLib.Entities
{

    [Table("charge_card")]
    public class ChargeCard
    {
        public ChargeCard()
        {
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public DateTime charge_date { get; set; }
        public double charge_amount { get; set; }
        public Guid guid_card { get; set; }

    }
}
