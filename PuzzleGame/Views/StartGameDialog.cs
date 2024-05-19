namespace PuzzleGame.Views
{
    public class StartGameDialog : Form
    {
        public int BoardSize { get; private set; }

        public StartGameDialog()
        {
            _boardSizeComboBox = new ComboBox();
            _startButton = new Button();
            _flowLayoutPanel = new FlowLayoutPanel();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // flowLayoutPanel
            _flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            _flowLayoutPanel.Dock = DockStyle.Fill;
            _flowLayoutPanel.WrapContents = false;
            _flowLayoutPanel.AutoSize = true;
            _flowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flowLayoutPanel.Padding = new Padding(10);

            // boardSizeComboBox
            _boardSizeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _boardSizeComboBox.Items.AddRange(new object[] { "3x3", "4x4", "5x5" });
            _boardSizeComboBox.SelectedIndex = 0; // Default to 3x3
            _boardSizeComboBox.Name = "_boardSizeComboBox";
            _boardSizeComboBox.Margin = new Padding(20, 4, 75, 0); // Adding space after the combobox
            _boardSizeComboBox.AutoSize = true;

            // startButton
            _startButton.Name = "_startButton";
            _startButton.Text = "Start Game";
            _startButton.AutoSize = true;
            _startButton.UseVisualStyleBackColor = true;
            _startButton.Click += startButton_Click;

            // Add controls to flowLayoutPanel
            _flowLayoutPanel.Controls.Add(_boardSizeComboBox);
            _flowLayoutPanel.Controls.Add(_startButton);

            // StartGameDialog
            MinimumSize = new Size(400, 50);
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(_flowLayoutPanel);
            Name = "StartGameDialog";
            Text = "Select Size";

            // Disable the fullscreen button
            MaximizeBox = false;
        }

        private void startButton_Click(object? sender, EventArgs e)
        {
            var selectedSize = _boardSizeComboBox?.SelectedItem?.ToString();
            BoardSize = selectedSize switch
            {
                "3x3" => 3,
                "4x4" => 4,
                "5x5" => 5,
                _ => BoardSize
            };
            DialogResult = DialogResult.OK;
            Close();
        }

        private readonly ComboBox _boardSizeComboBox;
        private readonly Button _startButton;
        private readonly FlowLayoutPanel _flowLayoutPanel;
    }
}
