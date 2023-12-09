//using Assignment02.Dto;

//namespace Assignment02.Services;

//public class OrderService
//{
//    private static List<OrderDto> _ordersList = new();
//    private readonly GoodsService _goodsService;
//    private readonly UserService _userService;

//    public OrderService()
//    {
//        _goodsService = new();
//        _userService = new();
//    }

//    public async Task<bool> PlacingAnOrder(OrderDto order)
//    {
//        throw new NotImplementedException();
//    }
//}
using Assignment02.Dto;
using System.Linq.Expressions;

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
    //计算客户的折扣
    private decimal Disconut(int points)
    {
        switch (points)
        {
            case < 1000:
                return 1m;
            case < 3000:
                return 0.95m;
            case < 10000:
                return 0.90m;
            default:
                return 0.85m;
        }

    }
    public async Task<bool> PlacingAnOrder(OrderDto order)
    {
        try
        {
            //获取商品信息和货物信息
            
            Task<UserDto> getUserTask  = _userService.GetUserById(order.UserId);
            Task<GoodsDto> getGoodsTask = _goodsService.GetGoodsById(order.GoodsId);
            //创建一个任务，当()中的所有 Task 对象都完成时，该任务将完成
            await Task.WhenAll(getUserTask, getGoodsTask);

            //获取Task<UserDto>结果值
            var user = getUserTask.Result;
            //获取Task<GoodsDto>结果值
            var goods = getGoodsTask.Result;

            //计算总价
            decimal allprices = goods.Price * order.Count * Disconut(user.Points);
            //总价保存到`FinalPrice`中
            order.FinalPrice = allprices;
            //订单保存到`_ordersList
            _ordersList.Add(order);
            return true;
        }

        catch (Exception)
        {
            return false;
        }

    }
}
