using AndroidUI.Data;
using AndroidUI.Data.Models;

namespace AndroidUI.Services
{
    public class TransferService
    {
        private readonly FinanceDatabase _db;
        private readonly AccountService _accountService;

        public TransferService(FinanceDatabase db, AccountService accountService)
        {
            _db = db;
            _accountService = accountService;
        }

        public async Task<List<Transfer>> GetAllAsync()
        {
            var db = await _db.GetDatabaseAsync();
            return await db.Table<Transfer>().OrderByDescending(t => t.Date).ThenByDescending(t => t.Id).ToListAsync();
        }

        public async Task SaveAsync(Transfer transfer)
        {
            var db = await _db.GetDatabaseAsync();
            if (transfer.Id == 0)
                await db.InsertAsync(transfer);
            else
                await db.UpdateAsync(transfer);

            await _accountService.UpdateBalanceAsync(transfer.FromAccountId);
            await _accountService.UpdateBalanceAsync(transfer.ToAccountId);
        }

        public async Task DeleteAsync(Transfer transfer)
        {
            var db = await _db.GetDatabaseAsync();
            await db.DeleteAsync(transfer);
            await _accountService.UpdateBalanceAsync(transfer.FromAccountId);
            await _accountService.UpdateBalanceAsync(transfer.ToAccountId);
        }
    }
}
