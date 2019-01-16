using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace JOBS.Web.Ui.Helper
{
    public static class FileHelper
    {
        public static void SaveFile(byte[] content, string path)
        {
            string filePath = GetFileFullPath(path);
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            //Save file
            using (FileStream str = File.Create(filePath))
            {
                str.Write(content, 0, content.Length);
            }
        }
        
        public static string GetFileFullPath(string path)
        {
            string relName = path.StartsWith("~") ? path : path.StartsWith("/") ? string.Concat("~", path) : path;

            string filePath = relName.StartsWith("~") ? HostingEnvironment.MapPath(relName) : relName;

            return filePath;
        }

        public static bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }
        public static bool CreateFolder(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        } 
    }
}