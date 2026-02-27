using System.Collections.Concurrent;
using TMPPP.Infrastructure.Data;
using TMPPP.Views;

namespace TMPPP.Controllers;

public class SingletonController
{
    private readonly MainMenuView _view;
    private readonly string _connectionString;

    public SingletonController(MainMenuView view, string connectionString)
    {
        _view = view;
        _connectionString = connectionString;
    }

    public void RunSingletonDemo()
    {
        var managerA = SqliteConnectionManager.Instance;
        var managerB = SqliteConnectionManager.Instance;
        var connectionA = managerA.GetConnection(_connectionString);
        var connectionB = managerB.GetConnection(_connectionString);

        var instanceHashes = new ConcurrentBag<int>();
        Parallel.For(0, 20, _ =>
        {
            instanceHashes.Add(SqliteConnectionManager.Instance.GetHashCode());
        });

        _view.ShowMessage("Singleton demo (thread-safe DB connection manager):");
        _view.ShowMessage($"Same manager instance: {ReferenceEquals(managerA, managerB)}");
        _view.ShowMessage($"Same DB connection instance: {ReferenceEquals(connectionA, connectionB)}");
        _view.ShowMessage($"Distinct instance hashes in parallel test: {instanceHashes.Distinct().Count()}");
        _view.ShowMessage($"Connection state: {connectionA.State}");
    }
}
