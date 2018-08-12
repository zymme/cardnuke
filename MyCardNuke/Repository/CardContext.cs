﻿using System;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;

using MyCardNuke.Entities;

namespace MyCardNuke.Repository
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
