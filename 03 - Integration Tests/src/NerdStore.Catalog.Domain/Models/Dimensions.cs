namespace NerdStore.Catalog.Domain.Models;

public class Dimensions
{
    public decimal Height { get; private set; }
    public decimal Width { get; private set; }
    public decimal Depth { get; private set; }

    public Dimensions(
        decimal height, 
        decimal width, 
        decimal depth)
    {
        Height = height;
        Width = width;
        Depth = depth;
    }

    public string FormattedDescription() => $"WxHxD: {Width} x {Height} x {Depth}";

    public override string ToString() => FormattedDescription();
}