namespace TMPPP.Controllers;

public sealed class MementoDemoRequestModel
{
    public MementoDemoRequestModel(
        decimal initialAmount,
        string initialCurrency,
        string initialMethod,
        string initialDescription,
        decimal reviewAmount,
        string reviewDescription,
        string finalMethod,
        string finalCurrency,
        string finalDescription,
        string restoreVersion)
    {
        InitialAmount = initialAmount;
        InitialCurrency = initialCurrency;
        InitialMethod = initialMethod;
        InitialDescription = initialDescription;
        ReviewAmount = reviewAmount;
        ReviewDescription = reviewDescription;
        FinalMethod = finalMethod;
        FinalCurrency = finalCurrency;
        FinalDescription = finalDescription;
        RestoreVersion = restoreVersion;
    }

    public decimal InitialAmount { get; }
    public string InitialCurrency { get; }
    public string InitialMethod { get; }
    public string InitialDescription { get; }
    public decimal ReviewAmount { get; }
    public string ReviewDescription { get; }
    public string FinalMethod { get; }
    public string FinalCurrency { get; }
    public string FinalDescription { get; }
    public string RestoreVersion { get; }
}
