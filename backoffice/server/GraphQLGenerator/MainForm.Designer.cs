
namespace GraphQLGenerator
{
    partial class MainForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkQuery = new System.Windows.Forms.CheckBox();
            this.chkRepository = new System.Windows.Forms.CheckBox();
            this.lbNamespaces = new System.Windows.Forms.ListBox();
            this.GenerateCode = new System.Windows.Forms.Button();
            this.lvModels = new System.Windows.Forms.ListView();
            this.Model = new System.Windows.Forms.ColumnHeader();
            this.chkDto = new System.Windows.Forms.CheckBox();
            this.chkMutation = new System.Windows.Forms.CheckBox();
            this.chkSubscription = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkSubscription);
            this.groupBox1.Controls.Add(this.chkMutation);
            this.groupBox1.Controls.Add(this.chkDto);
            this.groupBox1.Controls.Add(this.chkQuery);
            this.groupBox1.Controls.Add(this.chkRepository);
            this.groupBox1.Controls.Add(this.lbNamespaces);
            this.groupBox1.Controls.Add(this.GenerateCode);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 197);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // chkQuery
            // 
            this.chkQuery.AutoSize = true;
            this.chkQuery.Location = new System.Drawing.Point(407, 99);
            this.chkQuery.Name = "chkQuery";
            this.chkQuery.Size = new System.Drawing.Size(70, 24);
            this.chkQuery.TabIndex = 4;
            this.chkQuery.Text = "Query";
            this.chkQuery.UseVisualStyleBackColor = true;
            // 
            // chkRepository
            // 
            this.chkRepository.AutoSize = true;
            this.chkRepository.Location = new System.Drawing.Point(407, 29);
            this.chkRepository.Name = "chkRepository";
            this.chkRepository.Size = new System.Drawing.Size(102, 24);
            this.chkRepository.TabIndex = 3;
            this.chkRepository.Text = "Repository";
            this.chkRepository.UseVisualStyleBackColor = true;
            // 
            // lbNamespaces
            // 
            this.lbNamespaces.FormattingEnabled = true;
            this.lbNamespaces.ItemHeight = 20;
            this.lbNamespaces.Location = new System.Drawing.Point(6, 29);
            this.lbNamespaces.Name = "lbNamespaces";
            this.lbNamespaces.Size = new System.Drawing.Size(363, 164);
            this.lbNamespaces.TabIndex = 2;
            this.lbNamespaces.SelectedValueChanged += new System.EventHandler(this.lbNamespaces_SelectedValueChanged);
            // 
            // GenerateCode
            // 
            this.GenerateCode.Location = new System.Drawing.Point(598, 12);
            this.GenerateCode.Name = "GenerateCode";
            this.GenerateCode.Size = new System.Drawing.Size(190, 53);
            this.GenerateCode.TabIndex = 1;
            this.GenerateCode.Text = "Generate Code";
            this.GenerateCode.UseVisualStyleBackColor = true;
            this.GenerateCode.Click += new System.EventHandler(this.GenerateCode_Click);
            // 
            // lvModels
            // 
            this.lvModels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Model});
            this.lvModels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvModels.GridLines = true;
            this.lvModels.HideSelection = false;
            this.lvModels.Location = new System.Drawing.Point(0, 197);
            this.lvModels.Name = "lvModels";
            this.lvModels.Size = new System.Drawing.Size(800, 254);
            this.lvModels.TabIndex = 1;
            this.lvModels.UseCompatibleStateImageBehavior = false;
            this.lvModels.View = System.Windows.Forms.View.Details;
            // 
            // Model
            // 
            this.Model.Text = "Model";
            this.Model.Width = 460;
            // 
            // chkDto
            // 
            this.chkDto.AutoSize = true;
            this.chkDto.Location = new System.Drawing.Point(407, 64);
            this.chkDto.Name = "chkDto";
            this.chkDto.Size = new System.Drawing.Size(59, 24);
            this.chkDto.TabIndex = 4;
            this.chkDto.Text = "DTO";
            this.chkDto.UseVisualStyleBackColor = true;
            // 
            // chkMutation
            // 
            this.chkMutation.AutoSize = true;
            this.chkMutation.Location = new System.Drawing.Point(407, 134);
            this.chkMutation.Name = "chkMutation";
            this.chkMutation.Size = new System.Drawing.Size(91, 24);
            this.chkMutation.TabIndex = 4;
            this.chkMutation.Text = "Mutation";
            this.chkMutation.UseVisualStyleBackColor = true;
            // 
            // chkSubscription
            // 
            this.chkSubscription.AutoSize = true;
            this.chkSubscription.Location = new System.Drawing.Point(407, 164);
            this.chkSubscription.Name = "chkSubscription";
            this.chkSubscription.Size = new System.Drawing.Size(113, 24);
            this.chkSubscription.TabIndex = 4;
            this.chkSubscription.Text = "Subscription";
            this.chkSubscription.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 451);
            this.Controls.Add(this.lvModels);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lvModels;
        private System.Windows.Forms.ColumnHeader Model;
        private System.Windows.Forms.Button GenerateCode;
        private System.Windows.Forms.ListBox lbNamespaces;
        private System.Windows.Forms.CheckBox chkQuery;
        private System.Windows.Forms.CheckBox chkRepository;
        private System.Windows.Forms.CheckBox chkMutation;
        private System.Windows.Forms.CheckBox chkDto;
        private System.Windows.Forms.CheckBox chkSubscription;
    }
}