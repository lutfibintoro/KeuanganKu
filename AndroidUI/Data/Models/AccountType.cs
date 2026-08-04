using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AndroidUI.Data.Models
{
    public class AccountType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
