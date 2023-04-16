using Project2Api.Models;
using Project2Api.DbTools;
using ErrorOr;
using Project2Api.ServiceErrors;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Project2Api.Repositories;

namespace Project2Api.Services.Orders;

public class OrdersService : IOrdersService
{
    private readonly IOrdersRepository _ordersResponsitory;
    
    public OrdersService(IOrdersRepository ordersResponsitory)
    {
        _ordersResponsitory = ordersResponsitory;
    }
    
    public async Task<ErrorOr<Order>> CreateOrderAsync(Order order)
    {
        if (order.Id == Guid.Empty)
        {
            return Errors.Orders.InvalidOrder;
        }
        
        int? rowsAffected = await _ordersResponsitory.CreateOrderAsync(order);
        if (rowsAffected == null || rowsAffected == 0)
        {
            return Errors.Orders.DbError;
        }        

        return order;
    }

    public async Task<ErrorOr<Order>> GetOrderAsync(Guid id)
    {
        // check input
        if (id == Guid.Empty)
        {
            return Errors.Orders.InvalidOrder;
        }
        
        Order? order = await _ordersResponsitory.GetOrderByIdAsync(id);
        if (order == null)
        {
            return Errors.Orders.NotFound;
        }

        return order;
    }

    public async Task<ErrorOr<List<Order>>> GetAllOrdersAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 0 || pageSize < 0)
        {
            return Errors.Orders.InvalidOrder;
        }

        IEnumerable<Order>? orders = await _ordersResponsitory.GetOrdersAsync();

        if (orders == null)
        {
            return Errors.Orders.NotFound;
        }

        List<Order> paginatedOrders = orders
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return paginatedOrders.ToList();
    }

    public async Task<ErrorOr<Order>> UpdateOrderAsync(Guid id, Order order)
    {
        if (id == Guid.Empty) 
        {
            return Errors.Orders.InvalidOrder;
        }

        int? rowsAffected = await _ordersResponsitory.UpdateOrderAsync(order);

        if (rowsAffected == null)
        {
            return Errors.Orders.DbError;
        }
        else if (rowsAffected == 0)
        {
            return Errors.Orders.NotFound;
        }

        return order;
    }

    public async Task<ErrorOr<IActionResult>> DeleteOrderAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Errors.Orders.InvalidOrder;
        }

        bool success = await _ordersResponsitory.DeleteOrderAsync(id);
        
        if (!success)
        {
            return Errors.Orders.NotFound;
        }

        return new NoContentResult();
    }
}