using System.Text;
using AndroidUI.Data.Models;

namespace AndroidUI.Services
{
    public class ExportService
    {
        private readonly TransactionService _transactionService;

        public ExportService(TransactionService transactionService) => _transactionService = transactionService;

        public async Task<string> ExportToCsvAsync()
        {
            var transactions = await _transactionService.GetAllAsync();
            var sb = new StringBuilder();
            sb.AppendLine("ID,Tipe,Jumlah,Kategori,Akun,Catatan,Tanggal,Dibuat");

            foreach (var t in transactions)
            {
                var date = t.Date.ToString("yyyy-MM-dd");
                var created = t.CreatedAt.ToString("yyyy-MM-dd HH:mm");
                var category = t.Category?.Name ?? "-";
                var account = t.Account?.Name ?? "-";
                var note = (t.Note ?? "").Replace("\"", "\"\"");
                sb.AppendLine($"{t.Id},{t.Type},{t.Amount},{category},{account},\"{note}\",{date},{created}");
            }

            return sb.ToString();
        }

        public async Task<string> SaveToFileAsync()
        {
            var csv = await ExportToCsvAsync();
            var fileName = $"keuangan_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var path = Path.Combine(FileSystem.AppDataDirectory, fileName);
            await File.WriteAllTextAsync(path, csv);
            return path;
        }
    }
}
