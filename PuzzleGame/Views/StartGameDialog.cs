namespace PuzzleGame.Views;

public class StartGameDialog : Form
{
    private readonly ComboBox _boardSizeComboBox;
    private readonly RadioButton _gameTypeStepsRadioButton;
    private readonly RadioButton _gameTypeTimeRadioButton;
    private readonly Button _startButton;
    private readonly FlowLayoutPanel _flowLayoutPanel;
    private readonly FlowLayoutPanel _rowLayoutPanel;
    public int BoardSize { get; private set; }
    public bool IsTimeGame { get; private set; }

    public StartGameDialog()
    {
        _boardSizeComboBox = new ComboBox();
        _gameTypeStepsRadioButton = new RadioButton();
        _gameTypeTimeRadioButton = new RadioButton();
        _startButton = new Button();
        _flowLayoutPanel = new FlowLayoutPanel();
        _rowLayoutPanel = new FlowLayoutPanel();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        // flowLayoutPanel
        _flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
        _flowLayoutPanel.Dock = DockStyle.Fill;
        _flowLayoutPanel.WrapContents = false;
        _flowLayoutPanel.AutoSize = true;
        _flowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _flowLayoutPanel.Padding = new Padding(10);

        // rowLayoutPanel
        _rowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
        _rowLayoutPanel.Dock = DockStyle.Fill;
        _rowLayoutPanel.WrapContents = false;
        _rowLayoutPanel.AutoSize = true;
        _rowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _rowLayoutPanel.Padding = new Padding(10);

        // boardSizeComboBox
        _boardSizeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _boardSizeComboBox.Items.AddRange(["    3×3", "    4×4", "    5×5"]);
        _boardSizeComboBox.SelectedIndex = 0; // Default to 3x3
        _boardSizeComboBox.Name = "_boardSizeComboBox";
        _boardSizeComboBox.Margin = new Padding(20, 4, 50, 0); // Aligning
        _boardSizeComboBox.AutoSize = true;

        // startButton
        _startButton.Name = "_startButton";
        _startButton.Text = "Начать игру";
        _startButton.AutoSize = true;
        _startButton.UseVisualStyleBackColor = true;
        _startButton.Click += startButton_Click;

        // gameTypeStepsRadioButton
        _gameTypeStepsRadioButton.Name = "_gameTypeStepsRadioButton";
        _gameTypeStepsRadioButton.Text = "Играть на ходы";
        _gameTypeStepsRadioButton.AutoSize = true;
        _gameTypeStepsRadioButton.Margin = new Padding(20, 4, 50, 0); // Aligning
        _gameTypeStepsRadioButton.Checked = true;

        // gameTypeTimeRadioButton
        _gameTypeTimeRadioButton.Name = "_gameTypeTimeRadioButton";
        _gameTypeTimeRadioButton.Text = "Играть на время";
        _gameTypeTimeRadioButton.AutoSize = true;
        _gameTypeTimeRadioButton.Margin = new Padding(20, 4, 50, 0); // Aligning

        // Add controls to panels
        _rowLayoutPanel.Controls.Add(_boardSizeComboBox);
        _rowLayoutPanel.Controls.Add(_startButton);

        _flowLayoutPanel.Controls.Add(_rowLayoutPanel);
        _flowLayoutPanel.Controls.Add(_gameTypeStepsRadioButton);
        _flowLayoutPanel.Controls.Add(_gameTypeTimeRadioButton);

        // StartGameDialog
        MinimumSize = new Size(400, 50);
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Controls.Add(_flowLayoutPanel);
        Name = "StartGameDialog";
        Text = "Новая игра";
        Font = new Font("Comfortaa", 12, FontStyle.Bold);

        // Disable the fullscreen button
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
    }

    private void startButton_Click(object? sender, EventArgs e)
    {
        var selectedSize = _boardSizeComboBox.SelectedItem?.ToString();
        BoardSize = selectedSize switch
        {
            "    3×3" => 3,
            "    4×4" => 4,
            _ => 5
        };
        IsTimeGame = _gameTypeTimeRadioButton.Checked;
        DialogResult = DialogResult.OK;
        Close();
    }
}
