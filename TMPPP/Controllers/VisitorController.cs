using TMPPP.Domain.Behavioral.Visitor;

namespace TMPPP.Controllers;

public static class VisitorController
{
    public static VisitorDemoResult BuildPaymentVisitorDemo()
    {
        var batch = CompositeController.BuildPaymentBatch();

        var metricsVisitor = new PaymentMetricsVisitor();
        batch.Accept(metricsVisitor);

        var exportVisitor = new PaymentExportVisitor();
        batch.Accept(exportVisitor);

        return new VisitorDemoResult(
            batch.Name,
            metricsVisitor.Result,
            exportVisitor.Entries.ToList(),
            "Structura Composite ramane neschimbata, iar operatiile noi sunt adaugate prin visitori separati: unul calculeaza metrici, altul exporta platile intr-o forma liniara. Astfel algoritmii sunt separati de structura de date.");
    }
}
