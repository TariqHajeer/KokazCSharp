using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Helpers
{
    public class Logging
    {
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private const string logfileName = "Log.Txt";

        private readonly string logfilePath;
        IWebHostEnvironment _env;
        public Logging(IWebHostEnvironment env)
        {
            _env = env;
            logfilePath = Path.Combine(env.WebRootPath, logfileName);
        }
        public async void WriteExption(Exception exception)
        {

            _semaphore.Wait();
            try
            {

                if (!File.Exists(logfilePath))
                    File.Create(logfilePath);
                await File.AppendAllTextAsync(logfilePath, GetExption(exception));
            }
            catch
            {
                //TODO: find way to write this ex
            }
            finally
            {
                _semaphore.Release();
            }
        }
        public static string GetExption(Exception exception)
        {
            var ex = exception;
            string text = "";
            var st = new StackTrace(ex, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();
            var method = frame.GetMethod();
            var exfile = frame.GetFileName();
            var date = DateTime.UtcNow;
            text += date.ToString() + Environment.NewLine;
            text += exfile + Environment.NewLine;
            text += method + Environment.NewLine;
            text += line + Environment.NewLine;

            while (ex != null)
            {

                text += ex.Message + Environment.NewLine;
                ex = ex.InnerException;
            }
            text += "=======================================================" + Environment.NewLine;
            return text;
        }
        public static bool IsFileReady(string filename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    return inputStream.Length > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
