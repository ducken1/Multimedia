using System;
using System.Drawing.Imaging;
using System.Text;
using System.Drawing;
using System.IO;
using System.Collections;

Bitmap img = new Bitmap(@"C:\Users\duckeN\Desktop\Multimedia3\testslika.png");
int counter = 0;
for (int i = 0; i < img.Width; i++)
{
    for (int j = 0; j < img.Height; j++)
    {
        Color pixel = img.GetPixel(i, j);
        int r = pixel.R;
        int g = pixel.G;
        int b = pixel.B;


        if (counter < 10)
        {
            Console.WriteLine(pixel.R);
        }

        counter++;
        if (r < 255)
        {
            r = r + 1;
        }
        img.SetPixel(i, j, Color.FromArgb(r,g,b));
    }
}
img.Save(@"C:\Users\duckeN\Desktop\Multimedia3\testslika2.png", System.Drawing.Imaging.ImageFormat.Png);