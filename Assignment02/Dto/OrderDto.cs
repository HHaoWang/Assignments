namespace Assignment02.Dto;

public class OrderDto
{
    public int UserId { get; set; }
    public int GoodsId { get; set; }
    public int Count { get; set; }
    public decimal? FinalPrice { get; set; }
}