using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public class LiveInfo
    {
        public LiveInfo() { }
        public LiveInfo(string path,int watching) 
        { 
            this.Path = path;
            this.Watching = watching;
        }   
        public string Path { get; set; }
        public int Watching { get ; set; }
    }
}
