using AndroidUI.Data;
using AndroidUI.Data.Models;

namespace AndroidUI.Services
{
    public class TransactionService
    {
        private readonly FinanceDatabase _db;
        private readonly AccountService _accountService;

        public TransactionService(FinanceDatabase db, AccountService accountService)
        {
            _db = db;
            _accountService = accountService;
        }

        public async Task<List<Transaction>> GetAllAsync(int? month = null, int? year = null, int? categoryId = null, int? accountId = null, string? type = null)
        {
            var db = await _db.GetDatabaseAsync();
            var query = db.Table<Transaction>();

            if (month.HasValue && year.HasValue)
            {
                var from = new DateTime(year.Value, month.Value, 1);
                var to = from.AddMonths(1);
                query = query.Where(t => t.Date >= from && t.Date < to);
            }

            if (categoryId.HasValue)
                query = query.Where(t => t.CategoryId == categoryId.Value);

            if (accountId.HasValue)
                query = query.Where(t => t.AccountId == accountId.Value);

            if (type is not null)
                query = query.Where(t => t.Type == type);

            var transactions = await query.OrderByDescending(t => t.Date).ThenByDescending(t => t.Id).ToListAsync();

            var accounts = await _accountService.GetAllAsync();
            var catService = new CategoryService(_db);
            var categories = await catService.GetAllAsync();

            foreach (var t in transactions)
            {
                t.Account = accounts.FirstOrDefault(a => a.Id == t.AccountId);
                t.Category = categories.FirstOrDefault(c => c.Id == t.CategoryId);
            }

            return transactions;
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            var db = await _db.GetDatabaseAsync();
            var t = await db.FindAsync<Transaction>(id);
            if (t is null) return null;

            var accounts = await _accountService.GetAllAsync();
            var catService = new CategoryService(_db);
            var categories = await catService.GetAllAsync();

            t.Account = accounts.FirstOrDefault(a => a.Id == t.AccountId);
            t.Category = categories.FirstOrDefault(c => c.Id == t.CategoryId);
            return t;
        }

        public async Task SaveAsync(Transaction transaction)
        {
            var db = await _db.GetDatabaseAsync();
            if (transaction.Id == 0)
            {
                transaction.CreatedAt = DateTime.UtcNow;
                await db.InsertAsync(transaction);
            }
            else
            {
                var existing = await db.FindAsync<Transaction>(transaction.Id);
                if (existing is not null)
                {
                    existing.AccountId = transaction.AccountId;
                    existing.CategoryId = transaction.CategoryId;
                    existing.Type = transaction.Type;
                    existing.Amount = transaction.Amount;
                    existing.Note = transaction.Note;
                    existing.Date = transaction.Date;
                    await db.UpdateAsync(existing);
                }
            }

            await _accountService.UpdateBalanceAsync(transaction.AccountId);
        }

        public async Task DeleteAsync(Transaction transaction)
        {
            var db = await _db.GetDatabaseAsync();
            await db.DeleteAsync(transaction);
            await _accountService.UpdateBalanceAsync(transaction.AccountId);
        }

        public async Task<(decimal Income, decimal Expense)> GetMonthlySummaryAsync(int month, int year)
        {
            var db = await _db.GetDatabaseAsync();
            var from = new DateTime(year, month, 1);
            var to = from.AddMonths(1);

            var income = await db.ExecuteScalarAsync<decimal>(
                "SELECT COALESCE(SUM(Amount), 0) FROM Transactions WHERE Type = 'Income' AND Date >= ? AND Date < ?", from, to);
            var expense = await db.ExecuteScalarAsync<decimal>(
                "SELECT COALESCE(SUM(Amount), 0) FROM Transactions WHERE Type = 'Expense' AND Date >= ? AND Date < ?", from, to);

            return (income, expense);
        }

        public async Task<List<Transaction>> GetRecentAsync(int count = 5)
        {
            var db = await _db.GetDatabaseAsync();
            var transactions = await db.Table<Transaction>()
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.Id)
                .Take(count)
                .ToListAsync();

            var accounts = await _accountService.GetAllAsync();
            var catService = new CategoryService(_db);
            var categories = await catService.GetAllAsync();

            foreach (var t in transactions)
            {
                t.Account = accounts.FirstOrDefault(a => a.Id == t.AccountId);
                t.Category = categories.FirstOrDefault(c => c.Id == t.CategoryId);
            }

            return transactions;
        }
    }
}
