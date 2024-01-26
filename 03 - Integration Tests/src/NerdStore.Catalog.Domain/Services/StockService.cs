using MediatR;
using NerdStore.Catalog.Domain.Interfaces.Repositories;
using NerdStore.Catalog.Domain.Interfaces.Services;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalog.Domain.Services;

public class StockService : IStockService
{
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;

    public StockService(
        IProductRepository productRepository,
        IMediator mediator)
    {
        _productRepository = productRepository;
        _mediator = mediator;
    }

    public async Task<bool> DebitStockAsync(Guid productId, int quantity)
    {
        if (!await DebitStockItemAsync(productId, quantity)) return false;

        return await _productRepository.UnitOfWork.Commit();
    }

    public async Task<bool> ReplenishStockAsync(Guid productId, int quantity)
    {
        var result = await ReplenishStockItemAsync(productId, quantity);

        if (!result) return false;

        return await _productRepository.UnitOfWork.Commit();
    }

    public void Dispose()
    {
        _productRepository.Dispose();
    }

    private async Task<bool> DebitStockItemAsync(Guid productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null) return false;

        if (!product.HasStock(quantity))
        {
            await _mediator.Publish(new DomainNotification($"Stock", $"Product - {product.Name} without stock"));
            return false;
        }

        product.DebitStock(quantity);
        _productRepository.Update(product);
        return true;
    }

    private async Task<bool> ReplenishStockItemAsync(Guid productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null) return false;

        product.ReplenishStock(quantity);

        _productRepository.Update(product);

        return true;
    }
}