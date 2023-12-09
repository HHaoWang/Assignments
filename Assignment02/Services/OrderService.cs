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

    public decimal C_discount(int points)
    {
        //积分小于1000，不优惠
        //积分大于等于1000且小于3000的，打九五折
        //积分大于等于3000且小于10000的，打九折
        //积分大于10000的，打八五折

        if (points < 1000)
        {
            return 1.0m;
        }
        else if (points == 1000 || points < 3000)
        {
            return 0.95m;
        }
        else if (points == 3000 || points < 10000)
        {
            return 0.9m;
        }
        else if (points > 10000)
        {
            return 0.85m;
        }
    }
    public async Task<bool> PlacingAnOrder(OrderDto order)
    {
        //判断有无order
        if(order==null)
        {
            return false;
        }

        try
        {
            //获取对应的用户信息和商品信息
            var users_m = await_userService.GetUserById(order.UserId);
            var goods_m = await_goodsService.GetGoodsById(order.GoodsId);

            //判断有无用户和商品
            if (users_m == null || goods_m == null)
            {
                return false;
            }

            //设置discount和total
            decimal discount = C_discount(user.Point);
            decimal total = order.Count * discount * goods.Price;
            order.FinalPrice = total;
            _ordersList.Add(order);

            return true;
        }
        throw new NotImplementedException();
    }
}