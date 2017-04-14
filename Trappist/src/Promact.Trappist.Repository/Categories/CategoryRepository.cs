﻿using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Category;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TrappistDbContext _dbContext;

        public CategoryRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return (await _dbContext.Category.OrderByDescending(g => g.CreatedDateTime).ToListAsync());
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _dbContext.Category.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _dbContext.Category.FindAsync(id);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _dbContext.Category.Update(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsCategoryExistAsync(string categoryName, int id)
        {
            return await _dbContext.Category.AnyAsync(x => x.CategoryName.ToLowerInvariant().Equals(categoryName.ToLowerInvariant()) && x.Id != id);
        }

        public async Task RemoveCategoryAsync(Category category)
        {
            _dbContext.Category.Remove(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}