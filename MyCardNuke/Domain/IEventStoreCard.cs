using System;
using System.Threading.Tasks;
using MyCardNuke.Commands;

namespace MyCardNuke.Domain
{
    public interface IEventStoreCard
    {
        Task<bool> Connect();
        Task<bool> WriteNewCardToStream(AddCard card);
        Task<bool> WritePayCardToStream(PayCard card);
        bool Close();
    }
}
