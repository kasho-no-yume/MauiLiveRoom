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
        }   
        public string path { get; set; }
        public int watching { get ; set; }
    }
}
