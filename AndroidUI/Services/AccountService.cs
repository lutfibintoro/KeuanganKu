using AndroidUI.Data;
using AndroidUI.Data.Models;

namespace AndroidUI.Services
{
    public class AccountService
    {
        private readonly FinanceDatabase _db;

        public AccountService(FinanceDatabase db) => _db = db;

        public async Task<List<Account>> GetAllAsync()
        {
            var db = await _db.GetDatabaseAsync();
            return await db.Table<Account>().Where(a => !a.IsArchived).OrderBy(a => a.SortOrder).ToListAsync();
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            var db = await _db.GetDatabaseAsync();
            return await db.FindAsync<Account>(id);
        }

        public async Task SaveAsync(Account account)
        {
            var db = await _db.GetDatabaseAsync();
            if (account.Id == 0)
                await db.InsertAsync(account);
            else
                await db.UpdateAsync(account);
        }

        public async Task DeleteAsync(Account account)
        {
            var db = await _db.GetDatabaseAsync();
            await db.DeleteAsync(account);
        }

        public async Task UpdateBalanceAsync(int accountId)
        {
            var db = await _db.GetDatabaseAsync();
            var income = await db.ExecuteScalarAsync<decimal>(
                "SELECT COALESCE(SUM(Amount), 0) FROM Transactions WHERE AccountId = ? AND Type = 'Income'", accountId);
            var expense = await db.ExecuteScalarAsync<decimal>(
                "SELECT COALESCE(SUM(Amount), 0) FROM Transactions WHERE AccountId = ? AND Type = 'Expense'", accountId);
            var sent = await db.ExecuteScalarAsync<decimal>(
                "SELECT COALESCE(SUM(Amount), 0) FROM Transfer WHERE FromAccountId = ?", accountId);
            var received = await db.ExecuteScalarAsync<decimal>(
                "SELECT COALESCE(SUM(Amount), 0) FROM Transfer WHERE ToAccountId = ?", accountId);

            var account = await db.FindAsync<Account>(accountId);
            if (account is not null)
            {
                account.Balance = income + received - expense - sent;
                await db.UpdateAsync(account);
            }
        }

        public async Task<decimal> GetTotalBalanceAsync()
        {
            var db = await _db.GetDatabaseAsync();
            return await db.ExecuteScalarAsync<decimal>(
                "SELECT COALESCE(SUM(Balance), 0) FROM Account WHERE NOT IsArchived");
        }
    }
}
