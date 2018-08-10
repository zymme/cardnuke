using System;
using System.Threading.Tasks;

using MyCardNuke.Models;

namespace MyCardNuke.Domain
{
    public interface IEventStoreCard
    {
        Task<bool> Connect();
        Task<bool> WriteCardToStream(Card card);
        bool Close();
    }
}
