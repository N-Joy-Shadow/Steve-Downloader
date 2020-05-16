using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using steve_downloader.second_window;
using HtmlAgilityPack;
using System.Net;

namespace steve_downloader.download
{
    class Download
    {
        /// <summary>
        /// 
        /// </summary>
        public static string korean_link = "http://222.234.190.69/WordPress/wp-content/uploads/2020/03/koreanchat-creo-1.12-1.9.jar";



        second gqw = new second();
        public void thread_waiting()
        {
            var F_th = new Thread(F_Thread);
            var S_th = new Thread(S_Thread);
            if (gqw.set_download_start == true)
            {
                F_th.Start();
                S_th.Start();
            }
        }

        private static void F_Thread()
        {
            using(WebClient client = new WebClient())
            {
                ///client.DownloadFileAsync(korean_link, );
            }
        }
        private static void S_Thread()
        {

        }

    }
}

    
