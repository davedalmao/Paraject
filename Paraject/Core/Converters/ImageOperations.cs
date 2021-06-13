using System.Drawing;
using System.Drawing.Imaging;

namespace Paraject.Core.Converters
{
    public static class ImageOperations
    {
        public static byte[] ImageToBytes(Image userImage) //Get bytes(varbinary) of the image
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
    }
}