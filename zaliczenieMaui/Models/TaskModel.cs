using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace zaliczenieMaui.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
    }


}
