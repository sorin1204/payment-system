namespace TMPPP.Domain.Structural.Bridge;

public sealed class MobileRenderer : IDocumentRenderer
{
    public string Name => "Mobile";

    public string Render(string title, IReadOnlyCollection<string> lines, string footer)
    {
        var output = new List<string> { $"[{Name}] {title}" };
        output.AddRange(lines.Select(line => $"- {line}"));
        output.Add($"Tip: {footer}");
        return string.Join(Environment.NewLine, output);
    }
}
