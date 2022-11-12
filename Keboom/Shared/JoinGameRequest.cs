﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keboom.Shared
{
    public class JoinGameRequest
    {
        public string GameName { get; set; }
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }

        public int BoardWidth { get; set; } = 8;
        public int BoardHeight { get; set;} = 8;

        public int NumberOfMines { get; set; } = 19;
    }
}