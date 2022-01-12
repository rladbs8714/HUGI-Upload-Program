using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

using RtfPipe;

using Font = System.Drawing.Font;

namespace Management
{
    public partial class Main : Form
    {
        public static List<Button> buttons = new List<Button>();
        SetInputForm setInputForm;

        #region 윈도우 핸들
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        private const int SW_WINDOW_NORMAL = 1;
        private const int SW_WINDOW_MINIMIZED = 2;
        private const int SW_WINDOW_MAXIMIZED = 3;
        #endregion


        public Main()
        {
            InitializeComponent();

            Refrash();
        }

        /// <summary>
        /// 버튼을 생성하는 함수
        /// 아래의 버튼 만들기 이벤트와는 별개
        /// </summary>
        private void CreateButtons_Auto()
        {
            int startIndex = buttons.Count;
            for (int i = startIndex; i < SiteManager.siteCount; i++)
            {
                Button button = new Button
                {
                    Name = i.ToString(),
                    Parent = this,
                    Size = new Size(130, 65)
                };
                button.Click += new EventHandler(SiteButtonClick);
                button.MouseDown += new MouseEventHandler(SiteButton_MouseRightClick);
                buttons.Add(button);
                flowLayoutPanel1.Controls.Add(button);
            }
        }

        /// <summary>
        /// 사이트 리스트와 사이트 별 버튼들의 상태를 새로고침 하는 함수
        /// </summary>
        public void Refrash()
        {
            SiteManager.Refrash();

            CreateButtons_Auto();

            Console.WriteLine("---------------");

            foreach (Button button in buttons)
            {
                int index = int.Parse(button.Name);
                string name = SiteManager.fileInfoList[index].Name;
                name = name.Remove(name.Length - 4);
                button.Text = name;
            }
        }

