using TMPPP.Domain.Behavioral.Iterator;

namespace TMPPP.Controllers;

public class IteratorController
{
    public static IteratorDemoResult BuildPaymentIteratorDemo(int takeCount)
    {
        var collection = new PaymentBatchCollection(new[]
        {
            new PaymentQueueItem("PAY-2401", "Ana Popescu", 120m, "RON", "card", "pending"),
            new PaymentQueueItem("PAY-2402", "Victor Rusu", 580m, "RON", "bank", "processed"),
            new PaymentQueueItem("PAY-2403", "Lia Marin", 75m, "EUR", "cash", "refunded"),
            new PaymentQueueItem("PAY-2404", "Andrei Ionescu", 910m, "EUR", "card", "processed"),
            new PaymentQueueItem("PAY-2405", "Mara Enache", 260m, "RON", "bank", "pending")
        });

        var iterator = collection.CreateIterator();
        var first = iterator.First();
        var traversal = new List<IteratorTraversalStep>();
        var position = 1;
        var maxItems = Math.Max(1, takeCount);

        while (iterator.HasMore() && traversal.Count < maxItems)
        {
            traversal.Add(new IteratorTraversalStep(position, iterator.Next()));
            position++;
        }

        return new IteratorDemoResult(
            collection.Count,
            first,
            iterator.Current(),
            traversal,
            "Iteratorul parcurge secvential lotul de plati printr-o interfata standardizata, fara sa expuna structura interna a colectiei PaymentBatchCollection.");
    }
}
