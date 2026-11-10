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
        // list colorhex dipisah dengan /
        // contoh "#9C27B0/#3F51B5/#2196F3/#00BCD4/#009688/#4CAF50"
        public string ColorsHex { get; set; } = string.Empty;
    }
}