        /// <summary>
        /// 폰트의 정보가 담겨있는 문자열을 받아 가공하여 폰트를 리턴하는 함수
        /// </summary>
        /// <param name="fontElementString"></param>
        /// <returns></returns>
        private Font CreateFontByString(string fontElementString, out Color color)
        {
            List<string> fontElements = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (char ch in fontElementString)
            {
                if (ch == '|')
                {
                    fontElements.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(ch);
                }
            }
            fontElements.Add(sb.ToString());

            FontStyle fs;
            switch (fontElements[2])
            {
                case "굵게":
                    fs = FontStyle.Bold;
                    break;
                case "밑줄":
                    fs = FontStyle.Underline;
                    break;
                case "기울임꼴":
                    fs = FontStyle.Italic;
                    break;
                case "취소선":
                    fs = FontStyle.Strikeout;
                    break;

                default:
                    fs = FontStyle.Regular;
                    break;
            }
            
            color = CreateColorByString(fontElements[3]);

            return new Font(fontElements[0], Convert.ToInt32(fontElements[1]), fs, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// 색깔 RGB 값이 담겨있는 문자열을 받아 가공하여 Color를 리턴하는 함수
        /// </summary>
        /// <param name="rgbStr"></param>
        /// <returns></returns>
        private Color CreateColorByString(string rgbStr)
        {
            int r = int.Parse(rgbStr.Substring(0, rgbStr.IndexOf(',')));

            int firstMarkIndex = rgbStr.IndexOf(',');
            int g = int.Parse(rgbStr.Substring(firstMarkIndex + 1, firstMarkIndex));

            int secondMarkIndex = rgbStr.IndexOf(',', firstMarkIndex + 1);
            int b = int.Parse(rgbStr.Substring(secondMarkIndex + 1));

            return Color.FromArgb(r, g, b);
        }
        
        /// <summary>
        /// 프로그램이 종료될 때 발생하는 이벤트
        /// </summary>
        private void Close(object sender, FormClosingEventArgs e)
        {

        }

        /// <summary>
        /// 사이트 별 버튼을 클릭했을 때 발생하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SiteButtonClick(object sender, EventArgs e)
        {
            int siteIndex = int.Parse(((Control)sender).Name);
            string siteName = ((Control)sender).Text;

            int tapCount = 0;

            FileInfo siteInfoFile = SiteManager.fileInfoList.Find(x => x.Name == siteName + ".txt");
            Dictionary<string, string> siteInfoKeyValues = null;
            Dictionary<string, string> contentsValues = null;

            #region 예외처리
            if(siteInfoFile == null)
            {
                MessageBox.Show("파일 읽기 오류.");
                return;
            }
            try
            {
                siteInfoKeyValues = FileManager.LoadFile(siteInfoFile.FullName);
                contentsValues = FileManager.LoadFile(Paths.valueInputTextPath);
            }
            catch(DirectoryNotFoundException dnfE)
            {
                MessageBox.Show("정상적인 경로가 아닙니다.\n파일을 확인해 주세요.");
                return;
            }
            catch(FileNotFoundException fnfE)
            {
                MessageBox.Show("파일을 찾을 수 없습니다.");
                return;
            }
            #endregion

            try
            {
                tapCount = Convert.ToInt32(siteInfoKeyValues["제목에서 내용까지 탭"]);
            }
            catch
            {
                tapCount = 0;
            }
            int lineSpacing = Convert.ToInt32(siteInfoKeyValues["줄 간격"]) - 1;



            RichTextBox outputText = new RichTextBox();
            
            Color titleColor;
            Color valueColor;
            Font titleFont = CreateFontByString(siteInfoKeyValues["TitleFontKey"], out titleColor);
            Font valueFont = CreateFontByString(siteInfoKeyValues["ContentsFontKey"], out valueColor);

            List<string> keyAndContentsKeys = new List<string>();
            foreach(KeyValuePair<string, string> kvp in siteInfoKeyValues)
            {
                if(kvp.Key.Contains("KeyBox"))
                {
                    keyAndContentsKeys.Add(kvp.Value);
                }
            }
            
            foreach(string key in keyAndContentsKeys)
            {
                string title = siteInfoKeyValues[key];
                string contents = contentsValues[key];

                RichTextBox titleRtf = new RichTextBox();
                RichTextBox valueRtf = new RichTextBox();

                #region 후기 타이틀을 titleRtf에 넣고 가공함
                titleRtf.AppendText(title);
                titleRtf.SelectAll();
                titleRtf.Font = titleFont;
                titleRtf.SelectionColor = titleColor;
                outputText.Select(outputText.TextLength, 0);
                outputText.SelectedRtf = titleRtf.Rtf;
                #endregion

                #region 후기를 valueRtf에 넣고 가공함
                valueRtf.AppendText(contents);
                valueRtf.SelectAll();
                valueRtf.Font = valueFont;
                valueRtf.SelectionColor = valueColor;
                #endregion

                if (Convert.ToBoolean(siteInfoKeyValues["TitleUnderValueCheckBox"]))
                {
                    outputText.Select(outputText.TextLength, 0);
                }
                else
                {
                    outputText.Select(outputText.TextLength - 1, 0);
                }
                outputText.SelectedRtf = valueRtf.Rtf;

                // 구역당 간격
                for (int ls = 0; ls < lineSpacing; ++ls)
                {
                    outputText.AppendText("\r\n");
                }
            }

            #region 붙여넣을 html 코드 생성
            string html = Rtf.ToHtml(outputText.Rtf);   //test. 결과가 이상하면 다시 위 코드 주석 풀기
            #endregion

            #region 실행중인 메인 프로세스들 얻고 크롬 브라우저를 찾음
            List<string> mainWindowTitleList = Process.GetProcesses()
                                            .Select(p => p.MainWindowTitle)
                                            .Where(p => !string.IsNullOrEmpty(p))
                                            .Distinct()
                                            .OrderBy(p => p)
                                            .ToList();

            string hWndTitle = "";
            hWndTitle = mainWindowTitleList.Find(x => x.Contains("Chrome"));    //test. 결과가 이상하면 위 코드 주석 풀기
            #endregion
            
            #region 찾은 크롬 브라우저를 포커스 최상위로 만들기 위한 작업 찾았다면 붙여넣기
            if (hWndTitle.Any())
            {
                // 찾은 크롬 브라우저를 포커스 최상위로 만듬
                IntPtr hWnd = FindWindow(null, hWndTitle);

                if (!hWnd.Equals(IntPtr.Zero))
                {
                    SetForegroundWindow(hWnd);

                    #region 제목 넣기
                    if (tapCount > 0)
                    {
                        try
                        {
                            Clipboard.SetText(contentsValues["제목"]);
                            SendKeys.Send("^a");
                            SendKeys.Send("{DEL}");
                            SendKeys.Send("^v");
                        }
                        catch { }
                        for (int i = 0; i < tapCount; i++)
                        {
                            SendKeys.Send("{TAB}");
                            Console.WriteLine("tab");
                        }
                    }
                    #endregion

                    SendKeys.Send("^a");
                    SendKeys.Send("{DEL}");
                    
                    #region 특정 문장이 선행 될 경우
                    try
                    {
                        string firstString = File.ReadAllText(Paths.FirstStringPath + "\\" + siteName + ".txt");
                        ClipboardHelper.CopyToClipboard(firstString, "");
                        SendKeys.Send("^v");
                        //SendKeys.Send("{ENTER}");

                        Thread.Sleep(100);
                    }
                    catch(FileNotFoundException fnfE)
                    {
                        Console.WriteLine(siteName + "은 선행되는 문장이 없습니다.");
                    }
                    #endregion

                    #region 후기 내용 붙여 넣기
                    ClipboardHelper.CopyToClipboard(html, outputText.Text);
                    Console.WriteLine(Clipboard.GetText(TextDataFormat.Html));
                    
                    SendKeys.Send("{ENTER}");
                    SendKeys.Send("^v");
                    //SendKeys.Send("^{TAB}");
                    Thread.Sleep(100);
                    #endregion

                    #region 특정 문장이 후행 될 경우
                    try
                    {
                        string endString = File.ReadAllText(Paths.EndStringPath + "\\" + siteName + ".txt");
                        ClipboardHelper.CopyToClipboard(endString, "");
                        SendKeys.Send("^v");

                        Thread.Sleep(100);
                    }
                    catch(FileNotFoundException fnfE)
                    {
                        Console.WriteLine(siteName + "은 후행되는 문장이 없습니다.");
                    }
                    #endregion
                }
            }
            else
            {
                //찾은 크롬 브라우저가 없다면 메시지 박스로 알림
                MessageBox.Show("크롬 브라우저를 찾지 못했습니다");
            }
            #endregion
        }

        /// <summary>
        /// 버튼 만들기 버튼 이벤트
        /// 정확히는 텍스트 파일을 만든다. 그 만든 텍스트 파일이 Refrash()함수를 거쳐 위에 CreateButton_Auto()에서 구현된다.
        /// </summary>
        private void CreateButtonsButton(object sender, EventArgs e)
        {
            SiteManager.Refrash();

            int count = int.Parse(this.createButtonNumber.Value.ToString());

            for (int i = 0; i < count; i++)
            {
                string newTextFilePath = Paths.siteFolderPath + "\\" + (SiteManager.siteCount + i).ToString() + ".txt";
                File.WriteAllText(newTextFilePath, "", Encoding.UTF8);
            }

            Refrash();
        }

        /// <summary>
        /// 글 설정 버튼 이벤트
        /// </summary>
        private void SetInputFormButton_Click(object sender, EventArgs e)
        {
            setInputForm = new SetInputForm();
            setInputForm.Show();
        }

        /// <summary>
        /// 사이트 별 버튼 마우스 우클릭 이벤트
        /// </summary>
        private void SiteButton_MouseRightClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int siteIndex = int.Parse(((Control)sender).Name);
                string sitePath = SiteManager.fileInfoList[siteIndex].DirectoryName + "\\" + SiteManager.fileInfoList[siteIndex].Name;
                SiteInfo siteInfo = new SiteInfo(sitePath, siteIndex, ((Control)sender).Text, this);
                siteInfo.Show();
            }
        }
        
    }
}
