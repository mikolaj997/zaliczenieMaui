using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

namespace zaliczenieMaui.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "Nierozpoczęty";
        public DateTime? DueDate { get; set; } // Nowe pole na termin wykonania projektu
        public ObservableCollection<Task> Tasks { get; set; }
        public ICommand AddTaskCommand { get; set; }

       

      
    }


}
