using System.Data;

namespace Project2Api.Repositories;

public class UnitOfWork
{
    private IDbConnection _connection;
    private IDbTransaction _transaction;

    public UnitOfWork(IDbConnection connection)
    {
        _connection = connection;
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

    public void Commit()
    {
        _transaction.Commit();
    }

    public void Rollback()
    {
        _transaction.Rollback();
    }
}