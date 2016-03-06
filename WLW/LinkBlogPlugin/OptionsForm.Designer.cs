namespace AlvinAshcraft.LinkBuilder
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AtomFeedLabel = new System.Windows.Forms.Label();
            this.AtomFeedTextBox = new System.Windows.Forms.TextBox();
            this.PostPathLabel = new System.Windows.Forms.Label();
            this.PostPathTextBox = new System.Windows.Forms.TextBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.OptionsToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.PostPathDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.FeedTypeLabel = new System.Windows.Forms.Label();
            this.FeedTypeComboBox = new System.Windows.Forms.ComboBox();
            this.PostPrefixTextBox = new System.Windows.Forms.TextBox();
            this.PostPrefixLabel = new System.Windows.Forms.Label();
            this.MaxPostsLabel = new System.Windows.Forms.Label();
            this.MaxPostsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.BufferLabel = new System.Windows.Forms.Label();
            this.BufferNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.MaxPostsNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BufferNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // AtomFeedLabel
            // 
            this.AtomFeedLabel.AutoSize = true;
            this.AtomFeedLabel.Location = new System.Drawing.Point(9, 13);
            this.AtomFeedLabel.Name = "AtomFeedLabel";
            this.AtomFeedLabel.Size = new System.Drawing.Size(47, 13);
            this.AtomFeedLabel.TabIndex = 0;
            this.AtomFeedLabel.Text = "Feed Url";
            // 
            // AtomFeedTextBox
            // 
            this.AtomFeedTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AtomFeedTextBox.Location = new System.Drawing.Point(12, 29);
            this.AtomFeedTextBox.Name = "AtomFeedTextBox";
            this.AtomFeedTextBox.Size = new System.Drawing.Size(263, 20);
            this.AtomFeedTextBox.TabIndex = 1;
            // 
            // PostPathLabel
            // 
            this.PostPathLabel.AutoSize = true;
            this.PostPathLabel.Location = new System.Drawing.Point(9, 52);
            this.PostPathLabel.Name = "PostPathLabel";
            this.PostPathLabel.Size = new System.Drawing.Size(119, 13);
            this.PostPathLabel.TabIndex = 2;
            this.PostPathLabel.Text = "Path to Published Posts";
            // 
            // PostPathTextBox
            // 
            this.PostPathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PostPathTextBox.Location = new System.Drawing.Point(12, 68);
            this.PostPathTextBox.Name = "PostPathTextBox";
            this.PostPathTextBox.Size = new System.Drawing.Size(234, 20);
            this.PostPathTextBox.TabIndex = 3;
            // 
            // SaveButton
            // 
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SaveButton.Location = new System.Drawing.Point(119, 182);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 13;
            this.SaveButton.Text = "&Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CancelButton.Location = new System.Drawing.Point(200, 182);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 14;
            this.CancelButton.Text = "&Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // BrowseButton
            // 
            this.BrowseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BrowseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseButton.Location = new System.Drawing.Point(252, 67);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(23, 23);
            this.BrowseButton.TabIndex = 4;
            this.BrowseButton.Text = "...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            this.BrowseButton.MouseHover += new System.EventHandler(this.BrowseButton_MouseHover);
            // 
            // PostPathDialog
            // 
            this.PostPathDialog.ShowNewFolderButton = false;
            // 
            // FeedTypeLabel
            // 
            this.FeedTypeLabel.AutoSize = true;
            this.FeedTypeLabel.Location = new System.Drawing.Point(9, 96);
            this.FeedTypeLabel.Name = "FeedTypeLabel";
            this.FeedTypeLabel.Size = new System.Drawing.Size(58, 13);
            this.FeedTypeLabel.TabIndex = 5;
            this.FeedTypeLabel.Text = "Feed Type";
            // 
            // FeedTypeComboBox
            // 
            this.FeedTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FeedTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.FeedTypeComboBox.FormattingEnabled = true;
            this.FeedTypeComboBox.Items.AddRange(new object[] {
            "Atom 1.0",
            "Rss 2.0",
            "NewsBlur API"});
            this.FeedTypeComboBox.Location = new System.Drawing.Point(12, 112);
            this.FeedTypeComboBox.Name = "FeedTypeComboBox";
            this.FeedTypeComboBox.Size = new System.Drawing.Size(141, 21);
            this.FeedTypeComboBox.TabIndex = 6;
            // 
            // PostPrefixTextBox
            // 
            this.PostPrefixTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PostPrefixTextBox.Location = new System.Drawing.Point(12, 156);
            this.PostPrefixTextBox.Name = "PostPrefixTextBox";
            this.PostPrefixTextBox.Size = new System.Drawing.Size(116, 20);
            this.PostPrefixTextBox.TabIndex = 8;
            // 
            // PostPrefixLabel
            // 
            this.PostPrefixLabel.AutoSize = true;
            this.PostPrefixLabel.Location = new System.Drawing.Point(9, 140);
            this.PostPrefixLabel.Name = "PostPrefixLabel";
            this.PostPrefixLabel.Size = new System.Drawing.Size(80, 13);
            this.PostPrefixLabel.TabIndex = 7;
            this.PostPrefixLabel.Text = "Post Title Prefix";
            // 
            // MaxPostsLabel
            // 
            this.MaxPostsLabel.AutoSize = true;
            this.MaxPostsLabel.Location = new System.Drawing.Point(156, 96);
            this.MaxPostsLabel.Name = "MaxPostsLabel";
            this.MaxPostsLabel.Size = new System.Drawing.Size(99, 13);
            this.MaxPostsLabel.TabIndex = 9;
            this.MaxPostsLabel.Text = "Max. Post Retrieval";
            // 
            // MaxPostsNumericUpDown
            // 
            this.MaxPostsNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MaxPostsNumericUpDown.Location = new System.Drawing.Point(159, 112);
            this.MaxPostsNumericUpDown.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.MaxPostsNumericUpDown.Name = "MaxPostsNumericUpDown";
            this.MaxPostsNumericUpDown.Size = new System.Drawing.Size(116, 20);
            this.MaxPostsNumericUpDown.TabIndex = 10;
            // 
            // BufferLabel
            // 
            this.BufferLabel.AutoSize = true;
            this.BufferLabel.Location = new System.Drawing.Point(133, 140);
            this.BufferLabel.Name = "BufferLabel";
            this.BufferLabel.Size = new System.Drawing.Size(119, 13);
            this.BufferLabel.TabIndex = 11;
            this.BufferLabel.Text = "Last Post Buffer (Hours)";
            // 
            // BufferNumericUpDown
            // 
            this.BufferNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BufferNumericUpDown.Location = new System.Drawing.Point(136, 156);
            this.BufferNumericUpDown.Maximum = new decimal(new int[] {
            96,
            0,
            0,
            0});
            this.BufferNumericUpDown.Name = "BufferNumericUpDown";
            this.BufferNumericUpDown.Size = new System.Drawing.Size(139, 20);
            this.BufferNumericUpDown.TabIndex = 12;
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.SaveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 217);
            this.Controls.Add(this.BufferNumericUpDown);
            this.Controls.Add(this.BufferLabel);
            this.Controls.Add(this.MaxPostsNumericUpDown);
            this.Controls.Add(this.MaxPostsLabel);
            this.Controls.Add(this.PostPrefixLabel);
            this.Controls.Add(this.PostPrefixTextBox);
            this.Controls.Add(this.FeedTypeComboBox);
            this.Controls.Add(this.FeedTypeLabel);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.PostPathTextBox);
            this.Controls.Add(this.PostPathLabel);
            this.Controls.Add(this.AtomFeedTextBox);
            this.Controls.Add(this.AtomFeedLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options - Insert Shared Links";
            ((System.ComponentModel.ISupportInitialize)(this.MaxPostsNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BufferNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AtomFeedLabel;
        private System.Windows.Forms.TextBox AtomFeedTextBox;
        private System.Windows.Forms.Label PostPathLabel;
        private System.Windows.Forms.TextBox PostPathTextBox;
        private System.Windows.Forms.Button SaveButton;
        private new System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.ToolTip OptionsToolTip;
        private System.Windows.Forms.FolderBrowserDialog PostPathDialog;
        private System.Windows.Forms.Label FeedTypeLabel;
        private System.Windows.Forms.ComboBox FeedTypeComboBox;
        private System.Windows.Forms.TextBox PostPrefixTextBox;
        private System.Windows.Forms.Label PostPrefixLabel;
        private System.Windows.Forms.Label MaxPostsLabel;
        private System.Windows.Forms.NumericUpDown MaxPostsNumericUpDown;
        private System.Windows.Forms.Label BufferLabel;
        private System.Windows.Forms.NumericUpDown BufferNumericUpDown;
    }
}