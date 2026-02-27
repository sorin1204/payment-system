namespace TMPPP.Domain.Prototypes;

public interface IPrototype<out T>
{
    T CloneShallow();
    T CloneDeep();
}
