using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordWheelPlayer
{
    public class GameLetter
    {
        public string Letter { get; set; }
        public bool Used { get; set; }
        public bool MustInclude { get; set; }
    }
}
