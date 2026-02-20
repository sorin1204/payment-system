using TMPPP.Domain.Interfaces;
using TMPPP.Infrastructure.InMemory;
using TMPPP.Infrastructure.Notifications;

namespace TMPPP.Domain.Factories.AbstractFactory;

public sealed class InMemoryPaymentDomainFactory : PaymentDomainFactory
{
    private readonly InMemoryInvoiceRepository _invoiceRepository = new();
    private readonly InMemoryPaymentRepository _paymentRepository = new();
    private readonly ConsoleNotificationService _notificationService = new();

    public override IInvoiceRepository CreateInvoiceRepository()
    {
        return _invoiceRepository;
    }

    public override IPaymentRepository CreatePaymentRepository()
    {
        return _paymentRepository;
    }

    public override INotificationService CreateNotificationService()
    {
        return _notificationService;
    }
}
