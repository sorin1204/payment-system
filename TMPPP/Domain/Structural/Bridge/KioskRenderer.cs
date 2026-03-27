namespace TMPPP.Domain.Structural.Bridge;

public sealed class KioskRenderer : IDocumentRenderer
{
    public string Name => "Kiosk";

    public string Render(string title, IReadOnlyCollection<string> lines, string footer)
    {
        var output = new List<string> { $"=== {title.ToUpperInvariant()} ===" };
        output.AddRange(lines.Select(line => $"* {line}"));
        output.Add($"--- {footer} ---");
        return string.Join(Environment.NewLine, output);
    }
}
