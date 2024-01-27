using NerdStore.Catalog.Application.ViewModels;

namespace NerdStore.Catalog.Application.Interfaces.Services;

public interface IProductAppService : IDisposable
{
    Task<IEnumerable<ProductViewModel>> GetByCategoryAsync(int code);
    Task<ProductViewModel> GetByIdAsync(Guid id);
    Task<IEnumerable<ProductViewModel>> GetAllAsync();
    Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync();

    Task AddProductAsync(ProductViewModel productViewModel);
    Task UpdateProductAsync(ProductViewModel productViewModel);

    Task<ProductViewModel> DebitStockAsync(Guid id, int quantity);
    Task<ProductViewModel> ReplenishStockAsync(Guid id, int quantity);
}