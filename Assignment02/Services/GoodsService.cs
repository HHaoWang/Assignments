using Assignment02.Dto;

namespace Assignment02.Services;

public class GoodsService
{
    private static readonly List<GoodsDto> GoodsList = new()
    {
        new()
        {
            Id = 1,
            Name = "Pen",
            Price = 1
        },
        new()
        {
            Id = 2,
            Name = "Ruler",
            Price = 0.5m
        },
        new()
        {
            Id = 3,
            Name = "Dress",
            Price = 79.9m
        },
        new()
        {
            Id = 4,
            Name = "iPhone 15",
            Price = 5999
        }
    };

    public async Task<GoodsDto> GetGoodsById(int goodsId)
    {
        GoodsDto? goods = GoodsList.FirstOrDefault(g => g.Id == goodsId);
        await Task.Delay(4000);

        if (goods == null)
        {
            throw new Exception($"找不到Id为{goodsId}的商品！");
        }

        return goods;
    }
}