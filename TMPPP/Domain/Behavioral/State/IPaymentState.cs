using TMPPP.Domain.Entities;
using TMPPP.Domain.Enums;

namespace TMPPP.Domain.Behavioral.State;

public interface IPaymentState
{
    PaymentStatus Status { get; }
    string Name { get; }
    IReadOnlyCollection<string> AllowedActions { get; }
    PaymentStateTransitionResult Handle(Payment payment, PaymentStateAction action);
}
