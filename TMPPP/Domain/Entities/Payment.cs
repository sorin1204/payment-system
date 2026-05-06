using TMPPP.Domain.Behavioral.State;
using TMPPP.Domain.Enums;
using TMPPP.Domain.ValueObjects;

namespace TMPPP.Domain.Entities;

public sealed class Payment
{
    private IPaymentState _state;

    public Payment(Guid id, Guid invoiceId, Money amount, DateTime createdAt)
    {
        Id = id;
        InvoiceId = invoiceId;
        Amount = amount;
        CreatedAt = createdAt;
        _state = new PendingPaymentState();
        Status = _state.Status;
    }

    private Payment()
    {
        Amount = Money.Zero("RON");
        _state = new PendingPaymentState();
        Status = PaymentStatus.Pending;
    }

    public Guid Id { get; private set; }
    public Guid InvoiceId { get; private set; }
    public Money Amount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string StateName => EnsureStateInitialized().Name;

    public PaymentStateTransitionResult MarkProcessed()
    {
        return EnsureStateInitialized().Handle(this, PaymentStateAction.ProcessSucceeded);
    }

    public PaymentStateTransitionResult MarkFailed()
    {
        return EnsureStateInitialized().Handle(this, PaymentStateAction.ProcessFailed);
    }

    public PaymentStateTransitionResult MarkRefunded()
    {
        return EnsureStateInitialized().Handle(this, PaymentStateAction.RefundRequested);
    }

    public IReadOnlyCollection<string> GetAvailableActions()
    {
        return EnsureStateInitialized().AllowedActions;
    }

    internal void SetState(IPaymentState state)
    {
        _state = state;
        Status = state.Status;
    }

    internal void ForceState(IPaymentState state)
    {
        SetState(state);
    }

    private IPaymentState EnsureStateInitialized()
    {
        _state ??= PaymentStateFactory.Create(Status);
        return _state;
    }
}
