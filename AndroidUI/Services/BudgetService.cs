using AndroidUI.Data;
using AndroidUI.Data.Models;

namespace AndroidUI.Services
{
    public class BudgetService
    {
        private readonly FinanceDatabase _db;
        private readonly CategoryService _categoryService;

        public BudgetService(FinanceDatabase db, CategoryService categoryService)
        {
            _db = db;
            _categoryService = categoryService;
        }

        public async Task<List<Budget>> GetByMonthAsync(int month, int year)
        {
            var db = await _db.GetDatabaseAsync();
            var budgets = await db.Table<Budget>()
                .Where(b => b.Month == month && b.Year == year)
                .ToListAsync();

            var categories = await _categoryService.GetAllAsync();
            var from = new DateTime(year, month, 1);
            var to = from.AddMonths(1);

            foreach (var budget in budgets)
            {
                budget.Category = categories.FirstOrDefault(c => c.Id == budget.CategoryId);
                budget.Spent = await db.ExecuteScalarAsync<decimal>(
                    "SELECT COALESCE(SUM(Amount), 0) FROM Transactions WHERE CategoryId = ? AND Type = 'Expense' AND Date >= ? AND Date < ?",
                    budget.CategoryId, from, to);
            }

            return budgets;
        }

        public async Task SaveAsync(Budget budget)
        {
            var db = await _db.GetDatabaseAsync();
            if (budget.Id == 0)
                await db.InsertAsync(budget);
            else
                await db.UpdateAsync(budget);
        }

        public async Task DeleteAsync(Budget budget)
        {
            var db = await _db.GetDatabaseAsync();
            await db.DeleteAsync(budget);
        }

        public async Task<Budget?> GetByIdAsync(int id)
        {
            var db = await _db.GetDatabaseAsync();
            return await db.FindAsync<Budget>(id);
        }
    }
}
