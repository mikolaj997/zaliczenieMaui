using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace zaliczenieMaui.Models
{
    public class Comment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string AuthorEmail { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public string FilePath { get; set; }
    }
}
