using TMPPP.Domain.Behavioral.Memento;

namespace TMPPP.Controllers;

public class MementoController
{
    public static MementoDemoResult BuildPaymentDraftMementoDemo(MementoDemoRequestModel request)
    {
        var draft = new PaymentDraft(
            request.InitialAmount,
            request.InitialCurrency,
            request.InitialMethod,
            request.InitialDescription);

        var history = new PaymentDraftHistory();

        history.Add(draft.Save("initial"));

        draft.UpdateAmount(request.ReviewAmount);
        draft.UpdateDescription(request.ReviewDescription);
        history.Add(draft.Save("review"));

        draft.UpdatePaymentMethod(request.FinalMethod);
        draft.UpdateCurrency(request.FinalCurrency);
        draft.UpdateDescription(request.FinalDescription);
        history.Add(draft.Save("final"));

        var snapshotToRestore = history.Get(request.RestoreVersion);
        draft.Restore(snapshotToRestore);

        return new MementoDemoResult(
            ToDto(draft, $"current-after-restore:{snapshotToRestore.VersionLabel}", snapshotToRestore.SavedAtUtc),
            snapshotToRestore.VersionLabel,
            history.Snapshots.Select(snapshot => new PaymentDraftVersionDto(
                snapshot.VersionLabel,
                snapshot.Amount,
                snapshot.Currency,
                snapshot.PaymentMethod,
                snapshot.Description,
                snapshot.SavedAtUtc)).ToList(),
            "Originator-ul PaymentDraft isi salveaza starea in obiecte Memento, iar caretaker-ul PaymentDraftHistory pastreaza versiunile fara sa cunoasca detaliile interne ale draftului.");
    }

    private static PaymentDraftVersionDto ToDto(PaymentDraft draft, string versionLabel, DateTime savedAtUtc)
    {
        return new PaymentDraftVersionDto(
            versionLabel,
            draft.Amount,
            draft.Currency,
            draft.PaymentMethod,
            draft.Description,
            savedAtUtc);
    }
}
