namespace TMPPP.Domain.Structural.Bridge;

public interface IDocumentRenderer
{
    string Name { get; }
    string Render(string title, IReadOnlyCollection<string> lines, string footer);
}
