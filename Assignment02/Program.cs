using System.Diagnostics;
using Assignment02.Dto;
using Assignment02.Services;

OrderService orderService = new();

List<OrderDto> orders = new()
{
    new()
    {
        UserId = 1,
        GoodsId = 3,
        Count = 1,
    },
    new()
    {
        UserId = 3,
        GoodsId = 4,
        Count = 1,
    },
    new()
    {
        UserId = 10,
        GoodsId = 1,
        Count = 10,
    },
    new()
    {
        UserId = 2,
        GoodsId = 2,
        Count = 20,
    }
};

List<Task<bool>> orderTasks = new();

Stopwatch stopwatch = new();
stopwatch.Start();
foreach (OrderDto order in orders)
{
    Task<bool> task = orderService.PlacingAnOrder(order);
    orderTasks.Add(task);
}

Task<bool[]> waitResultTask = Task.WhenAll(orderTasks);
bool[] results = await waitResultTask;

for (int i = 0; i < results.Length; i++)
{
    Console.WriteLine(results[i] ? $"订单{i}下单成功！总价为：{orders[i].FinalPrice}" : $"订单{i}下单失败！");
}

stopwatch.Stop();
Console.WriteLine($"订单全部处理完毕，耗时：{stopwatch.Elapsed.TotalSeconds}s");