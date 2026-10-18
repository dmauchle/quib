namespace CheatSheet.Model;

internal record ProductId(int Value);

internal class Product
{
    public required ProductId ID { get; set; }
    public required string ProductName { get; set; }
    public required CategoryId CategoryId { get; set; }
    public required int Price { get; set; }
}
