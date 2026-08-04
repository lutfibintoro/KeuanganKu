using System;
using System.Collections.Generic;
using System.Text;
using AndroidUI.Data;
using AndroidUI.Data.Models;

namespace AndroidUI.Services
{
    public class IconService
    {
        private readonly FinanceDatabase _db;

        public IconService(FinanceDatabase db)
        {
            _db = db;
        }

        public async Task<string[]> GetAsync()
        {
            var db = await _db.GetDatabaseAsync();

            Icon icon = await db.Table<Icon>()
                .FirstOrDefaultAsync();

            return icon.Icons.Split('/', StringSplitOptions.RemoveEmptyEntries);
        }

        public async Task DeleteAsync(string icon)
        {
            var db = await _db.GetDatabaseAsync();

            Icon icons = await db.Table<Icon>()
                .FirstOrDefaultAsync();

            if (icons == null || string.IsNullOrEmpty(icons.Icons))
                return;

            string newIcons = string.Empty;

            if (icons.Icons.Contains($"/{icon}"))
                newIcons = icons.Icons.Replace($"/{icon}", string.Empty);

            else if (icons.Icons.Contains($"{icon}/"))
                newIcons = icons.Icons.Replace($"{icon}/", string.Empty);

            else if (icons.Icons.Contains(icon))
                newIcons = string.Empty;

            else
                newIcons = icons.Icons;

            icons.Icons = newIcons;
            await db.UpdateAsync(icons);
        }

        public async Task AddAsync(string icon)
        {
            var db = await _db.GetDatabaseAsync();

            Icon icons = await db.Table<Icon>()
                .FirstOrDefaultAsync();

            if (icons == null)
                return;

            icons.Icons = $"{icons.Icons}/{icon}";
            await db.UpdateAsync(icons);
        }
    }
}
