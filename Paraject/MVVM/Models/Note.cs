using PropertyChanged;
using System;
using System.ComponentModel;

namespace Paraject.MVVM.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Note : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        public int Id { get; set; }
        public int Project_Id_Fk { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
