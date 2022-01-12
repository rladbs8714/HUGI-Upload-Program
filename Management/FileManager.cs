using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Management
{
    public static class FileManager
    {
        public static void SaveFile(Dictionary<string, string> write, string path)
        {
            List<string> writeList = new List<string>();

            foreach (KeyValuePair<string, string> kvp in write)
            {
                writeList.Add(string.Format("#{0} :: {1}#", kvp.Key, kvp.Value));
                Console.WriteLine(writeList.Last());
            }

            try
            {
                File.WriteAllLines(path, writeList, Encoding.UTF8);
            }
            catch (FormatException e)
            {
                // 잘못된 문자열 받음
                Console.WriteLine("File Write Error");
            }
        }

        public static Dictionary<string, string> LoadFile(string path)
        {
            List<string> readList = File.ReadAllLines(path).ToList();

            Dictionary<string, string> dic = new Dictionary<string, string>();

            bool endLineCheck = true; // 정보를 다 읽었다면 true, 아직 읽을 정보가 남아있다면 false
            int checkPoint;
            string key = "";
            string value = "";
            StringBuilder valueSB = new StringBuilder();

            Console.WriteLine("---" + path + "---");
            foreach (string readLine in readList)
            {
                if (readLine.Any())
                {
                    int lineLength = readLine.Length;

                    if (readLine.First() == '#' && lineLength > 2)
                    {
                        if (lineLength > 2 && readLine.Last() == '#') // 가장 앞에도 #이 있고 가장 뒤에도 #이 있을 경우 -- # key :: value #
                        {
                            checkPoint = readLine.IndexOf("::");
                            key = readLine.Substring(1, checkPoint - 1 - 1);
                            value = readLine.Substring(checkPoint + 3, lineLength - 1 - (checkPoint + 3));

                            dic.Add(key, value);
                            endLineCheck = true;
                        }
                        else // 가장 앞에만 #이 있는 경우 -- key의 시작, value도 있을 수 있음 # key :: value
                        {
                            checkPoint = readLine.IndexOf("::");
                            key = readLine.Substring(1, checkPoint - 1 - 1);
                            valueSB.Append(readLine.Substring(checkPoint + 3)); // # 검출 할 필요가 없으니 끝까지
                            endLineCheck = false;
                        }
                    }
                    else
                    {
                        if (readLine.Last() != '#') // 처음과 마지막 모두 #이 없는 경우 -- 무조건 value
                        {
                            valueSB.Append("\r\n");
                            valueSB.Append(readLine);
                            endLineCheck = false;
                        }
                        else // 앞에는 #이 없고 뒤에만 있을 경우 -- value의 마지막 문장. value #
                        {
                            valueSB.Append("\r\n");
                            valueSB.Append(readLine.Substring(0, lineLength - 1));

                            dic.Add(key, valueSB.ToString());
                            valueSB.Clear();
                            endLineCheck = true;
                        }
                    }
                }
                else if(!endLineCheck)
                {
                    valueSB.Append("\r\n");
                }

                Console.WriteLine(readLine);
            }
            Console.WriteLine("---" + "END" + "---");

            return dic;
        }
    }
}
