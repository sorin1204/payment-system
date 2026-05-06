namespace TMPPP.Domain.Behavioral.TemplateMethod;

public sealed record PaymentTemplateDemoResult(
    string Template,
    string PaymentReference,
    decimal Amount,
    string Currency,
    string Method,
    IReadOnlyCollection<PaymentTemplateStep> Steps,
    string Outcome,
    string Explanation);
