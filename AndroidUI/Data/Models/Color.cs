using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AndroidUI.Data.Models
{
    public class Color
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string ColorsHex { get; set; } = string.Empty;
    }
}
