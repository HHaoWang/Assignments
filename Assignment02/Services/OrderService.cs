using Assignment02.Dto;

namespace Assignment02.Services;

public class OrderService
{
    private static List<OrderDto> _ordersList = new();
    private readonly GoodsService _goodsService;
    private readonly UserService _userService;

    public OrderService()
    {
        _goodsService = new();
        _userService = new();
    }

    public async Task<bool> PlacingAnOrder(OrderDto order)
    {
        throw new NotImplementedException();
    }
}