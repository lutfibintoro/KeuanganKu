using AndroidUI.Data;
using AndroidUI.Data.Models;

namespace AndroidUI.Services
{
    public class AccountService
    {
        private readonly FinanceDatabase _db;
        private readonly IServiceProvider _serviceProvider;

        public AccountService(FinanceDatabase db, IServiceProvider serviceProvider)
        {
            _db = db;
            _serviceProvider = serviceProvider;
        }

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

            using (var scope = _serviceProvider.CreateScope())
            {
                var transferService = scope.ServiceProvider.GetRequiredService<TransferService>();
                var transactionService = scope.ServiceProvider.GetRequiredService<TransactionService>();

                var transfer = await db.Table<Transfer>()
                    .Where(t => t.FromAccountId == account.Id || t.ToAccountId == account.Id)
                    .ToListAsync();

                foreach (var t in transfer)
                    await transferService.DeleteAsync(t);

                var transaction = await db.Table<Transaction>()
                    .Where(t => t.AccountId == account.Id)
                    .ToListAsync();

                foreach (var t in transaction)
                    await transactionService.DeleteAsync(t);
            }

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
                account.Balance = account.InitialBalance + income + received - expense - sent;
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
