﻿using System;
using System.Threading.Tasks;
using MyCardNuke.Commands;

using MyCardNukeDataLib.Entities;

namespace MyCardNuke.Domain
{
    public interface IEventStoreCard
    {
        Task<bool> Connect();
        Task<bool> WriteNewCardToStream(Card card);
        Task<bool> WritePayCardToStream(PayCreditCard card);
        Task<bool> WriteChargeCardToStream(ChargeCard card);
        bool Close();
    }
}
