using System;
using System.Collections.Generic;
using System.Text;
using AndroidUI.Data;
using static Android.Provider.CalendarContract;

namespace AndroidUI.Services
{
    public class ColorService
    {
        private readonly FinanceDatabase _db;

        public ColorService(FinanceDatabase db)
        {
            _db = db;
        }

        public async Task<string[]> GetAsync()
        {
            var db = await _db.GetDatabaseAsync();

            Data.Models.Color color = await db.Table<Data.Models.Color>()
                .FirstOrDefaultAsync();

            return color.ColorsHex.Split('/', StringSplitOptions.RemoveEmptyEntries);
        }

        public async Task DeleteAsync(string color)
        {
            var db = await _db.GetDatabaseAsync();

            Data.Models.Color colors = await db.Table<Data.Models.Color>()
                .FirstOrDefaultAsync();

            if (colors == null || string.IsNullOrEmpty(colors.ColorsHex))
                return;

            string newColorsHex = string.Empty;

            if (colors.ColorsHex.Contains($"/{color}"))
                newColorsHex = colors.ColorsHex.Replace($"/{color}", string.Empty);

            else if (colors.ColorsHex.Contains($"{color}/"))
                newColorsHex = colors.ColorsHex.Replace($"{color}/", string.Empty);

            else if (colors.ColorsHex.Contains(color))
                newColorsHex = string.Empty;

            else
                newColorsHex = colors.ColorsHex;

            colors.ColorsHex = newColorsHex;
            await db.UpdateAsync(colors);
        }

        public async Task AddAsync(string color)
        {
            var db = await _db.GetDatabaseAsync();

            Data.Models.Color colors = await db.Table<Data.Models.Color>()
                .FirstOrDefaultAsync();

            if (colors == null)
                return;

            colors.ColorsHex = $"{colors.ColorsHex}/{color}";
            await db.UpdateAsync(colors);
        }
    }
}
