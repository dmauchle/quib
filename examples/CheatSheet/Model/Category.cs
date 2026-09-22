namespace CheatSheet.Model;

internal record CategoryId(int Value);

internal class Category
{
    public CategoryId ID { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }
}