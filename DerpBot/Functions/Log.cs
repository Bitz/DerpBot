using System;
using System.IO;
using System.Text;

using static System.DateTime;
using static System.IO.Directory;
using static System.IO.File;


namespace DerpBot.Functions
{
    public class Log : TextWriter
    {
        private readonly TextWriter _log;

        public Log(TextWriter log)
        {
            _log = log;
        }

        public override void WriteLine(string message)
        {
            _log.WriteLine($"[{Now:hh:mm:sstt}] {message}");
        }


        //Do not use this though.
        public override void Write(char value)
        {
            _log.WriteLine($"[{Now:hh:mm:sstt}] {value}");
        }

        public override void Write(string message)
        {
            _log.Write($"[{Now:hh:mm:sstt}] {message}");
        }

        public override Encoding Encoding => new ASCIIEncoding();
    }

    class PrefixedWriter : TextWriter
    {
        private readonly TextWriter _originalOut;

        public PrefixedWriter()
        {
            _originalOut = Console.Out;
        }
       

        public override Encoding Encoding => new ASCIIEncoding();


        public override void WriteLine(string message)
        {
            _originalOut.WriteLine($"[{Now:hh:mm:sstt}] {message}");
        }


        //Do not use this though.
        public override void Write(char value)
        {
            _originalOut.WriteLine($"[{Now:hh:mm:sstt}] {value}");
        }

        public override void Write(string message)
        {
            _originalOut.Write($"[{Now:hh:mm:sstt}] {message}");
        }
    }

    public class Create
    {
        public static StreamWriter Log(string loggingpath)
        {
            CreateDirectory(loggingpath);

            string logName = $"{Now:MM-dd-yyyy--hh-mm-ss-tt}.log";
            //TODO: path handle for linux fs
            string logFullPath = $@"{loggingpath}\{logName}";
            StreamWriter logfile = CreateText(logFullPath);
            return logfile;
        }
    }

    public class HashDb
    {
        public static void Write(string s)
        {
            string fileName = "Delete-Hashes.txt";
            using (StreamWriter w = AppendText(fileName))
            {
                w.AutoFlush = true;
                if (!File.Exists(fileName))
                {
                    CreateText(fileName);
                }
                w.WriteLine(s);
            }

        }
    }
}
