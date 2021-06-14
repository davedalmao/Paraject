using Paraject.Core.Repositories;
using PropertyChanged;
using System;
using System.ComponentModel;
using System.Drawing;

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
        public string Option { get; set; } = Enum.GetName(ProjectOptions.Personal);
        public string Status { get; set; }
        //public string Deadline { get; set; } //string data tyep (instead of DateTime) so that I can change the Date's format
        public DateTime? Deadline { get; set; } //string data tyep (instead of DateTime) so that I can change the Date's format
        public DateTime DateCreated { get; set; }
        public Image Logo { get; set; }
    }
}
