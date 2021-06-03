using PropertyChanged;
using System;
using System.ComponentModel;

namespace Paraject.MVVM.Models
{
    [AddINotifyPropertyChangedInterface]

    public class UserAccount : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
