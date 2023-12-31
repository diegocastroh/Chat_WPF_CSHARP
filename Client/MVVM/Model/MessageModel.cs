﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MVVM.Model
{
    class MessageModel
    {
        public int Id { get; set; }
        public String Username { get; set; }
        public String UsernameColor { get; set; }
        public String ImageSource { get; set; }
        public String Message { get; set; }
        public DateTime Time { get; set; }

        public bool IsNativeOrigin { get; set; }
        public bool? FirstMessage { get; set; }


    }
}
