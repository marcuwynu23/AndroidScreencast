﻿namespace AndroidScreencast;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.pictureBox1 = new System.Windows.Forms.PictureBox();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
        this.SuspendLayout();

        // 
        // pictureBox1
        // 
        this.pictureBox1.Location = new System.Drawing.Point(12, 12);
        this.pictureBox1.Name = "pictureBox1";
        this.pictureBox1.Size = new System.Drawing.Size(760, 540); // Set initial size
        this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom; // Set to Zoom to maintain aspect ratio
        this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None; // Center it within the form
        this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill; // Fill the entire form area
        this.pictureBox1.TabIndex = 0;
        this.pictureBox1.TabStop = false;

        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(784, 561);
        this.Controls.Add(this.pictureBox1);
        this.Name = "Form1";
        this.Text = "Android Screencast";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);

        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
        this.ResumeLayout(false);
    }



    #endregion
}
