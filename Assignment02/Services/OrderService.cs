/*using Assignment02.Dto;

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
}*/

using Assignment02.Dto;

namespace Assignment02.Services
{
    public class OrderService
    {
        private static readonly List<OrderDto> _ordersList = new();
        private readonly GoodsService _goodsService = new GoodsService();
        private readonly UserService _userService = new UserService();

        // 订单方法
        public async Task<bool> PlacingAnOrder(OrderDto order)
        {
            if (order == null) // 如果订单为null，则返回false
            {
                return false;
            }

            try
            {
                // 获取订单对应的user和goods
                var user = await _userService.GetUserById(order.UserId);
                var goods = await _goodsService.GetGoodsById(order.GoodsId);

                // 如果未找到user或good，返回false
                if (user == null || goods == null)
                {
                    return false;
                }

                // 计算discount并计算total
                decimal discount = CalculateDiscount(user.Points);
                decimal total = goods.Price * order.Count * discount;

                // 将total赋值给订单的FinalPrice
                order.FinalPrice = total;

                // 将order添加到_ordersList中
                _ordersList.Add(order);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        // 计算折扣的方法
        private decimal CalculateDiscount(int points)
        {
            if (points < 1000)
            {
                return 1.0m; // 不优惠
            }
            else if (points < 3000)
            {
                return 0.95m; // 9.5折
            }
            else if (points < 10000)
            {
                return 0.9m; // 9折
            }
            else
            {
                return 0.85m; // 8.5折
            }
        }
    }
}
