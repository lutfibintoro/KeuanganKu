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
        // list icon dipisah dengan /
        // contoh "🐷/💰/💳/🏠/📦/🎯/📤/🤝/💡"
        public string Icons { get; set; } = string.Empty;
    }
}
