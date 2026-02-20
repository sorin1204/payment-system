using TMPPP.Domain.Interfaces;
using TMPPP.Domain.Processors;
using TMPPP.Domain.Services;

namespace TMPPP.Domain.Factories.AbstractFactory;

public abstract class PaymentDomainFactory : IPaymentDomainFactory, IDisposable
{
    public abstract IInvoiceRepository CreateInvoiceRepository();
    public abstract IPaymentRepository CreatePaymentRepository();
    public abstract INotificationService CreateNotificationService();

    public virtual IPaymentProcessor CreatePaymentProcessor(
        IPaymentRepository paymentRepository,
        INotificationService notificationService)
    {
        return new PaymentProcessor(paymentRepository, notificationService);
    }

    public virtual IPaymentService CreatePaymentService(
        IPaymentRepository paymentRepository,
        IInvoiceRepository invoiceRepository,
        IPaymentProcessor paymentProcessor)
    {
        return new PaymentService(paymentRepository, invoiceRepository, paymentProcessor);
    }

    public virtual void Dispose()
    {
    }
}
