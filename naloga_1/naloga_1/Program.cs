using System;
using System.Drawing.Imaging;
using System.Text;
using System.Drawing;
using System.IO;
using System.Collections;

public class SamplesBitArray
{


    public static void Main()
    {
        byte[,] barve_img = new byte[256, 3];
        BinaryReader br1 = new BinaryReader(File.Open("C:/Users/duckeN/Desktop/Multimedia/naloga_1/barvne_palete/smart.lut", FileMode.Open));

        for (int i = 0; i < 256; i++)
            for (int j = 0; j < 3; j++)
                barve_img[i, j] = br1.ReadByte();

        short[,] slika_img = new short[512, 512];
        BinaryReader br2 = new BinaryReader(File.Open("C:/Users/duckeN/Desktop/Multimedia/naloga_1/ct_posnetki/0078.img", FileMode.Open));

        for (int i = 0; i < 512; i++)
            for (int j = 0; j < 512; j++)
                slika_img[i, j] = br2.ReadInt16();



      
       Console.WriteLine("Vnesite pragovni parameter T: ");
       double parameterT = Convert.ToDouble(Console.ReadLine());
        //    if (neke1 - Math.Abs(Math.Round(neke1)) <= parameterT)
        // byte[] bytes = Encoding.ASCII.GetBytes(test);

 

        int[,] barvna_paleta = new int[512, 512];
        for (int i = 0; i < 512; i++)
            for (int j = 0; j < 512; j++)
                barvna_paleta[i, j] = (int)(((double)(slika_img[i, j] + 2048) / 4095) * 255);



        /*
        int[,,] slika = new int[512, 512, 3];
        for (int i = 0; i < 512; i++) {
            for (int j = 0; j < 512; j++) {
                for (int k = 0; k < 256; k++) {

                    slika[i, j, 0] = barvna_paleta[i, j] + (int)barve_img[k, 0];
                    slika[i, j, 1] = barvna_paleta[i, j] + (int)barve_img[k, 1];
                    slika[i, j, 2] = barvna_paleta[i, j] + (int)barve_img[k, 2];

                    if (slika[i, j, 0] > 255) slika[i, j, 0] = 255;
                    if (slika[i, j, 1] > 255) slika[i, j, 1] = 255;
                    if (slika[i, j, 2] > 255) slika[i, j, 2] = 255;

                    if (slika[i, j, 0] < 0) slika[i, j, 0] = 0;
                    if (slika[i, j, 1] < 0) slika[i, j, 1] = 0;
                    if (slika[i, j, 2] < 0) slika[i, j, 2] = 0;
                }
            }
        }
        */

        var bitmap = new Bitmap(512, 512, PixelFormat.Format24bppRgb);

        for (int i = 0; i < 512; i++)
        {
            for (int j = 0; j < 512; j++)
            {
                int r = barve_img[barvna_paleta[i, j], 0];
                int g = barve_img[barvna_paleta[i, j], 1];
                int b = barve_img[barvna_paleta[i, j], 2];
                bitmap.SetPixel(i, j, Color.FromArgb(r, g, b));

            }
        }
        bitmap.Save("C:/Users/duckeN/Desktop/Multimedia/test.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);

        Compress(slika_img, parameterT);
    }

    public static void Bits(int value)
    {

        FileStream fout = new FileStream("C:/Users/duckeN/Desktop/Multimedia/output.bin", FileMode.OpenOrCreate,
        FileAccess.Write, FileShare.ReadWrite);

        BinaryWriter bw1 = new BinaryWriter(fout);

        int remainder;
        string result = string.Empty;
        while (value > 0)
        {
            remainder = value % 2;
            value /= 2;
            result = remainder.ToString() + result;
        }

        result = result.PadLeft(12, '0');
        byte[] bytes = Encoding.ASCII.GetBytes(result);
        bw1.Write(bytes);
        return;
    }

    public static void Compress(short[,] slikca, double parameterT) 
    {
        FileStream fout = new FileStream("C:/Users/duckeN/Desktop/Multimedia/output.bin", FileMode.OpenOrCreate,
        FileAccess.Write, FileShare.ReadWrite);
        BinaryWriter bw1 = new BinaryWriter(fout);

       
        int pixelSize = (int)Math.Sqrt(Math.Floor((double)slikca.Length));
        Console.WriteLine(pixelSize);
        if (pixelSize == 1) return;


        short[,] slika1 = new short[pixelSize/2, pixelSize/2];
        short[,] slika2 = new short[pixelSize/2, pixelSize/2];
        short[,] slika3 = new short[pixelSize/2, pixelSize/2];
        short[,] slika4 = new short[pixelSize/2, pixelSize/2];

        double pixelValue1 =  0;
        double pixelValue2 = 0;
        double pixelValue3 = 0;
        double pixelValue4 = 0;

        for (int i = 0; i < pixelSize/2; i++)
        {
            for (int j = 0; j < pixelSize/2; j++)
            {
                slika1[i, j] = slikca[(i % slika1.Length), (j % slika1.Length)];
                slika2[i, j] = slikca[(i % slika2.Length), (j % slika2.Length)+ (pixelSize / 2)];
                slika3[i, j] = slikca[(i % slika3.Length) + (pixelSize / 2), (j % slika3.Length)];
                slika4[i, j] = slikca[(i % slika4.Length) + (pixelSize / 2), (j % slika4.Length) + (pixelSize / 2)];

                pixelValue1 = pixelValue1 +  (((double)(slika1[i, j] + 2048) / 4095) * 255);
                pixelValue2 = pixelValue2 + (((double)(slika2[i, j] + 2048) / 4095) * 255);
                pixelValue3 = pixelValue3 + (((double)(slika3[i, j] + 2048) / 4095) * 255);
                pixelValue4 = pixelValue4 + (((double)(slika4[i, j] + 2048) / 4095) * 255);

            }
        }

        pixelValue1 = pixelValue1 / (pixelSize / 2);
        pixelValue2 = pixelValue2 / (pixelSize / 2);
        pixelValue3 = pixelValue3 / (pixelSize / 2);
        pixelValue4 = pixelValue4 / (pixelSize / 2);



        if (pixelValue1 - Math.Abs(Math.Floor(pixelValue1)) <= parameterT)
        {
            bw1.Write(0);
            Bits((int)pixelValue1);
            
        }
        else
        {
            bw1.Write(1);
            Compress(slika1, parameterT);
        }

        if (pixelValue2 - Math.Abs(Math.Floor(pixelValue2)) <= parameterT)
        {
            bw1.Write(0);
            Bits((int)pixelValue2);
        }
        else
        {
            bw1.Write(1);
            Compress(slika2, parameterT);
        }

        if (pixelValue3 - Math.Abs(Math.Floor(pixelValue3)) <= parameterT)
        {
            bw1.Write(0);
            Bits((int)pixelValue3);
        }
        else
        {
            bw1.Write(1);
            Compress(slika3, parameterT);
        }

        if (pixelValue4 - Math.Abs(Math.Floor(pixelValue4)) <= parameterT)
        {
            bw1.Write(0);
            Bits((int)pixelValue4);
        }
        else
        {
            bw1.Write(1);
            Compress(slika4, parameterT);
        }
    }
   
    }
