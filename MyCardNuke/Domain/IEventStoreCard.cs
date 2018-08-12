using System;
using System.Threading.Tasks;
using MyCardNuke.Commands;

using MyCardNuke.Entities;

namespace MyCardNuke.Domain
{
    public interface IEventStoreCard
    {
        Task<bool> Connect();
        Task<bool> WriteNewCardToStream(Card card);
        Task<bool> WritePayCardToStream(PayCard card);
        bool Close();
    }
}
