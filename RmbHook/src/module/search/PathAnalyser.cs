﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace WrittingHelper.src.keyword
{
    public class PathAnalyser
    {
        //public struct PathInfo
        //{
        //    public int linenum;
        //    public HashSet<string> keywords;
        //}

        public PathAnalyser()
        {
        }

        public void reset()
        {
            keywordPaths.Clear();
            pathKeywords.Clear();
            keywordgroup.Remove(0, keywordgroup.Length);
        }

        // --- data structure ---
        Hashtable keywordPaths = new Hashtable();   // <string, Hashset<string>>
        public ICollection getKeywords() { return keywordPaths.Keys; }
        public HashSet<string> getKeywordPath(string key)
        {
            if (keywordPaths.Contains(key))
                return (HashSet<string>)keywordPaths[key];
            else
                return null;
        }
        StringBuilder keywordgroup = new StringBuilder();

        Hashtable pathKeywords = new Hashtable();
        public Hashtable getPathtable() { return pathKeywords; }
        public ICollection getPaths()
        {
            return pathKeywords.Keys;
        }
        public HashSet<string> getPathKeyword(string path)
        {
            if (pathKeywords.Contains(path))
            {
                PathInfo pi = (PathInfo)pathKeywords[path];
                return pi.keywords;
                //return (HashSet<string>)pathKeywords[path];
            }
            else
                return null;
        }

        // --- operation ---
        void addKeyPath(string key, string path)
        {
            string str = path.ToLower();

            HashSet<string> hss;
            if (keywordPaths.Contains(key))
            {
                hss = (HashSet<string>)keywordPaths[key];
                if (!hss.Contains(str))
                {
                    hss.Add(str);
                }
            }
            else
            {
                hss = new HashSet<string>();
                hss.Add(str);
                keywordPaths.Add(key, hss);
                keywordgroup.Append(key + " ");
            }
        }
        public void openPath(string path, bool list, int linenum)
        {
            if (path == null)
                return;
            if (File.Exists(path))
                return;
            if (path.EndsWith(":"))
                path += "\\";

            if (list)
            {
                DirectoryInfo folder = new DirectoryInfo(path);
                foreach (DirectoryInfo dir in folder.GetDirectories())
                {
                    //this.addKeyPath(dir.Name, dir.FullName + "\\");
                    this.openPath(dir.FullName, false,-1);
                }
            }

            PathInfo pi;
            path = path.ToLower();
            if (pathKeywords.Contains(path))
            {
                if (linenum >= 0)
                {
                   pi= (PathInfo)pathKeywords[path];
                    if (pi.linenum != linenum)
                        pi.linenum = linenum;
                }
                
                return;
            }
            //
            HashSet<string> hss = Keywords.mthis.getKeywords(this.getPathName(path));
            pi = new PathInfo();
            pi.keywords = hss;
            pi.linenum = linenum;
            //pathKeywords.Add(path, hss);    //NOTE: hss may be empty;
            pathKeywords.Add(path, pi);

            //
            foreach( string s in hss )
            {
                this.addKeyPath(s, path);
            }

            path = this.getParentPath(path);
            this.openPath(path, false,-1); // only list;

        }

        public ArrayList listPath(string path)
        {
            ArrayList als = new ArrayList();

            if (!path.EndsWith(":\\"))
                als.Add("..");

            DirectoryInfo folder = new DirectoryInfo(path);
            foreach (DirectoryInfo dir in folder.GetDirectories())
            {
                //als.Add(dir.FullName);
                als.Add(dir.Name);
            }
            foreach (FileInfo file in folder.GetFiles())
            {
                //als.Add(file.FullName);
                als.Add(file.Name);
            }

            return als;
        }
        public string getParentPath(string path)
        {
            if (!Directory.Exists(path))
                return null;
            if (path.EndsWith(":"))
                return null;

            DirectoryInfo folder = new DirectoryInfo(path);
            string s = folder.FullName;
            s = s.Substring(0, s.LastIndexOf("\\"));

            return s;
        }
        public string getPathName(string path)
        {
            if (Directory.Exists(path))
            {
                if (path.EndsWith(":"))
                    return path.Substring(0,path.Length-1);
                return new DirectoryInfo(path).Name;
            }
            else
                return new FileInfo(path).Name;
        }

        public HashSet<string> search(string key)
        {
            // analyse keys;
            string[] ss = key.Split(new string[] {",", " ", "."},
                StringSplitOptions.RemoveEmptyEntries);
            
            // find keywords;
            ArrayList alskeys = new ArrayList();
            ArrayList allkeys = new ArrayList();
            foreach (string s in ss)
            {
                ArrayList alskey = Searcher.search(s, keywordgroup.ToString());
                if (alskey.Count > 0)
                {
                    alskeys.Add(alskey);
                    foreach (string s1 in alskey)
                        allkeys.Add(s1);
                }
            }
            
            //
            ArrayList alhss = new ArrayList();
            HashSet<string> hss;
            foreach (ArrayList al in alskeys)
            {
                foreach (string s in al)
                {
                    if (keywordPaths.Contains(s))
                    {
                        hss = (HashSet<string>)keywordPaths[s];
                        alhss.Add(hss);
                    }
                }
            }

            //foreach (string s in ss)
            //{
            //    if (keywordPaths.Contains(s))
            //    {
            //        hss = (HashSet<string>)keywordPaths[s];
            //        alhss.Add(hss);
            //    }
            //}

            //
            ArrayList als = new ArrayList();
            HashSet<string> hsres = new HashSet<string>();
            foreach (HashSet<string> hs in alhss)
            {
                foreach (string s in hs)
                {
                    //if (this.checkContain(ss, s))
                    if (this.checkContain(allkeys, s))
                    {
                        //als.Add(s);
                        if (!hsres.Contains(s))
                            hsres.Add(s);
                    }
                }
            }
            return hsres;
        }

        bool checkContain(ArrayList keys, string path)
        {
            bool b = true;
            foreach (string s in keys)
            {
                if (!this.checkContain(s, path))
                {
                    b = false;
                    break;
                }
            }

            return b;
        }

        bool checkContain(string[] keys, string path)
        {
            bool b = true;
            foreach (string s in keys)
            {
                if (!this.checkContain(s, path))
                {
                    b = false;
                    break;
                }
            }

            return b;
        }
        bool checkContain(string key, string path)
        {
            bool b = false;
            //string s; 
            PathInfo pi;
            while (path != null)
            {
                //s = this.getPathName(path);
                if(pathKeywords.Contains(path))
                {
                    pi = (PathInfo)pathKeywords[path];
                    //HashSet<string> hss = (HashSet<string>)pathKeywords[path];
                    HashSet<string> hss = pi.keywords;
                    if( hss != null)
                        if( hss.Contains(key))
                        {
                            b=true;
                            break;
                        }
                }
                path = this.getParentPath(path);
            }
            return b;
        }
        bool checkMatch(string path, HashSet<string> hss)
        {
            bool match = false;
            string smin, smax;
            foreach (string s in hss)
            {
                if (s.Length == 0)
                    continue;
                if (s.Length > path.Length)
                {
                    smin = path;
                    smax = s;
                }
            }

            return match;
        }
    }
}
