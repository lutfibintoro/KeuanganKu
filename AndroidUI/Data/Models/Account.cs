using SQLite;

namespace AndroidUI.Data.Models
{
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "Cash";
        public decimal Balance { get; set; }
        public string? Icon { get; set; }
        public string? ColorHex { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsArchived { get; set; }
    }
}
