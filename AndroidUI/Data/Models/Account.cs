using SQLite;

namespace AndroidUI.Data.Models
{
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public decimal InitialBalance { get; set; }
        // ambil dari table icon lalu logic untuk ambil 1 saja
        public string? Icon { get; set; }
        // ambil dari table color lalu logic untuk ambil 1 saja
        public string? ColorHex { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsArchived { get; set; }
    }
}
