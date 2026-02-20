using TMPPP.Domain.Interfaces;
using TMPPP.Domain.Services;

namespace TMPPP.Domain.Factories.AbstractFactory;

public interface IPaymentDomainFactory
{
    IInvoiceRepository CreateInvoiceRepository();
    IPaymentRepository CreatePaymentRepository();
    INotificationService CreateNotificationService();
    IPaymentProcessor CreatePaymentProcessor(IPaymentRepository paymentRepository, INotificationService notificationService);
    IPaymentService CreatePaymentService(
        IPaymentRepository paymentRepository,
        IInvoiceRepository invoiceRepository,
        IPaymentProcessor paymentProcessor);
}
