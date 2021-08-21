using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ls.libs
{
    public class TextMan
    {
        public string mfilename = "";
        FileStream mfs = null;
        StreamReader msr = null;
        StreamWriter msw = null;
        bool misopen = false;

        public TextMan()
        {
        }
        public TextMan(string str)
        {
            mfilename = str;
        }

        public int Open()
        {
            if (mfilename.Length == 0)
                return -1;
            //
            try
            {
                mfs = new FileStream(mfilename, FileMode.Create, FileAccess.ReadWrite);
                msr = new StreamReader(mfs);
                msw = new StreamWriter(mfs);

                misopen = true;
                return 1;
            }
            catch (Exception e)
            {
                msw = null;
                misopen = false;
                return -1;
            }

        }
        public void Close()
        {
            //msr.Close();
            if (misopen)
            {
                try
                {
                    msw.Close();
                    mfs.Close();
                }
                catch (Exception e)
                {
                }
            }
        }


        public void Fluseh()
        {
            if (msw != null)
            {
                msw.Flush();
            }
        }
        public void Write(string str)
        {
            if (msw!=null)
                try
                {
                    msw.Write(str);
                }
                catch (Exception e)
                {
                }
        }
        public void WriteLine(string str)
        {
            if (msw != null)
            {
                try
                {
                    msw.WriteLine(str);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
        public void AppendLine(string str)
        {

        }

        // static method;
        public static void AppendText(string filename, string str)
        {
            try
            {
                //FileStream fs = File.OpenWrite(mfilename);
                File.AppendAllText(filename, str);

                //StreamWriter sw = new StreamWriter(mfilename,true);
                //sw.Write(s);
                //sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("TextMan exception: " + e.Message);
            }
        }

        public static string ReadLastLine(string path,int num)
        {

            // open read only, we don't want any chance of writing data
            using (System.IO.Stream fs = System.IO.File.OpenRead(path))
            {
                // check for empty file
                if (fs.Length == 0)
                {
                    return null;
                }

                // start at end of file
                fs.Position = fs.Length - 1;

                // the file must end with a '\n' char, if not a partial line write is in progress
                int byteFromFile = fs.ReadByte();
                if (byteFromFile != '\n')
                {
                    // partial line write in progress, do not return the line yet
                    return null;
                }

                // move back to the new line byte - the loop will decrement position again to get to the byte before it
                fs.Position--;

                // while we have not yet reached start of file, read bytes backwards until '\n' byte is hit
                while (fs.Position > 0)
                {
                    fs.Position--;
                    byteFromFile = fs.ReadByte();
                    if (byteFromFile < 0)
                    {
                        // the only way this should happen is if someone truncates the file out from underneath us while we are reading backwards
                        throw new System.IO.IOException("Error reading from file at " + path);
                    }
                    else if (byteFromFile == '\n')
                    {
                        if (--num <= 0)
                        {
                            // we found the new line, break out, fs.Position is one after the '\n' char
                            break;
                        }
                    }
                    fs.Position--;
                }

                // fs.Position will be right after the '\n' char or position 0 if no '\n' char
                byte[] bytes = new System.IO.BinaryReader(fs).ReadBytes((int)(fs.Length - fs.Position));
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
        }

        public static string ReadLastLine2(string path)
        {
            try
            {
                var lastLine = File.ReadAllLines(path).Last();
                return lastLine;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public static string GetLine(string path, uint line)
        {
            using (var reader = new StreamReader(path))
            {
                try
                {
                    for (uint i = 0; i <= line; i++)
                    {
                        if (reader.EndOfStream)
                            return string.Format("There {1} less than {0} line{2} in the file.", line,
                                                 ((line == 1) ? "is" : "are"), ((line == 1) ? "" : "s"));

                        if (i == line)
                            return reader.ReadLine();

                        reader.ReadLine();
                    }
                }
                catch (IOException ex)
                {
                    return ex.Message;
                }
                catch (OutOfMemoryException ex)
                {
                    return ex.Message;
                }
            }

            throw new Exception("Something bad happened.");
        }
    }
}
