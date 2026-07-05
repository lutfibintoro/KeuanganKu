using SQLite;

namespace AndroidUI.Data.Models
{
    public class Transfer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
