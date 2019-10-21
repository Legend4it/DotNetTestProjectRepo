namespace MultiLangWinFormDemo
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnEng = new System.Windows.Forms.Button();
            this.btnSe = new System.Windows.Forms.Button();
            this.ShowNewForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            resources.ApplyResources(this.lblMessage, "lblMessage");
            this.lblMessage.Name = "lblMessage";
            // 
            // btnEng
            // 
            resources.ApplyResources(this.btnEng, "btnEng");
            this.btnEng.Name = "btnEng";
            this.btnEng.UseVisualStyleBackColor = true;
            this.btnEng.Click += new System.EventHandler(this.btnEng_Click);
            // 
            // btnSe
            // 
            resources.ApplyResources(this.btnSe, "btnSe");
            this.btnSe.Name = "btnSe";
            this.btnSe.UseVisualStyleBackColor = true;
            this.btnSe.Click += new System.EventHandler(this.btnSe_Click);
            // 
            // ShowNewForm
            // 
            resources.ApplyResources(this.ShowNewForm, "ShowNewForm");
            this.ShowNewForm.Name = "ShowNewForm";
            this.ShowNewForm.UseVisualStyleBackColor = true;
            this.ShowNewForm.Click += new System.EventHandler(this.ShowNewForm_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ShowNewForm);
            this.Controls.Add(this.btnSe);
            this.Controls.Add(this.btnEng);
            this.Controls.Add(this.lblMessage);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnEng;
        private System.Windows.Forms.Button btnSe;
        private System.Windows.Forms.Button ShowNewForm;
    }
}

