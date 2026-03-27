namespace TMPPP.Domain.Structural.Bridge;

public sealed class EmailRenderer : IDocumentRenderer
{
    public string Name => "Email";

    public string Render(string title, IReadOnlyCollection<string> lines, string footer)
    {
        var output = new List<string> { $"Subject: {title}" };
        output.AddRange(lines);
        output.Add($"Footer: {footer}");
        return string.Join(Environment.NewLine, output);
    }
}
