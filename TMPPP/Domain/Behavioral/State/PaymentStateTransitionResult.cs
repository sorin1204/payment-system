using TMPPP.Domain.Enums;

namespace TMPPP.Domain.Behavioral.State;

public sealed record PaymentStateTransitionResult(
    bool Success,
    string Action,
    PaymentStatus PreviousStatus,
    PaymentStatus CurrentStatus,
    string Message,
    bool StateChanged);
