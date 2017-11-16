namespace NeuralNetForms
{
    partial class TrainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudGuessRate = new System.Windows.Forms.NumericUpDown();
            this.nudAccuracy = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.nudCycles = new System.Windows.Forms.NumericUpDown();
            this.txtGuessRate = new System.Windows.Forms.Label();
            this.txtAccuracyRate = new System.Windows.Forms.Label();
            this.txtCycleCount = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudGuessRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAccuracy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCycles)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Desired Guess Rate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Desired Accuracy Rate";
            // 
            // nudGuessRate
            // 
            this.nudGuessRate.DecimalPlaces = 2;
            this.nudGuessRate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudGuessRate.Location = new System.Drawing.Point(22, 62);
            this.nudGuessRate.Margin = new System.Windows.Forms.Padding(2);
            this.nudGuessRate.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudGuessRate.Name = "nudGuessRate";
            this.nudGuessRate.Size = new System.Drawing.Size(95, 20);
            this.nudGuessRate.TabIndex = 2;
            this.nudGuessRate.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudAccuracy
            // 
            this.nudAccuracy.DecimalPlaces = 2;
            this.nudAccuracy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudAccuracy.Location = new System.Drawing.Point(182, 61);
            this.nudAccuracy.Margin = new System.Windows.Forms.Padding(2);
            this.nudAccuracy.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudAccuracy.Name = "nudAccuracy";
            this.nudAccuracy.Size = new System.Drawing.Size(116, 20);
            this.nudAccuracy.TabIndex = 3;
            this.nudAccuracy.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 100);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Current Guess Rate";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(180, 100);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Current Accuracy Rate";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 164);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Desired Training Cycles";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(180, 164);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Current Training Cycles";
            // 
            // nudCycles
            // 
            this.nudCycles.Location = new System.Drawing.Point(24, 196);
            this.nudCycles.Margin = new System.Windows.Forms.Padding(2);
            this.nudCycles.Name = "nudCycles";
            this.nudCycles.Size = new System.Drawing.Size(91, 20);
            this.nudCycles.TabIndex = 8;
            // 
            // txtGuessRate
            // 
            this.txtGuessRate.AutoSize = true;
            this.txtGuessRate.Location = new System.Drawing.Point(24, 129);
            this.txtGuessRate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtGuessRate.Name = "txtGuessRate";
            this.txtGuessRate.Size = new System.Drawing.Size(21, 13);
            this.txtGuessRate.TabIndex = 9;
            this.txtGuessRate.Text = "0%";
            // 
            // txtAccuracyRate
            // 
            this.txtAccuracyRate.AutoSize = true;
            this.txtAccuracyRate.Location = new System.Drawing.Point(184, 129);
            this.txtAccuracyRate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtAccuracyRate.Name = "txtAccuracyRate";
            this.txtAccuracyRate.Size = new System.Drawing.Size(21, 13);
            this.txtAccuracyRate.TabIndex = 10;
            this.txtAccuracyRate.Text = "0%";
            // 
            // txtCycleCount
            // 
            this.txtCycleCount.AutoSize = true;
            this.txtCycleCount.Location = new System.Drawing.Point(184, 197);
            this.txtCycleCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtCycleCount.Name = "txtCycleCount";
            this.txtCycleCount.Size = new System.Drawing.Size(13, 13);
            this.txtCycleCount.TabIndex = 11;
            this.txtCycleCount.Text = "0";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(62, 312);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(78, 24);
            this.btnStart.TabIndex = 12;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.ClickStartTraining);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(186, 312);
            this.btnStop.Margin = new System.Windows.Forms.Padding(2);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(63, 24);
            this.btnStop.TabIndex = 13;
            this.btnStop.Text = "Stop";
            this.btnStop.UseCompatibleTextRendering = true;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.clickStop);
            // 
            // TrainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 398);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtCycleCount);
            this.Controls.Add(this.txtAccuracyRate);
            this.Controls.Add(this.txtGuessRate);
            this.Controls.Add(this.nudCycles);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudAccuracy);
            this.Controls.Add(this.nudGuessRate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TrainForm";
            this.Text = "TrainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.closeForm);
            ((System.ComponentModel.ISupportInitialize)(this.nudGuessRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAccuracy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCycles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudGuessRate;
        private System.Windows.Forms.NumericUpDown nudAccuracy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudCycles;
        private System.Windows.Forms.Label txtGuessRate;
        private System.Windows.Forms.Label txtAccuracyRate;
        private System.Windows.Forms.Label txtCycleCount;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
    }
}