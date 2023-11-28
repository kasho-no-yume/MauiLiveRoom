using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public class LiveInfo
    {
        public LiveInfo() 
        {
            this.path = "xydltql";
            this.watching = 5;
        }
        public LiveInfo(string path,int watching) 
        { 
            this.path = path;
            this.watching = watching;
            this.title = null;
            this.desc = null;
        }   
        public LiveInfo(string path, int watching, string title, string desc) : this(path, watching)
        {
            this.title = title;
            this.desc = desc;
        }

        public string path { get; set; }
        public int watching { get ; set; }
        public string title { get; set; }
        public string desc { get; set; }
    }
}
