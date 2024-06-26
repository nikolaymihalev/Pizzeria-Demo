﻿using BellaVitaPizzeria.Core.Models.Product;

namespace BellaVitaPizzeria.Core.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductInfoModel>> GetAllProductsAsync();
        Task<IEnumerable<ProductInfoModel>> GetSearchedProductsAsync(string title);
        Task<ProductQueryModel> GetProductsForPageAsync(string? category = null, int currentPage = 1);
        Task AddAsync(ProductFormModel model);
        Task EditAsync(ProductFormModel model);
        Task DeleteAsync(int id);
        Task<ProductInfoModel> GetByIdAsync(int id);
    }
}
