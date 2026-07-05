using AndroidUI.Data;
using AndroidUI.Data.Models;

namespace AndroidUI.Services
{
    public class CategoryService
    {
        private readonly FinanceDatabase _db;

        public CategoryService(FinanceDatabase db) => _db = db;

        public async Task<List<Category>> GetAllAsync()
        {
            var db = await _db.GetDatabaseAsync();
            return await db.Table<Category>().Where(c => !c.IsArchived).OrderBy(c => c.Type).ThenBy(c => c.SortOrder).ToListAsync();
        }

        public async Task<List<Category>> GetByTypeAsync(string type)
        {
            var db = await _db.GetDatabaseAsync();
            return await db.Table<Category>()
                .Where(c => c.Type == type && !c.IsArchived)
                .OrderBy(c => c.SortOrder)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            var db = await _db.GetDatabaseAsync();
            return await db.FindAsync<Category>(id);
        }

        public async Task SaveAsync(Category category)
        {
            var db = await _db.GetDatabaseAsync();
            if (category.Id == 0)
                await db.InsertAsync(category);
            else
                await db.UpdateAsync(category);
        }

        public async Task DeleteAsync(Category category)
        {
            var db = await _db.GetDatabaseAsync();
            await db.DeleteAsync(category);
        }
    }
}
