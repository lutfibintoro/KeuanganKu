using SQLite;

namespace AndroidUI.Data.Models
{
    public class Budget
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Ignore]
        public Category? Category { get; set; }
        [Ignore]
        public decimal Spent { get; set; }
    }
}
