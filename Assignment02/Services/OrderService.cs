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
        private static List<OrderDto> _ordersList = new();
        private readonly GoodsService _goodsService;
        private readonly UserService _userService;

        // order
        public async Task<bool> PlacingAnOrder(OrderDto order)
        {
            if (order == null) // 如果订单为null，则返回false
            {
                return false;
            }
            
            //捕获异常
            try
            {
                // 获取order对应的user和goods
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

                order.FinalPrice = total;//赋值

                _ordersList.Add(order);//添加

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 计算discount
        private decimal CalculateDiscount(int points)
        {
            if (points < 1000)
            {
                return 1.0m; // none
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
