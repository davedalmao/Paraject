namespace Paraject.MVVM.Models
{
    public class GridTileData
    {
        public object Content { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public GridTileData() { }

        public GridTileData(object content, int row, int column)
        {
            Content = content;
            Row = row;
            Column = column;
        }
    }
}
