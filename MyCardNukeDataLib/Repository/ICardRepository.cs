using System;
using System.Threading.Tasks;

using MyCardNukeDataLib.Entities;


namespace MyCardNukeDataLib.Repository
{
    public interface ICardRepository
    {
        Task<bool> Insert(Card card);
        Task<bool> InsertCharge(ChargeCard charge);
        bool GetByLastFour(string lastfour);
    }
}
