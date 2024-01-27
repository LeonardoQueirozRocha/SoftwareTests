using AutoMapper;
using NerdStore.Catalog.Application.Interfaces.Services;
using NerdStore.Catalog.Application.ViewModels;
using NerdStore.Catalog.Domain.Interfaces.Repositories;
using NerdStore.Catalog.Domain.Interfaces.Services;
using NerdStore.Catalog.Domain.Models;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalog.Application.Services;

public class ProductAppService : IProductAppService
{
    private readonly IProductRepository _productRepository;
    private readonly IStockService _stockService;
    private readonly IMapper _mapper;

    public ProductAppService(
        IProductRepository productRepository,
        IStockService stockService,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _stockService = stockService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductViewModel>> GetByCategoryAsync(int code)
    {
        return _mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetByCategoryAsync(code));
    }

    public async Task<ProductViewModel> GetByIdAsync(Guid id)
    {
        return _mapper.Map<ProductViewModel>(await _productRepository.GetByIdAsync(id));
    }

    public async Task<IEnumerable<ProductViewModel>> GetAllAsync()
    {
        return _mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetAllAsync());
    }

    public async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()
    {
        return _mapper.Map<IEnumerable<CategoryViewModel>>(await _productRepository.GetCategoriesAsync());
    }

    public async Task AddProductAsync(ProductViewModel productViewModel)
    {
        _productRepository.Add(_mapper.Map<Product>(productViewModel));
        await _productRepository.UnitOfWork.Commit();
    }

    public async Task UpdateProductAsync(ProductViewModel productViewModel)
    {
        _productRepository.Update(_mapper.Map<Product>(productViewModel));
        await _productRepository.UnitOfWork.Commit();
    }

    public async Task<ProductViewModel> DebitStockAsync(Guid id, int quantity)
    {
        if (!await _stockService.DebitStockAsync(id, quantity))
            throw new DomainException("An error occurred during debit stock operation");

        return _mapper.Map<ProductViewModel>(await _productRepository.GetByIdAsync(id));
    }

    public async Task<ProductViewModel> ReplenishStockAsync(Guid id, int quantity)
    {
        if (!await _stockService.ReplenishStockAsync(id, quantity))
            throw new DomainException("An error occurred during replenish stock operation");

        return _mapper.Map<ProductViewModel>(await _productRepository.GetByIdAsync(id));
    }

    public void Dispose()
    {
        _productRepository?.Dispose();
        _stockService?.Dispose();
    }
}