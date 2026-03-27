using TMPPP.Domain.Structural.Proxy;
using TMPPP.Views;

namespace TMPPP.Controllers;

public class ProxyController
{
    private readonly MainMenuView _view;

    public ProxyController(MainMenuView view)
    {
        _view = view;
    }

    public void RunProxyDemo()
    {
        var result = BuildProxyDemo();
        _view.ShowProxyDemo(result);
    }

    public static ProxyDemoResult BuildProxyDemo()
    {
        var deniedProxy = new SecureAuditReportProxy(new AuditUserContext("FrontDeskUser", "clerk"));
        var grantedProxy = new SecureAuditReportProxy(new AuditUserContext("FinanceLead", "admin"));

        var deniedAttempt = deniedProxy.GenerateMonthlyAudit();
        var grantedAttempt = grantedProxy.GenerateMonthlyAudit();

        return new ProxyDemoResult(
            deniedAttempt,
            grantedAttempt,
            "Proxy controleaza accesul la raportul real si aplica lazy loading: serviciul sensibil este creat doar pentru utilizatorii autorizati.");
    }
}
