using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;

namespace TMPPP.Domain.Behavioral.Chain;

public sealed class InvoiceValidationHandler : PaymentHandlerBase
{
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceValidationHandler(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    public override PaymentResult Handle(PaymentChainContext context)
    {
        if (context.Payment is null)
        {
            return Fail(context, "payment_context_missing", "Contextul nu contine plata curenta.");
        }

        var invoice = _invoiceRepository.GetById(context.Payment.InvoiceId);
        if (invoice is null)
        {
            return Fail(context, "invoice_not_found", $"Factura {context.Payment.InvoiceId} asociata platii nu exista.");
        }

        context.Invoice = invoice;
        Pass(context, $"Factura {invoice.Id} a fost validata in lant.");
        return Continue(context);
    }
}
