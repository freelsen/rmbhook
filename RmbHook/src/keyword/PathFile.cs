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

        private ArrayList alslinenum = new ArrayList();

        private int maxline = 256;
        private int skip = 1;       // BOM info; FF FE;
        private int newline = 2;    // utf8; 0d 00 0a 00;

        private char[] blanks = new char[256];

        public PathFile()
        {
            for (int i = 0; i < maxline; i++)
                blanks[i] = ' ';
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
            Hashtable hss = new Hashtable();
            string str;
            string rpath = relativepath + "\\";
            for( int i=0; i< als.Count; i++)
            {
                string s = (string)als[i];
                str = s;
                if (s[0] == '\\')
                    str = relativepath + s;
                else if (s[0] == '.')
                {
                    str = relativepath + s.Substring(1, s.Length - 1);
                    //str = s.Replace(
                }
                //Int32 it2 = (Int32)alslinenum[i];
                hss.Add(str, (Int32)alslinenum[i]);
            }

            //
            PathAnalyser pas = LsKeyword.getThis().getPathAnalyser();
            pas.reset();

            pas.openPath(relativepath, true, -1);
            foreach (DictionaryEntry de in hss)
            {
                //Int32 it = (Int32)de.Value;
                pas.openPath((string)de.Key, false, (Int32)de.Value);
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
                alslinenum.Clear();
                while (true)
                {
                    line = sr.ReadLine();
                    if (line != null)
                    {
                        als.Add(line);
                        alslinenum.Add((Int32)linenum);
                    }
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
        public void saveFile(StreamWriter sw, string line, int linenum)
        {
            try
            {
                if (linenum < 0)
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                else
                {
                    int loc = 2*(skip + linenum * (maxline + newline));
                    sw.BaseStream.Seek(loc, SeekOrigin.Begin);
                }
                sw.Write(line);
                sw.WriteLine(blanks, 0, maxline - line.Length);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void saveFile(Hashtable pathtable, string path)
        {
            try
            {
                FileStream fs = new FileStream(path,
                    FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

                ICollection ic = pathtable.Keys;
                IDictionaryEnumerator ide =  pathtable.GetEnumerator();
                string str,s;
                PathInfo pi;
                while (ide.MoveNext())
                {
                    s = (string)ide.Key;
                    pi = (PathInfo)ide.Value;
                    if (pi.isdelete)
                    {

                    }
                    else if (pi.linenum < 0) // append;
                    {
                        str = this.getRelative(s);
                        if (str.Length > maxline)
                            str = str.Substring(0, maxline);
                        //
                        saveFile(sw, str, pi.linenum);
                        //sw.Write(str);
                        //sw.WriteLine(cs, 0, size - str.Length);
                    }
                }
                //foreach (string s in ic)
                //{
                //    str = this.getRelative(s);
                //    if (str.Length > size)
                //        str = str.Substring(0, size);
                //    //
                //    sw.Write(str);
                //    sw.WriteLine(cs, 0, size - str.Length);
                //}

                sw.Close();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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
