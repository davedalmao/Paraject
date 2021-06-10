using PropertyChanged;
using System;
using System.ComponentModel;

namespace Paraject.MVVM.Models
{
    [AddINotifyPropertyChangedInterface]

    public class Project : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        public int Id { get; set; }
        public int User_Id_Fk { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Option { get; set; }
        public string Status { get; set; }
        public DateTime? Deadline { get; set; } = null;
        public DateTime DateCreated { get; set; }
    }
}
