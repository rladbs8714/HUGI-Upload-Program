using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Management
{
    public class LineForm
    {
        public string text;
        public Font font;
        public Color color;

        public LineForm(string line)
        {
            int index = line.IndexOf('|');

            string text = GetLineElement(line, index);
            index = line.IndexOf('|', index + 1);

            string fontName = GetLineElement(line, index);
            index = line.IndexOf('|', index + 1);

            int fontSize = int.Parse(GetLineElement(line, index));
            index = line.IndexOf('|', index + 1);

            string fontStyle = GetLineElement(line, index);
            index = line.IndexOf('|', index + 1);

            // set font
            FontStyle fs;
            switch(fontStyle)
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
            font = new Font(fontName, fontSize, fs);

            color = GetColor(GetLineElement(line, index));
        }

        private string GetLineElement(string line, int startIndex)
        {
            return line.Substring(startIndex + 1, line.IndexOf('|', startIndex + 1) - 1 - startIndex);
        }

        private Color GetColor(string rgbStr)
        {
            int r = int.Parse(rgbStr.Substring(0, rgbStr.IndexOf(',')));

            int firstMarkIndex = rgbStr.IndexOf(',');
            int g = int.Parse(rgbStr.Substring(firstMarkIndex + 1, firstMarkIndex));

            int secondMarkIndex = rgbStr.IndexOf(',', firstMarkIndex + 1);
            int b = int.Parse(rgbStr.Substring(secondMarkIndex + 1));
            
            return Color.FromArgb(r, g, b);
        }
    }
}
