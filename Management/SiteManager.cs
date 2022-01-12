using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Management
{
    public static class SiteManager
    {
        public static int siteCount;

        public static List<FileInfo> fileInfoList;
        
        public static void Refrash()
        {
            fileInfoList = new DirectoryInfo(Paths.siteFolderPath).GetFiles("*.txt", SearchOption.TopDirectoryOnly).ToList();
            //List<FileInfo> fileinfos = new DirectoryInfo(Paths.siteFolderPath).GetFiles("*.txt", SearchOption.TopDirectoryOnly).ToList();
            siteCount = fileInfoList.Count;

            int select = 0;
            for(int now = 0; now < fileInfoList.Count - 1; ++now)
            {
                select = now;
                for(int move = now + 1; move < fileInfoList.Count; ++move)
                {
                    string lhsText = fileInfoList[now].Name;
                    string rhsText = fileInfoList[move].Name;
                    int lhs, rhs;
                    int.TryParse(lhsText.Substring(0, lhsText.IndexOf('.')), out lhs);
                    int.TryParse(rhsText.Substring(0, rhsText.IndexOf('.')), out rhs);
                    if(lhs > rhs)
                    {
                        select = move;
                    }
                }

                if(now != select)
                {
                    FileInfo temp = fileInfoList[now];
                    fileInfoList[now] = fileInfoList[select];
                    fileInfoList[select] = temp;
                }
            }
            
            foreach(var file in fileInfoList)
            {
                Console.WriteLine(file.Name);
            }
        }

        public static string GetKeyByLine(string line)
        {
            int firstMark = line.IndexOf('|');
            int secondMark = line.IndexOf('|', firstMark);
            return line.Substring(firstMark + 1, secondMark);
        }
    }
}
