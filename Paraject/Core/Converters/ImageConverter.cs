using System.Drawing;
using System.Drawing.Imaging;

namespace Paraject.Core.Converters
{
    public static class ImageConverter
    {
        public static byte[] ImageToBytes(Image userImage) //Get bytes(varbinary) of the image (bytes is going to be saved to the database as varbinary(max))
        {
            if (userImage == null) { return null; }
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            using (Bitmap tempImage = new Bitmap(userImage))
            {
                /*copy the object (userImage) into a new object (tempImage), 
                  then use that object(tempImage) to "Write" */
                tempImage.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public static Image BytesToImage(byte[] buffer) //Get image from database
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(buffer))
            {
                return Image.FromStream(ms);
            }
        }

    }
}