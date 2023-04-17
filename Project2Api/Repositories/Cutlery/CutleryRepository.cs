using System.Data;
using Dapper;
using Project2Api.Models;

namespace Project2Api.Repositories;

public class CutleryRepository : ICutleryRepository
{
    private IDbConnection _connection;

    public CutleryRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public Task<int?> CreateCutleryAsync(Cutlery cutlery)
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "INSERT INTO cutlery(name, quantity) VALUES (@Name, @Quantity)";
                var parameters = new { Name = cutlery.Name, Quantity = cutlery.Quantity };
                int? rowsAffected = uow.Connection.Execute(sql, parameters, uow.Transaction); 
                uow.Commit();
                return Task.FromResult<int?>(rowsAffected);
            } catch (Exception e) {
                uow.Rollback();
                Console.WriteLine(e.Message);
                return Task.FromResult<int?>(null);
            }
        }
    }

    public Task<Cutlery?> GetCutleryByNameAsync(string name)
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "SELECT * FROM cutlery WHERE name = @Name";
                var parameters = new { Name = name };
                Cutlery cutlery = uow.Connection.QueryFirstOrDefault<Cutlery>(sql, parameters, uow.Transaction);
                uow.Commit();
                return Task.FromResult<Cutlery?>(cutlery);
            } catch (Exception e) {
                uow.Rollback();
                Console.WriteLine(e.Message);
                return Task.FromResult<Cutlery?>(null);
            }
        }
    }

    public Task<IEnumerable<Cutlery>?> GetAllCutleryAsync()
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "SELECT * FROM cutlery";
                IEnumerable<Cutlery> cutlery = uow.Connection.Query<Cutlery>(sql, uow.Transaction);
                uow.Commit();
                return Task.FromResult<IEnumerable<Cutlery>?>(cutlery);
            } catch (Exception e) {
                uow.Rollback();
                Console.WriteLine(e.Message);
                return Task.FromResult<IEnumerable<Cutlery>?>(null);
            }
        }
    }

    public Task<int?> UpdateCutleryAsync(Cutlery cutlery)
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "UPDATE cutlery SET quantity = @Quantity WHERE name = @Name";
                var parameters = new { Name = cutlery.Name, Quantity = cutlery.Quantity };
                int? rowsAffected = uow.Connection.Execute(sql, parameters, uow.Transaction);
                uow.Commit();
                return Task.FromResult<int?>(rowsAffected);
            } catch (Exception e) {
                uow.Rollback();
                Console.WriteLine(e.Message);
                return Task.FromResult<int?>(null);
            }
        }
    }

    public Task<bool> DeleteCutleryAsync(string name)
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            { 
                string sql = "DELETE FROM menu_item_cutlery WHERE cutlery_name = @Name";
                var parameters = new { Name = name };
                int? rowsAffected = uow.Connection.Execute(sql, parameters, uow.Transaction);
                
                sql = "DELETE FROM cutlery WHERE name = @Name";
                rowsAffected += uow.Connection.Execute(sql, parameters, uow.Transaction);
    
                uow.Commit();
                return Task.FromResult<bool>(rowsAffected > 0);
            } catch (Exception e) {
                uow.Rollback();
                Console.WriteLine(e.Message);
                return Task.FromResult<bool>(false);
            }
        }
    }
}