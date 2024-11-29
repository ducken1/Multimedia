namespace Steganografija
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Boolean> binariziranoSporocilo = new List<Boolean>();

            Bitmap img = new Bitmap(textBoxFilePath.Text);

            int counter = 0;
            int counter0 = 0;
            int counter1 = 1;
            int counter2 = 2;

            int dolzina = textBoxMessage.TextLength;
            string binary1 = Convert.ToString(dolzina, 2);
            binary1 = binary1.PadLeft(32, '0'); //dolzina
            for (int s = 0; s < 32; s++)
            {
                if(binary1[s] == 48)
                {
                    binariziranoSporocilo.Add(false);
                }
                else
                {
                    binariziranoSporocilo.Add(true);
                }

            }


            for (int k = 0; k < textBoxMessage.TextLength; k++)
            {
                char letter = Convert.ToChar(textBoxMessage.Text.Substring(k, 1));
                int value = Convert.ToInt32(letter);
                string binary2 = Convert.ToString(value, 2);
                binary2 = binary2.PadLeft(8, '0');

                for (int s1 = 0; s1 < 8; s1++)
                {
                    if(binary2[s1] == 48)
                    {
                        binariziranoSporocilo.Add(false);
                    }
                    else
                    {
                        binariziranoSporocilo.Add(true);
                    }
                    
                }
            }

            for (int g = 0; g < binariziranoSporocilo.Count; g++)
            {
                System.Diagnostics.Debug.Write(binariziranoSporocilo[g]);
            }
            System.Diagnostics.Debug.WriteLine("");

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);

                    //System.Diagnostics.Debug.WriteLine("R = [" + i + "][" + j + "] = " + pixel.R);
                    //System.Diagnostics.Debug.WriteLine("G = [" + i + "][" + j + "] = " + pixel.G);
                    //System.Diagnostics.Debug.WriteLine("B = [" + i + "][" + j + "] = " + pixel.B);     

                    string binary3 = Convert.ToString(pixel.R, 2);
                    binary3 = binary3.PadLeft(8, '0');
                    int pixelR = pixel.R;

                    string binary4 = Convert.ToString(pixel.G, 2);
                    binary4 = binary4.PadLeft(8, '0');
                    int pixelG = pixel.G;

                    string binary5 = Convert.ToString(pixel.B, 2);
                    binary5 = binary5.PadLeft(8, '0');
                    int pixelB = pixel.B;


                    if (counter0 < binariziranoSporocilo.Count)
                    {
                        if (binariziranoSporocilo[counter0])
                        {
                            binary3 = String.Concat(binary3.AsSpan(0, 7), "1");
                        }
                        else
                        {
                            binary3 = String.Concat(binary3.AsSpan(0, 7), "0");
                        }

                        System.Diagnostics.Debug.Write(counter0 + "-" + binariziranoSporocilo[counter0] + "-");
                        System.Diagnostics.Debug.WriteLine(pixelR);

                        pixelR = Convert.ToInt32(binary3, 2);        
                    }
                    if (counter1 < binariziranoSporocilo.Count)
                    {
                        if (binariziranoSporocilo[counter1]) {
                            binary4 = String.Concat(binary4.AsSpan(0, 7), "1");
                        }
                        else
                        {
                            binary4 = String.Concat(binary4.AsSpan(0, 7), "0");
                        }
                        pixelG = Convert.ToInt32(binary4, 2);
                        //System.Diagnostics.Debug.Write(counter + "-" + binariziranoSporocilo[counter1] + "-");
                       // System.Diagnostics.Debug.WriteLine(pixelG);

                    }
                    if (counter2 < binariziranoSporocilo.Count)
                    {
                        if (binariziranoSporocilo[counter2])
                        {
                            binary5 = String.Concat(binary5.AsSpan(0, 7), "1");
                        }
                        else
                        {
                            binary5 = String.Concat(binary5.AsSpan(0, 7), "0");
                        }
                        pixelB = Convert.ToInt32(binary5, 2);
                       // System.Diagnostics.Debug.Write(counter + "-" + binariziranoSporocilo[counter2] + "-");
                       // System.Diagnostics.Debug.WriteLine(pixelB);

                    }

                    img.SetPixel(i, j, Color.FromArgb(pixelR, pixelG, pixelB));

                    counter++;
                    counter0 = counter0 + 3;
                    counter1 = counter1 + 3;
                    counter2 = counter2 + 3;


                        //System.Diagnostics.Debug.WriteLine(binary);
                        //img.SetPixel(i, j, Color.FromArgb(value, value, value));

                }
                }
            
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image Files (*.png, *.jpg, *.tiff, *.jfif) | *.png; *.jpg; *.tiff; *.jfif";
            saveFileDialog.InitialDirectory = @"C:\Users\duckeN\Desktop\Multimedia3";

            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxFilePath.Text = saveFileDialog.FileName.ToString();
                pictureBox1.ImageLocation = textBoxFilePath.Text;

                img.Save(textBoxFilePath.Text, System.Drawing.Imaging.ImageFormat.Png);
            }
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png, *.jpg, *.tiff, *.jfif) | *.png; *.jpg; *.tiff; *.jfif";
            openFileDialog.InitialDirectory = @"C:\Users\duckeN\Desktop\Multimedia3";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxFilePath.Text = openFileDialog.FileName.ToString();
                pictureBox1.ImageLocation = textBoxFilePath.Text;
            }
        }

        private void buttonDecode_Click(object sender, EventArgs e)
        {
           // List<Char> dolzinaSporocila = new List<Char>();
            int counter = 0;
            int counter0 = 0;
            int counter1 = 1;
            int counter2 = 2;
            int count = 0;
            Bitmap img = new Bitmap(textBoxFilePath.Text);
            string message = "";
            string dolzinaCelega = "";
            string dolzinaSporocila = "";
            int length = 0;
            string test = "";
            string test2 = "";
            string beseda = "";
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);

                        string binary = Convert.ToString(pixel.R, 2);
                        binary = binary.PadLeft(8, '0');

                        string binary1 = Convert.ToString(pixel.G, 2);
                        binary1 = binary1.PadLeft(8, '0');

                        string binary2 = Convert.ToString(pixel.B, 2);
                        binary2 = binary2.PadLeft(8, '0');

                    if (counter < 100)
                    {
                        dolzinaCelega = dolzinaCelega + binary[7] + binary1[7] + binary2[7];
                        //System.Diagnostics.Debug.Write(binary[7]);
                       // System.Diagnostics.Debug.Write(binary1[7]);
                       // System.Diagnostics.Debug.Write(binary2[7]);
                    }

                    counter++;

                }
            }

            for (int g = 0; g < 32; g++)
            {
                dolzinaSporocila = dolzinaSporocila + dolzinaCelega[g];
            }
            length = Convert.ToInt32(dolzinaSporocila, 2);
           // System.Diagnostics.Debug.WriteLine(length);

            for (int f = 32; f < 8*length+32; f++)
            {
                test = test + dolzinaCelega[f];
                count++;
                if (count == 8)
                {
                    test2 = Convert.ToInt32(test, 2).ToString();
                    test2 = System.Text.Encoding.ASCII.GetString(new byte[] { Convert.ToByte(test2) });
                    //System.Diagnostics.Debug.Write(test2);
                    count = 0;
                    test = "";
                    beseda = beseda + test2;
                }

            };
            textBoxMessage.Text = beseda;
            //System.Diagnostics.Debug.WriteLine(beseda);

            //test2 = Convert.ToInt32(test, 2).ToString();
            //test2 = System.Text.Encoding.ASCII.GetString(new byte[] { Convert.ToByte(test2) });
           // System.Diagnostics.Debug.WriteLine(test2);



            /*
            for (int k = 0; k < dolzinasporocila.Count; k++)
            {
                System.Diagnostics.Debug.Write(dolzinasporocila[k].ToString());
            }
            */
        }

    }
}