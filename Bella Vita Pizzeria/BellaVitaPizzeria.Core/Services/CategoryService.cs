﻿using BellaVitaPizzeria.Core.Contracts;
using BellaVitaPizzeria.Core.Models.Category;
using BellaVitaPizzeria.Infrastructure.Common;
using BellaVitaPizzeria.Infrastructure.Constants;
using BellaVitaPizzeria.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BellaVitaPizzeria.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository repository;

        public CategoryService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task AddAsync(CategoryFormModel model)
        {
            var category = new Category() 
            {
                Name = model.Name,
            };

            try
            {
                await repository.AddAsync<Category>(category);
                await repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ApplicationException(ErrorMessagesConstants.OperationFailedErrorMessage);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await repository.GetByIdAsync<Category>(id);

            if (entity == null) 
            {
                throw new ArgumentException(ErrorMessagesConstants.OperationFailedErrorMessage);
            }

            await repository.DeleteAsync<Category>(id);

            await repository.SaveChangesAsync();
        }

        public async Task EditAsync(CategoryFormModel model)
        {
            var entity = await repository.GetByIdAsync<Category>(model.Id);

            if (entity == null) 
            {
                throw new ArgumentException(string.Format(ErrorMessagesConstants.InvalidModelErrorMessage, "category"));                
            }

            entity.Name = model.Name;

            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryInfoModel>> GetAllCategoriesAsync()
        {
            return await repository.AllReadonly<Category>()
                .Select(x => new CategoryInfoModel(
                    x.Id,
                    x.Name))
                .ToListAsync();
        }

        public async Task<CategoryInfoModel> GetByIdAsync(int id)
        {
            var entity = await repository.GetByIdAsync<Category>(id);

            if (entity != null) 
            {
                return new CategoryInfoModel(
                    entity.Id, 
                    entity.Name);                
            }

            throw new ArgumentException(string.Format(ErrorMessagesConstants.DoesntExistErrorMessage, "category"));
        }

        public async Task<CategoryInfoModel> GetByNameAsync(string name)
        {
            var entity = await repository.AllReadonly<Category>().FirstOrDefaultAsync(x=>x.Name==name);

            if (entity != null)
            {
                return new CategoryInfoModel(
                    entity.Id,
                    entity.Name);
            }

            throw new ArgumentException(string.Format(ErrorMessagesConstants.DoesntExistErrorMessage, "category"));
        }
    }
}
