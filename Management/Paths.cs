using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Management
{
    public static class Paths
    {
        // all
        public static string myDocuFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string programFolderPath = myDocuFolder + "\\BusinessManagement"; //---exists, foler
        public static string tempWordPath = programFolderPath + "\\Documents\\temp.docx";

        public static string siteFolderPath = programFolderPath + "\\Documents\\Site";
        public static string excelFilePath = programFolderPath + "\\List.xlsx"; //---exists, file
        public static string pathFilePath = programFolderPath + "\\prog.txt"; //---exists, file

        public static string programDocuFolderPath = programFolderPath + "\\Documents"; //---exists, folder
        public static string valueInputTextPath = programDocuFolderPath + "\\Value.txt"; //---exists, file

        public static string FirstStringPath = programDocuFolderPath + "\\FirstStrings";
        public static string EndStringPath = programDocuFolderPath + "\\EndStrings";

        public static string shortKeyListPath = programDocuFolderPath + "\\ShortKeyList";

        public static void Init()
        {
            // Folder Exists
            if (!new DirectoryInfo(programFolderPath).Exists)
            {
                new DirectoryInfo(programFolderPath).Create();
            }
            if (!new DirectoryInfo(programDocuFolderPath).Exists)
            {
                new DirectoryInfo(programDocuFolderPath).Create();
            }
            if (!new DirectoryInfo(FirstStringPath).Exists)
            {
                new DirectoryInfo(FirstStringPath).Create();
            }
            if (!new DirectoryInfo(EndStringPath).Exists)
            {
                new DirectoryInfo(EndStringPath).Create();
            }
            
            if (!new DirectoryInfo(siteFolderPath).Exists)
            {
                new DirectoryInfo(siteFolderPath).Create();
            }
            else
            {
                // site 폴더가 있다면
                // 메인 폼의 버튼 생성을 위해 그 폴더에 텍스트 파일이 몇개 있는지 알아야 함.
                SiteManager.Refrash();
            }

            if(!new DirectoryInfo(shortKeyListPath).Exists)
            {
                new DirectoryInfo(shortKeyListPath).Create();
            }

            // File Exists
            if (!new FileInfo(pathFilePath).Exists)
            {
                File.WriteAllText(pathFilePath, excelFilePath, Encoding.Default);
            }
            if (!new FileInfo(valueInputTextPath).Exists)
            {
                string[] lines = {  "0,", "1,", "2,", "3,", "4,",
                                    "5,", "6,", "7,", "8,", "9," };
                File.WriteAllLines(valueInputTextPath, lines, Encoding.UTF8);
            }
        }
    }
}
