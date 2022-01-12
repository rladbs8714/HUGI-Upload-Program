using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.IO;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using TextBox = System.Windows.Forms.TextBox;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace Management
{

    public partial class SetInputForm : Form
    {
        string[] dateDivider = new string[] { "/", ".", "-", ".", " " };
        Random random = new Random();
        ShortKeySetMessegeBox shortKeySetMessegeBox;

        iniUtil ini;
        Thread thT;

        List<string> lstShop = new List<string>();
        List<string> lstManager = new List<string>();
        List<string> lstReview = new List<string>();

        public SetInputForm()
        {
            InitializeComponent();

            TextBox[] valueTextBoxes = { DateTextBox, SectorNameTextBox, BusinessNameTextBox, StateTextBox, HumanNameTextBox,
                                        OtherTextBox1, OtherTextBox2, StateAndBNameTextBox, GeneralReviewTextBox, ExperiencesTextBox};

            Dictionary<string, string> read = FileManager.LoadFile(Paths.valueInputTextPath);

            TitleTextBox.Text = GetValueByKey(TitleKey, read);
            DateTextBox.Text = read[DateKey.Text];
            SectorNameTextBox.Text = read[SectorNameKey.Text];
            BusinessNameTextBox.Text = read[BusinessNameKey.Text];
            StateTextBox.Text = read[StateKey.Text];
            HumanNameTextBox.Text = read[HumanNameKey.Text];
            OtherTextBox1.Text = read[Other1Key.Text];
            OtherTextBox2.Text = read[Other2Key.Text];
            StateAndBNameTextBox.Text = read[StateAndBNameKey.Text];
            GeneralReviewTextBox.Text = read[GeneralReviewKey.Text];
            ExperiencesTextBox.Text = read[ExperiencesKey.Text];

            DateTextBox.Text = DateTime.Now.Month + dateDivider[random.Next(0, dateDivider.Length)] + (DateTime.Now.Day - 1).ToString();

            if (!new FileInfo(Paths.shortKeyListPath + "\\code.txt").Exists)
            {
                new FileInfo(Paths.shortKeyListPath + "\\code.txt").Create();
            }
            else
            {
                CodeTextBox.Text = File.ReadAllText(Paths.shortKeyListPath + "\\code.txt");
            }

            FileInfo exefileinfo = new FileInfo(System.Windows.Forms.Application.ExecutablePath);

            string path = exefileinfo.Directory.FullName.ToString();
            string fileName = @"\config.ini";
            string filePath = path + fileName;
            ini = new iniUtil(filePath);
            FileStream fs;
            if (!System.IO.File.Exists(filePath))
            {
                fs = System.IO.File.Create(filePath);
                fs.Close();
            }

            System.Windows.Forms.Application.Idle += chkThread;

            //Thread waitKeyInput = new Thread(WaitKeyInput);
            //waitKeyInput.SetApartmentState(ApartmentState.STA);
            //CheckForIllegalCrossThreadCalls = false;
            //waitKeyInput.Start();
        }

        private string GetValueByKey(Control key, Dictionary<string, string> read)
        {
            if (read.ContainsKey(key.Text))
            {
                return read[key.Text];
            }
            else
            {
                return "";
            }
        }

        private void FromClosingEvent(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void StateAndBNameChangedEvent(object sender, EventArgs e)
        {
            StateAndBNameTextBox.Text = StateTextBox.Text + " / " + BusinessNameTextBox.Text;
            SaveButton_Click(sender, e);
        }

        private void EnterTextBox(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void SaveAndExit_Click(object sender, EventArgs e)
        {
            SaveButton_Click(sender, e);

            Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            #region '#' 검출
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTextBox.Text);
            sb.Append(SectorNameTextBox.Text);
            sb.Append(BusinessNameTextBox.Text);
            sb.Append(StateTextBox.Text);
            sb.Append(HumanNameTextBox.Text);
            sb.Append(OtherTextBox1.Text);
            sb.Append(OtherTextBox2.Text);
            sb.Append(StateAndBNameTextBox.Text);
            sb.Append(GeneralReviewTextBox.Text);
            sb.Append(VisitTimeTextBox.Text);
            sb.Append(ExperiencesTextBox.Text);
            foreach (char ch in sb.ToString())
            {
                if (ch == '#')
                {
                    SaveAndExitButton.Text = "어딘가에 #이 있습니다!";
                    return;
                }
            }
            #endregion

            Dictionary<string, string> write = new Dictionary<string, string>();

            write.Add(TitleKey.Text, TitleTextBox.Text);
            write.Add(DateKey.Text, DateTextBox.Text);
            write.Add(SectorNameKey.Text, SectorNameTextBox.Text);
            write.Add(BusinessNameKey.Text, BusinessNameTextBox.Text);
            write.Add(StateKey.Text, StateTextBox.Text);
            write.Add(HumanNameKey.Text, HumanNameTextBox.Text);
            write.Add(Other1Key.Text, OtherTextBox1.Text);
            write.Add(Other2Key.Text, OtherTextBox2.Text);
            write.Add(StateAndBNameKey.Text, StateAndBNameTextBox.Text);
            write.Add(GeneralReviewKey.Text, GeneralReviewTextBox.Text);
            write.Add(VisitTimeKey.Text, VisitTimeTextBox.Text);
            write.Add(ExperiencesKey.Text, ExperiencesTextBox.Text);

            FileManager.SaveFile(write, Paths.valueInputTextPath);

            //DateTextBox.Text = DateTime.Now.Month + dateDivider[random.Next(0, dateDivider.Length)] + (DateTime.Now.Day - 1).ToString();
            string date = "";
            switch(random.Next(0, 7))
            {
                case 0:
                    date = string.Format("{0:D2}/{1:D2}", DateTime.Now.Month, DateTime.Now.Day - 1);
                    break;
                case 1:
                    date = string.Format("{0:D2}.{1:D2}", DateTime.Now.Month, DateTime.Now.Day - 1);
                    break;
                case 2:
                    date = string.Format("{0:D2}-{1:D2}", DateTime.Now.Month, DateTime.Now.Day - 1);
                    break;
                case 3:
                    date = string.Format("{0:D2}월{1:D2}일", DateTime.Now.Month, DateTime.Now.Day - 1);
                    break;
                case 4:
                    date = string.Format("{0:D2}{1:D2}", DateTime.Now.Month, DateTime.Now.Day - 1);
                    break;
                case 5:
                    date = string.Format("{0:D2} {1:D2}", DateTime.Now.Month, DateTime.Now.Day - 1);
                    break;
                case 6:
                    date = "어제";
                    break;
                default:
                    DateTextBox.Text = "-";
                    break;
            }
            DateTextBox.Text = date;
            File.WriteAllText(Paths.shortKeyListPath + "\\code.txt", CodeTextBox.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            shortKeySetMessegeBox = new ShortKeySetMessegeBox("1");
            shortKeySetMessegeBox.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            OtherTextBox1.Text = lstShop.Random<string>().Trim().ToString();
            OtherTextBox2.Text = lstManager.Random<string>().Trim().ToString();
            GeneralReviewTextBox.Text = lstReview.Random<string>().Trim().ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void SetInputForm_Load(object sender, EventArgs e)
        {
            lbMSG.Visible = true;
            lbMSG.Text = "데이터 로딩 중입니다..";
            btSelectFile.Enabled = false;

            thT = new Thread(excelTodb);
            thT.Start();

        }
        private void chkThread(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Idle -= chkThread;
            while (thT.IsAlive) { }

            lbMSG.Visible = false;
            btSelectFile.Enabled = true;
        }
        private void btSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Excel files (*.xls,*xlsx)|*.xls;*xlsx|All files (*.*)|*.*";
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strFile = openFileDialog1.FileName;
                ini.SetIniValue("PATH", "EXCEL", strFile);
            }
            this.Close();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            MessageBox.Show("F1");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MessageBox.Show("F2");
        }
        private void DeleteObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                GC.Collect();
            }
        }

        private void excelTodb(object obj)
        {
            EventWaitHandle ewh = obj as EventWaitHandle;

            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;
            Excel.Application ap = null;
            try
            {
                string strFile = ini.GetIniValue("PATH", "EXCEL");
                FileInfo fe = null;
                if (strFile != null && strFile != string.Empty)
                    fe = new FileInfo(strFile);

                if (fe == null || !fe.Exists || strFile == null && strFile == string.Empty)
                {
                    MessageBox.Show("처음 한번은 엑셀파일을 선택하셔야 합니다.");
                    return;
                }

                if (strFile != null && strFile != string.Empty)
                {
                    ap = new Excel.Application();
                    wb = ap.Workbooks.Open(strFile);
                    ws = wb.Sheets[1];
                    ap.Visible = false;
                    Range range = ws.UsedRange;
                    String data = "";
                    for (int i = 1; i <= range.Rows.Count; ++i)
                    {
                        for (int j = 1; j < range.Columns.Count; ++j)
                        {
                            if (range.Cells[i, j] != null && range.Cells[i, j].Value2 != null)
                            {
                                if (i == 1) continue;
                                switch (j)
                                {
                                    case 1:
                                        lstShop.Add(((range.Cells[i, j] as Range).Value2.ToString()));
                                        break;
                                    case 2:
                                        lstManager.Add(((range.Cells[i, j] as Range).Value2.ToString()));
                                        break;
                                    case 3:
                                        lstReview.Add(((range.Cells[i, j] as Range).Value2.ToString()));
                                        break;
                                }
                            }

                        }
                    }
                    DeleteObject(range);
                    DeleteObject(ws);
                    DeleteObject(wb);
                    ap.Quit();
                    DeleteObject(ap);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys key = keyData & ~(Keys.Shift | Keys.Control);
            switch (key)
            {
                case Keys.F1:
                    button11_Click(null, null);
                    break;
                case Keys.F2:
                    button12_Click(null, null);
                    break;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }

        
        private void WaitKeyInput()
        {

        }

        /// <summary>
        /// 딜레이 함수. UI를 제외한 프로그램 진행이 멈춘다.
        /// </summary>
        /// <param name="ms"></param>
        private void Delay(int ms)
        {
            DateTime datetimeNow = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, ms);
            DateTime datetimeAdd = datetimeNow.Add(duration);
            while (datetimeAdd >= datetimeNow)
            {
                System.Windows.Forms.Application.DoEvents();
                datetimeNow = DateTime.Now;
            }
        }

        private void InitialSetting_TextChanged(object sender, EventArgs e)
        {
            string initialString = ((TextBox)sender).Text.Replace(" ", "");
            ((TextBox)sender).Text = initialString;

            // 기존 초기 설정 코드
            if(!(initialString[1] >= 65 && initialString[1] <= 90))
            {
                if (initialString[0] != '[' || initialString[initialString.Length - 1] != ']')
                {
                    SectorNameTextBox.Text = "";
                    BusinessNameTextBox.Text = "";
                    StateTextBox.Text = "";
                    HumanNameTextBox.Text = "";
                    return;
                }

                string _first = initialString.Substring(1, initialString.IndexOf(']') - 1);
                initialString = initialString.Remove(0, initialString.IndexOf(']') + 1);
                string _humanName = initialString.Substring(1, initialString.IndexOf(']') - 1);
                initialString = initialString.Remove(0, initialString.IndexOf(']') + 1);
                string _sectorName = initialString.Substring(1, initialString.IndexOf(']') - 1);

                StateTextBox.Text = _first.Substring(0, _first.IndexOf('-'));
                BusinessNameTextBox.Text = _first.Remove(0, _first.IndexOf('-') + 1);
                HumanNameTextBox.Text = _humanName;
                SectorNameTextBox.Text = _sectorName;

                return;
            }

            // 새로운 초기 설정 코드 - 홈페이지에 있는 타이틀 전부를 끌어와도 된다.
            CodeTextBox.Text = initialString.Substring(0, initialString.IndexOf('(', 1));
            initialString = initialString.Remove(0, initialString.IndexOf('[', 1));

            StringBuilder sb = new StringBuilder();
            #region 중간 업소정보 찾기
            int closeCount = 0;
            foreach(char ch in initialString)
            {
                sb.Append(ch);
                if(ch == ']')
                {
                    if(closeCount == 2)
                    {
                        break;
                    }
                    closeCount++;
                }
            }
            #endregion
            string buInfo = sb.ToString();
            #region 중간 업소정보로 값 가져오기
            if (buInfo[0] != '[' || buInfo[buInfo.Length - 1] != ']')
            {
                SectorNameTextBox.Text = "";
                BusinessNameTextBox.Text = "";
                StateTextBox.Text = "";
                HumanNameTextBox.Text = "";
                return;
            }

            string first = buInfo.Substring(1, buInfo.IndexOf(']') - 1);
            buInfo = buInfo.Remove(0, buInfo.IndexOf(']') + 1);
            string humanName = buInfo.Substring(1, buInfo.IndexOf(']') - 1);
            buInfo = buInfo.Remove(0, buInfo.IndexOf(']') + 1);
            string sectorName = buInfo.Substring(1, buInfo.IndexOf(']') - 1);

            StateTextBox.Text = first.Substring(0, first.IndexOf('-'));
            BusinessNameTextBox.Text = first.Remove(0, first.IndexOf('-') + 1);
            HumanNameTextBox.Text = humanName;
            SectorNameTextBox.Text = sectorName;
            #endregion
            initialString = initialString.Remove(0, sb.ToString().Length);
            TitleTextBox.Text = initialString;

            // save
            SaveButton_Click(sender, e);
        }
    }

    public static class RandomSelector
    {
        static Random random = new Random();
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            int c = enumerable.Count();
            int i = random.Next(c);
            return enumerable.Skip(i).First();
        }
    }
}
