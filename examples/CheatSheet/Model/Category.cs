namespace CheatSheet.Model;

internal record CategoryId(int Value);

internal class Category
{
    public required CategoryId ID { get; set; }
    public required string CategoryName { get; set; }
    public required string Description { get; set; }
}