using SQLite;
using AndroidUI.Data.Models;

namespace AndroidUI.Data
{
    public class FinanceDatabase
    {
        private SQLiteAsyncConnection? _database;
        private readonly string _dbPath;

        public FinanceDatabase()
        {
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "finance.db3");
        }

        private async Task<SQLiteAsyncConnection> GetConnectionAsync()
        {
            if (_database is not null)
                return _database;

            _database = new SQLiteAsyncConnection(_dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

            await _database.CreateTableAsync<Account>();
            await _database.CreateTableAsync<Category>();
            await _database.CreateTableAsync<Transaction>();
            await _database.CreateTableAsync<Budget>();
            await _database.CreateTableAsync<Transfer>();

            if (await _database.Table<Category>().CountAsync() == 0)
                await SeedCategoriesAsync();

            if (await _database.Table<Account>().CountAsync() == 0)
                await SeedAccountsAsync();

            return _database;
        }

        public async Task<SQLiteAsyncConnection> GetDatabaseAsync() => await GetConnectionAsync();

        private async Task SeedCategoriesAsync()
        {
            var db = await GetConnectionAsync();
            var categories = new List<Category>
            {
                // Income
                new() { Name = "Gaji", Type = "Income", Icon = "💰", ColorHex = "#4CAF50", SortOrder = 1 },
                new() { Name = "Freelance", Type = "Income", Icon = "💻", ColorHex = "#8BC34A", SortOrder = 2 },
                new() { Name = "Investasi", Type = "Income", Icon = "📈", ColorHex = "#009688", SortOrder = 3 },
                new() { Name = "Hadiah", Type = "Income", Icon = "🎁", ColorHex = "#E91E63", SortOrder = 4 },
                new() { Name = "Lainnya (Pemasukan)", Type = "Income", Icon = "📥", ColorHex = "#607D8B", SortOrder = 5 },
                // Expense
                new() { Name = "Makanan", Type = "Expense", Icon = "🍽️", ColorHex = "#FF5722", SortOrder = 1 },
                new() { Name = "Transport", Type = "Expense", Icon = "🚗", ColorHex = "#FF9800", SortOrder = 2 },
                new() { Name = "Belanja", Type = "Expense", Icon = "🛒", ColorHex = "#9C27B0", SortOrder = 3 },
                new() { Name = "Tagihan", Type = "Expense", Icon = "📄", ColorHex = "#F44336", SortOrder = 4 },
                new() { Name = "Hiburan", Type = "Expense", Icon = "🎬", ColorHex = "#2196F3", SortOrder = 5 },
                new() { Name = "Kesehatan", Type = "Expense", Icon = "💊", ColorHex = "#00BCD4", SortOrder = 6 },
                new() { Name = "Pendidikan", Type = "Expense", Icon = "📚", ColorHex = "#3F51B5", SortOrder = 7 },
                new() { Name = "Olahraga", Type = "Expense", Icon = "🏋️", ColorHex = "#4CAF50", SortOrder = 8 },
                new() { Name = "Sosial", Type = "Expense", Icon = "🤝", ColorHex = "#E91E63", SortOrder = 9 },
                new() { Name = "Lainnya (Pengeluaran)", Type = "Expense", Icon = "📤", ColorHex = "#607D8B", SortOrder = 10 },
            };
            await db.InsertAllAsync(categories);
        }

        private async Task SeedAccountsAsync()
        {
            var db = await GetConnectionAsync();
            var accounts = new List<Account>
            {
                new() { Name = "Tunai", Type = "Cash", Icon = "💵", ColorHex = "#4CAF50", SortOrder = 1, Balance = 0 },
                new() { Name = "Bank", Type = "Bank", Icon = "🏦", ColorHex = "#2196F3", SortOrder = 2, Balance = 0 },
                new() { Name = "E-Wallet", Type = "E-Wallet", Icon = "📱", ColorHex = "#FF9800", SortOrder = 3, Balance = 0 },
            };
            await db.InsertAllAsync(accounts);
        }
    }
}
