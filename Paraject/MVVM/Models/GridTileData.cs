using PropertyChanged;
using System.ComponentModel;

namespace Paraject.MVVM.Models
{
    [AddINotifyPropertyChangedInterface]
    public class GridTileData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public object Content { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public GridTileData(object content, int row, int column)
        {
            Content = content;
            Row = row;
            Column = column;
        }
    }
}
