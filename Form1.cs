using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.IO.Compression;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Formula.Functions;
using iTextPdf = iTextSharp.text;
using iTextPdfCore = iTextSharp.text.pdf;


namespace Szablon_Dyżury
{
    public partial class Form1 : Form
    {
        //v15

        //Zmienne globalne do ponownego użycia później
        //Nie chce mi się myśleć teraz ale możliwe że nie potrzebuję niektórych plików wgl wzorowałam się innym projektem i może tam coś źle zrozumiałam idk poprawi się
        //Conajmniej miesiąc po dalej nie wiem czy wszystkie są potrzebne ale meh dalej mi się nie chce

        //Pliki
        string localAppData;
        string planyFolder;
        string saveFolder;
        string listaNaucz;
        string listaSale;
        string listaGodziny;
        string listaKlasyPraktyki;    // Plik z klasami na praktykach
        string listaWylaczeniNaucz;   // Plik z wyłączonymi nauczycielami

        //Słowniczek dla dgv
        Dictionary<string, string> mapowanieCelli = new Dictionary<string, string>();

        //Zmienne i kontrolki dla nauczycieli i sal zeby były dostępne wszedzie
        string[] nauczyciele;
        RadioButton rbNauczyciele;
        string[] dniTygodnia = new[] { "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek" };
        string[] sale;
        Label lbSale;

        //Na dodawanie do celli
        int editCol = -1;
        int editRow = -1;

        private Size oldSize;

        // Stałe do obliczania minut dyżurów
        // Całkowita pula minut dyżurów do rozdzielenia między nauczycieli
        const double CALKOWITE_MINUTY_DYZUROW = 4640.0;
        // Suma wszystkich godzin lekcyjnych wszystkich nauczycieli (podstawa proporcji)
        const double SUMA_GODZIN_NAUCZYCIELI = 1509.0;

        // Listy klas na praktykach i wyłączonych nauczycieli — ładowane z plików przy starcie
        private List<string> klasyNaPraktykach = new List<string>();
        private List<string> wylaczeniNauczyciele = new List<string>();



        //GOTOWANIE PAULINY VVVVV
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);

            foreach (Control cnt in this.Controls)
                ResizeAll(cnt, base.Size);

