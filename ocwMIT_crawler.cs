using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO.Compression;
using Delimon.Win32.IO;

namespace OCW_MIT_downloader
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var htmlPath = @"C:\someFolder\Mathematics   MIT OpenCourseWare   Free Online Course Materials";
            var folderPath = @"C:\someDownloadPath";
            var splitter = @"https://ocw.mit.edu/courses/mathematics/";
            var modID = "18";//must contain everything before the hyphen, lower case

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var errorLog = new List<string>();

            
            var moduleCodes = GetModuleCodes(htmlPath, modID, splitter);
            foreach(var c in moduleCodes)
                DownloadModule(c, modID, folderPath);         
        }


        static List<string> GetModuleCodes(string path, string course_id, string splitter)//splitter = @"https://ocw.mit.edu/courses/electrical-engineering-and-computer-science/"
        {
            var content = File.ReadAllText(path);
            List<string> moduleCodes = new List<string>();
            var xxx = content.Split(new string[] { splitter },StringSplitOptions.RemoveEmptyEntries);
            foreach (var x in xxx)
                if (x.Substring(0, course_id.Length) == course_id)
                {
                    var e = x.Split('/')[0].Split('\"')[0];
                    if (!moduleCodes.Contains(e))
                        moduleCodes.Add(e);
                }
            return moduleCodes;
        }

        static async void DownloadModule(string moduleCode, string course_id, string path)
        {
            var ele = moduleCode.Split('-');
            var l = ele.Length;
            var mC_new = string.Join("-", new string[] { ele[0], ele[1], ele[l - 2], ele[l - 1] });
            course_id = course_id.ToUpper();
            string url = @"https://ocw.mit.edu/ans15436/ZipForEndUsers/" + course_id + "/"+ mC_new + "/"+ mC_new + ".zip";//case sensitive
            WebClient wc = new WebClient();
            wc.DownloadFileCompleted += (o,s) => Console.Write(url + "\r\n");
            try {wc.DownloadFileTaskAsync(url, path + "\\" + moduleCode + ".zip").Wait();}
            catch
            {
                mC_new = mC_new.Substring(0, course_id.Length).ToUpper() + mC_new.Substring(course_id.Length);//some links suddenly uses upper case course letters
                url = @"https://ocw.mit.edu/ans15436/ZipForEndUsers/" + course_id + "/" + mC_new + "/" + mC_new + ".zip";
                try { wc.DownloadFileTaskAsync(url, path + "\\" + moduleCode + ".zip").Wait(); }
                catch
                {
                    Console.Write("Failed:" + url + "\r\n");
                }               
            }            
        }   
    }
}
