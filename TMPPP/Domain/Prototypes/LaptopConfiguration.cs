namespace TMPPP.Domain.Prototypes;

public sealed class LaptopConfiguration
{
    public LaptopConfiguration(string cpu, int ramGb, int storageGb)
    {
        Cpu = cpu;
        RamGb = ramGb;
        StorageGb = storageGb;
    }

    public string Cpu { get; set; }
    public int RamGb { get; set; }
    public int StorageGb { get; set; }

    public LaptopConfiguration Copy()
    {
        return new LaptopConfiguration(Cpu, RamGb, StorageGb);
    }
}
