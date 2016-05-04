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
        public string getPathfile() { return pathfile; }

        private string relativepath = ".\\";
        public string getRelativePath() { return relativepath; }

        public PathFile()
        {
        }

        public void onSetPathfile(string path)
        {
            FileInfo fi = new FileInfo(path);
            if (!fi.Exists)
                return;
            //
            this.relativepath = fi.Directory.FullName.ToLower() ;
            this.pathfile = fi.FullName.ToLower();

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
                str = s;
                if (s[0] == '\\')
                    str = relativepath + s;
                else if (s[0] == '.')
                {
                    str = relativepath + s.Substring(1, s.Length - 1);
                    //str = s.Replace(
                }
                hss.Add(str);
            }

            //
            PathAnalyser pas = LsKeyword.getThis().getPathAnalyser();
            pas.reset();

            pas.openPath(relativepath, true);
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
        public string getRelative(string path)
        {
            string str = path ;
            if (path.Contains(relativepath))
            {
                str = "."+path.Substring(relativepath.Length, 
                    path.Length-relativepath.Length);

            }
            return str;
        }
        public void saveFile(ICollection ic, string path)
        {
            int size = 256;

            char[] cs = new char[256];
            for( int i=0; i<size; i++)
                cs[i] = ' ';

            try
            {
                FileStream fs = new FileStream(path,
                    FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

                string str;
                foreach (string s in ic)
                {
                    str = this.getRelative(s);
                    if (str.Length > size)
                        str = str.Substring(0, size);
                    //
                    sw.Write(str);
                    sw.WriteLine(cs, 0, size - str.Length);
                }

                sw.Close();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
