namespace Finding
{
    public enum Pattern
    {
        Pattern1,//XRP tang gia
        Pattern2,//XRP giam gia
        Pattern3,//Lich su giao dich BUY
        Pattern4,//Lich su giao dich SELL






    }
    public partial class Form1 : Form
    {



        // private Dictionary<Pattern, byte[]> imageDictionary = new Dictionary<Pattern, byte[]>();
        private Dictionary<Pattern, List<byte[]>> imageDictionary = new Dictionary<Pattern, List<byte[]>>();
        private ToolTip comboBoxToolTip = new ToolTip();
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ShowImage();
        }
        private void Init()
        {
            LoadPatternImages();
            //ShowImage();
            PopulateComboBox();
            SetComboBoxToolTip();
        }
        private void LoadPatternImages()
        {
            foreach (Pattern pattern in Enum.GetValues(typeof(Pattern)))
            {
                List<byte[]> patternImages = new List<byte[]>();
                int imageIndex = 0;

                while (true)
                {
                    string imageName = $"{pattern}{(imageIndex == 0 ? "" : $"_{imageIndex}")}.jpg";
                    byte[] imageData;

                    try
                    {
                        imageData = File.ReadAllBytes(imageName);
                        patternImages.Add(imageData);
                        imageIndex++;
                    }
                    catch (FileNotFoundException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading image for {pattern}: {ex.Message}");
                        break;
                    }
                }

                if (patternImages.Count > 0)
                {
                    imageDictionary[pattern] = patternImages;
                }
            }
        }


        private void ShowImage()
        {


            int x = 10;
            int y = 20;
            int pictureBoxWidth = 10;
            int pictureBoxHeight = 10;

            foreach (var kvp in imageDictionary)
            {
                Pattern pattern = kvp.Key;
                List<byte[]> patternImages = kvp.Value;

                foreach (byte[] imageData in patternImages)
                {
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Location = new Point(x, y);
                    pictureBox.Size = new Size(pictureBoxWidth, pictureBoxHeight);
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox.Image = Image.FromStream(new MemoryStream(imageData));
                    pictureBox.Click += (sender, e) =>
                    {

                        ShowLargeImage(pattern, imageData);
                    };
                    flowLayoutPanel1.Controls.Add(pictureBox);

                    x += pictureBoxWidth + 5;
                }
            }

        }
        private void ShowImages(Pattern pattern)
        {

            flowLayoutPanel1.Controls.Clear();

            if (imageDictionary.ContainsKey(pattern))
            {
                foreach (byte[] imageData in imageDictionary[pattern])
                {
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Size = new Size(135, 135);
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox.Image = Image.FromStream(new MemoryStream(imageData));
                    pictureBox.Click += (sender, e) =>
                    {

                        ShowLargeImage(pattern, imageData);
                    };
                    flowLayoutPanel1.Controls.Add(pictureBox);
                }
            }
        }

        private void ShowLargeImage(Pattern pattern, byte[] imageData)
        {

            Form imageForm = new Form();
            imageForm.Text = pattern.ToString();
            imageForm.StartPosition = FormStartPosition.CenterScreen;
            imageForm.Size = new Size(900, 700);
            PictureBox largePictureBox = new PictureBox();
            largePictureBox.Dock = DockStyle.Fill;
            largePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            largePictureBox.Image = Image.FromStream(new MemoryStream(imageData));

            imageForm.Controls.Add(largePictureBox);


            largePictureBox.Click += (sender, e) =>
            {
                imageForm.Close();
            };

            imageForm.ShowDialog();
        }
        private void PopulateComboBox()
        {
            comboBox1.DataSource = Enum.GetValues(typeof(Pattern));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                Pattern selectedPattern = (Pattern)comboBox1.SelectedItem;
                ShowImages(selectedPattern);
            }
        }
        private void SetComboBoxToolTip()
        {
            comboBoxToolTip.ToolTipTitle = "Pattern Explanation";
            comboBoxToolTip.SetToolTip(comboBox1, "");

            comboBox1.MouseEnter += (sender, e) =>
            {
                //  if (comboBox1.SelectedItem != null)
                {
                    Pattern selectedPattern = (Pattern)comboBox1.SelectedItem;
                    string tooltipText = GetPatternExplanation(selectedPattern);
                    comboBoxToolTip.SetToolTip(comboBox1, tooltipText);
                }
            };
        }
        private string GetPatternExplanation(Pattern pattern)
        {

            switch (pattern)
            {
                case Pattern.Pattern1:
                    return "XRP tang gia";
                case Pattern.Pattern2:
                    return "XRP giam gia";
                case Pattern.Pattern3:
                    return "History Buy";
                case Pattern.Pattern4:
                    return "History Sell";


                default:
                    return "No explanation available.";
            }
        }


    }
}