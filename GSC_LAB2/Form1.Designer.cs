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
            visualModeComboBox = new ComboBox();
            algoritmChoseComboBox = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.BackColor = SystemColors.ButtonHighlight;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
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
            button1.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(848, 376);
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
            colorComboBox.BackColor = SystemColors.MenuBar;
            colorComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            colorComboBox.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            colorComboBox.FormattingEnabled = true;
            colorComboBox.Items.AddRange(new object[] { "Зелёный", "Красный", "Жёлтый", "Синий" });
            colorComboBox.Location = new Point(24, 396);
            colorComboBox.Name = "colorComboBox";
            colorComboBox.Size = new Size(243, 38);
            colorComboBox.TabIndex = 2;
            colorComboBox.SelectedIndexChanged += colorComboBox_SelectedIndexChanged;
            // 
            // visualModeComboBox
            // 
            visualModeComboBox.Anchor = AnchorStyles.Bottom;
            visualModeComboBox.BackColor = SystemColors.MenuBar;
            visualModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            visualModeComboBox.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            visualModeComboBox.FormattingEnabled = true;
            visualModeComboBox.Items.AddRange(new object[] { "C граничным многоугольником", "Без граничного многоугольника" });
            visualModeComboBox.Location = new Point(294, 396);
            visualModeComboBox.Name = "visualModeComboBox";
            visualModeComboBox.Size = new Size(391, 38);
            visualModeComboBox.TabIndex = 3;
            visualModeComboBox.SelectedIndexChanged += visualModeComboBox_SelectedIndexChanged;
            // 
            // algoritmChoseComboBox
            // 
            algoritmChoseComboBox.Anchor = AnchorStyles.Bottom;
            algoritmChoseComboBox.BackColor = SystemColors.MenuBar;
            algoritmChoseComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            algoritmChoseComboBox.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            algoritmChoseComboBox.FormattingEnabled = true;
            algoritmChoseComboBox.Items.AddRange(new object[] { "Для ориентированного многоугольника", "Для неориентированного многоугольника" });
            algoritmChoseComboBox.Location = new Point(24, 352);
            algoritmChoseComboBox.Name = "algoritmChoseComboBox";
            algoritmChoseComboBox.Size = new Size(661, 38);
            algoritmChoseComboBox.TabIndex = 4;
            algoritmChoseComboBox.SelectedIndexChanged += algoritmChoseComboBox_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(1015, 461);
            Controls.Add(algoritmChoseComboBox);
            Controls.Add(visualModeComboBox);
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
        private ComboBox visualModeComboBox;
        private ComboBox algoritmChoseComboBox;
    }
}