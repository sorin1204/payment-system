using System.Data;
using Microsoft.Data.Sqlite;

namespace TMPPP.Infrastructure.Data;

public sealed class SqliteConnectionManager
{
    private static readonly Lazy<SqliteConnectionManager> LazyInstance =
        new(() => new SqliteConnectionManager(), LazyThreadSafetyMode.ExecutionAndPublication);

    private readonly object _sync = new();
    private SqliteConnection? _connection;
    private string? _connectionString;

    private SqliteConnectionManager()
    {
    }

    public static SqliteConnectionManager Instance => LazyInstance.Value;

    public SqliteConnection GetConnection(string connectionString)
    {
        lock (_sync)
        {
            if (_connection is null)
            {
                _connectionString = connectionString;
                _connection = new SqliteConnection(connectionString);
                _connection.Open();
            }
            else if (!string.Equals(_connectionString, connectionString, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    "SqliteConnectionManager already initialized with a different connection string.");
            }
            else if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            return _connection;
        }
    }
}
