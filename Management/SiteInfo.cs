using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Management
{
    public partial class SiteInfo : Form
    {
        int index;
        string path;
        Main form1;

        public SiteInfo(string _path, int _index, string name, Main _form1)
        {
            InitializeComponent();
            path = _path;
            index = _index;
            form1 = _form1;

            Dictionary<string, string> read = FileManager.LoadFile(Paths.siteFolderPath + "\\" + name + ".txt");
            if (read.Any())
            {
                #region Set site name, line spacing, titleUnderValueCheck
                FileNameTextBox.Text = read[FileNameKey.Text];
                LineSpacingTextBox.Text = read[LineSpacingKey.Text];
                TapCountTextBox.Text = GetValueByKey(TapCountKey, read);
                TitleUnderValueCheckBox.Checked = Convert.ToBoolean(read[TitleUnderValueCheckBox.Name]);
                #endregion

                #region Set title font
                string titleFontValue = read[TitleFontKey.Name];
                List<string> titleFontElements = new List<string>();
                StringBuilder titleSB = new StringBuilder();
                foreach (char ch in titleFontValue)
                {
                    if (ch == '|')
                    {
                        titleFontElements.Add(titleSB.ToString());
                        titleSB.Clear();
                    }
                    else
                    {
                        titleSB.Append(ch);
                    }
                }
                titleFontElements.Add(titleSB.ToString());
                TitleFontComboBox.Text = titleFontElements[0];
                TitleFontSizeTextBox.Text = titleFontElements[1];
                TitleFontStyleComboBox.Text = titleFontElements[2];
                TitleFontColorTextBox.Text = titleFontElements[3];
                #endregion

                #region Set contents font
                string contentsFontValue = read[ContentsFontKey.Name];
                List<string> contentsFontElements = new List<string>();
                StringBuilder contentsSB = new StringBuilder();
                foreach (char ch in contentsFontValue)
                {
                    if (ch == '|')
                    {
                        contentsFontElements.Add(contentsSB.ToString());
                        contentsSB.Clear();
                    }
                    else
                    {
                        contentsSB.Append(ch);
                    }
                }
                contentsFontElements.Add(contentsSB.ToString());
                ContentsFontComboBox.Text = contentsFontElements[0];
                ContentsFontSizeTextBox.Text = contentsFontElements[1];
                ContentsFontStyleComboBox.Text = contentsFontElements[2];
                ContentsFontColorTextBox.Text = contentsFontElements[3];
                #endregion

                #region Set contents

                #region Set Keys
                List<ComboBox> keysList = new List<ComboBox>() { KeyBox1, KeyBox2, KeyBox3, KeyBox4, KeyBox5, KeyBox6, KeyBox7, KeyBox8, KeyBox9, KeyBox10 };
                foreach (ComboBox cb in keysList)
                {
                    try
                    {
                        cb.Text = read[cb.Name];
                    }
                    catch
                    {}
                }
                #endregion
                #region Set Values
                List<TextBox> valuesList = new List<TextBox>() { TitleBox1, TitleBox2, TitleBox3, TitleBox4, TitleBox5, TitleBox6, TitleBox7, TitleBox8, TitleBox9, TitleBox10 };
                for (int i = 0; i < valuesList.Count; ++i)
                {
                    try
                    {
                        valuesList[i].Text = read[keysList[i].Text];    // #region Set Keys에 있는 KeysList
                    }
                    catch
                    {

                    }
                }
                #endregion

                #endregion

                #region 선후행 설정
                if (new FileInfo(Paths.FirstStringPath + "\\" + FileNameTextBox.Text + ".txt").Exists)
                {
                    FirstStringTextBox.Text = File.ReadAllText(Paths.FirstStringPath + "\\" + FileNameTextBox.Text + ".txt");
                }

                if (new FileInfo(Paths.EndStringPath + "\\" + FileNameTextBox.Text + ".txt").Exists)
                {
                    EndStringTextBox.Text = File.ReadAllText(Paths.EndStringPath + "\\" + FileNameTextBox.Text + ".txt");
                }
                #endregion
            }
        }

        private string GetValueByKey(Control key, Dictionary<string, string> read)
        {
            if(read.ContainsKey(key.Text))
            {
                return read[key.Text];
            }
            else
            {
                return "";
            }
        }

        private void SaveFile(string newName)
        {
            string oldPath = Paths.siteFolderPath + "\\" + SiteManager.fileInfoList[index].Name;
            string newPath = Paths.siteFolderPath + "\\" + newName + ".txt";
            File.Move(oldPath, newPath);

            Close();
        }

        private bool TryWriteByKeyValue(ref Dictionary<string, string> write, string key, string value)
        {
            try
            {
                if(key != "" && value != "")
                {
                    write.Add(key, value);
                }
            }
            catch(ArgumentNullException e)
            {
                // 키가 Null일 경우의 Exception
                // 지금은 무조건 공백이라도 들어오기에 이 Exception이 발생한다면 무조건 오류임
                // Dictionary에 Null값이 들어가는지 확인할 것
                Console.WriteLine("SiteInfo.cs의 WriteByKeyValue()함수 내 주석을 참고하세요.");
            }
            catch(ArgumentException e)
            {
                // 리스트 내에 이미 동일한 키 값이 있을 경우 발생하는 Exception
                return true;
            }

            return false;
        }

        /// <summary>
        /// 저장하고 닫기 버튼 이벤트
        /// </summary>
        private void SaveAndClose_Click(object sender, EventArgs e)
        {
            #region new write code
            bool tryWriteCheck = false;
            Dictionary<string, string> write = new Dictionary<string, string>();
            tryWriteCheck = TryWriteByKeyValue(ref write, FileNameKey.Text, FileNameTextBox.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, LineSpacingKey.Text, LineSpacingTextBox.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, TapCountKey.Text, TapCountTextBox.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, TitleFontKey.Name, TitleFontComboBox.Text + "|" + TitleFontSizeTextBox.Text + "|" + TitleFontStyleComboBox.Text + "|" + TitleFontColorTextBox.Text);
            #region key : keybox.name, value : keybox.Text
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox1.Name, KeyBox1.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox2.Name, KeyBox2.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox3.Name, KeyBox3.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox4.Name, KeyBox4.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox5.Name, KeyBox5.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox6.Name, KeyBox6.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox7.Name, KeyBox7.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox8.Name, KeyBox8.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox9.Name, KeyBox9.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox10.Name, KeyBox10.Text);
            #endregion
            #region key : keybox.text, value : titlebox.text
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox1.Text, TitleBox1.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox2.Text, TitleBox2.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox3.Text, TitleBox3.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox4.Text, TitleBox4.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox5.Text, TitleBox5.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox6.Text, TitleBox6.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox7.Text, TitleBox7.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox8.Text, TitleBox8.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox9.Text, TitleBox9.Text);
            tryWriteCheck = TryWriteByKeyValue(ref write, KeyBox10.Text, TitleBox10.Text);
            #endregion
            tryWriteCheck = TryWriteByKeyValue(ref write, TitleUnderValueCheckBox.Name, TitleUnderValueCheckBox.Checked.ToString());
            tryWriteCheck = TryWriteByKeyValue(ref write, ContentsFontKey.Name, ContentsFontComboBox.Text + "|" + ContentsFontSizeTextBox.Text + "|" + ContentsFontStyleComboBox.Text + "|" + ContentsFontColorTextBox.Text);

            if(tryWriteCheck)
            {
                MessageBox.Show("키 값이 중복됩니다.\n키 값이 될 항목을 체크하세요.", "키 값 중복", MessageBoxButtons.OK);
                return;
            }

            FileManager.SaveFile(write, Paths.siteFolderPath + "\\" + FileNameTextBox.Text + ".txt");
            #endregion

            #region 선/후행 문장
            if (FirstStringTextBox.Text.Any())
            {
                File.WriteAllText(Paths.FirstStringPath + "\\" + FileNameTextBox.Text + ".txt", FirstStringTextBox.Text);
            }

            if(EndStringTextBox.Text.Any())
            {
                File.WriteAllText(Paths.EndStringPath + "\\" + FileNameTextBox.Text + ".txt", EndStringTextBox.Text);
            }
            #endregion

            form1.Refrash();

            Close();
        }

        /// <summary>
        /// 창이 닫히기 전 발생하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SiteInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// 기본 문장 텍스트 박스를 더블클릭 하면 발생하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetClipBoardHtmlFormat(object sender, MouseEventArgs e)
        {
            string html = Clipboard.GetText(TextDataFormat.Html);

            if (html.Length > 1)
            {
                html = html.Substring(html.IndexOf("<!--StartFragment-->") + 20);
                html = html.Remove(html.IndexOf("<!--EndFragment-->"));

                ((Control)sender).Text = html;

                if(((Control)sender).Name == "FirstStringTextBox")
                {
                    File.WriteAllText(Paths.FirstStringPath + "\\" + FileNameTextBox.Text + ".txt", html);
                }
                else if(((Control)sender).Name == "EndStringTextBox")
                {
                    File.WriteAllText(Paths.EndStringPath + "\\" + FileNameTextBox.Text + ".txt", html);
                }
            }
        }

        /// <summary>
        /// 후기 폰트 타이틀과 같게 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SameTitleValueFont_Click(object sender, EventArgs e)
        {
            ContentsFontComboBox.Text = TitleFontComboBox.Text;
            ContentsFontSizeTextBox.Text = TitleFontSizeTextBox.Text;
            ContentsFontStyleComboBox.Text = TitleFontStyleComboBox.Text;
            ContentsFontColorTextBox.Text = TitleFontColorTextBox.Text;
        }

        /// <summary>
        /// 텍스트박스에 커서가 가면 내용이 전체 선택 됨.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterTextBox(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
    }
}
