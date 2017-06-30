namespace NeuralNetForms
{
    partial class MainMenu
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.neuralNetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trainingFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.neuralNetToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.trainToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trainingFileBrowser = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNetName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTrainingFileName = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.neuralNetToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(564, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.importToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(197, 24);
            this.createToolStripMenuItem.Text = "Create";
            this.createToolStripMenuItem.Click += new System.EventHandler(this.createNewNet);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(197, 24);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveNet);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.neuralNetToolStripMenuItem,
            this.trainingFileToolStripMenuItem});
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(197, 24);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // neuralNetToolStripMenuItem
            // 
            this.neuralNetToolStripMenuItem.Name = "neuralNetToolStripMenuItem";
            this.neuralNetToolStripMenuItem.Size = new System.Drawing.Size(159, 24);
            this.neuralNetToolStripMenuItem.Text = "Neural Net";
            this.neuralNetToolStripMenuItem.Click += new System.EventHandler(this.selectLoadNet);
            // 
            // trainingFileToolStripMenuItem
            // 
            this.trainingFileToolStripMenuItem.Name = "trainingFileToolStripMenuItem";
            this.trainingFileToolStripMenuItem.Size = new System.Drawing.Size(159, 24);
            this.trainingFileToolStripMenuItem.Text = "Training File";
            this.trainingFileToolStripMenuItem.Click += new System.EventHandler(this.clickLoadTrainingFile);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(197, 24);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.clickImport);
            // 
            // neuralNetToolStripMenuItem1
            // 
            this.neuralNetToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trainToolStripMenuItem1,
            this.updateToolStripMenuItem,
            this.sendTextToolStripMenuItem});
            this.neuralNetToolStripMenuItem1.Name = "neuralNetToolStripMenuItem1";
            this.neuralNetToolStripMenuItem1.Size = new System.Drawing.Size(93, 24);
            this.neuralNetToolStripMenuItem1.Text = "Neural Net";
            // 
            // trainToolStripMenuItem1
            // 
            this.trainToolStripMenuItem1.Name = "trainToolStripMenuItem1";
            this.trainToolStripMenuItem1.Size = new System.Drawing.Size(143, 24);
            this.trainToolStripMenuItem1.Text = "Train";
            this.trainToolStripMenuItem1.Click += new System.EventHandler(this.clickTrain);
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(143, 24);
            this.updateToolStripMenuItem.Text = "Update";
            this.updateToolStripMenuItem.Click += new System.EventHandler(this.clickUpdate);
            // 
            // sendTextToolStripMenuItem
            // 
            this.sendTextToolStripMenuItem.Name = "sendTextToolStripMenuItem";
            this.sendTextToolStripMenuItem.Size = new System.Drawing.Size(143, 24);
            this.sendTextToolStripMenuItem.Text = "Send Text";
            this.sendTextToolStripMenuItem.Click += new System.EventHandler(this.clickSendText);
            // 
            // trainingFileBrowser
            // 
            this.trainingFileBrowser.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 98);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Net: ";
            // 
            // txtNetName
            // 
            this.txtNetName.AutoSize = true;
            this.txtNetName.Location = new System.Drawing.Point(35, 130);
            this.txtNetName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtNetName.Name = "txtNetName";
            this.txtNetName.Size = new System.Drawing.Size(51, 17);
            this.txtNetName.TabIndex = 2;
            this.txtNetName.Text = "default";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 169);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Training File:";
            // 
            // txtTrainingFileName
            // 
            this.txtTrainingFileName.AutoSize = true;
            this.txtTrainingFileName.Location = new System.Drawing.Point(35, 209);
            this.txtTrainingFileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtTrainingFileName.Name = "txtTrainingFileName";
            this.txtTrainingFileName.Size = new System.Drawing.Size(56, 17);
            this.txtTrainingFileName.TabIndex = 4;
            this.txtTrainingFileName.Text = "<none>";
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 399);
            this.Controls.Add(this.txtTrainingFileName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNetName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainMenu";
            this.Text = "Neural Net";
            this.Load += new System.EventHandler(this.loadMain);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem neuralNetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trainingFileToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog trainingFileBrowser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label txtNetName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label txtTrainingFileName;
        private System.Windows.Forms.ToolStripMenuItem neuralNetToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem trainToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
    }
}

