using System.Data;
using Dapper;
using Project2Api.Models;

namespace Project2Api.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly IDbConnection _connection;

    public MenuItemRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public Task<int?> CreateMenuItemAsync(MenuItem menuItem)
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "INSERT INTO menu_item(name, price, quantity, category) VALUES (@Name, @Price, @Quantity, @Category)";
                var menuItemsParameters = new { Name = menuItem.Name, Price = menuItem.Price, Quantity = menuItem.Quantity, Category = menuItem.Category };               

                int rowsAffected = uow.Connection.Execute(sql, menuItemsParameters, uow.Transaction);

                foreach (string cutlery in menuItem.Cutlery)
                {
                    sql = "INSERT INTO menu_item_cutlery(menu_item_name, cutlery_name, quantity) VALUES (@MenuItemName, @CutleryName, 1)";
                    var cutleryParameters = new { MenuItemName = menuItem.Name, CutleryName = cutlery };

                    uow.Connection.Execute(sql, cutleryParameters, uow.Transaction);
                }

                uow.Commit();

                return Task.FromResult<int?>(rowsAffected); 

            } catch (Exception e)
            {
                uow.Rollback();
                Console.WriteLine(e.Message); 
                return Task.FromResult<int?>(null);
            }
        }
    }

    public Task<MenuItem?> GetMenuItemByNameAsync(string name)
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "SELECT * FROM menu_item WHERE name = @Name";
                var parameters = new { Name = name };

                MenuItem menuItem = uow.Connection.QueryFirstOrDefault<MenuItem>(sql, parameters, uow.Transaction);

                sql = "SELECT cutlery_name FROM menu_item_cutlery WHERE menu_item_name = @MenuItemName";
                var cutleryParameters = new { MenuItemName = menuItem.Name };

                List<string> cutlery = uow.Connection.Query<string>(sql, cutleryParameters, uow.Transaction).ToList();
                menuItem.Cutlery = cutlery;

                return Task.FromResult<MenuItem?>(menuItem);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Menu Item Repository]: " + e.Message);
                return Task.FromResult<MenuItem?>(null);
            }
        }
    }

    public Task<IEnumerable<MenuItem>?> GetMenuItemsAsync()
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "SELECT * FROM menu_item";

                IEnumerable<MenuItem> menuItems = uow.Connection.Query<MenuItem>(sql, uow.Transaction);

                return Task.FromResult<IEnumerable<MenuItem>?>(menuItems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Task.FromResult<IEnumerable<MenuItem>?>(null);
            }
        }
    }

    public Task<int?> UpdateMenuItemAsync(MenuItem menuItem)
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "UPDATE menu_item SET price = @Price, quantity = @Quantity, category = @Category WHERE name = @Name";
                var parameters = new { Name = menuItem.Name, Price = menuItem.Price, Quantity = menuItem.Quantity, Category = menuItem.Category };

                int rows_affected = uow.Connection.Execute(sql, parameters, uow.Transaction);

                sql = "DELETE FROM menu_item_cutlery WHERE menu_item_name = @MenuItemName";
                var cutleryDeleteParameters = new { MenuItemName = menuItem.Name };

                uow.Connection.Execute(sql, cutleryDeleteParameters, uow.Transaction);

                foreach (string cutlery in menuItem.Cutlery)
                {
                    sql = "INSERT INTO menu_item_cutlery(menu_item_name, cutlery_name, quantity) VALUES (@MenuItemName, @CutleryName, 1)";
                    var cutleryInsertParameters = new { MenuItemName = menuItem.Name, CutleryName = cutlery };

                    uow.Connection.Execute(sql, cutleryInsertParameters, uow.Transaction);
                }

                uow.Commit();

                return Task.FromResult<int?>(rows_affected);
            }
            catch (Exception e)
            {
                uow.Rollback();
                Console.WriteLine(e.Message);
                return Task.FromResult<int?>(null);
            }
        }
    }
    
    public Task<bool> DeleteMenuItemAsync(string name)
    {
        using (UnitOfWork uow = new UnitOfWork(_connection))
        {
            try
            {
                string sql = "DELETE FROM menu_item_cutlery WHERE menu_item_name = @Name";
                var parameters = new { Name = name };

                uow.Connection.Execute(sql, parameters, uow.Transaction);

                sql = "DELETE FROM ordered_menu_item WHERE menu_item_name = @Name";
                uow.Connection.Execute(sql, parameters, uow.Transaction);

                sql = "DELETE FROM menu_item WHERE name = @Name";
                uow.Connection.Execute(sql, parameters, uow.Transaction);
                
                uow.Commit();

                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                uow.Rollback();
                Console.WriteLine(e.Message);
                return Task.FromResult(false);
            }
        }
    }
}