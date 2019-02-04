namespace AlvinAshcraft.LinkBuilder
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// Dialog that allows the user to change plugin options.
    /// </summary>
    public partial class OptionsForm : Form
    {
        /// <summary>
        /// Object to hold the plugin options.
        /// </summary>
        private readonly PluginSettings _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsForm"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public OptionsForm(PluginSettings options)
        {
            _options = options;
            InitializeComponent();
            AtomFeedTextBox.Text = options.FeedUrlOption;
            PostPathTextBox.Text = options.PostPathOption;
            FeedTypeComboBox.Text = options.FeedTypeOption;
            PostPrefixTextBox.Text = options.PostPrefixOption;
            MaxPostsNumericUpDown.Value = options.MaxPostsOption;
            BufferNumericUpDown.Value = options.BufferOption;
        }

        /// <summary>
        /// Handles the Click event of the BrowseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(PostPathTextBox.Text))
            {
                PostPathDialog.SelectedPath = PostPathTextBox.Text;
            }

            var result = PostPathDialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                PostPathTextBox.Text = PostPathDialog.SelectedPath;
            }
        }

        /// <summary>
        /// Handles the MouseHover event of the BrowseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BrowseButton_MouseHover(object sender, EventArgs e)
        {
            OptionsToolTip.SetToolTip(BrowseButton, "Browse for folder.");
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            _options.FeedUrlOption = AtomFeedTextBox.Text;
            _options.PostPathOption = PostPathTextBox.Text;
            _options.FeedTypeOption = FeedTypeComboBox.Text;
            _options.PostPrefixOption = PostPrefixTextBox.Text;
            _options.MaxPostsOption = Convert.ToInt32(MaxPostsNumericUpDown.Value);
            _options.BufferOption = Convert.ToInt32(BufferNumericUpDown.Value);

            Close();
        }

        /// <summary>
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}