            oldSize = base.Size;
        }
        private void ResizeAll(Control control, Size newSize)
        {
            int width = newSize.Width - oldSize.Width;
            control.Left += (control.Left * width) / oldSize.Width;
            control.Width += (control.Width * width) / oldSize.Width;

            int height = newSize.Height - oldSize.Height;
            control.Top += (control.Top * height) / oldSize.Height;
            control.Height += (control.Height * height) / oldSize.Height;
        }
        //GOTOWANIE PAULINY ^^^^^




        //Piętra
        private Dictionary<string, string> pietraBazowe = new Dictionary<string, string>()
        {
            { "W.F.", "wf" },
            { "Wejście gł.", "wejscie" },
            { "Boisko", "boisko" },
            { "Szatnia", "szatnia" },
            { "Parter", "parter1" },
            { "Piętro 1", "pi1" },
            { "Piętro 2", "pii1" }
        };

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Nie mogłam patrzeć na ten syf zrobiłam funkcje osobne

            TworzenieSciezek();

            WczytajNauczycieli();
            WczytajSale();

            DodajTooltipy();
            DodajZdarzenia();

            Wstaw_tab();

            // Wczytanie list z plików AppData
            WczytajKlasyPraktyki();
            WczytajWylaczonych();

            oldSize = Size;
        }

        //Pliki
        private void TworzenieSciezek()
        {
            localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string basePath = Path.Combine(localAppData, "AplikDyzur");

            planyFolder = Path.Combine(basePath, "Plany Nauczycieli");
            saveFolder = Path.Combine(basePath, "Save");
            listaNaucz = Path.Combine(basePath, "Nauczyciele.txt");
            listaSale = Path.Combine(basePath, "Sale.txt");
            listaGodziny = Path.Combine(basePath, "Godziny.txt");
            listaKlasyPraktyki = Path.Combine(basePath, "KlasyPraktyki.txt");
            listaWylaczeniNaucz = Path.Combine(basePath, "WylaczeniNauczyciele.txt");

            TworzeniePlikow();
        }

        //Nauczyciele
        private void WczytajNauczycieli()
        {
            nauczyciele = File.ReadAllLines(listaNaucz);

            DodanieCombo();
            DodanieRadioNauczycieli();
        }

        private void DodanieCombo()
        {
            cb_Nauczyciele.Items.Clear();
            cb_Nauczyciele.Items.Add(" ");
            cb_Nauczyciele.Items.AddRange(nauczyciele);
            cb_Nauczyciele.SelectedIndex = 0;
            cb_Nauczyciele.Font = new Font("Verdana", 9);
            cb_Nauczyciele.Hide();

            cb_Nauczyciele.SelectedIndexChanged += cb_Nauczyciele_SelectedIndexChanged;
        }

        private void DodanieRadioNauczycieli()
        {
            gB_naucz.Controls.Clear();


            // rB_n_Odznacz trafia do scrollPanela zamiast bezpośrednio do gB_naucz
            rB_n_Odznacz.Location = new Point(10, 10);
            gB_naucz.Controls.Add(rB_n_Odznacz);

            int y = 50;

            foreach (string nauczyciel in nauczyciele)
            {
                var rb = new RadioButton
                {
                    Text = nauczyciel.Trim(),
                    Location = new Point(10, y),
                    AutoSize = true,
                    Name = ZnajdzSkrot(nauczyciel)
                };

                rb.CheckedChanged += Nauczyciel_CheckedChanged;

                gB_naucz.Controls.Add(rb);

                y += 40;
            }

            // Separator przed sekcją praktyk/wyłączonych
            y += 10;
            var sep = new Label
            {
                //BorderStyle = BorderStyle.Fixed3D, //npoi sie z tym gryzie xddddd
                Location = new Point(4, y),
                Width = gB_naucz.Width - 24,
                Height = 2,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            gB_naucz.Controls.Add(sep);
            y += 10;

            //pozostalosc po scrollu niewazne
            var scrollPanel = this.gB_naucz;
            ZbudujSekcjePraktykiWylaczeni(scrollPanel, y);
        }

        private string ZnajdzSkrot(string nauczyciel)
        {
            var czesci = nauczyciel.Split('(', ')');
            return czesci.Length > 1 ? czesci[1] : nauczyciel;
        }

        //Sale
        private void WczytajSale()
        {
            sale = File.ReadAllLines(listaSale);

            int y = 60;

            foreach (string sala in sale)
            {
                var label = new Label
                {
                    Text = sala.Trim(),
                    Location = new Point(10, y),
                    AutoSize = true,
                    BackColor = Color.White,
                    Name = ZnajdzNazweSali(sala)
                };

                gB_sale.Controls.Add(label);

                y += 40;
            }
        }

        private string ZnajdzNazweSali(string sala)
        {
            var czesci = sala.Split(' ');
            return czesci[0];
        }

        //Tooltipy
        private void DodajTooltipy()
        {
            tT_przyciski.SetToolTip(btn_close, "Zamknij");
            tT_przyciski.SetToolTip(btn_min, "Minimalizuj");
            tT_przyciski.SetToolTip(btn_zamien, "Zamień monitor");
            tT_przyciski.SetToolTip(btn_save, "Zapisz");
            tT_przyciski.SetToolTip(btn_wczytaj, "Wczytaj z pliku");
            tT_przyciski.SetToolTip(btn_pobierz_plany, "Pobierz plany");
            tT_przyciski.SetToolTip(btn_reset, "Resetuj");
            tT_przyciski.SetToolTip(btn_pdf, "Eksportuj do PDF");
            tT_przyciski.SetToolTip(btn_edytor, "Edytuj plan nauczyciela");
        }

        //Zdarzenia
        private void DodajZdarzenia()
        {
            rB_n_Odznacz.CheckedChanged += OdznaczNauczyciela_CheckedChanged;
            cB_s_Odznacz.CheckedChanged += OdznaczSale_CheckedChanged;
        }

        private void Wstaw_tab()
        {
            dgv.Width = 1395;
            dgv.Height = 1031;

            dgv.Font = new Font("Verdana", 10);
            dgv.BackgroundColor = Color.Beige;
            dgv.GridColor = Color.DarkGray;

            dgv.ColumnCount = 15;

            var headerFont = new Font("Verdana", 10, FontStyle.Bold);

            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                var col = dgv.Columns[i];

                col.Width = 98;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.Resizable = DataGridViewTriState.False;

                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = headerFont;
            }

            dgv.Columns[0].Width = 48;
            dgv.Columns[8].Width = 48;

            dgv.Columns[0].HeaderText = "Od - Do";
            dgv.Columns[1].HeaderText = "W.F.";
            dgv.Columns[2].HeaderText = "Wejście gł.";
            dgv.Columns[3].HeaderText = "Boisko";
            dgv.Columns[4].HeaderText = "Szatnia";
            dgv.Columns[5].HeaderText = "Parter";
            dgv.Columns[6].HeaderText = "Piętro 1";
            dgv.Columns[7].HeaderText = "Piętro 2";
            dgv.Columns[8].HeaderText = "Od - Do";
            dgv.Columns[9].HeaderText = "Parter";
            dgv.Columns[10].HeaderText = "Parter";
            dgv.Columns[11].HeaderText = "Piętro 1";
            dgv.Columns[12].HeaderText = "Piętro 1";
            dgv.Columns[13].HeaderText = "Piętro 2";
            dgv.Columns[14].HeaderText = "Piętro 2";

            //Wiersze
            for (int n = 0; n < 49; n++)
            {
                int val = n % 10;

                DataGridViewRow row = new DataGridViewRow();

                if (val == 9)
                {
                    row.CreateCells(dgv);

                    foreach (DataGridViewCell cell in row.Cells)
                        cell.Style.BackColor = Color.White;
                }
                else
                {
                    string oddo = val == 0 ? "Rano" : "Po L" + val;
                    row.CreateCells(dgv, oddo, "", "", "", "", "", "", "", oddo);
                }

                row.Cells[0].Style.BackColor = SystemColors.Control;
                row.Cells[8].Style.BackColor = SystemColors.Control;

                row.Cells[0].Style.ForeColor = Color.FromArgb(255, 57, 31, 11);
                row.Cells[8].Style.ForeColor = Color.FromArgb(255, 57, 31, 11);

                row.Resizable = DataGridViewTriState.False;

                dgv.Rows.Add(row);
            }

            dgv.EnableHeadersVisualStyles = false;

            foreach (DataGridViewColumn col in dgv.Columns)
                col.HeaderCell.Style.BackColor = SystemColors.Control;

            for (int i = 0; i < dgv.RowCount; i++)
            {
                dgv.Rows[i].Height = 20;

                if (i % 10 == 9)
                {
                    foreach (DataGridViewCell cell in dgv.Rows[i].Cells)
                        cell.Style.BackColor = SystemColors.Control;
                }
            }

            GenerujMapowanie();

            dgv.CellClick += dgv_CellClick;
        }

        private void GenerujMapowanie()
        {
            mapowanieCelli.Clear();

            //Skróty
            string[] dni = { "pon", "wtr", "srd", "czw", "pia" };

            for (int dz = 0; dz < dni.Length; dz++)
            {
                for (int nr = 0; nr < 9; nr++)
                {
                    int row = dz * 10 + nr;

                    int[] kolumny = { 1, 2, 3, 4, 5, 6, 7, 9, 10, 11, 12, 13, 14 };

                    foreach (int col in kolumny)
                    {
                        string header = dgv.Columns[col].HeaderText;

                        string pietro = "PiątekJumpscare";

                        //Podwojone 
                        if (col == 10 && header == "Parter")
                            pietro = "parter2";
                        else if (col == 12 && header == "Piętro 1")
                            pietro = "pi2";
                        else if (col == 14 && header == "Piętro 2")
                            pietro = "pii2";

                        string budynek = (col < 8 ? "g" : "p");

                        string key = col + "_" + row;
                        string val = $"{budynek}_{dni[dz]}_{pietro}_{nr}";

                        mapowanieCelli[key] = val;
                    }
                }
            }
        }

        //Przyciski
        private void btn_close_Click(object sender, EventArgs e) { this.Close(); }
        private void btn_min_Click(object sender, EventArgs e) { this.WindowState = FormWindowState.Minimized; }
        private void btn_zamien_Click(object sender, EventArgs e) //Zamiana monitora
        {
            var ekrany = Screen.AllScreens; //Wszystkie ekrany
            if (ekrany.Length <= 1) return; //Nic się nie dzieje jak nie ma więcej

            Screen obecny = Screen.FromControl(this); //Obecny monitor
            int index = Array.IndexOf(ekrany, obecny); //Nr obecnego monitora

            int nextIndex = (index + 1) % ekrany.Length; //Nr następnego monitora
            Screen nastepnyMonitor = ekrany[nextIndex]; //Nastepny monitor

            bool maksym = (this.WindowState == FormWindowState.Maximized); //Nawet Maksym znalazł swoje miejsce w tym projekcie
            this.WindowState = FormWindowState.Normal; //Inaczej się nie przeniesie, nie może być zmaksymalizowany

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(nastepnyMonitor.Bounds.X, nastepnyMonitor.Bounds.Y);

            if (maksym) this.WindowState = FormWindowState.Maximized; //Z powrotem jak było
        }
        private void btn_reset_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Czy na pewno chcesz wyczyścić plan?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            for (int i = 0; i < dgv.RowCount; i++)
            {
                for (int j = 0; j < dgv.ColumnCount; j++)
                {
                    if (j == 0 || j == 8) continue;
                    dgv.Rows[i].Cells[j].Value = "";
                }
            }

            cb_Nauczyciele.Hide();
        }

        //CHECKPOINT !!!!!!!!!!!!!

        string cellNazwa = "g_pon_0";

        private void terminySal_Click(object sender, EventArgs e)
        {
            //Reset
            if (cB_s_Odznacz.Checked)
            {
                foreach (Label lb in gB_sale.Controls.OfType<Label>())
                {
                    lb.BackColor = Color.White;
                    lb.Text = lb.Name;
                }
                return;
            }

            //Czyszczenie
            foreach (Control c in gB_sale.Controls)
            {
                if (c is Label lb)
                {
                    lb.BackColor = Color.White;
                    lb.Text = lb.Name;
                }
            }

            if (!cellNazwa.Contains("_")) return;

            string[] czesci = cellNazwa.Split('_');
            if (czesci.Length < 3) return;

            string budynek = czesci[0]; //g/p
            string dzienSkrot = czesci[1]; //pon, wtr itd
            string czasPart = czesci[2]; //Nr lekcji

            //Nazwa dnia
            string dzien = "";
            switch (dzienSkrot)
            {
                case "pon": dzien = "Poniedziałek"; break;
                case "wtr": dzien = "Wtorek"; break;
                case "srd": dzien = "Środa"; break;
                case "czw": dzien = "Czwartek"; break;
                case "pia": dzien = "Piątek"; break;
                default: dzien = dzienSkrot; break;
            }

            int nrGodziny;
            try { nrGodziny = Convert.ToInt32(czasPart); }
            catch { return; }

            //Przeszukiwanie planów — dla każdego nauczyciela sprawdzamy czy ma lekcję w tej godzinie
            foreach (string nauczyciel in nauczyciele)
            {
                string[] cz = nauczyciel.Split('(', ')');
                if (cz.Length < 2) continue;
                string skrot = cz[1];

                // Pomijamy wyłączonych nauczycieli — nie mają sensu w tym kontekście
                if (wylaczeniNauczyciele.Contains(skrot)) continue;

                string planPath = Path.Combine(planyFolder, $"{skrot}.txt");
                if (!File.Exists(planPath)) continue;

                string[] linie = File.ReadAllLines(planPath);
                bool inSection = false;

                foreach (string raw in linie)
                {
                    string l = raw.Trim();
                    if (string.IsNullOrEmpty(l)) continue;

                    if (!inSection)
                    {
                        if (l.StartsWith(dzien)) inSection = true;
                        continue;
                    }

                    //Jeśli zaczyna sie nowym dniem
                    bool nowyDzien = false;
                    foreach (string d in dniTygodnia)
                        if (l.StartsWith(d)) { nowyDzien = true; break; }

                    if (nowyDzien) break;

                    string[] surowePola = l.Split(';');
                    string[] pola = new string[surowePola.Length];

                    for (int i = 0; i < surowePola.Length; i++)
                        pola[i] = surowePola[i].Trim();

                    if (pola.Length < 4) continue;

                    if (!int.TryParse(pola[0], out int nrZPliku)) continue;

                    if (nrZPliku == nrGodziny)
                    {
                        string sala = pola[1];
                        string klasa = pola[2];
                        string przedmiot = pola[3];

                        // Pomijamy puste sale (nauczyciel nie ma lekcji)
                        if (string.IsNullOrEmpty(sala) || sala == "-") continue;

                        // Wyciągamy sam numer sali bez przyrostka 'p' (pracownia)
                        // np. "223p" -> "223", "119" -> "119"
                        string numerSali = sala.TrimEnd('p').Trim();

                        // Szukamy labela po samym numerze — Name labela to pierwszy człon z listaSale
                        // ale mogą też być etykiety z samym numerem, więc próbujemy obu wersji
                        Control[] matches = gB_sale.Controls.Find(numerSali, true);

                        // Jak nie znalazło po samym numerze, spróbuj z oryginalną wartością (bez 'p')
                        if (matches.Length == 0)
                            matches = gB_sale.Controls.Find(sala.Trim(), true);

                        if (matches.Length > 0 && matches[0] is Label lbSala)
                        {
                            lbSala.BackColor = Color.LightBlue;
                            // Jeśli kilku nauczycieli jest w tej samej sali, dopisujemy ich po przecinku
                            if (lbSala.Text == lbSala.Name)
                                lbSala.Text = $"{numerSali} ({skrot} - {klasa} - {przedmiot})";
                            else
                                lbSala.Text += $", {skrot}";
                        }
                    }
                }
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int x = e.ColumnIndex;
            int y = e.RowIndex;

            if (x < 0 || y < 0) return; //Nagłówki

            //cellNazwa (popłakałam sie przez to)
            string key = x + "_" + y;
            if (mapowanieCelli.TryGetValue(key, out string nazwa))
            {
                cellNazwa = nazwa;  //Np. "g_pon_2"
                terminySal_Click(null, null); //Aktualizacja
            }
            else
            {
                //Reset
                foreach (Label lb in gB_sale.Controls.OfType<Label>())
                {
                    lb.BackColor = Color.White;
                    lb.Text = lb.Name;
                }
            }

            //Combo
            if (x == 0 || x == 8 || y == 9 || y == 19 || y == 29 || y == 39)
            {
                cb_Nauczyciele.Hide();
                return;
            }

            //Ustawienie go
            Rectangle cellR = dgv.GetCellDisplayRectangle(x, y, true);
            Point dgvCellLocation = dgv.PointToScreen(cellR.Location);
            Point cbLocation = this.PointToClient(dgvCellLocation);

            int offsetX = (cellR.Width - cb_Nauczyciele.Width) / 2;
            int offsetY = (cellR.Height - cb_Nauczyciele.Height) / 2;

            cb_Nauczyciele.Location = new Point(cbLocation.X + offsetX, cbLocation.Y + offsetY);

            string wartosc = dgv.Rows[y].Cells[x].Value?.ToString() ?? "";
            int pos = cb_Nauczyciele.Items.IndexOf(wartosc);
            if (pos >= 0) cb_Nauczyciele.SelectedIndex = pos;
            else cb_Nauczyciele.SelectedIndex = 0;

            cb_Nauczyciele.Show();
            editCol = x;
            editRow = y;




            //GOTOWANIE PAULINY VVVVV

            if (!cellNazwa.Contains("_")) return;

            string[] name = cellNazwa.Split('_');
            if (name.Length < 3) return;

            clickCellShowAvailability(sender, e);

            //GOTOWANIE PAULINY ^^^^^
        }


        private void cb_Nauczyciele_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Indexy
            int x = this.dgv.CurrentCell.ColumnIndex;
            int y = this.dgv.CurrentCell.RowIndex;

            //Zapamiętujemy poprzednią wartość komórki zanim ją nadpiszemy
            //Potrzebne żeby odświeżyć minuty nauczyciela który był tam wcześniej (np. przy usuwaniu dyżuru)
            string poprzednia = this.dgv.Rows[y].Cells[x].Value?.ToString() ?? "";

            //Ustawienie komórki
            this.dgv.Rows[y].Cells[x].Value = this.cb_Nauczyciele.SelectedItem;
            this.cb_Nauczyciele.Hide();

            //Odświeżenie minut dla nowo wybranego nauczyciela
            OdswiezMinutyNauczyciela(this.cb_Nauczyciele.SelectedItem?.ToString() ?? "");

            //Jeśli poprzednia wartość była różna (czyli ktoś był tam wcześniej), odświeżamy też jego minuty
            if (!string.IsNullOrWhiteSpace(poprzednia) && poprzednia != this.cb_Nauczyciele.SelectedItem?.ToString())
                OdswiezMinutyNauczyciela(poprzednia);
        }

        private void WyswietlPlanNauczyciela()
        {
            //Sprawdzenie
            if (rB_n_Odznacz.Checked)
            {
                panel_plan.Controls.Clear();
                lb_nauczyciel.Text = "Obecny nauczyciel: - ";
                return;
            }

            //Sprawdzanie który gagatek
            RadioButton rb = gB_naucz.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            string skrotNauczyciela;

            if (rb != null) skrotNauczyciela = rb.Name;
            else skrotNauczyciela = " - ";

            //Ustawienie tekstu na labelu
            lb_nauczyciel.Text = $"Obecny nauczyciel: {skrotNauczyciela}";

            panel_plan.Controls.Clear(); //Czyszczenie
            if (skrotNauczyciela == " - ") return; //Koniec jeśli nikt nie jest wybrany

            //Pobieranie pliku
            string plik = Path.Combine(planyFolder, $"{skrotNauczyciela}.txt");
            if (!File.Exists(plik)) return;
            string[] linie = File.ReadAllLines(plik);

            int y = 0;

            foreach (string raw in linie)
            {
                string l = raw.Trim();
                if (l == null || l.Trim() == "") continue;

                //Linie z nazwami dni
                bool znalezionoDzien = false;
                foreach (string dzienTyg in dniTygodnia)
                {
                    if (l.StartsWith(dzienTyg))
                    {
                        znalezionoDzien = true;
                        break;
                    }
                }

                if (znalezionoDzien)
                {
                    Label lbDzien = new Label
                    {
                        Text = l,
                        Font = new Font("Verdana", 10, FontStyle.Bold),
                        Location = new Point(0, y),
                        AutoSize = true,
                        Padding = new Padding(0, 10, 0, 0)
                    };
                    panel_plan.Controls.Add(lbDzien);
                    y += 25;
                    continue;
                }

                //Linie z lekcjami: numer ; sala ; klasa ; przedmiot ; uwagi
                string[] pola = l.Split(';');
                for (int i = 0; i < pola.Length; i++)
                    pola[i] = pola[i].Trim();

                string numerLekcji = pola[0];
                string sala = pola[1];
                string klasa = pola[2];
                string przedmiot = pola[3];

                Label lbLekcja = new Label
                {
                    Text = $"{numerLekcji}. {sala} - {klasa} - {przedmiot}",
                    Location = new Point(0, y),
                    AutoSize = true
                };
                panel_plan.Controls.Add(lbLekcja);

                y += 20; //Odstęp
            }

            //Przewijanie w rszie potrzeby
            panel_plan.AutoScroll = true;
        }




        //GOTOWANIE PAULINY VVVVV

        private void clickCellShowAvailability(object sender, DataGridViewCellEventArgs e)
        {
            // Reset kolorów radio buttonów
            foreach (Control ctrl in gB_naucz.Controls)
                if (ctrl is RadioButton rb)
                    rb.BackColor = Color.FromArgb(255, 245, 171, 169);

            int x = e.ColumnIndex;
            int y = e.RowIndex;

            int day = y / 10;
            int poLekcji = y % 10;

            // Wiersz separatora (index 9 w każdym dniu) — brak dyżuru
            if (poLekcji == 9) return;

            string ppietro;
            if (x == 1) ppietro = "wf";
            else if (x == 2 || x == 3 || x == 4 || x == 5 || x == 9 || x == 10) ppietro = "0";
            else if (x == 6 || x == 11 || x == 12) ppietro = "1";
            else if (x == 7 || x == 13 || x == 14) ppietro = "2";
            else return;

            bool pracownie = (x > 8);

            string nazwaD = day < dniTygodnia.Length ? dniTygodnia[day] : "";

            foreach (string nauczyciel in nauczyciele)
            {
                string[] cz = nauczyciel.Split('(', ')');
                if (cz.Length < 2) continue;
                string skrot = cz[1];

                string planPath = Path.Combine(planyFolder, $"{skrot}.txt");
                if (!File.Exists(planPath)) continue;

                string[] linie = File.ReadAllLines(planPath);

                int currentDay = -1;
                bool znalezionoLekcje = false;

                foreach (string rawLine in linie)
                {
                    string l = rawLine.Trim();
                    if (string.IsNullOrEmpty(l)) continue;

                    // Sprawdzamy czy to nagłówek dnia
                    bool toDzien = false;
                    foreach (string d in dniTygodnia)
                    {
                        if (l == d) { toDzien = true; break; }
                    }
                    if (toDzien)
                    {
                        currentDay++;
                        continue;
                    }

                    // Tylko interesuje nas właściwy dzień
                    if (currentDay != day) continue;

                    string[] pola = l.Split(';');
                    if (pola.Length < 2) continue;

                    if (!int.TryParse(pola[0].Trim(), out int nrLekcji)) continue;
                    if (nrLekcji != poLekcji) continue;

                    // Znaleziono lekcję dla tego slotu
                    znalezionoLekcje = true;

                    string rawSala = pola[1].Trim();

                    // Brak lekcji
                    if (rawSala == "-" || string.IsNullOrEmpty(rawSala)) break;

                    bool rawPracownie = rawSala.EndsWith("p");
                    string salaCyfrowa = rawSala.TrimEnd('p').Trim();

                    // Sala niestandardowa (litera, "SG3", "B1" itp.) — nie można określić piętra
                    if (!int.TryParse(salaCyfrowa, out int salaNr))
                    {
                        // Sala WF/boisko/szatnia — oznaczamy specjalnie
                        Control[] szukajRb = this.Controls.Find(skrot, true);
                        if (szukajRb.Length > 0 && szukajRb[0] is RadioButton rbSpec)
                            rbSpec.BackColor = Color.FromArgb(255, 233, 211, 240);
                        break;
                    }

                    if (rawPracownie != pracownie) break; // Budynek się nie zgadza

                    Control[] found = this.Controls.Find(skrot, true);
                    if (found.Length == 0 || !(found[0] is RadioButton rbN)) break;

                    int pietro = salaNr / 100; // 0 = parter, 1 = I piętro, 2 = II piętro

                    Color kolor = Color.Red; // domyślnie — nie powinno wystąpić

                    switch (ppietro)
                    {
                        case "wf":
                        case "0": // dyżur na parterze
                            if (pietro == 0) kolor = Color.DarkSeaGreen;
                            else if (pietro == 1) kolor = Color.Gold;
                            else kolor = Color.SandyBrown;
                            break;
                        case "1": // dyżur na I piętrze
                            if (pietro == 0) kolor = Color.Gold;
                            else if (pietro == 1) kolor = Color.DarkSeaGreen;
                            else kolor = Color.Gold;
                            break;
                        case "2": // dyżur na II piętrze
                            if (pietro == 0) kolor = Color.SandyBrown;
                            else if (pietro == 1) kolor = Color.Gold;
                            else kolor = Color.DarkSeaGreen;
                            break;
                    }

                    rbN.BackColor = kolor;
                    break;
                }

                // Jeśli nauczyciel nie ma lekcji w tym slocie — może wziąć dyżur
                if (!znalezionoLekcje)
                {
                    Control[] found = this.Controls.Find(skrot, true);
                    if (found.Length > 0 && found[0] is RadioButton rbFree)
                        if (rbFree.BackColor == Color.FromArgb(255, 245, 171, 169)) // nie nadpisujemy już ustawionego koloru
                            rbFree.BackColor = Color.DarkSeaGreen;
                }
            }
        }

        private void clickTeachSecond(object sender)
        {
            // Reset kolorów dgv
            foreach (DataGridViewRow row in dgv.Rows)
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Style.BackColor = Color.White;

            for (int n = 0; n < dgv.RowCount; n++)
            {
                dgv.Rows[n].Cells[0].Style.BackColor = SystemColors.Control;
                dgv.Rows[n].Cells[8].Style.BackColor = SystemColors.Control;
                dgv.Rows[n].Cells[0].Style.ForeColor = Color.FromArgb(255, 57, 31, 11);
                dgv.Rows[n].Cells[8].Style.ForeColor = Color.FromArgb(255, 57, 31, 11);
                dgv.Rows[n].Resizable = DataGridViewTriState.False;
            }

            RadioButton rb = (RadioButton)sender;
            string nauczyciel = rb.Name;
            string planPath = Path.Combine(planyFolder, nauczyciel + ".txt");
            if (!File.Exists(planPath)) return;

            string[] lines = File.ReadAllLines(planPath);

            int currentDay = -1;

            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();
                if (string.IsNullOrEmpty(line)) continue;

                // Nagłówek dnia
                bool toDzien = false;
                foreach (string d in dniTygodnia)
                {
                    if (line == d) { toDzien = true; break; }
                }
                if (toDzien) { currentDay++; continue; }

                if (currentDay < 0) continue;

                string[] pola = line.Split(';');
                if (pola.Length < 2) continue;

                if (!int.TryParse(pola[0].Trim(), out int po)) continue;
                if (po == 9 || po == 10) continue; // brak miejsca w dgv

                string rawSala = pola[1].Trim();
                if (rawSala == "-" || string.IsNullOrEmpty(rawSala)) continue;

                bool pracownie = rawSala.EndsWith("p");
                string salaCyfrowa = rawSala.TrimEnd('p').Trim();

                int dgvRow = currentDay * 10 + po;
                if (dgvRow >= dgv.RowCount) continue;

                // Sale niestandardowe — sala WF/boisko (litera lub "SG3" itp.)
                if (!int.TryParse(salaCyfrowa, out int salaNr))
                {
                    // WF/boisko — kolorujemy kolumnę 1 (W.F.)
                    if (rawSala.ToUpper().StartsWith("S") || rawSala.ToUpper().StartsWith("B"))
                    {
                        var cellWF = dgv.Rows[dgvRow].Cells[1];
                        cellWF.Style.BackColor = IsOccupied(cellWF, nauczyciel) ? Color.LightBlue : Color.DarkSeaGreen;
                    }
                    continue;
                }

                int pietro = salaNr / 100;

                if (pracownie)
                {
                    // Pracownie — kolumny 9-14
                    int startCol;
                    if (pietro == 0) startCol = 9;
                    else if (pietro == 1) startCol = 11;
                    else startCol = 13;

                    for (int i = startCol; i <= startCol + 1; i++)
                    {
                        var cell = dgv.Rows[dgvRow].Cells[i];
                        cell.Style.BackColor = IsOccupied(cell, nauczyciel) ? Color.LightBlue : Color.DarkSeaGreen;
                    }
                }
                else
                {
                    // Budynek główny
                    if (salaNr < 10) // sale specjalne: WF, wejście, boisko, szatnia — kolumny 1-4
                    {
                        for (int i = 2; i <= 5; i++)
                        {
                            var cell = dgv.Rows[dgvRow].Cells[i];
                            cell.Style.BackColor = IsOccupied(cell, nauczyciel) ? Color.LightBlue : Color.DarkSeaGreen;
                        }
                    }
                    else
                    {
                        int col;
                        if (pietro == 0) col = 5;      // Parter
                        else if (pietro == 1) col = 6; // Piętro 1
                        else col = 7;                  // Piętro 2

                        var cell = dgv.Rows[dgvRow].Cells[col];
                        cell.Style.BackColor = IsOccupied(cell, nauczyciel) ? Color.LightBlue : Color.DarkSeaGreen;
                    }
                }
            }
        }

        // Pomocnicza: sprawdza czy komórka jest już zajęta przez tego samego nauczyciela
        private bool IsOccupied(DataGridViewCell cell, string skrot)
        {
            if (cell.Value == null || cell.Value.ToString() == "") return false;
            string[] parts = cell.Value.ToString().Split('(', ')');
            return parts.Length > 1 && parts[1] == skrot;
        }

        //GOTOWANIE PAULINY ^^^^^




        private void Nauczyciel_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton)?.Checked == true)
            {
                WyswietlPlanNauczyciela();
                clickTeachSecond(sender);
            }
        }

        //Radio Odznacz
        private void OdznaczNauczyciela_CheckedChanged(object sender, EventArgs e)
        {
            if (rB_n_Odznacz.Checked)
            {
                //Odznaczenie
                foreach (RadioButton rb in gB_naucz.Controls.OfType<RadioButton>())
                    rb.Checked = false;

                //Czyszczenie
                panel_plan.Controls.Clear();
                lb_nauczyciel.Text = "Obecny nauczyciel: - ";




                //GOTOWANIE PAULINY VVVVV

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Style.BackColor = Color.White;
                    }
                }

                //GOTOWANIE PAULINY ^^^^^




            }
        }

        //Check Odznacz
        private void OdznaczSale_CheckedChanged(object sender, EventArgs e)
        {
            if (cB_s_Odznacz.Checked)
            {
                //Czyszczenie
                foreach (Label lb in gB_sale.Controls.OfType<Label>())
                {
                    lb.BackColor = Color.White;
                    lb.Text = lb.Name; //Sam numer
                }
            }
            else terminySal_Click(null, null); //Odświeżenie
        }

        //Zapisywanie (przynajmniej już nie crashuje vscode)
        private void btn_save_Click(object sender, EventArgs e)
        {
            string defaultFileName = $"Plan_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv"; //Nazwa to jest obecna data, godzina

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.InitialDirectory = saveFolder; //Domyślny folder
                sfd.FileName = defaultFileName; //Domyślna nazwa pliku
                sfd.Filter = "CSV files (*.csv)|*.csv"; //Filtr
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = sfd.FileName;

                    //Pobieranie danych do zapisu
                    using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
                    {
                        for (int i = 0; i < dgv.RowCount; i++)
                        {
                            List<string> rowValues = new List<string>();
                            for (int j = 0; j < dgv.ColumnCount; j++)
                            {
                                string cellText = dgv.Rows[i].Cells[j].Value?.ToString() ?? " ";

                                if (cellText.Contains(","))
                                    cellText = $"\"{cellText}\"";

                                rowValues.Add(cellText);
                            }
                            sw.WriteLine(string.Join(";", rowValues));
                        }
                    }

                    MessageBox.Show($"Plik zapisany: {filePath}", "Zapis zakończony", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btn_wczytaj_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Wybierz plik CSV do wczytania";
                ofd.Filter = "Pliki CSV|*.csv|Wszystkie pliki|*.*";
                ofd.InitialDirectory = saveFolder;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = ofd.FileName;
                    string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

                    for (int i = 0; i < dgv.RowCount; i++)
                    {
                        if (i >= lines.Length) break;

                        string[] values = lines[i].Split(';');
                        for (int j = 0; j < dgv.ColumnCount; j++)
                        {
                            if (j < values.Length)
                                dgv.Rows[i].Cells[j].Value = string.IsNullOrWhiteSpace(values[j]) ? " " : values[j];
                            else
                                dgv.Rows[i].Cells[j].Value = " ";
                        }
                    }

                    MessageBox.Show($"Plik wczytany: {filePath}", "Wczytano", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Odświeżenie minut po wczytaniu planu
                    OdswiezWszystkieMinuty();
                }
            }
        }

        //Edytowanie planów
        private void btn_edytor_Click(object sender, EventArgs e)
        {
            var edytor = new FormEdytorPlanow(planyFolder, listaNaucz);
            edytor.ShowDialog(this);

            //Odświeżenie po ewentualnych zmianach w planach
            WczytajNauczycieli();
            OdswiezWszystkieMinuty();
        }

        //Jestem na 15 piwie
        //Dla Paulinki
        //https://learn.microsoft.com/en-us/dotnet/api/system.net.webclient?view=net-9.0
        //https://html-agility-pack.net/
        //QoL vvv (ale manualne dodawanie też zrobimy, żeby można było edytować i dodać np. wykrzykniki czy coś, albo się dopsze:
        //dla każdego nauczyciela, który następną lekcję ma na innym budynku, nie będize miał dyzuru + wszystkie dni rano dla PI <3)

        private void btn_pobierz_plany_Click(object sender, EventArgs e) { PobierzPlanyNauczycieli(); }

        private List<string> PobierzLinkiNauczycieli() //Ma za zadanie zwrócić tablicę z linkami do stron z planami osobno
        {
            string url = "https://plan.zst-tarnow.pl/lista.html"; //Zmienna z linkiem (tak jest taka podstrona /lista.html bo plan.zst-tarnow jest na framesecie)
            WebClient client = new WebClient();
            string html = client.DownloadString(url); //Zapisuje html 

            var strona = new HtmlAgilityPack.HtmlDocument(); //Zapisuje html
            strona.LoadHtml(html); //Ładuje html

            var h4 = strona.DocumentNode.SelectSingleNode("//h4[contains(text(),'Nauczyciele')]");//Szuka "<h4>Nauczyciele</h4>"
            if (h4 == null)
            {
                MessageBox.Show("Nie znaleziono listy z nauczycielami (h4)");
                return new List<string>(); //Zwraca pustą tablicę, więc w sumie nic więcej się nie wykona
            }

            var ul = h4.SelectSingleNode("following-sibling::ul[1]"); //Szuka następny element po h4 który ma być DOKŁADNIE PO i DOKŁADNIE BYĆ ul-em
            if (ul == null)
            {
                MessageBox.Show("Nie znaleziono listy z nauczycielami (ul)");
                return new List<string>(); //Zwraca pustą tablicę, więc w sumie nic więcej się nie wykona
            }

            var linki = ul.SelectNodes(".//a"); //"Tablica" zawierająca wszyskie a jakie znalazło w ul, czyli w tym przypadku wszystkie odnośniki od podstron planów
            List<string> urls = new List<string>(); //Nowa tablica (tzn lista ale nie istotne) z wszystkimi linkami pobranymi z odnośników
            if (linki != null)
            {
                foreach (var a in linki) //Dla każdego a
                {
                    string href = a.GetAttributeValue("href", ""); //Wyciąga href z każdego a (jak nie ma, to puste), a href to np. plany/n57.html
                    if (!string.IsNullOrEmpty(href))
                        urls.Add("https://plan.zst-tarnow.pl/" + href); //Dodaje do tablicy urls link do podstrony
                }
            }

            client.Dispose(); //Sprzątu sprzątu
            return urls; //Zwraca tą tablicę
        }

        private void PobierzPlanyNauczycieli()
        {
            // Czyszczenie pliku godzin przed nowym pobieraniem
            // BEZ tego przy każdym kolejnym kliknięciu "Pobierz plany" minuty się dopisywały zamiast nadpisywać
            File.WriteAllText(listaGodziny, "", Encoding.UTF8);

            var linki = PobierzLinkiNauczycieli(); //Pobiera sobie te linki
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8; //Inaczej polskie znaki w skrótach się nawet nie zapisują

            foreach (var link in linki) //Instrukcja dla każdego linku czyli każdego nauczyciela
            {
                string html = client.DownloadString(link);
                var strona = new HtmlAgilityPack.HtmlDocument();
                strona.LoadHtml(html);

                //Nazywanie pliku
                var span = strona.DocumentNode.SelectSingleNode("//span[@class='tytulnapis']");
                string skrot = "PiątekJumpscare"; //Plik sie nazwie PiątekJumpscare jak skrót się nie wczyta :3 (np miałam problem z Żurowską)
                if (span != null)
                {
                    var match = System.Text.RegularExpressions.Regex.Match(span.InnerText, @"\((.*?)\)"); //Propozycja MichasiaGPT, wyciąga skrót z nawiasów
                    if (match.Success) //Jeśli znajdzie
                        skrot = match.Groups[1].Value; //0 wyciągnie wszystko, 1 tylko następne czyli zawartość nawiasu, teraz "skrot" przechowuje np. PI
                }

                string nazwaPliku = Path.Combine(planyFolder, skrot + ".txt"); //Tworzenie pliku, np. PI.txt

                //Tworzenie tabelki z planem
                var tabela = strona.DocumentNode.SelectSingleNode("//table[@class='tabela']"); //Szuka tabeli z klasą tabela bo ktoś zbudował tą strone na kilku tabelach jeez Wardzała by go zjadła
                if (tabela == null) //Jeśli nie znaleziono tabelki to zapisuje pusty szablon
                {
                    string templatePath = Path.Combine(planyFolder, "template.txt");
                    string nauczyciel = Path.GetFileNameWithoutExtension(nazwaPliku);
                    string templateContent = File.ReadAllText(templatePath);
                    File.WriteAllText(nazwaPliku, templateContent);

                    MessageBox.Show($"Nie znaleziono żadnego planu dla {nauczyciel} (zapisano pusty)");
                    continue;
                }

                int hCounter = 0;

                StringBuilder sb = new StringBuilder(); //Stringbuilder dla ułatwienia
                string[] dni = { "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek" };
                var wiersze = tabela.SelectNodes(".//tr[position()>1]"); //Wyszukuje wszystkie wiersze poza pierwszym z nagłówkami
                if (wiersze == null) continue;

                foreach (string dzien in dni) //Każda iteracja to jeden dzień tygodnia zapisany w pliku
                {
                    sb.AppendLine(dzien); //Dodanie obecnego dnia
                    int kolumna = Array.IndexOf(dni, dzien) + 3; //Bierze z tablicy index dnia (czyli np. 4 dla Piątku)
                                                                 //i dodaje 3 (bo na stronie w tr dni tygodnia są dopiero od 3-go th)
                                                                 //Teraz kolumna dla piątku to 7, czyli będzie analizować wszystkie siódme pozycje
                                                                 //(th) w reszcie wierszy

                    foreach (var wiersz in wiersze) //Tutaj jest to iterowanie co mówiłam wczesniej
                    {
                        var komorkaNr = wiersz.SelectSingleNode("./td[@class='nr']"); //Szuka td z klasą nr żeby go wyciagnąć i zapisać
                        string nr = komorkaNr.InnerText.Trim();                       //(Osobno od reszty bo między nimi są jeszcze godziny
                                                                                      //które pomijamy i reszta właściwie nawet nie ma już swoich nazw)

                        var komorka = wiersz.SelectSingleNode($"./td[{kolumna}]"); //Szuka td jakby n-tego z kolejności (czyli tego do numeru dnia)
                        string kl = "-", sal = "-", pr = "-", gr = ""; //Domyślne pola do wpisania do linii, jak po prosut nic sie nie znajdzie to będzie to

                        if (komorka != null && komorka.InnerText.Trim() != "")
                        {   //Jak nie ma nic w środku to sie wykona
                            //Pobiera sobie klase przedmiot sale i zapisuje do pliku według wzoru

                            var o = komorka.SelectSingleNode(".//a[@class='o']"); //Klasa (oddział)
                            if (o != null)
                            {
                                hCounter++; //Dodaje godzinę

                                kl = o.InnerText.Trim(); //Trim dopiero tutaj żeby sie nie przekonwertowało wcześniej na stringa

                                var next = o.NextSibling;
                                if (next != null)
                                {
                                    string raw = next.InnerText?.Trim();
                                    if (!string.IsNullOrEmpty(raw))
                                    {
                                        raw = raw.Trim();
                                        if (!string.IsNullOrEmpty(raw)) gr = raw;
                                    }
                                }
                            }
                            string wynik = kl;
                            if (!string.IsNullOrEmpty(gr)) wynik += gr;
                            gr = wynik;

                            var p = komorka.SelectSingleNode(".//span[@class='p']")?.InnerText; //Przedmiot
                            var s = komorka.SelectSingleNode(".//a[@class='s']")?.InnerText; //Sala

                            //if (o != null && o != "") kl = o;
                            if (p != null && p != "") pr = p;
                            if (s != null && s != "") sal = s;
                        }

                        sb.AppendLine($"{nr} ; {sal} ; {gr} ; {pr} ;");
                    }
                    sb.AppendLine(); //Linja odstenpuf
                }

                // Pobrany plan nauczyciela — od razu aplikujemy klasy na praktykach
                // Każda lekcja z klasą z listy praktyk zamieniana jest na pustą komórkę
                string planString = UsunKlasyNaPraktykachZPlanu(sb.ToString());

                File.WriteAllText(nazwaPliku, planString, Encoding.UTF8); //Zapisanie do pliku wyniku stringbuildera, encoding jest też bo lubi nie zapisywac polskicj znaków

                // Wyłączeni nauczyciele — zerujemy hCounter żeby minuty się im nie liczyły
                if (wylaczeniNauczyciele.Contains(skrot)) hCounter = 0;

                //Obliczanie minut — proporcjonalnie do liczby godzin nauczyciela względem sumy wszystkich
                double minuty = CALKOWITE_MINUTY_DYZUROW * (Convert.ToDouble(hCounter) / SUMA_GODZIN_NAUCZYCIELI);
                minuty = Math.Round(minuty, 2);

                //Dodanie minut do pliku
                File.AppendAllText(listaGodziny, (skrot + $" {minuty}\r\n"), Encoding.UTF8);

            }

            client.Dispose(); //Sprzątu sprzątu
            MessageBox.Show("Pobrano wszystkie plany.", "", MessageBoxButtons.OK, MessageBoxIcon.Information); //Yayyy

            //Dodawanie godzin (później minut) obok imienia nauczyciela
            nauczyciele = File.ReadAllLines(listaNaucz);

            var godzinyPlik = File.ReadAllLines(listaGodziny);

            foreach (string linia in godzinyPlik)
            {
                string[] parts = linia.Split(' ');
                string skrot = parts[0];
                double mins = Convert.ToDouble(parts[1]);

                foreach (Control ctrl in gB_naucz.Controls)
                {
                    if (ctrl is RadioButton rb && rb.Name == skrot)
                    {
                        string nazwa = rb.Text;

                        //yusuwanie duplikatow
                        nazwa = System.Text.RegularExpressions.Regex.Replace(nazwa, @"\s-\s\d?\d.?\d?\dmin.*$", "");

                        //limit minut i miejsce na wykorzystane (wypełniane przez OdswiezMinutyNauczyciela)
                        rb.Text = nazwa + $" - {mins}min | wyk.: 0min | zost.: {mins}min";
                        break;
                    }
                }
            }

            //Odświeżenie wykorzystanych minut jeśli już jest jakiś plan wczytany
            OdswiezWszystkieMinuty();
        }

        // Oblicza ile minut dyżurów ma już przypisanych dany nauczyciel w dgv
        // Każda komórka z nazwiskiem nauczyciela = 15 minut (jedna przerwa)
        private double ObliczWykorzystaneMinuty(string skrotNauczyciela)
        {
            double wykorzystane = 0;

            for (int r = 0; r < dgv.RowCount; r++)
            {
                for (int c = 1; c < dgv.ColumnCount; c++)
                {
                    if (c == 8) continue; //Kolumna "Od - Do" po środku, pomijamy

                    string val = dgv.Rows[r].Cells[c].Value?.ToString() ?? "";

                    //Wartość komórki to "P.Piątek (PI)", więc szukamy po "(PI)"
                    if (val.Contains($"({skrotNauczyciela})"))
                        wykorzystane += 15; //15 minut na jeden dyżur
                }
            }

            return wykorzystane;
        }

        // Odświeża wyświetlane minuty (limit / wykorzystane / pozostałe) dla jednego nauczyciela
        // Wywołuje się po przypisaniu dyżuru z comboboxa
        private void OdswiezMinutyNauczyciela(string pelneNazwisko)
        {
            if (string.IsNullOrWhiteSpace(pelneNazwisko)) return;

            //Wyciągamy skrót z "P.Piątek (PI)"
            string skrot = ZnajdzSkrot(pelneNazwisko);
            if (string.IsNullOrWhiteSpace(skrot)) return;

            // Wyłączony nauczyciel — minuty go nie dotyczą
            if (wylaczeniNauczyciele.Contains(skrot)) return;

            //Szukamy tego nauczyciela w pliku godzin żeby mieć jego limit
            if (!File.Exists(listaGodziny)) return;
            string[] godzinyPlik = File.ReadAllLines(listaGodziny);

            double limit = -1;
            foreach (string linia in godzinyPlik)
            {
                string[] parts = linia.Trim().Split(' ');
                if (parts.Length < 2) continue;
                if (parts[0] == skrot)
                {
                    double.TryParse(parts[1], System.Globalization.NumberStyles.Any,
                                    System.Globalization.CultureInfo.InvariantCulture, out limit);
                    break;
                }
            }

            if (limit < 0) return; //Nie znaleziono w pliku, nie aktualizujemy

            double wykorzystane = ObliczWykorzystaneMinuty(skrot);
            double pozostalo = Math.Round(limit - wykorzystane, 2);

            //Aktualizacja tekstu radio buttona
            foreach (Control ctrl in gB_naucz.Controls)
            {
                if (ctrl is RadioButton rb && rb.Name == skrot)
                {
                    //Usuwamy starą część z minutami i dopisujemy świeżą
                    string nazwa = System.Text.RegularExpressions.Regex.Replace(
                        rb.Text, @"\s-\s\d?\d.?\d?\dmin.*$", "");

                    rb.Text = nazwa + $" - {limit}min | wyk.: {wykorzystane}min | zost.: {pozostalo}min";
                    break;
                }
            }
        }

        // Odświeża minuty dla wszystkich nauczycieli naraz
        // Przydatne po wczytaniu planu z pliku
        private void OdswiezWszystkieMinuty()
        {
            if (!File.Exists(listaGodziny)) return;
            string[] godzinyPlik = File.ReadAllLines(listaGodziny);

            foreach (string linia in godzinyPlik)
            {
                string[] parts = linia.Trim().Split(' ');
                if (parts.Length < 2) continue;

                string skrot = parts[0];

                // Wyłączony nauczyciel — 0/0/0 zamiast normalnych minut
                if (wylaczeniNauczyciele.Contains(skrot))
                {
                    foreach (Control ctrl in gB_naucz.Controls)
                    {
                        if (ctrl is RadioButton rb && rb.Name == skrot)
                        {
                            string nazwa = System.Text.RegularExpressions.Regex.Replace(
                                rb.Text, @"\s-\s\d?\d.?\d?\dmin.*$", "");
                            rb.Text = nazwa + " - [wyłączony]";
                            break;
                        }
                    }
                    continue;
                }

                if (!double.TryParse(parts[1], System.Globalization.NumberStyles.Any,
                                     System.Globalization.CultureInfo.InvariantCulture, out double limit)) continue;

                double wykorzystane = ObliczWykorzystaneMinuty(skrot);
                double pozostalo = Math.Round(limit - wykorzystane, 2);

                foreach (Control ctrl in gB_naucz.Controls)
                {
                    if (ctrl is RadioButton rb && rb.Name == skrot)
                    {
                        string nazwa = System.Text.RegularExpressions.Regex.Replace(
                            rb.Text, @"\s-\s\d?\d.?\d?\dmin.*$", "");

                        rb.Text = nazwa + $" - {limit}min | wyk.: {wykorzystane}min | zost.: {pozostalo}min";
                        break;
                    }
                }
            }
        }

        //APPDATA.
        private void TworzeniePlikow()
        {
            string AplikDyzur = Path.Combine(localAppData, "AplikDyzur");

            if (!Directory.Exists(AplikDyzur)) Directory.CreateDirectory(AplikDyzur);
            if (!Directory.Exists(planyFolder)) Directory.CreateDirectory(planyFolder);
            if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);

            File.WriteAllText(listaGodziny, "");

            // Tworzymy pliki list jeśli nie istnieją — przeżyją restart aplikacji
            if (!File.Exists(listaKlasyPraktyki)) File.WriteAllText(listaKlasyPraktyki, "", Encoding.UTF8);
            if (!File.Exists(listaWylaczeniNaucz)) File.WriteAllText(listaWylaczeniNaucz, "", Encoding.UTF8);

            if (!File.Exists(listaNaucz))
            {
                try
                {
                    WebClient client = new WebClient();
                    client.Encoding = Encoding.UTF8;
                    string url = "https://plan.zst-tarnow.pl/lista.html";
                    string html = client.DownloadString(url);

                    var strona = new HtmlAgilityPack.HtmlDocument();
                    strona.LoadHtml(html);

                    //Szukanie h4 i ul
                    var h4 = strona.DocumentNode.SelectSingleNode("//h4[contains(text(),'Nauczyciele')]");
                    if (h4 != null)
                    {
                        var ul = h4.SelectSingleNode("following-sibling::ul[1]");
                        if (ul != null)
                        {
                            var liNodes = ul.SelectNodes(".//a");
                            if (liNodes != null)
                            {
                                List<string> nauczyciele = new List<string>();
                                foreach (var a in liNodes)
                                {
                                    string text = a.InnerText.Trim();
                                    if (!string.IsNullOrEmpty(text))
                                    {
                                        string[] prefix = { "dyr. ", "vice-dyr. ", "kier. " };
                                        foreach (var pref in prefix)
                                            if (text.StartsWith(pref, StringComparison.OrdinalIgnoreCase)) //Żeby nie brało pod uwage wielkosci liter
                                                text = text.Substring(pref.Length).Trim();
                                        nauczyciele.Add(text);
                                    }
                                }
                                File.WriteAllLines(listaNaucz, nauczyciele, Encoding.UTF8);
                            }
                        }
                    }

                    client.Dispose();
                }
                catch
                {
                    MessageBox.Show("Nie udało się pobrać listy nauczycieli", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    File.WriteAllText(listaNaucz, "");
                }
            }

            if (!File.Exists(listaSale))
            {
                try
                {
                    WebClient client = new WebClient();
                    client.Encoding = Encoding.UTF8;
                    string url = "https://plan.zst-tarnow.pl/lista.html";
                    string html = client.DownloadString(url);

                    var strona = new HtmlAgilityPack.HtmlDocument();
                    strona.LoadHtml(html);

                    //Szukanie h4 i ul
                    var h4 = strona.DocumentNode.SelectSingleNode("//h4[contains(text(),'Sale')]");
                    if (h4 != null)
                    {
                        var ul = h4.SelectSingleNode("following-sibling::ul[1]");
                        if (ul != null)
                        {
                            var liNodes = ul.SelectNodes(".//a");
                            if (liNodes != null)
                            {
                                List<string> sale = new List<string>();
                                foreach (var a in liNodes)
                                {
                                    string text = a.InnerText.Trim();
                                    if (!string.IsNullOrEmpty(text))
                                    {
                                        string pierwEle = text.Split(' ')[0]; //Żeby nie zapisywało sie jaikes "12 fizyczna"
                                        sale.Add(pierwEle);
                                    }
                                }
                                File.WriteAllLines(listaSale, sale, Encoding.UTF8);
                            }
                        }
                    }

                    client.Dispose();
                }
                catch
                {
                    MessageBox.Show("Nie udało się pobrać listy sal", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    File.WriteAllText(listaSale, "");
                }
            }
        }

        //Eksport do excela
        //Generuje się nowy plik, bo nie mogę przeciez nadpisywac tamtego oficjalnego xd więc pan piątus sobie wynik skopiuje tam ok
        private void ExportDGV_NPOI(DataGridView dgv, string filePath)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Harmonogram"); //Tworzenie nowego arkusza pliku wszystko

            sheet.SetColumnWidth(0, 15 * 256); //Utawienie szerokości kolumny 1 na dni tygodnia

            //Nagłówki budynków
            var row0 = sheet.CreateRow(0);
            row0.CreateCell(1).SetCellValue("Budynek główny");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 1, 8));

            row0.CreateCell(10).SetCellValue("Pracownie");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 10, 16));

            //Nagłówki kolumn
            string[] naglowki = new string[dgv.Columns.Count];
            var row1 = sheet.CreateRow(1);

            for (int c = 0; c < dgv.Columns.Count; c++) //Pętla po kolumnach w dgv
            {
                string header = dgv.Columns[c].HeaderText;
                //Zamiana nagłówków na te co są w szabloniku
                if (header == "Od - Do") header = "Od Do";
                if (header == "W.F.") header = "W. F.";
                if (header == "Piętro 1") header = "1 Piętro";
                if (header == "Piętro 2") header = "2 Piętro";

                naglowki[c] = header;

                int excelCol = c + 1;
                if (c >= 8) excelCol++;

                row1.CreateCell(excelCol).SetCellValue(header);
            }

            int excelRow = 2;

            //Przenoszenie danych
            for (int r = 0; r < dgv.Rows.Count; r++)
            {
                int dzienIndex = r / 10;

                //Wstawienie nagłówków dla następnego dnia
                if (r % 10 == 0)
                {
                    if (r > 0)
                    {
                        //Nagłówki powtarzane nad każdym dniem
                        var rowHeader = sheet.CreateRow(excelRow);
                        for (int c = 0; c < naglowki.Length; c++)
                        {
                            int excelCol = c + 1;
                            if (c >= 8) excelCol++;
                            rowHeader.CreateCell(excelCol).SetCellValue(naglowki[c]);
                        }
                        excelRow++;
                    }

                    //Dni tygodnia
                    if (dzienIndex < dniTygodnia.Length)
                    {
                        var rowDay = sheet.CreateRow(excelRow);
                        rowDay.CreateCell(0).SetCellValue(dniTygodnia[dzienIndex]);   //Lewa tabelka z dniami
                        rowDay.CreateCell(9).SetCellValue(dniTygodnia[dzienIndex].ToLower()); //Prawa tabelka z dniami
                    }
                }

                var rowData = sheet.GetRow(excelRow);
                if (rowData == null)
                    rowData = sheet.CreateRow(excelRow);


                for (int c = 0; c < dgv.Columns.Count; c++) //Pętla po kolumnach
                {
                    string val;
                    //Pobranie wartości
                    if (dgv.Rows[r].Cells[c].Value == null) val = "";
                    else val = dgv.Rows[r].Cells[c].Value.ToString();

                    //Odpowiednia zamiana
                    if (val == "Rano") val = "7:30";
                    if (val.StartsWith("Po L")) val = val.Replace("Po L", "Po lekcji ");

                    //Obliczenie kolumny
                    int excelCol = c + 1;
                    if (c >= 8) excelCol++;

                    rowData.CreateCell(excelCol).SetCellValue(val); //Wpisanie danych do excela
                }

                excelRow++; //Nastepny wiersz
            }

            //Automatyczne dostosowywanie kolumn do szerokości dla przejrzystości
            for (int i = 0; i <= 20; i++) sheet.AutoSizeColumn(i);

            //Zapis
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write)) workbook.Write(fs);
        }


        private void btnExport_Click(object sender, EventArgs e) //Weeeeeeeee
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Files|*.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportDGV_NPOI(dgv, sfd.FileName);
                    MessageBox.Show("Pomyślnie wyeksportowano");
                }
            }
        }

        //Konwersja xlsx (według szablonu oficjalnego pliku) do csv wczytywalnego przez program
        private void KonwertujXlsxDoCsv_NPOI(string xlsxPath, string csvPath)
        {
            IWorkbook workbook; //Tuuuuuuuuuuuuuuuu operujemy
            using (var fs = new FileStream(xlsxPath, FileMode.Open, FileAccess.Read)) workbook = new XSSFWorkbook(fs); //Odczyt xlsx 

            var sheet = workbook.GetSheetAt(0); //Wczytanie arkusza zeszytu whatever to coś na dole

            string[] lekcje = { "Rano", "Po L1", "Po L2", "Po L3", "Po L4", "Po L5", "Po L6", "Po L7", "Po L8" }; //Żeby zamienic lekcje
            var zakresy = new (int startRowMain, int startRowLabs)[] { (7, 7), (18, 18), (29, 29), (40, 40), (51, 51) }; //Od których wierszy zacząć czytać

            using (var sw = new StreamWriter(csvPath, false, Encoding.UTF8))
            {
                foreach (var z in zakresy) //Jeden dzień
                {
                    for (int i = 0; i < lekcje.Length; i++) //Kolejne wiersze z nazwiskami
                    {
                        int rowGlowny = z.startRowMain + i; //Nr wiersza w arkuszu dla głównego
                        int rowPrac = z.startRowLabs + i; //Nr wiersza w artkuszu dla pracowni 

                        //Nie chce mi sie tlumaczyc chce isc spac paulinka inteligentna zrozumie na logike

                        string[] glowny = new string[7];
                        string[] prac = new string[6];

                        for (int c = 0; c < 7; c++)
                        {
                            string value = "";

                            IRow row = sheet.GetRow(rowGlowny - 1); //-1 bo getrow jest od 0
                            if (row != null)
                            {
                                ICell cell = row.GetCell(3 + c); //Pierwsza do odczytu

                                if (cell != null)
                                    value = cell.ToString();
                            }

                            glowny[c] = value;
                        }

                        for (int c = 0; c < 6; c++) //To samo ale pracownie
                        {
                            string value = "";

                            IRow row = sheet.GetRow(rowPrac - 1);
                            if (row != null)
                            {
                                ICell cell = row.GetCell(12 + c);

                                if (cell != null)
                                    value = cell.ToString();
                            }

                            prac[c] = value;
                        }

                        string line = lekcje[i] + ";" + string.Join(";", glowny) + ";" + lekcje[i] + ";" + string.Join(";", prac); //Zbudowanie linii
                        sw.WriteLine(line); //Zapis linii
                    }

                    sw.WriteLine(" ; ; ; ; ; ; ; ; ; ; ; ; ; ; "); //Pusta linia między dniami
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e) //Nwm co tu do tłumaczenia no na logike
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Pliki Excel (*.xlsx)|*.xlsx";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                string inputPath = ofd.FileName;
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string csvPath = Path.Combine(saveFolder, $"Plan_IMPORT_{timestamp}.csv");

                try
                {
                    KonwertujXlsxDoCsv_NPOI(inputPath, csvPath);
                    MessageBox.Show($"Import zakończony.\nZapisano:\n{csvPath}");
                }
                catch
                {
                    MessageBox.Show("Nie udało się zaimportować pliku.");
                }
            }
        }

        //Eksport do PDF vvvvv

        //Dodaje przycisk "Eksportuj PDF" programowo obok btnExport
        //Designer nie jest tutaj więc tak jest łatwiej niż grzebanie w .Designer.cs
        private void btn_pdf_Click(object sender, EventArgs e)
        {
            //Małe okienko do wpisania nagłówka
            string naglowek = "";

            using (Form okienko = new Form())
            {
                okienko.Text = "Nagłówek PDF";
                okienko.Size = new System.Drawing.Size(420, 160);
                okienko.StartPosition = FormStartPosition.CenterParent;
                okienko.FormBorderStyle = FormBorderStyle.FixedDialog;
                okienko.MaximizeBox = false;
                okienko.MinimizeBox = false;

                var lb = new Label
                {
                    Text = "Wpisz nagłówek dla planu:",
                    Location = new System.Drawing.Point(12, 15),
                    AutoSize = true
                };

                var tb = new TextBox
                {
                    Location = new System.Drawing.Point(12, 38),
                    Width = 378,
                    Font = new System.Drawing.Font("Verdana", 10)
                };

                var btnOK = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Location = new System.Drawing.Point(220, 75),
                    Width = 80
                };

                var btnAnuluj = new Button
                {
                    Text = "Anuluj",
                    DialogResult = DialogResult.Cancel,
                    Location = new System.Drawing.Point(310, 75),
                    Width = 80
                };

                okienko.Controls.AddRange(new Control[] { lb, tb, btnOK, btnAnuluj });
                okienko.AcceptButton = btnOK;
                okienko.CancelButton = btnAnuluj;

                if (okienko.ShowDialog() != DialogResult.OK) return;
                naglowek = tb.Text.Trim();
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PDF Files|*.pdf";
                sfd.FileName = $"Plan_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf";
                sfd.InitialDirectory = saveFolder;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportDGV_PDF(dgv, sfd.FileName, naglowek);
                        MessageBox.Show("Pomyślnie wyeksportowano do PDF.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Nie udało się wyeksportować PDF:\n{ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExportDGV_PDF(DataGridView dgv, string filePath, string naglowek)
        {
            var pageSize = new iTextPdf.Rectangle(iTextPdf.PageSize.A3.Height, iTextPdf.PageSize.A3.Width);
            var doc = new iTextPdf.Document(pageSize, 15f, 15f, 15f, 15f); //Małe marginesy żeby zmaksymalizować miejsce

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                iTextPdfCore.PdfWriter.GetInstance(doc, fs);
                doc.Open();

                //preferowany arial żeby były polskie znaki
                string arialPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                string arialBoldPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arialbd.ttf");

                iTextPdfCore.BaseFont baseFontNormal, baseFontBold;

                if (File.Exists(arialPath) && File.Exists(arialBoldPath))
                {
                    baseFontNormal = iTextPdfCore.BaseFont.CreateFont(arialPath, iTextPdfCore.BaseFont.IDENTITY_H, iTextPdfCore.BaseFont.EMBEDDED);
                    baseFontBold = iTextPdfCore.BaseFont.CreateFont(arialBoldPath, iTextPdfCore.BaseFont.IDENTITY_H, iTextPdfCore.BaseFont.EMBEDDED);
                }
                else
                {
                    baseFontNormal = iTextPdfCore.BaseFont.CreateFont(iTextPdfCore.BaseFont.HELVETICA, iTextPdfCore.BaseFont.CP1252, iTextPdfCore.BaseFont.NOT_EMBEDDED);
                    baseFontBold = iTextPdfCore.BaseFont.CreateFont(iTextPdfCore.BaseFont.HELVETICA_BOLD, iTextPdfCore.BaseFont.CP1252, iTextPdfCore.BaseFont.NOT_EMBEDDED);
                }

                var fontNaglowek = new iTextPdf.Font(baseFontBold, 10f, iTextPdf.Font.BOLD);   //Nagłówek dokumentu
                var fontNaglowekTab = new iTextPdf.Font(baseFontBold, 5.5f, iTextPdf.Font.BOLD);  //Nagłówki kolumn/budynków
                var fontDzien = new iTextPdf.Font(baseFontBold, 6f, iTextPdf.Font.BOLD);  //Nazwa dnia tygodnia
                var fontKomorka = new iTextPdf.Font(baseFontNormal, 5.5f, iTextPdf.Font.NORMAL);//Dane nauczycieli
                var fontOdDo = new iTextPdf.Font(baseFontBold, 5.5f, iTextPdf.Font.BOLD);  //Kolumny "Od Do"

                //Nagłówek dokumentu
                if (!string.IsNullOrEmpty(naglowek))
                {
                    var pNaglowek = new iTextPdf.Paragraph(naglowek, fontNaglowek)
                    {
                        Alignment = iTextPdf.Element.ALIGN_CENTER,
                        SpacingAfter = 5f
                    };
                    doc.Add(pNaglowek);
                }

                //Kolorki
                var colorNaglowek = new iTextPdf.BaseColor(220, 220, 220);
                var colorOdDo = new iTextPdf.BaseColor(240, 240, 240);
                var colorDzien = new iTextPdf.BaseColor(200, 200, 200);
                var colorBialy = iTextPdf.BaseColor.WHITE;

                string[] naglowki = new string[dgv.Columns.Count];
                for (int c = 0; c < dgv.Columns.Count; c++)
                {
                    string h = dgv.Columns[c].HeaderText;
                    if (h == "Od - Do") h = "Od Do";
                    if (h == "W.F.") h = "W.F.";
                    naglowki[c] = h;
                }

                //Tabela: 15 kolumn dgv + 1 kolumna na dzień tygodnia po lewej = 16 kolumn
                int colCount = dgv.Columns.Count + 1; //+1 za dzień tygodnia
                float[] szerokosci = new float[colCount];

                szerokosci[0] = 30f; //Kolumna dnia tygodnia
                for (int c = 0; c < dgv.Columns.Count; c++)
                {
                    float sz = (c == 0 || c == 8) ? 22f : 38f; //Od Do wąskie, reszta równe
                    szerokosci[c + 1] = sz;
                }

                var tabela = new iTextPdfCore.PdfPTable(szerokosci)
                {
                    WidthPercentage = 100f,
                    SpacingBefore = 3f
                };

                //Pomocna funkcja do tworzenia komórki
                iTextPdfCore.PdfPCell MakeCell(string text, iTextPdf.Font font, iTextPdf.BaseColor bg,
                                               int align = iTextPdf.Element.ALIGN_CENTER, int colspan = 1)
                {
                    var cell = new iTextPdfCore.PdfPCell(new iTextPdf.Phrase(text, font))
                    {
                        BackgroundColor = bg,
                        HorizontalAlignment = align,
                        VerticalAlignment = iTextPdf.Element.ALIGN_MIDDLE,
                        Colspan = colspan,
                        Padding = 2f,      //Trochę więcej paddingu żeby tekst nie był ściśnięty
                        PaddingTop = 4f,
                        PaddingBottom = 4f
                    };
                    return cell;
                }

                //Wiersz z nagłówkami budynków (jak w Excel: "Budynek główny" i "Pracownie")
                tabela.AddCell(MakeCell("", fontNaglowekTab, colorNaglowek));
                tabela.AddCell(MakeCell("Budynek główny", fontNaglowekTab, colorNaglowek, iTextPdf.Element.ALIGN_CENTER, 8));
                tabela.AddCell(MakeCell("Pracownie", fontNaglowekTab, colorNaglowek, iTextPdf.Element.ALIGN_CENTER, 7));

                //Wiersz z nazwami kolumn
                tabela.AddCell(MakeCell("", fontNaglowekTab, colorNaglowek));
                for (int c = 0; c < dgv.Columns.Count; c++)
                    tabela.AddCell(MakeCell(naglowki[c], fontNaglowekTab, colorNaglowek));

                //Dane — pętla po wierszach dgv
                for (int r = 0; r < dgv.RowCount; r++)
                {
                    int dzienIndex = r / 10;
                    int wierszWDniu = r % 10;

                    //Na początku każdego dnia — wiersz z nazwą dnia
                    if (wierszWDniu == 0)
                    {
                        string nazwaD = dzienIndex < dniTygodnia.Length ? dniTygodnia[dzienIndex] : "";
                        tabela.AddCell(MakeCell(nazwaD, fontDzien, colorDzien, iTextPdf.Element.ALIGN_LEFT, colCount));
                    }

                    //Separator między dniami (wiersz 9 czyli index % 10 == 9) — cienki pasek
                    if (wierszWDniu == 9)
                    {
                        for (int c = 0; c < colCount; c++)
                        {
                            var sep = MakeCell("", fontKomorka, colorNaglowek);
                            sep.FixedHeight = 3f;
                            tabela.AddCell(sep);
                        }
                        continue;
                    }

                    //Kolumna dnia — pusta (dzień już jest w wierszu nagłówkowym powyżej)
                    tabela.AddCell(MakeCell("", fontKomorka, colorBialy));

                    //Komórki danych
                    for (int c = 0; c < dgv.Columns.Count; c++)
                    {
                        string val = dgv.Rows[r].Cells[c].Value?.ToString() ?? "";

                        if (val == "Rano") val = "7:30";
                        if (val.StartsWith("Po L")) val = val.Replace("Po L", "Po L");

                        bool isOdDo = (c == 0 || c == 8);
                        var bg = isOdDo ? colorOdDo : colorBialy;
                        var font = isOdDo ? fontOdDo : fontKomorka;

                        tabela.AddCell(MakeCell(val, font, bg));
                    }
                }

                doc.Add(tabela);
                doc.Close();
            }
        }

        //Eksport do PDF ^^^^^




        // =====================================================================
        // KLASY NA PRAKTYKACH + WYŁĄCZENI NAUCZYCIELE
        // Sekcja budowana dynamicznie wewnątrz scrollPanela w gB_naucz, po radio buttonach
        // =====================================================================

        // Wczytuje listę klas na praktykach z pliku AppData
        private void WczytajKlasyPraktyki()
        {
            klasyNaPraktykach.Clear();
            if (!File.Exists(listaKlasyPraktyki)) return;

            foreach (string linia in File.ReadAllLines(listaKlasyPraktyki, Encoding.UTF8))
            {
                string k = linia.Trim();
                if (!string.IsNullOrEmpty(k)) klasyNaPraktykach.Add(k);
            }
        }

        // Wczytuje listę wyłączonych nauczycieli (skróty) z pliku AppData
        private void WczytajWylaczonych()
        {
            wylaczeniNauczyciele.Clear();
            if (!File.Exists(listaWylaczeniNaucz)) return;

            foreach (string linia in File.ReadAllLines(listaWylaczeniNaucz, Encoding.UTF8))
            {
                string s = linia.Trim();
                if (!string.IsNullOrEmpty(s)) wylaczeniNauczyciele.Add(s);
            }
        }

        // Zapisuje listy do plików AppData — wywoływane po każdej zmianie
        private void ZapiszKlasyPraktyki()
        {
            File.WriteAllLines(listaKlasyPraktyki, klasyNaPraktykach, Encoding.UTF8);
        }

        private void ZapiszWylaczonych()
        {
            File.WriteAllLines(listaWylaczeniNaucz, wylaczeniNauczyciele, Encoding.UTF8);
        }

        // Buduje sekcję praktyk i wyłączonych wewnątrz przekazanego panelu, zaczynając od podanego y
        // Wywoływana przez DodanieRadioNauczycieli po wstawieniu radio buttonów
        private void ZbudujSekcjePraktykiWylaczeni(GroupBox panel, int startY)
        {
            // Usuwamy stare kontrolki sekcji praktyk/wyłączonych jeśli panel jest przebudowywany
            // (rozpoznajemy je po tagu "praktykiSection")
            var stare = panel.Controls.OfType<Control>()
                .Where(c => c.Tag is string t && t == "praktykiSection")
                .ToList();
            foreach (var c in stare) { panel.Controls.Remove(c); c.Dispose(); }

            int y = startY;

            // Helper: tworzy kontrolkę z tagiem żeby można ją było wyczyścić przy przebudowie
            void Dodaj(Control ctrl) { ctrl.Tag = "praktykiSection"; panel.Controls.Add(ctrl); }

            // Przycisk do ręcznego zastosowania listy klas na praktykach do już pobranych planów
            // Przydatne kiedy lista zmieniła się po ostatnim pobieraniu ze strony
            var btnZastosuj = new Button
            {
                Text = "Zastosuj praktyki do planów",
                Location = new Point(10, y),
                Width = 180,
                Height = 24,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Verdana", 7, FontStyle.Bold),
                BackColor = Color.LightSteelBlue
            };
            btnZastosuj.Click += (s, ev) =>
            {
                AktualizujPlanySPraktykami();
                MessageBox.Show("Zastosowano klasy na praktykach do wszystkich planów.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            Dodaj(btnZastosuj);
            y += 32;

            // ---- SEKCJA: Klasy na praktykach ----
            Dodaj(new Label
            {
                Text = "Klasy na praktykach:",
                Font = new Font("Verdana", 8, FontStyle.Bold),
                Location = new Point(10, y),
                AutoSize = true
            });
            y += 22;

            // Jeden wiersz na każdą dodaną klasę: [nazwa klasy] [✕ usuń]
            foreach (string klasa in klasyNaPraktykach.ToList())
            {
                string klasa_captured = klasa;

                Dodaj(new Label
                {
                    Text = klasa,
                    Location = new Point(10, y),
                    AutoSize = false,
                    Width = 80,
                    Height = 22,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Verdana", 8)
                });

                var btnUsun = new Button
                {
                    Text = "✕",
                    Location = new Point(94, y),
                    Size = new Size(26, 22),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Verdana", 7),
                    BackColor = Color.MistyRose
                };
                btnUsun.Click += (s, ev) =>
                {
                    klasyNaPraktykach.Remove(klasa_captured);
                    ZapiszKlasyPraktyki();
                    // Przebudowujemy całą listę radio buttonów żeby sekcja się odświeżyła
                    WczytajNauczycieli();
                    OdswiezWszystkieMinuty();
                };
                Dodaj(btnUsun);
                y += 26;
            }

            // Wiersz do dodawania nowej klasy: [textbox] [Dodaj]
            var tbNowaKlasa = new TextBox
            {
                Location = new Point(10, y),
                Width = 80,
                Font = new Font("Verdana", 8)
            };
            var btnDodajKlase = new Button
            {
                Text = "Dodaj",
                Location = new Point(94, y),
                Size = new Size(50, 22),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Verdana", 7),
                BackColor = Color.Honeydew
            };
            btnDodajKlase.Click += (s, ev) =>
            {
                // Bierzemy tylko pierwszą część nazwy klasy (przed myślnikiem lub spacją) — obsługa grup
                // np. "3P-gr2" -> "3P", "2P" -> "2P"
                string wpis = tbNowaKlasa.Text.Trim();
                if (string.IsNullOrEmpty(wpis)) return;

                // Wycinamy przyrostek grupy jeśli jest (np. -gr1, -gr2, /1 itp.) — zostawiamy sam oddział
                string bazaKlasy = System.Text.RegularExpressions.Regex.Match(wpis, @"^[A-Za-z0-9]+").Value;
                if (string.IsNullOrEmpty(bazaKlasy)) bazaKlasy = wpis;

                if (!klasyNaPraktykach.Contains(bazaKlasy))
                {
                    klasyNaPraktykach.Add(bazaKlasy);
                    ZapiszKlasyPraktyki();
                }
                WczytajNauczycieli();
                OdswiezWszystkieMinuty();
            };
            // Enter też dodaje
            tbNowaKlasa.KeyDown += (s, ev) => { if (ev.KeyCode == Keys.Return) btnDodajKlase.PerformClick(); };
            Dodaj(tbNowaKlasa);
            Dodaj(btnDodajKlase);
            y += 30;

            // Separator
            Dodaj(new Label
            {
                //BorderStyle = BorderStyle.Fixed3D,
                Location = new Point(4, y),
                Width = panel.Width - 24,
                Height = 2
            });
            y += 10;

            // ---- SEKCJA: Wyłączeni nauczyciele ----
            Dodaj(new Label
            {
                Text = "Wyłączeni nauczyciele:",
                Font = new Font("Verdana", 8, FontStyle.Bold),
                Location = new Point(10, y),
                AutoSize = true
            });
            y += 22;

            foreach (string skrot in wylaczeniNauczyciele.ToList())
            {
                string skrot_captured = skrot;

                Dodaj(new Label
                {
                    Text = skrot,
                    Location = new Point(10, y),
                    AutoSize = false,
                    Width = 80,
                    Height = 22,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Verdana", 8)
                });

                var btnWlacz = new Button
                {
                    Text = "✕",
                    Location = new Point(94, y),
                    Size = new Size(26, 22),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Verdana", 7),
                    BackColor = Color.MistyRose
                };
                btnWlacz.Click += (s, ev) =>
                {
                    wylaczeniNauczyciele.Remove(skrot_captured);
                    ZapiszWylaczonych();
                    WczytajNauczycieli();
                    OdswiezWszystkieMinuty();
                };
                Dodaj(btnWlacz);
                y += 26;
            }

            // Combo do wyboru nauczyciela do wyłączenia
            var cbWylacz = new ComboBox
            {
                Location = new Point(10, y),
                Width = 110,
                Font = new Font("Verdana", 7),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbWylacz.Items.Add("-- wybierz --");
            foreach (string n in nauczyciele)
            {
                string skrot = ZnajdzSkrot(n);
                if (!wylaczeniNauczyciele.Contains(skrot))
                    cbWylacz.Items.Add(skrot);
            }
            cbWylacz.SelectedIndex = 0;

            var btnWylaczDodaj = new Button
            {
                Text = "Wyłącz",
                Location = new Point(126, y),
                Size = new Size(55, 22),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Verdana", 7),
                BackColor = Color.LightYellow
            };
            btnWylaczDodaj.Click += (s, ev) =>
            {
                if (cbWylacz.SelectedIndex <= 0) return;
                string skrot = cbWylacz.SelectedItem.ToString();
                if (!wylaczeniNauczyciele.Contains(skrot))
                {
                    wylaczeniNauczyciele.Add(skrot);
                    ZapiszWylaczonych();
                }
                WczytajNauczycieli();
                OdswiezWszystkieMinuty();
            };
            Dodaj(cbWylacz);
            Dodaj(btnWylaczDodaj);
        }

        // Bierze zawartość pliku planu (jako string) i zamienia lekcje z klasami na praktykach na puste
        // Działa na surowym stringu żeby można było go użyć też podczas pobierania ze strony
        private string UsunKlasyNaPraktykachZPlanu(string planContent)
        {
            if (klasyNaPraktykach.Count == 0) return planContent;

            var wynik = new StringBuilder();
            foreach (string raw in planContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None))
            {
                string l = raw;

                // Linie nagłówkowe dni zostawiamy bez zmian
                bool toDzien = false;
                foreach (string d in dniTygodnia)
                    if (l.Trim().StartsWith(d)) { toDzien = true; break; }
                if (toDzien || l.Trim() == "") { wynik.AppendLine(l); continue; }

                // Parsujemy linię lekcji: nr ; sala ; klasa ; przedmiot ;
                string[] pola = l.Split(';');
                if (pola.Length < 4) { wynik.AppendLine(l); continue; }

                string klasaPole = pola[2].Trim();

                // Wyciągamy bazę klasy z pola klasy (np. "3P-gr2" -> "3P", "2P" -> "2P")
                // Obsługujemy grupy — bierzemy tylko pierwszą sekwencję liter i cyfr
                string bazaKlasy = System.Text.RegularExpressions.Regex.Match(klasaPole, @"^[A-Za-z0-9]+").Value;

                // Jeśli klasa jest na liście praktyk — zamieniamy lekcję na pustą
                if (!string.IsNullOrEmpty(bazaKlasy) && klasyNaPraktykach.Contains(bazaKlasy))
                {
                    // Zostawiamy numer lekcji, reszta idzie na myślniki (brak lekcji)
                    wynik.AppendLine($"{pola[0].Trim()} ; - ; - ; - ;");
                    continue;
                }

                wynik.AppendLine(l);
            }

            return wynik.ToString();
        }

        // Aplikuje klasy na praktykach do już zapisanych planów wszystkich nauczycieli
        // Wywoływane kiedy lista klas na praktykach się zmienia po stronie UI (nie przy pobieraniu)
        private void AktualizujPlanySPraktykami()
        {
            if (!Directory.Exists(planyFolder)) return;

            foreach (string nauczyciel in nauczyciele)
            {
                string skrot = ZnajdzSkrot(nauczyciel);
                string planPath = Path.Combine(planyFolder, $"{skrot}.txt");
                if (!File.Exists(planPath)) continue;

                string oryginal = File.ReadAllText(planPath, Encoding.UTF8);
                string zmodyfikowany = UsunKlasyNaPraktykachZPlanu(oryginal);

                // Tylko zapisujemy jeśli coś się faktycznie zmieniło
                if (oryginal != zmodyfikowany)
                    File.WriteAllText(planPath, zmodyfikowany, Encoding.UTF8);
            }

            OdswiezWszystkieMinuty();
        }

    }
}

//Dostałam wylewu.
//WERSJA GOSI BO SIE POGUBIE WAHHHHHH DAJCIE MI DRUGI MONITOR