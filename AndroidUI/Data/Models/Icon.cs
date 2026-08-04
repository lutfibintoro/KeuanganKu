using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AndroidUI.Data.Models
{
    public class Icon
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Icons { get; set; } = string.Empty;
    }
}
