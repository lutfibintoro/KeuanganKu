using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AndroidUI.Data.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public int ColorId { get; set; }
        public int IconId { get; set; }

        [Ignore]
        public Color? Color { get; set; }
        [Ignore]
        public Icon? Icon { get; set; }
    }
}
