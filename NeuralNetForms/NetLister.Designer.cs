namespace NeuralNetForms
{
    partial class NetLister
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
            this.netList = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPercentRuns = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPercentCorrect = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // netList
            // 
            this.netList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colPercentRuns,
            this.colPercentCorrect});
            this.netList.FullRowSelect = true;
            this.netList.GridLines = true;
            this.netList.Location = new System.Drawing.Point(33, 35);
            this.netList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.netList.MultiSelect = false;
            this.netList.Name = "netList";
            this.netList.Size = new System.Drawing.Size(434, 97);
            this.netList.TabIndex = 8;
            this.netList.UseCompatibleStateImageBehavior = false;
            this.netList.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 278;
            // 
            // colPercentRuns
            // 
            this.colPercentRuns.Text = "PercentRuns";
            this.colPercentRuns.Width = 76;
            // 
            // colPercentCorrect
            // 
            this.colPercentCorrect.Text = "PercentCorrect";
            this.colPercentCorrect.Width = 76;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(331, 178);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(99, 26);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.clickCancel);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(60, 178);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(116, 26);
            this.btnLoad.TabIndex = 5;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.selectLoad);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(199, 178);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(108, 26);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.clickDelete);
            // 
            // NetLister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 265);
            this.Controls.Add(this.netList);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnLoad);
            this.Name = "NetLister";
            this.Text = "Net List";
            this.Load += new System.EventHandler(this.loadLister);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView netList;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colPercentRuns;
        private System.Windows.Forms.ColumnHeader colPercentCorrect;
    }
}