using System;

using Microsoft.EntityFrameworkCore;

using MyCardNukeDataLib.Entities;

namespace MyCardNukeDataLib.Context
{
    public class CardContext : DbContext
    {
        public CardContext(DbContextOptions<CardContext> options) : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<ChargeCard> Charges { get; set; }
    }
}
