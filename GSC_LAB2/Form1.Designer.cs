namespace GSC_LAB2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;




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
            pictureBox1 = new PictureBox();
            button1 = new Button();
            colorComboBox = new ComboBox();
            tsoComboBox = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.BackColor = SystemColors.ButtonHighlight;
            pictureBox1.Location = new Point(-1, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1014, 336);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.MouseDown += pictureBox_MouseDown;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button1.BackColor = SystemColors.Control;
            button1.Font = new Font("Segoe UI Semilight", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(848, 391);
            button1.Name = "button1";
            button1.Size = new Size(155, 46);
            button1.TabIndex = 1;
            button1.Text = "Очистить";
            button1.UseVisualStyleBackColor = false;
            button1.Click += btClear_Click;
            // 
            // colorComboBox
            // 
            colorComboBox.Anchor = AnchorStyles.Bottom;
            colorComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            colorComboBox.Font = new Font("Segoe UI Semilight", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            colorComboBox.FormattingEnabled = true;
            colorComboBox.Items.AddRange(new object[] { "Зелёный", "Красный", "Жёлтый", "Синий" });
            colorComboBox.Location = new Point(21, 391);
            colorComboBox.Name = "colorComboBox";
            colorComboBox.Size = new Size(257, 38);
            colorComboBox.TabIndex = 2;
            colorComboBox.SelectedIndexChanged += colorComboBox_SelectedIndexChanged;
            // 
            // tsoComboBox
            // 
            tsoComboBox.Anchor = AnchorStyles.Bottom;
            tsoComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            tsoComboBox.Font = new Font("Segoe UI Semilight", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            tsoComboBox.FormattingEnabled = true;
            tsoComboBox.Items.AddRange(new object[] { "ТМО - Объединение", "ТМО - Пересечение", "ТМО - Симметричная разность", "ТМО - Разность A/B", "ТМО - Разность B/A" });
            tsoComboBox.Location = new Point(307, 391);
            tsoComboBox.Name = "tsoComboBox";
            tsoComboBox.Size = new Size(355, 38);
            tsoComboBox.TabIndex = 3;
            tsoComboBox.SelectedIndexChanged += tsoComboBox_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(1015, 461);
            Controls.Add(tsoComboBox);
            Controls.Add(colorComboBox);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            MinimumSize = new Size(1000, 500);
            Name = "Form1";
            Text = "Form1";
            SizeChanged += formSizeChangedListener;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Button button1;
        private ComboBox colorComboBox;
        private ComboBox tsoComboBox;
    }
}