using AndroidUI.Data;
using AndroidUI.Data.Models;

namespace AndroidUI.Services
{
    public class ReportService
    {
        private readonly FinanceDatabase _db;
        private readonly CategoryService _categoryService;

        public ReportService(FinanceDatabase db, CategoryService categoryService)
        {
            _db = db;
            _categoryService = categoryService;
        }

        public record CategorySummary(string CategoryName, string Icon, string ColorHex, decimal Amount, decimal Percentage);

        public async Task<List<CategorySummary>> GetExpenseByCategoryAsync(int month, int year)
        {
            var db = await _db.GetDatabaseAsync();
            var from = new DateTime(year, month, 1);
            var to = from.AddMonths(1);
            var categories = await _categoryService.GetByTypeAsync("Expense");

            var totalExpense = await db.ExecuteScalarAsync<decimal>(
                "SELECT COALESCE(SUM(Amount), 0) FROM Transactions WHERE Type = 'Expense' AND Date >= ? AND Date < ?", from, to);

            var result = new List<CategorySummary>();
            foreach (var cat in categories)
            {
                var amount = await db.ExecuteScalarAsync<decimal>(
                    "SELECT COALESCE(SUM(Amount), 0) FROM Transactions WHERE CategoryId = ? AND Type = 'Expense' AND Date >= ? AND Date < ?",
                    cat.Id, from, to);

                if (amount > 0)
                {
                    var pct = totalExpense > 0 ? Math.Round(amount / totalExpense * 100, 1) : 0;
                    result.Add(new CategorySummary(cat.Name, cat.Icon ?? "", cat.ColorHex ?? "#607D8B", amount, pct));
                }
            }

            return result.OrderByDescending(r => r.Amount).ToList();
        }

        public record MonthlySummary(int Month, int Year, decimal Income, decimal Expense, decimal Balance);

        public async Task<List<MonthlySummary>> GetMonthlyTrendAsync(int year)
        {
            var db = await _db.GetDatabaseAsync();
            var result = new List<MonthlySummary>();

            for (int m = 1; m <= 12; m++)
            {
                var from = new DateTime(year, m, 1);
                var to = from.AddMonths(1);

                var income = await db.ExecuteScalarAsync<decimal>(
                    "SELECT COALESCE(SUM(Amount), 0) FROM Transactions WHERE Type = 'Income' AND Date >= ? AND Date < ?", from, to);
                var expense = await db.ExecuteScalarAsync<decimal>(
                    "SELECT COALESCE(SUM(Amount), 0) FROM Transactions WHERE Type = 'Expense' AND Date >= ? AND Date < ?", from, to);

                result.Add(new MonthlySummary(m, year, income, expense, income - expense));
            }

            return result;
        }
    }
}
