using SQLite;

namespace AndroidUI.Data.Models
{
    [Table("Transactions")]
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int? CategoryId { get; set; }
        public string Type { get; set; } = "Expense";
        public decimal Amount { get; set; }
        public string? Note { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Ignore]
        public Account? Account { get; set; }
        [Ignore]
        public Category? Category { get; set; }
    }
}
