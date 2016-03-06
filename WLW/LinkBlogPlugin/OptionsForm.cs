//--------------------------------------------------------------------------------------------------------------------- 
// <copyright file="OptionsForm.cs" company="Alvinitech">
//   Copyright by Alvin Ashcraft 2013 - Alvinitech.
// </copyright>
// <summary>
//   Defines the OptionsForm type.
// </summary>
//---------------------------------------------------------------------------------------------------------------------
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
        private readonly PluginSettings options;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsForm"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public OptionsForm(PluginSettings options)
        {
            this.options = options;
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
            this.options.FeedUrlOption = AtomFeedTextBox.Text;
            this.options.PostPathOption = PostPathTextBox.Text;
            this.options.FeedTypeOption = FeedTypeComboBox.Text;
            this.options.PostPrefixOption = PostPrefixTextBox.Text;
            this.options.MaxPostsOption = Convert.ToInt32(MaxPostsNumericUpDown.Value);
            this.options.BufferOption = Convert.ToInt32(BufferNumericUpDown.Value);

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
