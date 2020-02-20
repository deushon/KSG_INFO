using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

namespace KSG_INFO
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            try
            {
                InitializeComponent();
            }
            catch(System.Exception ex)
            {
                bool allerr = false;
                try
                {
                    System.IO.File.AppendAllText("FormInitiER.txt", "Start_ERROR" + Environment.NewLine + "Start_MAIN_ERROR" + Environment.NewLine + System.DateTime.Now.ToLongDateString() + "//" + System.DateTime.Now.ToLongTimeString() + Environment.NewLine + "\\Name: " + ex.Source + Environment.NewLine + "\\Message: " + ex.Message + Environment.NewLine + "\\StakTr: " + ex.StackTrace + Environment.NewLine + "\\TrSize: " + ex.TargetSite + Environment.NewLine + "\\Data: " + ex.Data + Environment.NewLine + "END_MAIN_ERROR");
                }
                catch { MessageBox.Show("Обноружены ошибки инициализации формы и Ошибки записи лога!!!"); allerr = true; }
                try
                {
                    System.Exception DOPer = ex.InnerException;
                    while (DOPer != null)
                    {
                        System.IO.File.AppendAllText("FormInitiER.txt", Environment.NewLine + "Start_DOPERROR" + Environment.NewLine + "Start_MAIN_ERROR" + Environment.NewLine + System.DateTime.Now.ToLongDateString() + "//" + System.DateTime.Now.ToLongTimeString() + Environment.NewLine + "\\Name: " + DOPer.Source + Environment.NewLine + "\\Message: " + DOPer.Message + Environment.NewLine + "\\StakTr: " + DOPer.StackTrace + Environment.NewLine + "\\TrSize: " + DOPer.TargetSite + Environment.NewLine + "\\Data: " + DOPer.Data + Environment.NewLine + "END_DOP_ERROR");
                        DOPer = DOPer.InnerException;
                    }
                    System.IO.File.AppendAllText("FormInitiER.txt", Environment.NewLine + "Correct of INITI DOP ERROR" + Environment.NewLine + "END_ERROR");
                }
                catch { System.IO.File.AppendAllText("FormInitiER.txt", Environment.NewLine + "Promlem of INITI DOP ERROR" + Environment.NewLine + "END_ERROR"); }
                if(!allerr) MessageBox.Show("Обноружены ошибки инициализации формы! Подробнее в журнале.");
            }
        }
        const int NomerSborki = 18;
        //Коэфиценты для расчета.
        const double ksbazest = 22598.52;
        const double dsbazest = 12291.46;
        const double stkoef = 2.1;
        const double dsALkoef = 1.1;
        const double dsONkoef = 2.1;
        string typeKSG;
        string PachNotes = "Улучшена производительность, стабилизирована работы системы на старых ПК. Добавлен сбор логов об ошибках. Скорректированно машатбирование МКБ и Ном.";
        string curcat = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        Form2 loader = new Form2();
        private void FTPLOAD(string URL, string login, string password, string filename, string outpt, bool open)
        {
            WebClient FTPSERVER = new WebClient();
            FTPSERVER.Credentials = new NetworkCredential(login, password);
            try
            {
                byte[] newFileData = FTPSERVER.DownloadData("ftp://" + URL + filename);
                System.IO.File.WriteAllBytes(curcat + @"\" + outpt, newFileData);
                if (open)
                    System.Diagnostics.Process.Start(outpt);
            }
            catch (WebException e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                FTPLOAD("192.168.1.39:24/", "ftadmin", "QQwerty1284", "curentversion.txt", "curentversion.txt", false);
                int curver = Convert.ToInt32(System.IO.File.ReadAllText(curcat + @"\curentversion.txt"));
                if (curver != NomerSborki)
                {
                    MessageBox.Show("Найдено обновление, программа будет обновлена и перезапущенна.");
                    System.IO.Directory.CreateDirectory(curcat + @"\clientup");
                    FTPLOAD("192.168.1.39:24/", "ftadmin", "QQwerty1284", "KSG_INFO.exe", @"clientup\KSG_INFO.exe", false);
                    System.IO.File.WriteAllText(curcat + @"\samouper.bat", "timeout /t 5 " + Environment.NewLine + @"MOVE /Y clientup\KSG_INFO.exe KSG_INFO.exe" + Environment.NewLine + "start KSG_INFO.exe");
                    System.Diagnostics.Process.Start(curcat + @"\samouper.bat");
                    System.Environment.Exit(0);
                }
                else
                {
                    if (System.IO.File.Exists(curcat + @"\PN.DAT"))
                    {
                        if (System.IO.File.ReadAllText(curcat + @"\PN.DAT") != NomerSborki.ToString())
                        {
                            System.IO.File.WriteAllText(curcat + @"\PN.DAT", NomerSborki.ToString());
                            MessageBox.Show(PachNotes, "Подробности обновления");
                        }
                    }
                    else
                    {
                        System.IO.File.WriteAllText(curcat + @"\PN.DAT", NomerSborki.ToString());
                        MessageBox.Show(PachNotes, "Подробности обновления");
                    }

                    loader.Show();
                    // TODO: данная строка кода позволяет загрузить данные в таблицу "kSG_INFODataSet1.NOM_INFO". При необходимости она может быть перемещена или удалена.
                    this.nOM_INFOTableAdapter.Fill(this.kSG_INFODataSet1.NOM_INFO);
                    // TODO: данная строка кода позволяет загрузить данные в таблицу "kSG_INFODataSet1.MKB_INFO". При необходимости она может быть перемещена или удалена.
                    this.mKB_INFOTableAdapter.Fill(this.kSG_INFODataSet1.MKB_INFO);


                    // TODO: данная строка кода позволяет загрузить данные в таблицу "kSG_INFODataSet1.MKB_INFO". При необходимости она может быть перемещена или удалена.
                    //this.mKB_INFOTableAdapter.Fill(this.kSG_INFODataSet.MKB_INFO);
                    // TODO: данная строка кода позволяет загрузить данные в таблицу "kSG_INFODataSet.NOM_INFO". При необходимости она может быть перемещена или удалена.
                    //this.nOM_INFOTableAdapter.Fill(this.kSG_INFODataSet.NOM_INFO);
                    //Проверка обновлений.

                    KSG_dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    KSG_dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                    MKBdataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    MKBdataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                    NOMdataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    NOMdataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    //KSG_dataGridView.colum
                    //radioButton2.Checked = true;




                    this.bIG_GRUPPERTableAdapter.Fill(this.kSG_INFODataSet.BIG_GRUPPER);


                    SearchNom.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    SearchNom.AutoCompleteSource = AutoCompleteSource.ListItems;

                    SearchMKB.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    SearchMKB.AutoCompleteSource = AutoCompleteSource.ListItems;
                    SearchMKB.Text = "";
                    SearchNom.Text = "";
                    SearchMKB.Focus();
                    loader.Hide();
                }
            }
            catch (System.Exception ex)
            {
                bool allerr = false;
                try
                {
                    System.IO.File.AppendAllText("ProgrammInitiER.txt", "Start_ERROR" + Environment.NewLine + "Start_MAIN_ERROR" + Environment.NewLine + System.DateTime.Now.ToLongDateString() + "//" + System.DateTime.Now.ToLongTimeString() + Environment.NewLine + "\\Name: " + ex.Source + Environment.NewLine + "\\Message: " + ex.Message + Environment.NewLine + "\\StakTr: " + ex.StackTrace + Environment.NewLine + "\\TrSize: " + ex.TargetSite + Environment.NewLine + "\\Data: " + ex.Data + Environment.NewLine + "END_MAIN_ERROR");
                }
                catch { MessageBox.Show("Обноружены ошибки загрузки программы и Ошибки записи лога!!!"); allerr = true; }
                try
                {
                    System.Exception DOPer = ex.InnerException;
                    while (DOPer != null)
                    {
                        System.IO.File.AppendAllText("ProgrammInitiER.txt", Environment.NewLine + "Start_DOPERROR" + Environment.NewLine + "Start_MAIN_ERROR" + Environment.NewLine + System.DateTime.Now.ToLongDateString() + "//" + System.DateTime.Now.ToLongTimeString() + Environment.NewLine + "\\Name: " + DOPer.Source + Environment.NewLine + "\\Message: " + DOPer.Message + Environment.NewLine + "\\StakTr: " + DOPer.StackTrace + Environment.NewLine + "\\TrSize: " + DOPer.TargetSite + Environment.NewLine + "\\Data: " + DOPer.Data + Environment.NewLine + "END_DOP_ERROR");
                        DOPer = DOPer.InnerException;
                    }
                    System.IO.File.AppendAllText("ProgrammInitiER.txt", Environment.NewLine + "Correct of INITI DOP ERROR" + Environment.NewLine + "END_ERROR");
                }
                catch { System.IO.File.AppendAllText("ProgrammInitiER.txt.txt", Environment.NewLine + "Promlem of INITI DOP ERROR" + Environment.NewLine + "END_ERROR"); }
                if (!allerr) MessageBox.Show("Обноружены ошибки загрузки программыы! Подробнее в журнале.");
            }
        }

    
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                SearchMKB.Text = "";
                SearchNom.Text = "";

                typeKSG = "st";

                loader.Show();
                this.kSGTableAdapter.Fill(this.kSG_INFODataSet.KSG, typeKSG);
                loader.Hide();

            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                SearchMKB.Text = "";
                SearchNom.Text = "";

                typeKSG = "ds";

                loader.Show();

                this.kSGTableAdapter.Fill(this.kSG_INFODataSet.KSG, typeKSG);
                loader.Hide();
            }
        }

        private void SMKBBUTON_Click(object sender, EventArgs e)
        {
            try
            {
                loader.Show();
                SearchNom.Text = "";
                this.kSGTableAdapter.FillByMKB(this.kSG_INFODataSet.KSG, SearchMKB.Text,typeKSG);
                fKBIGGRUPPERKSGBindingSource.Filter = "MKB_CODE LIKE '%" + SearchMKB.Text + "%'" ;

            }
            catch
            {
                //kSG_INFODataSet.er
            }
                loader.Hide();
        }

        private void SNBUTON_Click(object sender, EventArgs e)
        {
            try
            {
                loader.Show();

                SearchMKB.Text = "";
                this.kSGTableAdapter.FillByNOM(this.kSG_INFODataSet.KSG, SearchNom.Text, typeKSG);
                //fKBIGGRUPPERKSGBindingSource.Filter = "NOM_CODE LIKE '%" + SearchNom.Text + "%'";
            }
            catch
            {

            }
                loader.Hide();
        }

        private void SALLBUTTON_Click(object sender, EventArgs e)
        {
            loader.Show();
            SearchMKB.Text = "";
            SearchNom.Text = "";
            this.kSGTableAdapter.Fill(this.kSG_INFODataSet.KSG, typeKSG);
            loader.Hide();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            tabControl1.SelectedTab = LOOK;
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //Определение подходящих коэфицентов.
            double bazest=0;
            if (typeKSG == "st") bazest = ksbazest;
            else if (typeKSG == "ds") bazest = dsbazest;
            double koef = 0;
            if (typeKSG == "st") koef = stkoef;
            else if (typeKSG == "ds")
            {
                if (KSG_dataGridView[4, KSG_dataGridView.CurrentRow.Index].Value.ToString() == "Онкология")
                    koef = dsONkoef;
                else
                koef = dsALkoef;
            }

            try { KSGlabel.Text = "KSG: " + KSG_dataGridView[0, KSG_dataGridView.CurrentRow.Index].Value + " Стоимость = " + Convert.ToString(Math.Round(Convert.ToDouble(KSG_dataGridView[2, KSG_dataGridView.CurrentRow.Index].Value) * bazest * koef,2))+ " ₽ " + KSG_dataGridView[1, KSG_dataGridView.CurrentRow.Index].Value; }
            catch { }

            if (SearchMKB.Text != "")
                fKBIGGRUPPERKSGBindingSource.Filter = "MKB_CODE LIKE '%" + SearchMKB.Text + "%'";
            else
               fKBIGGRUPPERKSGBindingSource.Filter = "";
            //else if (SearchNom.Text != "")
            //fKBIGGRUPPERKSGBindingSource.Filter = "NOM_CODE LIKE '%" + SearchNom.Text + "%'";
            //Сокрытие таблицы номеклатуры если она пуста
            bool isnom = false;
            try
            {
                foreach (DataGridViewRow ok in NOMdataGridView.Rows)
                {

                    if (ok.Cells[1].Value.ToString() != "") isnom = true;
                }
            }
            catch { }
            if (!isnom)
            {
                label4.Visible = false;
                NOMdataGridView.Visible = false;
                tableLayoutPanel2.RowStyles[3].SizeType = 0;
            }
            else
            {
                label4.Visible = true;
                NOMdataGridView.Visible = true;
                tableLayoutPanel2.RowStyles[3].SizeType = SizeType.Percent;
            }
            //Сокрытие таблицы мкб если она пуста
            bool ismkb = false;
            try
            {
                foreach (DataGridViewRow ok in MKBdataGridView.Rows)
                {

                    if (ok.Cells[1].Value.ToString() != "") ismkb = true;
                }
            }
            catch { }
            if (!ismkb)
            {
                label3.Visible = false;
                MKBdataGridView.Visible = false;
                tableLayoutPanel2.RowStyles[1].SizeType = 0;
            }
            else
            {
                label3.Visible = true;
                MKBdataGridView.Visible = true;
                tableLayoutPanel2.RowStyles[1].SizeType = SizeType.Percent;
            }

        }


        private void SearchNom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SNBUTTON.PerformClick();
        }

        private void SearchMKB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SMKBBUTTON.PerformClick();
        }


        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            int sizer = this.Size.Height + this.Size.Width;
            if (sizer > 1398)
            {
                KSG_dataGridView.Font = new System.Drawing.Font("Tahoma", 12 + ((sizer - 1398) / 300), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            }
            else
            {
                KSG_dataGridView.Font = new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            }
            NOMdataGridView.Font = KSG_dataGridView.Font;
            MKBdataGridView.Font = KSG_dataGridView.Font;
            MKBdataGridView.ColumnHeadersHeight = Convert.ToInt32(KSG_dataGridView.Font.Size*2+8);
            NOMdataGridView.ColumnHeadersHeight = Convert.ToInt32(KSG_dataGridView.Font.Size*2+8);
        }
    }
}
