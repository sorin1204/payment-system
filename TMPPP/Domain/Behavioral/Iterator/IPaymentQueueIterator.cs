namespace TMPPP.Domain.Behavioral.Iterator;

public interface IPaymentQueueIterator
{
    PaymentQueueItem First();
    PaymentQueueItem Next();
    PaymentQueueItem Current();
    bool HasMore();
}
