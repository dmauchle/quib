namespace CheatSheet.Model;

internal record ProductId(int Value);

internal class Product
{
    public ProductId ID { get; set; }
    public string ProductName { get; set; }
    public CategoryId CategoryId { get; set; }
    public int Price { get; set; }
}
