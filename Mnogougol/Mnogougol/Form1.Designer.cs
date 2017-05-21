namespace Mnogougol
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gener = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.maximum = new System.Windows.Forms.NumericUpDown();
            this.minimum = new System.Windows.Forms.NumericUpDown();
            this.Typem = new System.Windows.Forms.ListBox();
            this.export = new System.Windows.Forms.Button();
            this.beams = new System.Windows.Forms.CheckBox();
            this.Test = new System.Windows.Forms.Button();
            this.m = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maximum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(519, 371);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(767, 128);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(168, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(558, 13);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(143, 238);
            this.listBox1.TabIndex = 3;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(764, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Число вершин";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // gener
            // 
            this.gener.Location = new System.Drawing.Point(767, 315);
            this.gener.Name = "gener";
            this.gener.Size = new System.Drawing.Size(168, 69);
            this.gener.TabIndex = 5;
            this.gener.Text = "Сгенерировать  мноугогольник";
            this.gener.UseVisualStyleBackColor = true;
            this.gener.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.maximum);
            this.groupBox1.Controls.Add(this.minimum);
            this.groupBox1.Location = new System.Drawing.Point(760, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(175, 79);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Границы";
            // 
            // maximum
            // 
            this.maximum.Location = new System.Drawing.Point(7, 41);
            this.maximum.Name = "maximum";
            this.maximum.Size = new System.Drawing.Size(130, 20);
            this.maximum.TabIndex = 1;
            this.maximum.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // minimum
            // 
            this.minimum.Location = new System.Drawing.Point(7, 16);
            this.minimum.Name = "minimum";
            this.minimum.Size = new System.Drawing.Size(130, 20);
            this.minimum.TabIndex = 0;
            this.minimum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minimum.ValueChanged += new System.EventHandler(this.minimum_ValueChanged_1);
            // 
            // Typem
            // 
            this.Typem.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.Typem.AllowDrop = true;
            this.Typem.BackColor = System.Drawing.SystemColors.Window;
            this.Typem.FormattingEnabled = true;
            this.Typem.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.Typem.Items.AddRange(new object[] {
            "Выпуклый: первый алгоритм",
            "Выпуклый: второй алгоритм",
            "Звёздный"});
            this.Typem.Location = new System.Drawing.Point(767, 204);
            this.Typem.Name = "Typem";
            this.Typem.ScrollAlwaysVisible = true;
            this.Typem.Size = new System.Drawing.Size(168, 82);
            this.Typem.TabIndex = 8;
            // 
            // export
            // 
            this.export.Location = new System.Drawing.Point(767, 395);
            this.export.Name = "export";
            this.export.Size = new System.Drawing.Size(168, 69);
            this.export.TabIndex = 9;
            this.export.Text = "Export to TeX";
            this.export.UseVisualStyleBackColor = true;
            this.export.Click += new System.EventHandler(this.export_Click);
            // 
            // beams
            // 
            this.beams.AutoSize = true;
            this.beams.Location = new System.Drawing.Point(767, 171);
            this.beams.Name = "beams";
            this.beams.Size = new System.Drawing.Size(100, 17);
            this.beams.TabIndex = 10;
            this.beams.Text = "Показать лучи";
            this.beams.UseVisualStyleBackColor = true;
            this.beams.CheckedChanged += new System.EventHandler(this.beams_CheckedChanged);
            // 
            // Test
            // 
            this.Test.Location = new System.Drawing.Point(558, 323);
            this.Test.Name = "Test";
            this.Test.Size = new System.Drawing.Size(143, 53);
            this.Test.TabIndex = 11;
            this.Test.Text = "Тест на время построения  m многоугольников";
            this.Test.UseVisualStyleBackColor = true;
            this.Test.Click += new System.EventHandler(this.Test_Click);
            // 
            // m
            // 
            this.m.Location = new System.Drawing.Point(608, 288);
            this.m.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.m.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.m.Name = "m";
            this.m.Size = new System.Drawing.Size(93, 20);
            this.m.TabIndex = 12;
            this.m.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(570, 290);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "m=";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 476);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m);
            this.Controls.Add(this.Test);
            this.Controls.Add(this.beams);
            this.Controls.Add(this.export);
            this.Controls.Add(this.Typem);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gener);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.maximum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button gener;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown maximum;
        private System.Windows.Forms.NumericUpDown minimum;
        public System.Windows.Forms.ListBox Typem;
        public System.Windows.Forms.Button export;
        private System.Windows.Forms.CheckBox beams;
        private System.Windows.Forms.Button Test;
        private System.Windows.Forms.NumericUpDown m;
        private System.Windows.Forms.Label label2;
    }
}

