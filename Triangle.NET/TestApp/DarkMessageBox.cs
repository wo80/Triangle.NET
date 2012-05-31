using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MeshExplorer
{
    class DarkMessageBox : Form
    {
        #region Designer

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
            this.btnClose = new MeshExplorer.Controls.DarkButton();
            this.btnShowLog = new MeshExplorer.Controls.DarkButton();
            this.lbMessage = new System.Windows.Forms.Label();
            this.lbInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(336, 87);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(92, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnShowLog
            // 
            this.btnShowLog.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnShowLog.Location = new System.Drawing.Point(234, 87);
            this.btnShowLog.Name = "btnShowLog";
            this.btnShowLog.Size = new System.Drawing.Size(92, 23);
            this.btnShowLog.TabIndex = 1;
            this.btnShowLog.Text = "Show Log";
            this.btnShowLog.UseVisualStyleBackColor = true;
            // 
            // lbMessage
            // 
            this.lbMessage.BackColor = System.Drawing.Color.Transparent;
            this.lbMessage.ForeColor = System.Drawing.Color.White;
            this.lbMessage.Location = new System.Drawing.Point(12, 9);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Size = new System.Drawing.Size(412, 30);
            this.lbMessage.TabIndex = 2;
            this.lbMessage.Text = "Message";
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.BackColor = System.Drawing.Color.Transparent;
            this.lbInfo.ForeColor = System.Drawing.Color.White;
            this.lbInfo.Location = new System.Drawing.Point(12, 47);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(28, 13);
            this.lbInfo.TabIndex = 3;
            this.lbInfo.Text = "Info";
            // 
            // DarkMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.ClientSize = new System.Drawing.Size(436, 118);
            this.Controls.Add(this.lbInfo);
            this.Controls.Add(this.lbMessage);
            this.Controls.Add(this.btnShowLog);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DarkMessageBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Message Box";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.DarkButton btnClose;
        private Label lbMessage;
        private Label lbInfo;
        private Controls.DarkButton btnShowLog;

        #endregion

        public DarkMessageBox()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var rect = this.ClientRectangle;
            rect.Height -= 40;

            e.Graphics.FillRectangle(Brushes.DimGray, rect);
        }

        public static DialogResult Show(string title, string message, string info, bool log)
        {
            DarkMessageBox m = new DarkMessageBox();

            if (!log)
            {
                m.btnShowLog.Enabled = false;
                m.btnShowLog.Visible = false;
            }

            m.Text = title;

            m.lbInfo.Text = info;
            m.lbMessage.Text = message;

            return m.ShowDialog();
        }

        public static DialogResult Show(string title, string message, string info)
        {
            return Show(title, message, info, false);
        }

        public static DialogResult Show(string title, string message)
        {
            return Show(title, message, "", false);
        }
    }
}
