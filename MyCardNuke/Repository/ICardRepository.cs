using System;
using System.Threading.Tasks;

using MyCardNuke.Entities;


namespace MyCardNuke.Repository
{
    public interface ICardRepository
    {
        Task<bool> Insert(Card card);
        bool GetByLastFour(string lastfour);
    }
}
