using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Szablon_Dyżury
{
    public class FormEdytorPlanow : Form
    {
        //Ścieżki
        private string planyFolder;
        private string listaNaucz;

        //Kontrolki
        private ComboBox cb_wybierz;
        private Label lb_wybierz;
        private DataGridView dgv_plan;
        private Button btn_zapisz;
        private Button btn_zamknij;
        private Label lb_info;
        private Panel pnl_top;
        private Panel pnl_bottom;

        //Obecny nauczyciel
        private string obecnySkrot = "";

        //Dni tygodnia
        private static readonly string[] dniTygodnia = { "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek" };

        public FormEdytorPlanow(string planyFolder, string listaNaucz)
        {
            this.planyFolder = planyFolder;
            this.listaNaucz = listaNaucz;

            this.SuspendLayout();
            ZbudujForme();
            this.ResumeLayout(false);

            WczytajListeNauczycieli();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void ZbudujForme()
        {
            //Ustawienia okna
            this.Text = "Edytor planów nauczycieli";
            this.Size = new Size(700, 620);
            this.MinimumSize = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.BackColor = Color.Beige;
            this.Font = new Font("Verdana", 9);

            //Panel górny z wyborem nauczyciela
            pnl_top = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = SystemColors.Control, Padding = new Padding(8, 8, 8, 0) };
            lb_wybierz = new Label { Text = "Nauczyciel:", Location = new Point(10, 15), AutoSize = true };
            cb_wybierz = new ComboBox { Location = new Point(100, 11), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Verdana", 9) };
            cb_wybierz.SelectedIndexChanged += cb_wybierz_SelectedIndexChanged;
            lb_info = new Label { Text = "", Location = new Point(415, 15), AutoSize = true, ForeColor = Color.Gray };

            pnl_top.Controls.AddRange(new Control[] { lb_wybierz, cb_wybierz, lb_info });

            //DataGridView do edycji planu
            dgv_plan = new DataGridView
            {
                Dock = DockStyle.Fill,
                Font = new Font("Verdana", 9),
                BackgroundColor = Color.Beige,
                GridColor = Color.DarkGray,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
            };

            dgv_plan.EnableHeadersVisualStyles = false;

            ZbudujKolumnyDGV();

            //Panel dolny z przyciskami
            pnl_bottom = new Panel { Dock = DockStyle.Bottom, Height = 45, BackColor = SystemColors.Control, Padding = new Padding(8) };

            btn_zapisz = new Button { Text = "Zapisz", Size = new Size(90, 28), Location = new Point(10, 8), FlatStyle = FlatStyle.Flat };
            btn_zapisz.FlatAppearance.BorderColor = Color.DarkGray;
            btn_zapisz.Click += btn_zapisz_Click;

            btn_zamknij = new Button { Text = "Zamknij", Size = new Size(90, 28), Location = new Point(110, 8), FlatStyle = FlatStyle.Flat };
            btn_zamknij.FlatAppearance.BorderColor = Color.DarkGray;
            btn_zamknij.Click += (s, e) => this.Close();

            pnl_bottom.Controls.AddRange(new Control[] { btn_zapisz, btn_zamknij });

            this.Controls.Add(dgv_plan);
            this.Controls.Add(pnl_top);
            this.Controls.Add(pnl_bottom);
        }

        private void ZbudujKolumnyDGV()
        {
            dgv_plan.Columns.Clear();

            //Kolumna z dniem/nr lekcji, tylko do odczytu
            var colOznaczenie = new DataGridViewTextBoxColumn
            {
                HeaderText = "Dzień / Lekcja",
                Name = "oznaczenie",
                ReadOnly = true,
                FillWeight = 30,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            colOznaczenie.DefaultCellStyle.BackColor = SystemColors.Control;
            colOznaczenie.DefaultCellStyle.ForeColor = Color.FromArgb(255, 57, 31, 11);
            colOznaczenie.DefaultCellStyle.Font = new Font("Verdana", 9, FontStyle.Bold);

            //Kolumna sala
            var colSala = new DataGridViewTextBoxColumn
            {
                HeaderText = "Sala",
                Name = "sala",
                FillWeight = 20,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            //Kolumna klasa
            var colKlasa = new DataGridViewTextBoxColumn
            {
                HeaderText = "Klasa",
                Name = "klasa",
                FillWeight = 25,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            //Kolumna przedmiot
            var colPrzedmiot = new DataGridViewTextBoxColumn
            {
                HeaderText = "Przedmiot",
                Name = "przedmiot",
                FillWeight = 35,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            dgv_plan.Columns.AddRange(new DataGridViewColumn[] { colOznaczenie, colSala, colKlasa, colPrzedmiot });

            dgv_plan.EnableHeadersVisualStyles = false;
            foreach (DataGridViewColumn col in dgv_plan.Columns)
            {
                col.HeaderCell.Style.BackColor = SystemColors.Control;
                col.HeaderCell.Style.Font = new Font("Verdana", 9, FontStyle.Bold);
                col.Resizable = DataGridViewTriState.False;
            }
        }

        private void WczytajListeNauczycieli()
        {
            cb_wybierz.Items.Clear();
            cb_wybierz.Items.Add("-- wybierz --");

            if (!File.Exists(listaNaucz)) return;

            string[] nauczyciele = File.ReadAllLines(listaNaucz, Encoding.UTF8);

            foreach (string n in nauczyciele)
            {
                string trim = n.Trim();
                if (!string.IsNullOrEmpty(trim)) cb_wybierz.Items.Add(trim);
            }

            cb_wybierz.SelectedIndex = 0;
        }

        private void cb_wybierz_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_wybierz.SelectedIndex <= 0)
            {
                dgv_plan.Rows.Clear();
                obecnySkrot = "";
                lb_info.Text = "";
                return;
            }

            string wybrany = cb_wybierz.SelectedItem.ToString();

            //Wyciąganie skrótu z nawiasow
            string skrot = WyciagnijSkrot(wybrany);

            string planPath = Path.Combine(planyFolder, skrot + ".txt");

            if (!File.Exists(planPath))
            {
                lb_info.Text = "Brak pliku planu";
                lb_info.ForeColor = Color.Red;
                dgv_plan.Rows.Clear();
                obecnySkrot = "";
                return;
            }

            obecnySkrot = skrot;
            lb_info.Text = skrot + ".txt";
            lb_info.ForeColor = Color.Gray;

            WczytajPlanDoGrid(planPath);
        }

        private string WyciagnijSkrot(string nauczyciel)
        {
            var czesci = nauczyciel.Split('(', ')');
            return czesci.Length > 1 ? czesci[1].Trim() : nauczyciel.Trim();
        }

        private void WczytajPlanDoGrid(string planPath)
        {
            dgv_plan.Rows.Clear();

            string[] linie = File.ReadAllLines(planPath, Encoding.UTF8);
            string obecnyDzien = "";

            foreach (string raw in linie)
            {
                string l = raw.Trim();
                if (string.IsNullOrEmpty(l)) continue;

                //Sprawdzenie czy to nazwa dnia
                bool toDzien = false;
                foreach (string dzien in dniTygodnia)
                {
                    if (l.StartsWith(dzien))
                    {
                        toDzien = true;
                        obecnyDzien = dzien;
                        break;
                    }
                }

                if (toDzien)
                {
                    //Wiersz nagłówkowy z dniem — cały szary, tylko kolumna oznaczenia
                    int idx = dgv_plan.Rows.Add(obecnyDzien, "", "", "");
                    var row = dgv_plan.Rows[idx];

                    //Szare tło dla całego wiersza z dniem
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Style.BackColor = SystemColors.Control;
                        cell.Style.Font = new Font("Verdana", 9, FontStyle.Bold);
                        cell.ReadOnly = true;
                    }
                    continue;
                }

                //Linia z danymi lekcji: nr ; sala ; klasa ; przedmiot ;
                string[] pola = l.Split(';');
                string nr = pola.Length > 0 ? pola[0].Trim() : "";
                string sala = pola.Length > 1 ? pola[1].Trim() : "";
                string klasa = pola.Length > 2 ? pola[2].Trim() : "";
                string przedmiot = pola.Length > 3 ? pola[3].Trim() : "";

                //Oznaczenie wiersza: "Rano" dla 0, "L1"-"L8" dla reszty
                string oznaczenie;
                if (nr == "0") oznaczenie = "  Rano";
                else if (int.TryParse(nr, out int nrInt)) oznaczenie = $"  L{nrInt}";
                else oznaczenie = "  " + nr;

                dgv_plan.Rows.Add(oznaczenie, sala, klasa, przedmiot);

                //Czytelna wysokość wierszy z danymi
                dgv_plan.Rows[dgv_plan.Rows.Count - 1].Height = 22;
            }

            //Wyrównanie wysokości wierszy nagłówkowych
            for (int i = 0; i < dgv_plan.Rows.Count; i++)
            {
                string val = dgv_plan.Rows[i].Cells[0].Value?.ToString() ?? "";
                bool toDzien = dniTygodnia.Any(d => val.Trim().StartsWith(d));
                if (toDzien) dgv_plan.Rows[i].Height = 24;
            }
        }

        private void btn_zapisz_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(obecnySkrot))
            {
                MessageBox.Show("Nie wybrano żadnego nauczyciela.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string planPath = Path.Combine(planyFolder, obecnySkrot + ".txt");

            try
            {
                StringBuilder sb = new StringBuilder();
                string obecnyDzien = "";

                for (int i = 0; i < dgv_plan.Rows.Count; i++)
                {
                    string oznaczenie = dgv_plan.Rows[i].Cells["oznaczenie"].Value?.ToString()?.Trim() ?? "";

                    //Wiersz z dniem
                    bool toDzien = dniTygodnia.Any(d => oznaczenie.StartsWith(d));
                    if (toDzien)
                    {
                        if (!string.IsNullOrEmpty(obecnyDzien)) sb.AppendLine();
                        obecnyDzien = oznaczenie.Trim();
                        sb.AppendLine(obecnyDzien);
                        continue;
                    }

                    //Wyciąganie nr lekcji z oznaczenienia ("  Rano" → 0, "  L3" → 3)
                    string nr = "";
                    if (oznaczenie.Contains("Rano")) nr = "0";
                    else
                    {
                        string czysty = oznaczenie.Replace("L", "").Trim();
                        nr = czysty;
                    }

                    string sala = dgv_plan.Rows[i].Cells["sala"].Value?.ToString()?.Trim() ?? "-";
                    string klasa = dgv_plan.Rows[i].Cells["klasa"].Value?.ToString()?.Trim() ?? "-";
                    string przedmiot = dgv_plan.Rows[i].Cells["przedmiot"].Value?.ToString()?.Trim() ?? "-";

                    if (string.IsNullOrEmpty(sala)) sala = "-";
                    if (string.IsNullOrEmpty(klasa)) klasa = "-";
                    if (string.IsNullOrEmpty(przedmiot)) przedmiot = "-";

                    sb.AppendLine($"{nr} ; {sala} ; {klasa} ; {przedmiot} ;");
                }

                File.WriteAllText(planPath, sb.ToString(), Encoding.UTF8);

                lb_info.Text = obecnySkrot + ".txt — zapisano";
                lb_info.ForeColor = Color.Green;
                MessageBox.Show($"Plan {obecnySkrot} zapisany.", "Zapis", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Po chwili wróć do szarego
                var timer = new System.Windows.Forms.Timer { Interval = 2500 };
                timer.Tick += (ts, te) =>
                {
                    lb_info.Text = obecnySkrot + ".txt";
                    lb_info.ForeColor = Color.Gray;
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie udało się zapisać:\n{ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }


}