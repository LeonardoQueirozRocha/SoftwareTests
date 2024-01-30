using NerdStore.Sales.Application.Interfaces;
using NerdStore.Sales.Application.ViewModels;
using NerdStore.Sales.Domain.Enums;
using NerdStore.Sales.Domain.Interfaces.Repositories;

namespace NerdStore.Sales.Application.Queries;

public class OrderQueries : IOrderQueries
{
    private readonly IOrderRepository _orderRepository;

    public OrderQueries(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<CartViewModel> GetCustomerCartAsync(Guid customerId)
    {
        var order = await _orderRepository.GetOrderDraftByCustomerIdAsync(customerId);

        if (order is null) return null;

        var cart = new CartViewModel
        {
            CustomerId = order.CustomerId,
            TotalValue = order.TotalValue,
            OrderId = order.Id,
            DiscountValue = order.Discount,
            SubTotal = order.Discount + order.TotalValue,
            VoucherCode = order.VoucherId.HasValue ? order.Voucher.Code : null
        };

        var cartItems = order.OrderItems.Select(item => new CartItemViewModel
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            Quantity = item.Quantity,
            UnitValue = item.UnitValue,
            TotalValue = item.UnitValue * item.Quantity
        });

        cart.Items.AddRange(cartItems);

        return cart;
    }

    public async Task<IEnumerable<OrderViewModel>> GetCustomerOrdersAsync(Guid customerId)
    {
        var orders = await _orderRepository.GetListByCustomerIdAsync(customerId);

        orders = orders
            .Where(p => p.OrderStatus is OrderStatus.Paid or OrderStatus.Canceled)
            .OrderByDescending(p => p.Code);

        if (!orders.Any()) return null;

        var ordersView = new List<OrderViewModel>();

        var ordersViewModel = orders.Select(order => new OrderViewModel
        {
            Id = order.Id,
            TotalValue = order.TotalValue,
            OrderStatus = (int)order.OrderStatus,
            Code = order.Code,
            RegistrationDate = order.RegistrationDate
        });

        ordersView.AddRange(ordersViewModel);

        return ordersView;
    }
}