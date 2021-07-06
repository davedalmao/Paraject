using Paraject.Core.Enums;
using PropertyChanged;
using System;
using System.ComponentModel;

namespace Paraject.MVVM.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Subtask : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        public int Id { get; set; }
        public int Task_Id_Fk { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; } = Enum.GetName(Priorities.Low);
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
