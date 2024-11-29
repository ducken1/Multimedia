using System;
using System.Drawing.Imaging;
using System.Text;
using System.Drawing;
using System.IO;
using System.Collections;

public class Projektna1
{
    public static void Main()
    {

        List<Boolean> bytes = new List<Boolean>();
        List<int> razlike = new List<int>();

        int[] seznam = new int[] { 55, 53, 53, 53, 53, 53, 10, 10, 11, 11, 11, 11 };

        string output = Convert.ToString(seznam[0], 2);
        output = output.PadLeft(8, '0');
        string test = "";
        int razlika = 0;
        int razlikaCounter = 0;
        int j = 0;
        string absolutnokodiranje = "";
        for (int i = 0; i < output.Length; i++)
        {
            if (output[i] == '1')
            {
                bytes.Add(true);
            }
            else
            {
                bytes.Add(false);
            }
        }
        for (int i = 0; i < seznam.Length; i++)
        {
            if (i == seznam.Length - 1){break;}

            razlika = seznam[i+1] - seznam[i];

            razlike.Add(razlika);

        }

        for (int i = 0; i < razlike.Count; i++)
        {
            //Console.WriteLine(i + " " + razlike[i]);

            if (razlike[i] != 0 && razlike[i] >= -30 && razlike[i] <= 30)
            {
                bytes.Add(false);
                bytes.Add(false);

                if (razlike[i] >= -2 && razlike[i] <= -1 || razlike[i] >= 1 && razlike[i] <= 2)
                {
                    bytes.Add(false);
                    bytes.Add(false);
                    if (razlike[i] == -2) { bytes.Add(false); bytes.Add(false);}
                    else if (razlike[i] == -1) { bytes.Add(false); bytes.Add(true);}
                    else if (razlike[i] == 1) { bytes.Add(true); bytes.Add(false);}
                    else if (razlike[i] == 2) { bytes.Add(true); bytes.Add(true);}
                }
                else if (razlike[i] >= -6 && razlike[i] <= -3 || razlike[i] >= 3 && razlike[i] <= 6)
                {
                    bytes.Add(false);
                    bytes.Add(true);
                    if (razlike[i] == -6) { bytes.Add(false); bytes.Add(false); bytes.Add(false);}
                    else if (razlike[i] == -5) { bytes.Add(false); bytes.Add(false); bytes.Add(true);}
                    else if (razlike[i] == -4) { bytes.Add(false); bytes.Add(true); bytes.Add(false);}
                    else if (razlike[i] == -3) { bytes.Add(false); bytes.Add(true); bytes.Add(true);}
                    else if (razlike[i] == 3) { bytes.Add(true); bytes.Add(false); bytes.Add(false);}
                    else if (razlike[i] == 4) { bytes.Add(true); bytes.Add(false); bytes.Add(true);} 
                    else if (razlike[i] == 5) { bytes.Add(true); bytes.Add(true); bytes.Add(false);}
                }
                else if (razlike[i] >= -14 && razlike[i] <= -6 || razlike[i] >= 7 && razlike[i] <= 14)
                {
                    bytes.Add(true);
                    bytes.Add(false);
                    if (razlike[i] == -14) { bytes.Add(false); bytes.Add(false); bytes.Add(false);bytes.Add(false);}
                    else if (razlike[i] == -13) { bytes.Add(false); bytes.Add(false); bytes.Add(false); bytes.Add(true);}
                    else if (razlike[i] == -12) { bytes.Add(false); bytes.Add(false); bytes.Add(true); bytes.Add(false); }
                    else if (razlike[i] == -11) { bytes.Add(false); bytes.Add(false); bytes.Add(true); bytes.Add(true); }
                    else if (razlike[i] == -10) { bytes.Add(false); bytes.Add(true); bytes.Add(false); bytes.Add(false); }
                    else if (razlike[i] == -9) { bytes.Add(false); bytes.Add(true); bytes.Add(false); bytes.Add(true); }
                    else if (razlike[i] == -8) { bytes.Add(false); bytes.Add(true); bytes.Add(true); bytes.Add(true); }
                    else if (razlike[i] == -7) { bytes.Add(true); bytes.Add(false); bytes.Add(false); bytes.Add(false); }
                    else if (razlike[i] == 7) { bytes.Add(true); bytes.Add(false); bytes.Add(false); bytes.Add(true); }
                    else if (razlike[i] == 8) { bytes.Add(true); bytes.Add(false); bytes.Add(true); bytes.Add(false); }
                    else if (razlike[i] == 9) { bytes.Add(true); bytes.Add(false); bytes.Add(true); bytes.Add(true); }
                    else if (razlike[i] == 10) { bytes.Add(true); bytes.Add(true); bytes.Add(false); bytes.Add(false); }
                    else if (razlike[i] == 11) { bytes.Add(true); bytes.Add(true); bytes.Add(false); bytes.Add(true); }
                    else if (razlike[i] == 12) { bytes.Add(true); bytes.Add(true); bytes.Add(true); bytes.Add(false); }
                    else if (razlike[i] == -13) { bytes.Add(true); bytes.Add(true); bytes.Add(true); bytes.Add(true); }
                }
                else if (razlike[i] >= -30 && razlike[i] <= -15 || razlike[i] >= 15 && razlike[i] <= 30)
                {
                    bytes.Add(true);
                    bytes.Add(true);

                    if (razlike[i] == -30) { bytes.Add(false); bytes.Add(false); bytes.Add(false); bytes.Add(false); bytes.Add(false); }
                    else if (razlike[i] == -29) { bytes.Add(false); bytes.Add(false); bytes.Add(false); bytes.Add(false); bytes.Add(true); }
                    else if (razlike[i] == -28) { bytes.Add(false); bytes.Add(false); bytes.Add(false); bytes.Add(true); bytes.Add(false); }
                    else if (razlike[i] == -27) { bytes.Add(false); bytes.Add(false); bytes.Add(false); bytes.Add(true); bytes.Add(true); }
                    else if (razlike[i] == -26) { bytes.Add(false); bytes.Add(false); bytes.Add(true); bytes.Add(false); bytes.Add(false); }
                    else if (razlike[i] == -25) { bytes.Add(false); bytes.Add(false); bytes.Add(true); bytes.Add(false); bytes.Add(true); }
                    else if (razlike[i] == -24) { bytes.Add(false); bytes.Add(false); bytes.Add(true); bytes.Add(true); bytes.Add(false); }
                    else if (razlike[i] == -23) { bytes.Add(false); bytes.Add(false); bytes.Add(true); bytes.Add(true); bytes.Add(true); }
                    else if (razlike[i] == -22) { bytes.Add(false); bytes.Add(true); bytes.Add(false); bytes.Add(false); bytes.Add(false); }
                    else if (razlike[i] == -21) { bytes.Add(false); bytes.Add(true); bytes.Add(false); bytes.Add(false); bytes.Add(true); }
                    else if (razlike[i] == -20) { bytes.Add(false); bytes.Add(true); bytes.Add(false); bytes.Add(true); bytes.Add(false); }
                    else if (razlike[i] == -19) { bytes.Add(false); bytes.Add(true); bytes.Add(false); bytes.Add(true); bytes.Add(true); }
                    else if (razlike[i] == -18) { bytes.Add(false); bytes.Add(true); bytes.Add(true); bytes.Add(false); bytes.Add(false); }
                    else if (razlike[i] == -17) { bytes.Add(false); bytes.Add(true); bytes.Add(true); bytes.Add(false); bytes.Add(true); }
                    else if (razlike[i] == -16) { bytes.Add(false); bytes.Add(true); bytes.Add(true); bytes.Add(true); bytes.Add(false); }
                    else if (razlike[i] == -15) { bytes.Add(false); bytes.Add(true); bytes.Add(true); bytes.Add(true); bytes.Add(true); }
                    else if (razlike[i] == 15) { bytes.Add(true); bytes.Add(false); bytes.Add(false); bytes.Add(false); bytes.Add(false); }
                    else if (razlike[i] == 16) { bytes.Add(true); bytes.Add(false); bytes.Add(false); bytes.Add(false); bytes.Add(true); }
                    else if (razlike[i] == 17) { bytes.Add(true); bytes.Add(false); bytes.Add(false); bytes.Add(true); bytes.Add(false); }
                    else if (razlike[i] == 18) { bytes.Add(true); bytes.Add(false); bytes.Add(false); bytes.Add(true); bytes.Add(true); }
                    else if (razlike[i] == 19) { bytes.Add(true); bytes.Add(false); bytes.Add(true); bytes.Add(false); bytes.Add(false); }
                    else if (razlike[i] == 20) { bytes.Add(true); bytes.Add(false); bytes.Add(true); bytes.Add(false); bytes.Add(true); }
                    else if (razlike[i] == 21) { bytes.Add(true); bytes.Add(false); bytes.Add(true); bytes.Add(true); bytes.Add(false); }
                    else if (razlike[i] == 22) { bytes.Add(true); bytes.Add(false); bytes.Add(true); bytes.Add(true); bytes.Add(true); }
                    else if (razlike[i] == 23) { bytes.Add(true); bytes.Add(true); bytes.Add(false); bytes.Add(false); bytes.Add(false); }
                    else if (razlike[i] == 24) { bytes.Add(true); bytes.Add(true); bytes.Add(false); bytes.Add(false); bytes.Add(true); }
                    else if (razlike[i] == 25) { bytes.Add(true); bytes.Add(true); bytes.Add(false); bytes.Add(true); bytes.Add(false); }
                    else if (razlike[i] == 26) { bytes.Add(true); bytes.Add(true); bytes.Add(false); bytes.Add(true); bytes.Add(true); }
                    else if (razlike[i] == 27) { bytes.Add(true); bytes.Add(true); bytes.Add(true); bytes.Add(false); bytes.Add(false); }
                    else if (razlike[i] == 28) { bytes.Add(true); bytes.Add(true); bytes.Add(true); bytes.Add(false); bytes.Add(true); }
                    else if (razlike[i] == 29) { bytes.Add(true); bytes.Add(true); bytes.Add(true); bytes.Add(true); bytes.Add(false); }
                    else if (razlike[i] == 30) { bytes.Add(true); bytes.Add(true); bytes.Add(true); bytes.Add(true); bytes.Add(true); }
                }
            }
            else if (razlike[i] == 0)
            {
                bytes.Add(false);
                bytes.Add(true);
                j = i;
                razlikaCounter = 0;
                while (razlike[j] == 0)
                {
                    razlikaCounter++;
                    j++;
                    if (j >= razlike.Count) { break; }
                }
                //Console.WriteLine("hehe"+razlikaCounter);
                if (razlikaCounter == 1) { bytes.Add(false);bytes.Add(false);bytes.Add(false); }
                else if (razlikaCounter == 2) { bytes.Add(false); bytes.Add(false); bytes.Add(true); }
                else if (razlikaCounter == 3) { bytes.Add(false); bytes.Add(true); bytes.Add(false); }
                else if (razlikaCounter == 4) { bytes.Add(false); bytes.Add(true); bytes.Add(true); }
                else if (razlikaCounter == 5) { bytes.Add(true); bytes.Add(false); bytes.Add(false); }
                else if (razlikaCounter == 6) { bytes.Add(true); bytes.Add(false); bytes.Add(true); }
                else if (razlikaCounter == 7) { bytes.Add(true); bytes.Add(true); bytes.Add(false); }
                else if (razlikaCounter == 8) { bytes.Add(true); bytes.Add(true); bytes.Add(true); }
                i = j-1;
            }
            else if (razlike[i] < -30 || razlike[i] > 30)
            {
                //Console.WriteLine(i + " " + razlike[i]);
                bytes.Add(true);
                bytes.Add(false);
                if (razlike[i] < 0) { bytes.Add(true); }
                else { bytes.Add(false); }

                absolutnokodiranje = Convert.ToString(Math.Abs(razlike[i]), 2).ToString();
                absolutnokodiranje = absolutnokodiranje.PadLeft(8, '0');

                
                for (int g = 0; g < absolutnokodiranje.Length; g++)
                {
                    if (absolutnokodiranje[g] == '1')
                    {
                        bytes.Add(true);
                    }
                    else
                    {
                        bytes.Add(false);
                    }
                }
                
            }

            if (i == razlike.Count-1) { bytes.Add(true); bytes.Add(true); }    
        }

        for (int i = 0; i < bytes.Count; i++)
        {
            if(bytes[i])
            {
                test = test + "1";
            }
            else
            {
                test = test + "0";
            }
        }
        Console.WriteLine(test);

    }
}

