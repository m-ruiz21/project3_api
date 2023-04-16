using System.Data;

namespace Project2Api.Repositories;

public class UnitOfWork : IDisposable
{
    private IDbConnection _connection;
    private IDbTransaction _transaction;

    public UnitOfWork(IDbConnection connection)
    {
        _connection = connection;
        _connection.Open();
        _transaction = _connection.BeginTransaction();
    }

    public IDbConnection Connection
    {
        get { return _connection; }
    }

    public IDbTransaction Transaction
    {
        get { return _transaction; }
    }

    /// <summary>
    /// Commits to DB transaction.
    /// </summary>
    public void Commit()
    {
        _transaction.Commit();
    }

    /// <summary>
    /// Rolls back DB transaction.
    /// </summary>
    public void Rollback()
    {
        _transaction.Rollback();
    }

    public void Dispose()
    {
        _transaction?.Dispose(); 
        if (_connection.State == ConnectionState.Open) {
                _connection.Close();
        }
    }
}