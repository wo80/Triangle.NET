namespace MeshExplorer
{
    partial class FormLog
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
            this.listLog = new System.Windows.Forms.ListView();
            this.colMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listLog
            // 
            this.listLog.BackColor = System.Drawing.Color.White;
            this.listLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colMessage,
            this.colInfo});
            this.listLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listLog.ForeColor = System.Drawing.Color.White;
            this.listLog.FullRowSelect = true;
            this.listLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listLog.Location = new System.Drawing.Point(0, 0);
            this.listLog.Name = "listLog";
            this.listLog.Size = new System.Drawing.Size(584, 262);
            this.listLog.TabIndex = 2;
            this.listLog.UseCompatibleStateImageBehavior = false;
            this.listLog.View = System.Windows.Forms.View.Details;
            this.listLog.DoubleClick += new System.EventHandler(this.listLog_DoubleClick);
            this.listLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listLog_KeyDown);
            // 
            // colMessage
            // 
            this.colMessage.Text = "Message";
            this.colMessage.Width = 350;
            // 
            // colInfo
            // 
            this.colInfo.Text = "Info";
            this.colInfo.Width = 200;
            // 
            // FormLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 262);
            this.Controls.Add(this.listLog);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Log";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLog_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listLog;
        private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.ColumnHeader colInfo;
    }
}