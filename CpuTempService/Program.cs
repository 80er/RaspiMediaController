using System;
using System.IO;
using System.Net.Mime;
using System.Threading;

namespace CpuTempService
{
    class Program
    {
        static void Main(string[] args)
        {
            var notifier = new NotifyIfContentChanged("/sys/class/thermal/thermal_zone0/temp");
            notifier.ContentChanged += NotifierOnContentChanged;
            while (true)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Waiting....");
            }
        }

        private static void NotifierOnContentChanged(object sender, string s)
        {
            var temp = Int32.Parse(s);
            var tempInFloat = (float) temp / 1000.0;
            Console.WriteLine("Temperatur is : {0:0.00}", tempInFloat);
        }
    }

    public class NotifyIfContentChanged
    {
        private String PreviousContent = string.Empty;

        private FileInfo FileToWatch { get; set; }

        private Timer Timer;

        public event EventHandler<string> ContentChanged;  

        private void Callback(object state)
        {
            var content = File.ReadAllText(FileToWatch.FullName);
            if (PreviousContent != content)
            {
                PreviousContent = content;
                ContentChanged(null, content);
            }
        }

        public NotifyIfContentChanged(string path)
        {
            FileToWatch = new FileInfo(path);
            Timer = new Timer(Callback, null, 0, 100);
        }


    }
}
