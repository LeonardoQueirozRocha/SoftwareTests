using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalog.Domain.Models;

public class Category : Entity
{
    public string Name { get; private set; }
    public int Code { get; private set; }

    // EF Relation
    public ICollection<Product> Products { get; set; }

    protected Category() { }

    public Category(string name, int code)
    {
        Name = name;
        Code = code;
    }

    public override string ToString()
    {
        return $"{Name} - {Code}";
    }
}