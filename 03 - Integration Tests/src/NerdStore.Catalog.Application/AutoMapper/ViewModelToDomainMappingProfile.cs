using AutoMapper;
using NerdStore.Catalog.Application.ViewModels;
using NerdStore.Catalog.Domain.Models;

namespace NerdStore.Catalog.Application.AutoMapper;

public class ViewModelToDomainMappingProfile : Profile
{
    public ViewModelToDomainMappingProfile()
    {
        CreateMap<ProductViewModel, Product>()
            .ConstructUsing(p => new Product(
                p.CategoryId,
                p.Name,
                p.Description,
                p.Active,
                p.Value,
                p.RegistrationDate,
                p.Image,
                new Dimensions(
                    p.Height,
                    p.Width,
                    p.Depth)));

        CreateMap<CategoryViewModel, Category>()
            .ConstructUsing(c => new Category(c.Name, c.Code));
    }
}