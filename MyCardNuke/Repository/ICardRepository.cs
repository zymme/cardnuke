﻿using System;
using System.Threading.Tasks;

using MyCardNuke.Entities;


namespace MyCardNuke.Repository
{
    public interface ICardRepository
    {
        Task<bool> Insert(Card card);
        Task<bool> InsertCharge(ChargeCard charge);
        bool GetByLastFour(string lastfour);
    }
}
