namespace test2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
    }
}