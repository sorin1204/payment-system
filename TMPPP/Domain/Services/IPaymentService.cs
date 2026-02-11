using TMPPP.Domain.Entities;
using TMPPP.Domain.Interfaces;
using TMPPP.Domain.ValueObjects;

namespace TMPPP.Domain.Services;

public interface IPaymentService
{
    Payment CreatePayment(Guid invoiceId, Money amount);
    PaymentResult ProcessPayment(Guid paymentId, IPaymentMethod method);
}
