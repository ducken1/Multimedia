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
        Bitmap image1;
        try
        {
            // Retrieve the image.
            image1 = new Bitmap(@"C:\Users\duckeN\Desktop\Multimedia\Multimedia2\slika1.tiff", true);

            int x, y;
            int a = image1.Width;
            int b = image1.Height;
            int counter = 0;
            int counter1 = 0;
            bool avecji = false;
            // Loop through the images pixels to reset color.
            for (x = 0; x < image1.Width; x++)
            {
                for (y = 0; y < image1.Height; y++)
                {
                        Color pixelColor = image1.GetPixel(x, y);
                        Color newColor = Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B);
                        image1.SetPixel(x, y, newColor);
                    

                }
            }
            if(a % 8 == 0)
            {
                a = a;
            }
            else
            {
                do
                {
                    a = a + 1;
                    counter++;
                } while (a % 8 != 0);
            }
            if (b % 8 == 0)
            {
                b = b;
            }
            else
            {
                do
                {
                    b = b + 1;
                    counter1++;
                } while (b % 8 != 0);
            }
            
            

            Bitmap resized = new Bitmap(image1, new Size(image1.Width + counter, image1.Height + counter1));
            for (x = 0; x < resized.Width; x++)
            {
                for (y = 0; y < resized.Height; y++)
                {
                    if (x > image1.Width)
                    {
                        Color newColor1 = Color.FromArgb(255, 255, 255);
                        resized.SetPixel(x, y, newColor1);
                    }
                    if (y > image1.Height)
                    {
                        Color newColor = Color.FromArgb(255, 255, 255);
                        resized.SetPixel(x, y, newColor);
                    }

                }
            }

            List<int[,,]> listOfBitMaps = new List<int[,,]>();
            int[,,] f;


            double pixelR = 0;
            double pixelG = 0;
            double pixelB = 0;
            double Cu = 0;
            double Cv = 0;
            int[,] zigzag = new int[64, 3];
            int[,] pixelZZ = new int[64, 3];

            for (int i = 0; i < resized.Width; i = i+8)
            {
                for (int j = 0; j < resized.Height; j = j+8)
                {
                    f = new int[8,8,3];

                    for (int k = i, u = 0; k < (i + 8); k++, u++)
                    {
                        for (int l = j, v = 0; l < (j + 8); l++, v++)
                        {
                            pixelR += (resized.GetPixel(i, j).R - 128) * Math.Cos(((2 * (k + i) + 1) * u * Math.PI) / 16) * Math.Cos(((2 * (l + j) + 1) * v * Math.PI) / 16);
                            pixelG += (resized.GetPixel(i, j).G - 128) * Math.Cos(((2 * (k + i) + 1) * u * Math.PI) / 16) * Math.Cos(((2 * (l + j) + 1) * v * Math.PI) / 16);
                            pixelB += (resized.GetPixel(i, j).B - 128) * Math.Cos(((2 * (k + i) + 1) * u * Math.PI) / 16) * Math.Cos(((2 * (l + j) + 1) * v * Math.PI) / 16);

                            if (u == 0)
                            {
                                Cu = 1 / Math.Sqrt(2);
                            }
                            else
                            {
                                Cu = 1;
                            }

                            if (v == 0)
                            {
                                Cv = 1 / Math.Sqrt(2);
                            }
                            else
                            {
                                Cv = 1;
                            }

                            f[u, v, 0] = (int)(0.25 * Cu * Cv * pixelR);
                            f[u, v, 1] = (int)(0.25 * Cu * Cv * pixelG);
                            f[u, v, 2] = (int)(0.25 * Cu * Cv * pixelB);
                        }

                    }



                    listOfBitMaps.Add(f);
                }

            }

            Console.WriteLine("Vnesite faktor stiskanja: ");
            int faktor = Convert.ToInt32(Console.ReadLine());
            List<int[,]> listOfZigZag = new List<int[,]>();

            for (int i = 0; i < listOfBitMaps.Count; i++)
            {
                zigzag = ZigZag(listOfBitMaps[i]);

                for (int j = 63; j > 63 - faktor; j--)
                {
                     zigzag[j, 0] = 0;
                     zigzag[j, 1] = 0;
                     zigzag[j, 2] = 0;
                }
                listOfZigZag.Add(zigzag);
            }

            FileStream fout = new FileStream(@"C:\Users\duckeN\Desktop\Multimedia\Multimedia2\output.bin", FileMode.Create);

       BinaryWriter bw1 = new BinaryWriter(fout);
            List<Boolean> bytes = new List<Boolean>();
            bool tipB = false;

            for (int i = 0; i < listOfZigZag.Count; i++)
            {
                pixelZZ = listOfZigZag[i];

                for (int j = 0; j < 64; j++)
                {
                    if (j == 0)
                    {
                        for (int k = 0; k < 12; k++)
                        {
                            bytes.Add((pixelZZ[k, 0] % Math.Pow(2, 11 - k)) == 1);
                            pixelZZ[k, 0] /= (int)Math.Pow(2, 11 - k);

                        }
                     
                    }

                    if (pixelZZ[j, 0] != 0)
                    {
                        bytes.Add(true);
                            
                            int potrebniBiti = countBits(Math.Abs(pixelZZ[j, 0]))+1;

                            for (int g = 0; g < 4; g++) {
                                bytes.Add(potrebniBiti % Math.Pow(2, 3 - g) == 1);
                                potrebniBiti /= (int)Math.Pow(2, 3 - g);
                                }
                            
                            for (int h = 0; h < potrebniBiti-1; h++)
                            {
                                if (h == 0)
                                {
                                if (pixelZZ[j, 0] > 0)
                                {
                                    bytes.Add(false);
                                }
                                else
                                {
                                    bytes.Add(true);
                                }
                                h++;
                                }
                                bytes.Add((pixelZZ[h, 0] % Math.Pow(2, potrebniBiti -2 - h) == 1));
                                pixelZZ[h, 0] /= (int)Math.Pow(2, potrebniBiti - 2 - h);
                            }
                    }
                    
                    if (pixelZZ[j, 0] == 0)
                    {
                        int countNula = 1;
                        bytes.Add(false);
                        j++;
                        if (faktor == 1)
                        {
                            j = j - 1;
                        }
                        if (faktor == 0)
                        {
                            j = j - 1;
                        }
                        while (pixelZZ[j, 0] == 0)
                        {
                            countNula++;
                            j++;

                            if (j >= 64)
                            {
                                break;
                            }
                        



                        if (pixelZZ[j, 0] != 0)
                        {
                                for (int t = 0; t < 6; t++)
                                {
                                    bytes.Add((countNula % Math.Pow(2, 5 - t) == 1));
                                    countNula /= (int)Math.Pow(2, 5 - t);
                                }

                                int potrebniBiti1 = countBits(pixelZZ[j, 0]) + 1;

                             for (int w = 0; w < 4; w++)
                             {
                                 bytes.Add(potrebniBiti1 % Math.Pow(2, 3 - w) == 1);
                                 potrebniBiti1 /= (int)Math.Pow(2, 3 - w);
                             }
                             for (int h = 0; h < potrebniBiti1-1; h++)
                             {
                                 if (h == 0)
                                 {
                                     if (pixelZZ[j, 0] > 0)
                                     {
                                         bytes.Add(false);
                                     }
                                     else
                                     {
                                         bytes.Add(true);
                                     }
                                     h++;
                                 }
                                 bytes.Add((pixelZZ[h, 0] % Math.Pow(2, potrebniBiti1 - 2 - h) == 1));
                                 pixelZZ[h, 0] /= (int)Math.Pow(2, potrebniBiti1 - 2 - h);
                             }

                        }
                        }

                    }

                    //////


             




                }
            }

            for (int i = 0; i < listOfZigZag.Count; i++)
            {
                pixelZZ = listOfZigZag[i];

                for (int j = 0; j < 64; j++)
                {
                    if (j == 0)
                    {
                        for (int k = 0; k < 12; k++)
                        {
                            bytes.Add((pixelZZ[k, 1] % Math.Pow(2, 11 - k)) == 1);
                            pixelZZ[k, 1] /= (int)Math.Pow(2, 11 - k);

                        }

                    }

                    if (pixelZZ[j, 1] != 0)
                    {
                        bytes.Add(true);

                        int potrebniBiti = countBits(Math.Abs(pixelZZ[j, 1])) + 1;

                        for (int g = 0; g < 4; g++)
                        {
                            bytes.Add(potrebniBiti % Math.Pow(2, 3 - g) == 1);
                            potrebniBiti /= (int)Math.Pow(2, 3 - g);
                        }

                        for (int h = 0; h < potrebniBiti - 1; h++)
                        {
                            if (h == 0)
                            {
                                if (pixelZZ[j, 1] > 0)
                                {
                                    bytes.Add(false);
                                }
                                else
                                {
                                    bytes.Add(true);
                                }
                                h++;
                            }
                            bytes.Add((pixelZZ[h, 1] % Math.Pow(2, potrebniBiti - 2 - h) == 1));
                            pixelZZ[h, 1] /= (int)Math.Pow(2, potrebniBiti - 2 - h);
                        }
                    }

                    if (pixelZZ[j, 1] == 0)
                    {
                        int countNula = 1;
                        bytes.Add(false);
                        j++;
                        if (faktor == 1)
                        {
                            j = j - 1;
                        }
                        if (faktor == 0)
                        {
                            j = j - 1;
                        }
                        while (pixelZZ[j, 1] == 0)
                        {
                            countNula++;
                            j++;

                            if (j >= 64)
                            {
                                break;
                            }




                            if (pixelZZ[j, 1] != 0)
                            {
                                for (int t = 0; t < 6; t++)
                                {
                                    bytes.Add((countNula % Math.Pow(2, 5 - t) == 1));
                                    countNula /= (int)Math.Pow(2, 5 - t);
                                }

                                int potrebniBiti1 = countBits(pixelZZ[j, 1]) + 1;

                                for (int w = 0; w < 4; w++)
                                {
                                    bytes.Add(potrebniBiti1 % Math.Pow(2, 3 - w) == 1);
                                    potrebniBiti1 /= (int)Math.Pow(2, 3 - w);
                                }
                                for (int h = 0; h < potrebniBiti1 - 1; h++)
                                {
                                    if (h == 0)
                                    {
                                        if (pixelZZ[j, 1] > 0)
                                        {
                                            bytes.Add(false);
                                        }
                                        else
                                        {
                                            bytes.Add(true);
                                        }
                                        h++;
                                    }
                                    bytes.Add((pixelZZ[h, 1] % Math.Pow(2, potrebniBiti1 - 2 - h) == 1));
                                    pixelZZ[h, 1] /= (int)Math.Pow(2, potrebniBiti1 - 2 - h);
                                }

                            }
                        }

                    }

                    //////






                }
            }

            for (int i = 0; i < listOfZigZag.Count; i++)
            {
                pixelZZ = listOfZigZag[i];

                for (int j = 0; j < 64; j++)
                {
                    if (j == 0)
                    {
                        for (int k = 0; k < 12; k++)
                        {
                            bytes.Add((pixelZZ[k, 2] % Math.Pow(2, 11 - k)) == 1);
                            pixelZZ[k, 2] /= (int)Math.Pow(2, 11 - k);

                        }

                    }

                    if (pixelZZ[j, 2] != 0)
                    {
                        bytes.Add(true);

                        int potrebniBiti = countBits(Math.Abs(pixelZZ[j, 2])) + 1;

                        for (int g = 0; g < 4; g++)
                        {
                            bytes.Add(potrebniBiti % Math.Pow(2, 3 - g) == 1);
                            potrebniBiti /= (int)Math.Pow(2, 3 - g);
                        }

                        for (int h = 0; h < potrebniBiti - 1; h++)
                        {
                            if (h == 0)
                            {
                                if (pixelZZ[j, 2] > 0)
                                {
                                    bytes.Add(false);
                                }
                                else
                                {
                                    bytes.Add(true);
                                }
                                h++;
                            }
                            bytes.Add((pixelZZ[h, 2] % Math.Pow(2, potrebniBiti - 2 - h) == 1));
                            pixelZZ[h, 2] /= (int)Math.Pow(2, potrebniBiti - 2 - h);
                        }
                    }

                    if (pixelZZ[j, 2] == 0)
                    {
                        int countNula = 1;
                        bytes.Add(false);
                        j++;
                        if (faktor == 1 || faktor == 0)
                        {
                            j = j - 1;
                        }
                        while (pixelZZ[j, 2] == 0)
                        {
                            countNula++;
                            j++;

                            if (j >= 64)
                            {
                                break;
                            }




                            if (pixelZZ[j, 2] != 0)
                            {
                                for (int t = 0; t < 6; t++)
                                {
                                    bytes.Add((countNula % Math.Pow(2, 5 - t) == 1));
                                    countNula /= (int)Math.Pow(2, 5 - t);
                                }

                                int potrebniBiti1 = countBits(pixelZZ[j, 2]) + 1;

                                for (int w = 0; w < 4; w++)
                                {
                                    bytes.Add(potrebniBiti1 % Math.Pow(2, 3 - w) == 1);
                                    potrebniBiti1 /= (int)Math.Pow(2, 3 - w);
                                }
                                for (int h = 0; h < potrebniBiti1 - 1; h++)
                                {
                                    if (h == 0)
                                    {
                                        if (pixelZZ[j, 2] > 0)
                                        {
                                            bytes.Add(false);
                                        }
                                        else
                                        {
                                            bytes.Add(true);
                                        }
                                        h++;
                                    }
                                    bytes.Add((pixelZZ[h, 2] % Math.Pow(2, potrebniBiti1 - 2 - h) == 1));
                                    pixelZZ[h, 2] /= (int)Math.Pow(2, potrebniBiti1 - 2 - h);
                                }

                            }
                        }

                    }

                    //////






                }
            }

            BitArray biteki = new BitArray(bytes.ToArray());
            byte[] bajti = new byte[(biteki.Length / 8) + 1];
            biteki.CopyTo(bajti, 0);

          bw1.Write(bajti);
            bw1.Close();


           FileStream fout1 = new FileStream(@"C:\Users\duckeN\Desktop\Multimedia2\output.bin", FileMode.Open, FileAccess.Read);

            BinaryReader bw2 = new BinaryReader(fout1);


            Console.WriteLine(listOfBitMaps.Count);
            resized.Save(@"C:\Users\duckeN\Desktop\Multimedia\Multimedia2\resizedpic.png");
            resized.Save(@"C:\Users\duckeN\Desktop\Multimedia\Multimedia2\resizedpic1.tiff");
            resized.Save(@"C:\Users\duckeN\Desktop\Multimedia\Multimedia2\resizedpic2.jfif");
            resized.Save(@"C:\Users\duckeN\Desktop\Multimedia\Multimedia2\resizedpic3.jpg");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("There was an error." +
                "Check the path to the image file.");
        }

    }
    public static int countBits(int number)
    {
        return (int)Math.Log(number, 2.0) + 1;
    }
    public static int[,] ZigZag(int[,,] f)
    {
        int[,] zigzagMatrix = new int[64,3];

        zigzagMatrix[0, 0] = f[0, 0, 0];  zigzagMatrix[0, 1] = f[0, 0, 1];  zigzagMatrix[0, 2] = f[0, 0, 2];
        zigzagMatrix[1, 0] = f[1, 0, 0];  zigzagMatrix[1, 1] = f[1, 0, 1];  zigzagMatrix[1, 2] = f[1, 0, 2];
        zigzagMatrix[2, 0] = f[0, 1, 0];  zigzagMatrix[2, 1] = f[0, 1, 1];  zigzagMatrix[2, 2] = f[0, 1, 2];
        zigzagMatrix[3, 0] = f[0, 2, 0];  zigzagMatrix[3, 1] = f[0, 2, 1];  zigzagMatrix[3, 2] = f[0, 2, 2];
        zigzagMatrix[4, 0] = f[1, 1, 0];  zigzagMatrix[4, 1] = f[1, 1, 1];  zigzagMatrix[4, 2] = f[1, 1, 2];
        zigzagMatrix[5, 0] = f[2, 0, 0];  zigzagMatrix[5, 1] = f[2, 0, 1];  zigzagMatrix[5, 2] = f[2, 0, 2];
        zigzagMatrix[6, 0] = f[3, 0, 0];  zigzagMatrix[6, 1] = f[3, 0, 1];  zigzagMatrix[6, 2] = f[3, 0, 2];
        zigzagMatrix[7, 0] = f[2, 1, 0];  zigzagMatrix[7, 1] = f[2, 1, 1];  zigzagMatrix[7, 2] = f[2, 1, 2];
        zigzagMatrix[8, 0] = f[1, 2, 0];  zigzagMatrix[8, 1] = f[1, 2, 1];  zigzagMatrix[8, 2] = f[1, 2, 2];
        zigzagMatrix[9, 0] = f[0, 3, 0];  zigzagMatrix[9, 1] = f[0, 3, 1];  zigzagMatrix[9, 2] = f[0, 3, 2];
        zigzagMatrix[10, 0] = f[0, 4, 0]; zigzagMatrix[10, 1] = f[0, 4, 1]; zigzagMatrix[10, 2] = f[0, 4, 2];
        zigzagMatrix[11, 0] = f[1, 3, 0]; zigzagMatrix[11, 1] = f[1, 3, 1]; zigzagMatrix[11, 2] = f[1, 3, 2];
        zigzagMatrix[12, 0] = f[2, 2, 0]; zigzagMatrix[12, 1] = f[2, 2, 1]; zigzagMatrix[12, 2] = f[2, 2, 2];
        zigzagMatrix[13, 0] = f[3, 1, 0]; zigzagMatrix[13, 1] = f[3, 1, 1]; zigzagMatrix[13, 2] = f[3, 1, 2];
        zigzagMatrix[14, 0] = f[4, 0, 0]; zigzagMatrix[14, 1] = f[4, 0, 1]; zigzagMatrix[14, 2] = f[4, 0, 2];
        zigzagMatrix[15, 0] = f[5, 0, 0]; zigzagMatrix[15, 1] = f[5, 0, 1]; zigzagMatrix[15, 2] = f[5, 0, 2];
        zigzagMatrix[16, 0] = f[4, 1, 0]; zigzagMatrix[16, 1] = f[4, 1, 1]; zigzagMatrix[16, 2] = f[4, 1, 2];
        zigzagMatrix[17, 0] = f[3, 2, 0]; zigzagMatrix[17, 1] = f[3, 2, 1]; zigzagMatrix[17, 2] = f[3, 2, 2];
        zigzagMatrix[18, 0] = f[2, 3, 0]; zigzagMatrix[18, 1] = f[2, 3, 1]; zigzagMatrix[18, 2] = f[2, 3, 2];
        zigzagMatrix[19, 0] = f[1, 4, 0]; zigzagMatrix[19, 1] = f[1, 4, 1]; zigzagMatrix[19, 2] = f[1, 4, 2];
        zigzagMatrix[20, 0] = f[0, 5, 0]; zigzagMatrix[20, 1] = f[0, 5, 1]; zigzagMatrix[20, 2] = f[0, 5, 2];
        zigzagMatrix[21, 0] = f[0, 6, 0]; zigzagMatrix[21, 1] = f[0, 6, 1]; zigzagMatrix[21, 2] = f[0, 6, 2];
        zigzagMatrix[22, 0] = f[1, 5, 0]; zigzagMatrix[22, 1] = f[1, 5, 1]; zigzagMatrix[22, 2] = f[1, 5, 2];
        zigzagMatrix[23, 0] = f[2, 4, 0]; zigzagMatrix[23, 1] = f[2, 4, 1]; zigzagMatrix[23, 2] = f[2, 4, 2];
        zigzagMatrix[24, 0] = f[3, 3, 0]; zigzagMatrix[24, 1] = f[3, 3, 1]; zigzagMatrix[24, 2] = f[3, 3, 2];
        zigzagMatrix[25, 0] = f[4, 2, 0]; zigzagMatrix[25, 1] = f[4, 2, 1]; zigzagMatrix[25, 2] = f[4, 2, 2];
        zigzagMatrix[26, 0] = f[5, 1, 0]; zigzagMatrix[26, 1] = f[5, 1, 1]; zigzagMatrix[26, 2] = f[5, 1, 2];
        zigzagMatrix[27, 0] = f[6, 0, 0]; zigzagMatrix[27, 1] = f[6, 0, 1]; zigzagMatrix[27, 2] = f[6, 0, 2];
        zigzagMatrix[28, 0] = f[7, 0, 0]; zigzagMatrix[28, 1] = f[7, 0, 1]; zigzagMatrix[28, 2] = f[7, 0, 2];
        zigzagMatrix[29, 0] = f[6, 1, 0]; zigzagMatrix[29, 1] = f[6, 1, 1]; zigzagMatrix[29, 2] = f[6, 1, 2];
        zigzagMatrix[30, 0] = f[5, 2, 0]; zigzagMatrix[30, 1] = f[5, 2, 1]; zigzagMatrix[30, 2] = f[5, 2, 2];
        zigzagMatrix[31, 0] = f[4, 3, 0]; zigzagMatrix[31, 1] = f[4, 3, 1]; zigzagMatrix[31, 2] = f[4, 3, 2];
        zigzagMatrix[32, 0] = f[3, 4, 0]; zigzagMatrix[32, 1] = f[3, 4, 1]; zigzagMatrix[32, 2] = f[3, 4, 2];
        zigzagMatrix[33, 0] = f[2, 5, 0]; zigzagMatrix[33, 1] = f[2, 5, 1]; zigzagMatrix[33, 2] = f[2, 5, 2];
        zigzagMatrix[34, 0] = f[1, 6, 0]; zigzagMatrix[34, 1] = f[1, 6, 1]; zigzagMatrix[34, 2] = f[1, 6, 2];
        zigzagMatrix[35, 0] = f[0, 7, 0]; zigzagMatrix[35, 1] = f[0, 7, 1]; zigzagMatrix[35, 2] = f[0, 7, 2];
        zigzagMatrix[36, 0] = f[1, 7, 0]; zigzagMatrix[36, 1] = f[1, 7, 1]; zigzagMatrix[36, 2] = f[1, 7, 2];
        zigzagMatrix[37, 0] = f[2, 6, 0]; zigzagMatrix[37, 1] = f[2, 6, 1]; zigzagMatrix[37, 2] = f[2, 6, 2];
        zigzagMatrix[38, 0] = f[3, 5, 0]; zigzagMatrix[38, 1] = f[3, 5, 1]; zigzagMatrix[38, 2] = f[3, 5, 2];
        zigzagMatrix[39, 0] = f[4, 4, 0]; zigzagMatrix[39, 1] = f[4, 4, 1]; zigzagMatrix[39, 2] = f[4, 4, 2];
        zigzagMatrix[40, 0] = f[5, 3, 0]; zigzagMatrix[40, 1] = f[5, 3, 1]; zigzagMatrix[40, 2] = f[5, 3, 2];
        zigzagMatrix[41, 0] = f[6, 2, 0]; zigzagMatrix[41, 1] = f[6, 2, 1]; zigzagMatrix[41, 2] = f[6, 2, 2];
        zigzagMatrix[42, 0] = f[7, 1, 0]; zigzagMatrix[42, 1] = f[7, 1, 1]; zigzagMatrix[42, 2] = f[7, 1, 2];
        zigzagMatrix[43, 0] = f[7, 2, 0]; zigzagMatrix[43, 1] = f[7, 2, 1]; zigzagMatrix[43, 2] = f[7, 2, 2];
        zigzagMatrix[44, 0] = f[6, 3, 0]; zigzagMatrix[44, 1] = f[6, 3, 1]; zigzagMatrix[44, 2] = f[6, 3, 2];
        zigzagMatrix[45, 0] = f[5, 4, 0]; zigzagMatrix[45, 1] = f[5, 4, 1]; zigzagMatrix[45, 2] = f[5, 4, 2];
        zigzagMatrix[46, 0] = f[4, 5, 0]; zigzagMatrix[46, 1] = f[4, 5, 1]; zigzagMatrix[46, 2] = f[4, 5, 2];
        zigzagMatrix[47, 0] = f[3, 6, 0]; zigzagMatrix[47, 1] = f[3, 6, 1]; zigzagMatrix[47, 2] = f[3, 6, 2];
        zigzagMatrix[48, 0] = f[2, 7, 0]; zigzagMatrix[48, 1] = f[2, 7, 1]; zigzagMatrix[48, 2] = f[2, 7, 2];
        zigzagMatrix[49, 0] = f[3, 7, 0]; zigzagMatrix[49, 1] = f[3, 7, 1]; zigzagMatrix[49, 2] = f[3, 7, 2];
        zigzagMatrix[50, 0] = f[4, 6, 0]; zigzagMatrix[50, 1] = f[4, 6, 1]; zigzagMatrix[50, 2] = f[4, 6, 2];
        zigzagMatrix[51, 0] = f[5, 5, 0]; zigzagMatrix[51, 1] = f[5, 5, 1]; zigzagMatrix[51, 2] = f[5, 5, 2];
        zigzagMatrix[52, 0] = f[6, 4, 0]; zigzagMatrix[52, 1] = f[6, 4, 1]; zigzagMatrix[52, 2] = f[6, 4, 2];
        zigzagMatrix[53, 0] = f[7, 3, 0]; zigzagMatrix[53, 1] = f[7, 3, 1]; zigzagMatrix[53, 2] = f[7, 3, 2];
        zigzagMatrix[54, 0] = f[7, 4, 0]; zigzagMatrix[54, 1] = f[7, 4, 1]; zigzagMatrix[54, 2] = f[7, 4, 2];
        zigzagMatrix[55, 0] = f[6, 5, 0]; zigzagMatrix[55, 1] = f[6, 5, 1]; zigzagMatrix[55, 2] = f[6, 5, 2];
        zigzagMatrix[56, 0] = f[5, 6, 0]; zigzagMatrix[56, 1] = f[5, 6, 1]; zigzagMatrix[56, 2] = f[5, 6, 2];
        zigzagMatrix[57, 0] = f[4, 7, 0]; zigzagMatrix[57, 1] = f[4, 7, 1]; zigzagMatrix[57, 2] = f[4, 7, 2];
        zigzagMatrix[58, 0] = f[5, 7, 0]; zigzagMatrix[58, 1] = f[5, 7, 1]; zigzagMatrix[58, 2] = f[5, 7, 2];
        zigzagMatrix[59, 0] = f[6, 6, 0]; zigzagMatrix[59, 1] = f[6, 6, 1]; zigzagMatrix[59, 2] = f[6, 6, 2];
        zigzagMatrix[60, 0] = f[7, 5, 0]; zigzagMatrix[60, 1] = f[7, 5, 1]; zigzagMatrix[60, 2] = f[7, 5, 2];
        zigzagMatrix[61, 0] = f[7, 6, 0]; zigzagMatrix[61, 1] = f[7, 6, 1]; zigzagMatrix[61, 2] = f[7, 6, 2];
        zigzagMatrix[62, 0] = f[6, 7, 0]; zigzagMatrix[62, 1] = f[6, 7, 1]; zigzagMatrix[62, 2] = f[6, 7, 2];
        zigzagMatrix[63, 0] = f[7, 7, 0]; zigzagMatrix[63, 1] = f[7, 7, 1]; zigzagMatrix[63, 2] = f[7, 7, 2];



        return zigzagMatrix;
    }

    public static int[,,] ZigZagReverse(int[,] f1)
    {
        int[,,] zigzagReverseMatrix = new int[8,8,3];

        zigzagReverseMatrix[0, 0, 0] = f1[0, 0];  zigzagReverseMatrix[0, 0, 1]=  f1[0, 1] ; zigzagReverseMatrix[0, 0, 2]=  f1[0, 2] ;
        zigzagReverseMatrix[1, 0, 0] = f1[1, 0];  zigzagReverseMatrix[1, 0, 1]=  f1[1, 1] ; zigzagReverseMatrix[1, 0, 2]=  f1[1, 2] ;
        zigzagReverseMatrix[0, 1, 0] = f1[2, 0];  zigzagReverseMatrix[0, 1, 1]=  f1[2, 1] ; zigzagReverseMatrix[0, 1, 2]=  f1[2, 2] ;
        zigzagReverseMatrix[0, 2, 0] = f1[3, 0];  zigzagReverseMatrix[0, 2, 1]=  f1[3, 1] ; zigzagReverseMatrix[0, 2, 2]=  f1[3, 2] ;
        zigzagReverseMatrix[1, 1, 0] = f1[4, 0];  zigzagReverseMatrix[1, 1, 1]=  f1[4, 1] ; zigzagReverseMatrix[1, 1, 2]=  f1[4, 2] ;
        zigzagReverseMatrix[2, 0, 0] = f1[5, 0];  zigzagReverseMatrix[2, 0, 1]=  f1[5, 1] ; zigzagReverseMatrix[2, 0, 2]=  f1[5, 2] ;
        zigzagReverseMatrix[3, 0, 0] = f1[6, 0];  zigzagReverseMatrix[3, 0, 1]=  f1[6, 1] ; zigzagReverseMatrix[3, 0, 2]=  f1[6, 2] ;
        zigzagReverseMatrix[2, 1, 0] = f1[7, 0];  zigzagReverseMatrix[2, 1, 1]=  f1[7, 1] ; zigzagReverseMatrix[2, 1, 2]=  f1[7, 2] ;
        zigzagReverseMatrix[1, 2, 0] = f1[8, 0];  zigzagReverseMatrix[1, 2, 1]=  f1[8, 1] ; zigzagReverseMatrix[1, 2, 2]=  f1[8, 2] ;
        zigzagReverseMatrix[0, 3, 0] = f1[9, 0];  zigzagReverseMatrix[0, 3, 1]=  f1[9, 1] ; zigzagReverseMatrix[0, 3, 2]=  f1[9, 2] ;
        zigzagReverseMatrix[0, 4, 0] = f1[10, 0]; zigzagReverseMatrix[0, 4, 1] = f1[10, 1]; zigzagReverseMatrix[0, 4, 2] = f1[10, 2];
        zigzagReverseMatrix[1, 3, 0] = f1[11, 0]; zigzagReverseMatrix[1, 3, 1] = f1[11, 1]; zigzagReverseMatrix[1, 3, 2] = f1[11, 2];
        zigzagReverseMatrix[2, 2, 0] = f1[12, 0]; zigzagReverseMatrix[2, 2, 1] = f1[12, 1]; zigzagReverseMatrix[2, 2, 2] = f1[12, 2];
        zigzagReverseMatrix[3, 1, 0] = f1[13, 0]; zigzagReverseMatrix[3, 1, 1] = f1[13, 1]; zigzagReverseMatrix[3, 1, 2] = f1[13, 2];
        zigzagReverseMatrix[4, 0, 0] = f1[14, 0]; zigzagReverseMatrix[4, 0, 1] = f1[14, 1]; zigzagReverseMatrix[4, 0, 2] = f1[14, 2];
        zigzagReverseMatrix[5, 0, 0] = f1[15, 0]; zigzagReverseMatrix[5, 0, 1] = f1[15, 1]; zigzagReverseMatrix[5, 0, 2] = f1[15, 2];
        zigzagReverseMatrix[4, 1, 0] = f1[16, 0]; zigzagReverseMatrix[4, 1, 1] = f1[16, 1]; zigzagReverseMatrix[4, 1, 2] = f1[16, 2];
        zigzagReverseMatrix[3, 2, 0] = f1[17, 0]; zigzagReverseMatrix[3, 2, 1] = f1[17, 1]; zigzagReverseMatrix[3, 2, 2] = f1[17, 2];
        zigzagReverseMatrix[2, 3, 0] = f1[18, 0]; zigzagReverseMatrix[2, 3, 1] = f1[18, 1]; zigzagReverseMatrix[2, 3, 2] = f1[18, 2];
        zigzagReverseMatrix[1, 4, 0] = f1[19, 0]; zigzagReverseMatrix[1, 4, 1] = f1[19, 1]; zigzagReverseMatrix[1, 4, 2] = f1[19, 2];
        zigzagReverseMatrix[0, 5, 0] = f1[20, 0]; zigzagReverseMatrix[0, 5, 1] = f1[20, 1]; zigzagReverseMatrix[0, 5, 2] = f1[20, 2];
        zigzagReverseMatrix[0, 6, 0] = f1[21, 0]; zigzagReverseMatrix[0, 6, 1] = f1[21, 1]; zigzagReverseMatrix[0, 6, 2] = f1[21, 2];
        zigzagReverseMatrix[1, 5, 0] = f1[22, 0]; zigzagReverseMatrix[1, 5, 1] = f1[22, 1]; zigzagReverseMatrix[1, 5, 2] = f1[22, 2];
        zigzagReverseMatrix[2, 4, 0] = f1[23, 0]; zigzagReverseMatrix[2, 4, 1] = f1[23, 1]; zigzagReverseMatrix[2, 4, 2] = f1[23, 2];
        zigzagReverseMatrix[3, 3, 0] = f1[24, 0]; zigzagReverseMatrix[3, 3, 1] = f1[24, 1]; zigzagReverseMatrix[3, 3, 2] = f1[24, 2];
        zigzagReverseMatrix[4, 2, 0] = f1[25, 0]; zigzagReverseMatrix[4, 2, 1] = f1[25, 1]; zigzagReverseMatrix[4, 2, 2] = f1[25, 2];
        zigzagReverseMatrix[5, 1, 0] = f1[26, 0]; zigzagReverseMatrix[5, 1, 1] = f1[26, 1]; zigzagReverseMatrix[5, 1, 2] = f1[26, 2];
        zigzagReverseMatrix[6, 0, 0] = f1[27, 0]; zigzagReverseMatrix[6, 0, 1] = f1[27, 1]; zigzagReverseMatrix[6, 0, 2] = f1[27, 2];
        zigzagReverseMatrix[7, 0, 0] = f1[28, 0]; zigzagReverseMatrix[7, 0, 1] = f1[28, 1]; zigzagReverseMatrix[7, 0, 2] = f1[28, 2];
        zigzagReverseMatrix[6, 1, 0] = f1[29, 0]; zigzagReverseMatrix[6, 1, 1] = f1[29, 1]; zigzagReverseMatrix[6, 1, 2] = f1[29, 2];
        zigzagReverseMatrix[5, 2, 0] = f1[30, 0]; zigzagReverseMatrix[5, 2, 1] = f1[30, 1]; zigzagReverseMatrix[5, 2, 2] = f1[30, 2];
        zigzagReverseMatrix[4, 3, 0] = f1[31, 0]; zigzagReverseMatrix[4, 3, 1] = f1[31, 1]; zigzagReverseMatrix[4, 3, 2] = f1[31, 2];
        zigzagReverseMatrix[3, 4, 0] = f1[32, 0]; zigzagReverseMatrix[3, 4, 1] = f1[32, 1]; zigzagReverseMatrix[3, 4, 2] = f1[32, 2];
        zigzagReverseMatrix[2, 5, 0] = f1[33, 0]; zigzagReverseMatrix[2, 5, 1] = f1[33, 1]; zigzagReverseMatrix[2, 5, 2] = f1[33, 2];
        zigzagReverseMatrix[1, 6, 0] = f1[34, 0]; zigzagReverseMatrix[1, 6, 1] = f1[34, 1]; zigzagReverseMatrix[1, 6, 2] = f1[34, 2];
        zigzagReverseMatrix[0, 7, 0] = f1[35, 0]; zigzagReverseMatrix[0, 7, 1] = f1[35, 1]; zigzagReverseMatrix[0, 7, 2] = f1[35, 2];
        zigzagReverseMatrix[1, 7, 0] = f1[36, 0]; zigzagReverseMatrix[1, 7, 1] = f1[36, 1]; zigzagReverseMatrix[1, 7, 2] = f1[36, 2];
        zigzagReverseMatrix[2, 6, 0] = f1[37, 0]; zigzagReverseMatrix[2, 6, 1] = f1[37, 1]; zigzagReverseMatrix[2, 6, 2] = f1[37, 2];
        zigzagReverseMatrix[3, 5, 0] = f1[38, 0]; zigzagReverseMatrix[3, 5, 1] = f1[38, 1]; zigzagReverseMatrix[3, 5, 2] = f1[38, 2];
        zigzagReverseMatrix[4, 4, 0] = f1[39, 0]; zigzagReverseMatrix[4, 4, 1] = f1[39, 1]; zigzagReverseMatrix[4, 4, 2] = f1[39, 2];
        zigzagReverseMatrix[5, 3, 0] = f1[40, 0]; zigzagReverseMatrix[5, 3, 1] = f1[40, 1]; zigzagReverseMatrix[5, 3, 2] = f1[40, 2];
        zigzagReverseMatrix[6, 2, 0] = f1[41, 0]; zigzagReverseMatrix[6, 2, 1] = f1[41, 1]; zigzagReverseMatrix[6, 2, 2] = f1[41, 2];
        zigzagReverseMatrix[7, 1, 0] = f1[42, 0]; zigzagReverseMatrix[7, 1, 1] = f1[42, 1]; zigzagReverseMatrix[7, 1, 2] = f1[42, 2];
        zigzagReverseMatrix[7, 2, 0] = f1[43, 0]; zigzagReverseMatrix[7, 2, 1] = f1[43, 1]; zigzagReverseMatrix[7, 2, 2] = f1[43, 2];
        zigzagReverseMatrix[6, 3, 0] = f1[44, 0]; zigzagReverseMatrix[6, 3, 1] = f1[44, 1]; zigzagReverseMatrix[6, 3, 2] = f1[44, 2];
        zigzagReverseMatrix[5, 4, 0] = f1[45, 0]; zigzagReverseMatrix[5, 4, 1] = f1[45, 1]; zigzagReverseMatrix[5, 4, 2] = f1[45, 2];
        zigzagReverseMatrix[4, 5, 0] = f1[46, 0]; zigzagReverseMatrix[4, 5, 1] = f1[46, 1]; zigzagReverseMatrix[4, 5, 2] = f1[46, 2];
        zigzagReverseMatrix[3, 6, 0] = f1[47, 0]; zigzagReverseMatrix[3, 6, 1] = f1[47, 1]; zigzagReverseMatrix[3, 6, 2] = f1[47, 2];
        zigzagReverseMatrix[2, 7, 0] = f1[48, 0]; zigzagReverseMatrix[2, 7, 1] = f1[48, 1]; zigzagReverseMatrix[2, 7, 2] = f1[48, 2];
        zigzagReverseMatrix[3, 7, 0] = f1[49, 0]; zigzagReverseMatrix[3, 7, 1] = f1[49, 1]; zigzagReverseMatrix[3, 7, 2] = f1[49, 2];
        zigzagReverseMatrix[4, 6, 0] = f1[50, 0]; zigzagReverseMatrix[4, 6, 1] = f1[50, 1]; zigzagReverseMatrix[4, 6, 2] = f1[50, 2];
        zigzagReverseMatrix[5, 5, 0] = f1[51, 0]; zigzagReverseMatrix[5, 5, 1] = f1[51, 1]; zigzagReverseMatrix[5, 5, 2] = f1[51, 2];
        zigzagReverseMatrix[6, 4, 0] = f1[52, 0]; zigzagReverseMatrix[6, 4, 1] = f1[52, 1]; zigzagReverseMatrix[6, 4, 2] = f1[52, 2];
        zigzagReverseMatrix[7, 3, 0] = f1[53, 0]; zigzagReverseMatrix[7, 3, 1] = f1[53, 1]; zigzagReverseMatrix[7, 3, 2] = f1[53, 2];
        zigzagReverseMatrix[7, 4, 0] = f1[54, 0]; zigzagReverseMatrix[7, 4, 1] = f1[54, 1]; zigzagReverseMatrix[7, 4, 2] = f1[54, 2];
        zigzagReverseMatrix[6, 5, 0] = f1[55, 0]; zigzagReverseMatrix[6, 5, 1] = f1[55, 1]; zigzagReverseMatrix[6, 5, 2] = f1[55, 2];
        zigzagReverseMatrix[5, 6, 0] = f1[56, 0]; zigzagReverseMatrix[5, 6, 1] = f1[56, 1]; zigzagReverseMatrix[5, 6, 2] = f1[56, 2];
        zigzagReverseMatrix[4, 7, 0] = f1[57, 0]; zigzagReverseMatrix[4, 7, 1] = f1[57, 1]; zigzagReverseMatrix[4, 7, 2] = f1[57, 2];
        zigzagReverseMatrix[5, 7, 0] = f1[58, 0]; zigzagReverseMatrix[5, 7, 1] = f1[58, 1]; zigzagReverseMatrix[5, 7, 2] = f1[58, 2];
        zigzagReverseMatrix[6, 6, 0] = f1[59, 0]; zigzagReverseMatrix[6, 6, 1] = f1[59, 1]; zigzagReverseMatrix[6, 6, 2] = f1[59, 2];
        zigzagReverseMatrix[7, 5, 0] = f1[60, 0]; zigzagReverseMatrix[7, 5, 1] = f1[60, 1]; zigzagReverseMatrix[7, 5, 2] = f1[60, 2];
        zigzagReverseMatrix[7, 6, 0] = f1[61, 0]; zigzagReverseMatrix[7, 6, 1] = f1[61, 1]; zigzagReverseMatrix[7, 6, 2] = f1[61, 2];
        zigzagReverseMatrix[6, 7, 0] = f1[62, 0]; zigzagReverseMatrix[6, 7, 1] = f1[62, 1]; zigzagReverseMatrix[6, 7, 2] = f1[62, 2];
        zigzagReverseMatrix[7, 7, 0] = f1[63, 0]; zigzagReverseMatrix[7, 7, 1] = f1[63, 1]; zigzagReverseMatrix[7, 7, 2] = f1[63, 2];



        return zigzagReverseMatrix;
    }


}
