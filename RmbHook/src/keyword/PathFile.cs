using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace RmbHook.src.keyword
{
    public class PathFile
    {
        private string pathfile = "";
        private string relativepath = ".\\";

        public PathFile()
        {
        }

        public void onSetPathfile(string path)
        {
            FileInfo fi = new FileInfo(path);
            if (!fi.Exists)
                return;
            //
            this.relativepath = fi.Directory.FullName;
            this.pathfile = fi.FullName;

            Console.WriteLine(this.relativepath);
            Console.WriteLine(this.pathfile);
        }

        public void loadFile()
        {
            if (pathfile == null || pathfile.Length == 0) return;

            ArrayList als = this.readFile(pathfile);

            // proc relative path;
            HashSet<string> hss = new HashSet<string>();
            string str;
            string rpath = relativepath + "\\";
            foreach (string s in als)
            {
                if (s[0] == '\\')
                    str = relativepath + s;
                else
                    str = s.Replace(".\\", rpath);
                hss.Add(str);
            }

            //
            PathAnalyser pas = LsKeyword.getThis().getPathAnalyser();
            pas.reset();
            foreach (string s in hss)
            {
                pas.openPath(s, false);
            }
            //LsKeyword.getThis().getSearchForm().showList(hss);
        }
        ArrayList readFile(string path)
        {
            string line;
            ArrayList als = new ArrayList();
            try
            {
                FileStream fs = new FileStream(path,
                    FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs);

                sr.BaseStream.Seek(0, SeekOrigin.Begin);

                int linenum = 0;
                while (true)
                {
                    line = sr.ReadLine();
                    if (line != null)
                        als.Add(line);
                    else
                        break;

                    linenum++;
                }

                sr.Close();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return als;
        }
    }
}
