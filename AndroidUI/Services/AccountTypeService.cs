using System;
using System.Collections.Generic;
using System.Text;
using AndroidUI.Data;
using AndroidUI.Data.Models;

namespace AndroidUI.Services
{
    public class AccountTypeService
    {
        private readonly FinanceDatabase _db;

        public AccountTypeService(FinanceDatabase db) { _db = db; }

        public async Task<AccountType[]> GetAllAccountTypeAsync()
        {
            var db = await _db.GetDatabaseAsync();
            return await db.Table<AccountType>().ToArrayAsync();
        }

        public async Task SaveAsync(AccountType accountType)
        {
            var db = await _db.GetDatabaseAsync();
            if (accountType.Id == 0)
                await db.InsertAsync(accountType);
            else
                await db.UpdateAsync(accountType);
        }

        public async Task DeleteAsync(AccountType accountType)
        {
            var db = await _db.GetDatabaseAsync();
            await db.DeleteAsync(accountType);
        }
    }
}
