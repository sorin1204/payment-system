namespace TMPPP.Domain.Behavioral.Chain;

public sealed record PaymentChainStep(string Handler, string Outcome, string Message);
