namespace Szablon_Dyżury
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel_boczny = new System.Windows.Forms.Panel();
            this.tC_naucz_sale = new System.Windows.Forms.TabControl();
            this.tP_nauczyciele = new System.Windows.Forms.TabPage();
            this.gB_naucz = new System.Windows.Forms.GroupBox();
            this.rB_n_Odznacz = new System.Windows.Forms.RadioButton();
            this.tP_sale = new System.Windows.Forms.TabPage();
            this.gB_sale = new System.Windows.Forms.GroupBox();
            this.cB_s_Odznacz = new System.Windows.Forms.CheckBox();
            this.tP_plan = new System.Windows.Forms.TabPage();
            this.panel_plan = new System.Windows.Forms.Panel();
            this.lb_nauczyciel = new System.Windows.Forms.Label();
            this.panel_glowny = new System.Windows.Forms.Panel();
            this.cb_Nauczyciele = new System.Windows.Forms.ComboBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.lb_pia = new System.Windows.Forms.Label();
            this.lb_czw = new System.Windows.Forms.Label();
            this.lb_srd = new System.Windows.Forms.Label();
            this.lb_wtr = new System.Windows.Forms.Label();
            this.lb_pon = new System.Windows.Forms.Label();
            this.lb_pracownie = new System.Windows.Forms.Label();
            this.lb_glowny = new System.Windows.Forms.Label();
            this.tT_przyciski = new System.Windows.Forms.ToolTip(this.components);
            this.btn_edytor = new System.Windows.Forms.Button();
            this.btn_pdf = new System.Windows.Forms.Button();
            this.btn_inport = new System.Windows.Forms.Button();
            this.btn_export = new System.Windows.Forms.Button();
            this.btn_zamien = new System.Windows.Forms.Button();
            this.btn_reset = new System.Windows.Forms.Button();
            this.btn_pobierz_plany = new System.Windows.Forms.Button();
            this.btn_wczytaj = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_min = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.panel_boczny.SuspendLayout();
            this.tC_naucz_sale.SuspendLayout();
            this.tP_nauczyciele.SuspendLayout();
            this.gB_naucz.SuspendLayout();
            this.tP_sale.SuspendLayout();
            this.gB_sale.SuspendLayout();
            this.tP_plan.SuspendLayout();
            this.panel_glowny.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_boczny
            // 
            this.panel_boczny.AutoScroll = true;
            this.panel_boczny.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel_boczny.BackColor = System.Drawing.Color.Beige;
            this.panel_boczny.Controls.Add(this.btn_edytor);
            this.panel_boczny.Controls.Add(this.btn_pdf);
            this.panel_boczny.Controls.Add(this.btn_inport);
            this.panel_boczny.Controls.Add(this.btn_export);
            this.panel_boczny.Controls.Add(this.btn_zamien);
            this.panel_boczny.Controls.Add(this.btn_reset);
            this.panel_boczny.Controls.Add(this.btn_pobierz_plany);
            this.panel_boczny.Controls.Add(this.btn_wczytaj);
            this.panel_boczny.Controls.Add(this.btn_save);
            this.panel_boczny.Controls.Add(this.btn_min);
            this.panel_boczny.Controls.Add(this.btn_close);
            this.panel_boczny.Controls.Add(this.tC_naucz_sale);
            this.panel_boczny.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_boczny.Location = new System.Drawing.Point(1434, 0);
            this.panel_boczny.Name = "panel_boczny";
            this.panel_boczny.Size = new System.Drawing.Size(450, 1061);
            this.panel_boczny.TabIndex = 0;
            // 
            // tC_naucz_sale
            // 
            this.tC_naucz_sale.Controls.Add(this.tP_nauczyciele);
            this.tC_naucz_sale.Controls.Add(this.tP_sale);
            this.tC_naucz_sale.Controls.Add(this.tP_plan);
            this.tC_naucz_sale.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tC_naucz_sale.Location = new System.Drawing.Point(3, 52);
            this.tC_naucz_sale.Name = "tC_naucz_sale";
            this.tC_naucz_sale.SelectedIndex = 0;
            this.tC_naucz_sale.Size = new System.Drawing.Size(435, 1005);
            this.tC_naucz_sale.TabIndex = 0;
            // 
            // tP_nauczyciele
            // 
            this.tP_nauczyciele.AutoScroll = true;
            this.tP_nauczyciele.Controls.Add(this.gB_naucz);
            this.tP_nauczyciele.Location = new System.Drawing.Point(4, 27);
            this.tP_nauczyciele.Name = "tP_nauczyciele";
            this.tP_nauczyciele.Padding = new System.Windows.Forms.Padding(3);
            this.tP_nauczyciele.Size = new System.Drawing.Size(427, 974);
            this.tP_nauczyciele.TabIndex = 0;
            this.tP_nauczyciele.Text = "Lista nauczycieli";
            this.tP_nauczyciele.UseVisualStyleBackColor = true;
            // 
            // gB_naucz
            // 
            this.gB_naucz.AutoSize = true;
            this.gB_naucz.BackColor = System.Drawing.Color.White;
            this.gB_naucz.Controls.Add(this.rB_n_Odznacz);
            this.gB_naucz.Location = new System.Drawing.Point(6, 0);
            this.gB_naucz.Name = "gB_naucz";
            this.gB_naucz.Size = new System.Drawing.Size(415, 961);
            this.gB_naucz.TabIndex = 0;
            this.gB_naucz.TabStop = false;
            // 
            // rB_n_Odznacz
            // 
            this.rB_n_Odznacz.AutoSize = true;
            this.rB_n_Odznacz.Checked = true;
            this.rB_n_Odznacz.Location = new System.Drawing.Point(10, 20);
            this.rB_n_Odznacz.Name = "rB_n_Odznacz";
            this.rB_n_Odznacz.Size = new System.Drawing.Size(95, 22);
            this.rB_n_Odznacz.TabIndex = 0;
            this.rB_n_Odznacz.TabStop = true;
            this.rB_n_Odznacz.Text = "Odznacz";
            this.rB_n_Odznacz.UseVisualStyleBackColor = true;
            // 
            // tP_sale
            // 
            this.tP_sale.AutoScroll = true;
            this.tP_sale.Controls.Add(this.gB_sale);
            this.tP_sale.Location = new System.Drawing.Point(4, 27);
            this.tP_sale.Name = "tP_sale";
            this.tP_sale.Padding = new System.Windows.Forms.Padding(3);
            this.tP_sale.Size = new System.Drawing.Size(427, 974);
            this.tP_sale.TabIndex = 1;
            this.tP_sale.Text = "Lista sal";
            this.tP_sale.UseVisualStyleBackColor = true;
            // 
            // gB_sale
            // 
            this.gB_sale.AutoSize = true;
            this.gB_sale.BackColor = System.Drawing.Color.White;
            this.gB_sale.Controls.Add(this.cB_s_Odznacz);
            this.gB_sale.Location = new System.Drawing.Point(6, 0);
            this.gB_sale.Name = "gB_sale";
            this.gB_sale.Size = new System.Drawing.Size(372, 964);
            this.gB_sale.TabIndex = 1;
            this.gB_sale.TabStop = false;
            // 
            // cB_s_Odznacz
            // 
            this.cB_s_Odznacz.AutoSize = true;
            this.cB_s_Odznacz.Location = new System.Drawing.Point(10, 20);
            this.cB_s_Odznacz.Name = "cB_s_Odznacz";
            this.cB_s_Odznacz.Size = new System.Drawing.Size(96, 22);
            this.cB_s_Odznacz.TabIndex = 2;
            this.cB_s_Odznacz.Text = "Odznacz";
            this.cB_s_Odznacz.UseVisualStyleBackColor = true;
            // 
            // tP_plan
            // 
            this.tP_plan.BackColor = System.Drawing.Color.White;
            this.tP_plan.Controls.Add(this.panel_plan);
            this.tP_plan.Controls.Add(this.lb_nauczyciel);
            this.tP_plan.Location = new System.Drawing.Point(4, 27);
            this.tP_plan.Name = "tP_plan";
            this.tP_plan.Size = new System.Drawing.Size(427, 974);
            this.tP_plan.TabIndex = 2;
            this.tP_plan.Text = "Plan lekcji";
            // 
            // panel_plan
            // 
            this.panel_plan.BackColor = System.Drawing.Color.Beige;
            this.panel_plan.Location = new System.Drawing.Point(5, 56);
            this.panel_plan.Name = "panel_plan";
            this.panel_plan.Size = new System.Drawing.Size(347, 913);
            this.panel_plan.TabIndex = 1;
            // 
            // lb_nauczyciel
            // 
            this.lb_nauczyciel.AutoSize = true;
            this.lb_nauczyciel.Font = new System.Drawing.Font("Verdana", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_nauczyciel.Location = new System.Drawing.Point(12, 16);
            this.lb_nauczyciel.Name = "lb_nauczyciel";
            this.lb_nauczyciel.Size = new System.Drawing.Size(224, 25);
            this.lb_nauczyciel.TabIndex = 0;
            this.lb_nauczyciel.Text = "Obecny nauczyciel: -";
            // 
            // panel_glowny
            // 
            this.panel_glowny.AutoScroll = true;
            this.panel_glowny.AutoSize = true;
            this.panel_glowny.BackColor = System.Drawing.Color.Beige;
            this.panel_glowny.Controls.Add(this.cb_Nauczyciele);
            this.panel_glowny.Controls.Add(this.dgv);
            this.panel_glowny.Controls.Add(this.lb_pia);
            this.panel_glowny.Controls.Add(this.lb_czw);
            this.panel_glowny.Controls.Add(this.lb_srd);
            this.panel_glowny.Controls.Add(this.lb_wtr);
            this.panel_glowny.Controls.Add(this.lb_pon);
            this.panel_glowny.Controls.Add(this.lb_pracownie);
            this.panel_glowny.Controls.Add(this.lb_glowny);
            this.panel_glowny.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_glowny.Location = new System.Drawing.Point(0, 0);
            this.panel_glowny.Name = "panel_glowny";
            this.panel_glowny.Size = new System.Drawing.Size(1151, 1061);
            this.panel_glowny.TabIndex = 1;
            this.panel_glowny.Tag = "end";
            // 
            // cb_Nauczyciele
            // 
            this.cb_Nauczyciele.FormattingEnabled = true;
            this.cb_Nauczyciele.Location = new System.Drawing.Point(1027, 79);
            this.cb_Nauczyciele.Name = "cb_Nauczyciele";
            this.cb_Nauczyciele.Size = new System.Drawing.Size(121, 33);
            this.cb_Nauczyciele.TabIndex = 2;
            this.cb_Nauczyciele.Visible = false;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.BackgroundColor = System.Drawing.Color.Gold;
            this.dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.GridColor = System.Drawing.Color.White;
            this.dgv.Location = new System.Drawing.Point(30, 30);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowHeadersWidth = 51;
            this.dgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.Size = new System.Drawing.Size(991, 589);
            this.dgv.TabIndex = 1;
            // 
            // lb_pia
            // 
            this.lb_pia.AutoSize = true;
            this.lb_pia.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_pia.Location = new System.Drawing.Point(3, 918);
            this.lb_pia.Name = "lb_pia";
            this.lb_pia.Size = new System.Drawing.Size(23, 69);
            this.lb_pia.TabIndex = 9;
            this.lb_pia.Text = "P\r\nI\r\nĄ";
            // 
            // lb_czw
            // 
            this.lb_czw.AutoSize = true;
            this.lb_czw.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_czw.Location = new System.Drawing.Point(3, 722);
            this.lb_czw.Name = "lb_czw";
            this.lb_czw.Size = new System.Drawing.Size(29, 69);
            this.lb_czw.TabIndex = 8;
            this.lb_czw.Text = "C\r\nZ\r\nW";
            // 
            // lb_srd
            // 
            this.lb_srd.AutoSize = true;
            this.lb_srd.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_srd.Location = new System.Drawing.Point(3, 521);
            this.lb_srd.Name = "lb_srd";
            this.lb_srd.Size = new System.Drawing.Size(25, 69);
            this.lb_srd.TabIndex = 7;
            this.lb_srd.Text = "Ś\r\nR\r\nD";
            // 
            // lb_wtr
            // 
            this.lb_wtr.AutoSize = true;
            this.lb_wtr.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_wtr.Location = new System.Drawing.Point(3, 321);
            this.lb_wtr.Name = "lb_wtr";
            this.lb_wtr.Size = new System.Drawing.Size(29, 69);
            this.lb_wtr.TabIndex = 6;
            this.lb_wtr.Text = "W\r\nT\r\nR";
            // 
            // lb_pon
            // 
            this.lb_pon.AutoSize = true;
            this.lb_pon.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_pon.Location = new System.Drawing.Point(3, 122);
            this.lb_pon.Name = "lb_pon";
            this.lb_pon.Size = new System.Drawing.Size(25, 69);
            this.lb_pon.TabIndex = 5;
            this.lb_pon.Text = "P\r\nO\r\nN";
            // 
            // lb_pracownie
            // 
            this.lb_pracownie.AutoSize = true;
            this.lb_pracownie.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_pracownie.Location = new System.Drawing.Point(1041, 4);
            this.lb_pracownie.Name = "lb_pracownie";
            this.lb_pracownie.Size = new System.Drawing.Size(107, 23);
            this.lb_pracownie.TabIndex = 4;
            this.lb_pracownie.Text = "Pracownie";
            // 
            // lb_glowny
            // 
            this.lb_glowny.AutoSize = true;
            this.lb_glowny.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_glowny.Location = new System.Drawing.Point(312, 4);
            this.lb_glowny.Name = "lb_glowny";
            this.lb_glowny.Size = new System.Drawing.Size(168, 23);
            this.lb_glowny.TabIndex = 3;
            this.lb_glowny.Text = "Budynek główny";
            // 
            // btn_edytor
            // 
            this.btn_edytor.BackColor = System.Drawing.Color.Beige;
            this.btn_edytor.BackgroundImage = global::Szablon_Dyżury.Properties.Resources.btnedit;
            this.btn_edytor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_edytor.FlatAppearance.BorderSize = 0;
            this.btn_edytor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_edytor.Font = new System.Drawing.Font("Verdana", 12F);
            this.btn_edytor.Location = new System.Drawing.Point(7, 12);
            this.btn_edytor.Name = "btn_edytor";
            this.btn_edytor.Size = new System.Drawing.Size(34, 29);
            this.btn_edytor.TabIndex = 11;
            this.btn_edytor.UseVisualStyleBackColor = false;
            this.btn_edytor.Click += new System.EventHandler(this.btn_edytor_Click);
            // 
            // btn_pdf
            // 
            this.btn_pdf.BackColor = System.Drawing.Color.Beige;
            this.btn_pdf.BackgroundImage = global::Szablon_Dyżury.Properties.Resources.btnpdf;
            this.btn_pdf.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_pdf.FlatAppearance.BorderSize = 0;
            this.btn_pdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_pdf.Font = new System.Drawing.Font("Verdana", 12F);
            this.btn_pdf.Location = new System.Drawing.Point(44, 12);
            this.btn_pdf.Name = "btn_pdf";
            this.btn_pdf.Size = new System.Drawing.Size(34, 29);
            this.btn_pdf.TabIndex = 10;
            this.btn_pdf.UseVisualStyleBackColor = false;
            this.btn_pdf.Click += new System.EventHandler(this.btn_pdf_Click);
            // 
            // btn_inport
            // 
            this.btn_inport.BackColor = System.Drawing.Color.Beige;
            this.btn_inport.BackgroundImage = global::Szablon_Dyżury.Properties.Resources.Inport;
            this.btn_inport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_inport.FlatAppearance.BorderSize = 0;
            this.btn_inport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_inport.Font = new System.Drawing.Font("Verdana", 12F);
            this.btn_inport.Location = new System.Drawing.Point(84, 12);
            this.btn_inport.Name = "btn_inport";
            this.btn_inport.Size = new System.Drawing.Size(34, 29);
            this.btn_inport.TabIndex = 9;
            this.btn_inport.UseVisualStyleBackColor = false;
            this.btn_inport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btn_export
            // 
            this.btn_export.BackColor = System.Drawing.Color.Beige;
            this.btn_export.BackgroundImage = global::Szablon_Dyżury.Properties.Resources.Export;
            this.btn_export.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_export.FlatAppearance.BorderSize = 0;
            this.btn_export.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_export.Font = new System.Drawing.Font("Verdana", 12F);
            this.btn_export.Location = new System.Drawing.Point(124, 12);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(34, 29);
            this.btn_export.TabIndex = 8;
            this.btn_export.UseVisualStyleBackColor = false;
            this.btn_export.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btn_zamien
            // 
            this.btn_zamien.BackColor = System.Drawing.Color.Beige;
            this.btn_zamien.BackgroundImage = global::Szablon_Dyżury.Properties.Resources.Zamien;
            this.btn_zamien.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_zamien.FlatAppearance.BorderSize = 0;
            this.btn_zamien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_zamien.Font = new System.Drawing.Font("Verdana", 12F);
            this.btn_zamien.Location = new System.Drawing.Point(324, 12);
            this.btn_zamien.Name = "btn_zamien";
            this.btn_zamien.Size = new System.Drawing.Size(34, 29);
            this.btn_zamien.TabIndex = 7;
            this.btn_zamien.UseVisualStyleBackColor = false;
            this.btn_zamien.Click += new System.EventHandler(this.btn_zamien_Click);
            // 
            // btn_reset
            // 
            this.btn_reset.BackColor = System.Drawing.Color.Beige;
            this.btn_reset.BackgroundImage = global::Szablon_Dyżury.Properties.Resources.Reset;
            this.btn_reset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_reset.FlatAppearance.BorderSize = 0;
            this.btn_reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_reset.Font = new System.Drawing.Font("Verdana", 12F);
            this.btn_reset.Location = new System.Drawing.Point(164, 12);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(34, 29);
            this.btn_reset.TabIndex = 6;
            this.btn_reset.UseVisualStyleBackColor = false;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // btn_pobierz_plany
            // 
            this.btn_pobierz_plany.BackColor = System.Drawing.Color.Beige;
            this.btn_pobierz_plany.BackgroundImage = global::Szablon_Dyżury.Properties.Resources.Pobierz;
            this.btn_pobierz_plany.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_pobierz_plany.FlatAppearance.BorderSize = 0;
            this.btn_pobierz_plany.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_pobierz_plany.Font = new System.Drawing.Font("Verdana", 12F);
            this.btn_pobierz_plany.Location = new System.Drawing.Point(204, 12);
            this.btn_pobierz_plany.Name = "btn_pobierz_plany";
            this.btn_pobierz_plany.Size = new System.Drawing.Size(34, 29);
            this.btn_pobierz_plany.TabIndex = 5;
            this.btn_pobierz_plany.UseVisualStyleBackColor = false;
            this.btn_pobierz_plany.Click += new System.EventHandler(this.btn_pobierz_plany_Click);
            // 
            // btn_wczytaj
            // 
            this.btn_wczytaj.BackColor = System.Drawing.Color.Beige;
            this.btn_wczytaj.BackgroundImage = global::Szablon_Dyżury.Properties.Resources.Wczytaj;
            this.btn_wczytaj.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_wczytaj.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_wczytaj.FlatAppearance.BorderSize = 0;
            this.btn_wczytaj.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_wczytaj.Font = new System.Drawing.Font("Verdana", 12F);
            this.btn_wczytaj.Location = new System.Drawing.Point(244, 12);
            this.btn_wczytaj.Name = "btn_wczytaj";
            this.btn_wczytaj.Size = new System.Drawing.Size(34, 29);
            this.btn_wczytaj.TabIndex = 4;
            this.btn_wczytaj.UseVisualStyleBackColor = false;
            this.btn_wczytaj.Click += new System.EventHandler(this.btn_wczytaj_Click);
            // 
            // btn_save
            // 
            this.btn_save.BackColor = System.Drawing.Color.Beige;
            this.btn_save.BackgroundImage = global::Szablon_Dyżury.Properties.Resources.Zapisz;
            this.btn_save.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_save.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_save.FlatAppearance.BorderSize = 0;
            this.btn_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_save.Font = new System.Drawing.Font("Verdana", 12F);
            this.btn_save.Location = new System.Drawing.Point(284, 12);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(34, 29);
            this.btn_save.TabIndex = 3;
            this.btn_save.UseVisualStyleBackColor = false;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_min
            // 
            this.btn_min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_min.BackColor = System.Drawing.Color.Beige;
            this.btn_min.BackgroundImage = global::Szablon_Dyżury.Properties.Resources.Mini;
            this.btn_min.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_min.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_min.FlatAppearance.BorderSize = 0;
            this.btn_min.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_min.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn_min.Location = new System.Drawing.Point(364, 12);
            this.btn_min.Name = "btn_min";
            this.btn_min.Size = new System.Drawing.Size(34, 29);
            this.btn_min.TabIndex = 2;
            this.btn_min.UseVisualStyleBackColor = false;
            this.btn_min.Click += new System.EventHandler(this.btn_min_Click);
            // 
            // btn_close
            // 
            this.btn_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_close.BackColor = System.Drawing.Color.Beige;
            this.btn_close.BackgroundImage = global::Szablon_Dyżury.Properties.Resources.Zamknij;
            this.btn_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_close.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_close.FlatAppearance.BorderSize = 0;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn_close.Location = new System.Drawing.Point(404, 12);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(34, 29);
            this.btn_close.TabIndex = 1;
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Beige;
            this.ClientSize = new System.Drawing.Size(1884, 1061);
            this.Controls.Add(this.panel_glowny);
            this.Controls.Add(this.panel_boczny);
            this.Font = new System.Drawing.Font("Verdana", 15F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(31)))), ((int)(((byte)(11)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Aplikacja do wspomagania układania dyżurów";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel_boczny.ResumeLayout(false);
            this.tC_naucz_sale.ResumeLayout(false);
            this.tP_nauczyciele.ResumeLayout(false);
            this.tP_nauczyciele.PerformLayout();
            this.gB_naucz.ResumeLayout(false);
            this.gB_naucz.PerformLayout();
            this.tP_sale.ResumeLayout(false);
            this.tP_sale.PerformLayout();
            this.gB_sale.ResumeLayout(false);
            this.gB_sale.PerformLayout();
            this.tP_plan.ResumeLayout(false);
            this.tP_plan.PerformLayout();
            this.panel_glowny.ResumeLayout(false);
            this.panel_glowny.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_boczny;
        private System.Windows.Forms.TabControl tC_naucz_sale;
        private System.Windows.Forms.TabPage tP_nauczyciele;
        private System.Windows.Forms.TabPage tP_sale;
        private System.Windows.Forms.Panel panel_glowny;
        private System.Windows.Forms.GroupBox gB_naucz;
        private System.Windows.Forms.GroupBox gB_sale;
        private System.Windows.Forms.RadioButton rB_n_Odznacz;
        private System.Windows.Forms.TabPage tP_plan;
        private System.Windows.Forms.Label lb_nauczyciel;
        private System.Windows.Forms.Panel panel_plan;
        private System.Windows.Forms.Button btn_min;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.CheckBox cB_s_Odznacz;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.ComboBox cb_Nauczyciele;
        private System.Windows.Forms.Button btn_wczytaj;
        private System.Windows.Forms.Button btn_pobierz_plany;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.ToolTip tT_przyciski;
        private System.Windows.Forms.Label lb_glowny;
        private System.Windows.Forms.Label lb_pracownie;
        private System.Windows.Forms.Label lb_pon;
        private System.Windows.Forms.Label lb_wtr;
        private System.Windows.Forms.Label lb_srd;
        private System.Windows.Forms.Label lb_czw;
        private System.Windows.Forms.Label lb_pia;
        private System.Windows.Forms.Button btn_zamien;
        private System.Windows.Forms.Button btn_export;
        private System.Windows.Forms.Button btn_inport;
        private System.Windows.Forms.Button btn_pdf;
        private System.Windows.Forms.Button btn_edytor;
    }
}

