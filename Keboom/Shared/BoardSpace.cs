using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keboom.Shared;

public class BoardSpace
{
    public bool HasMine { get; set; }
    public int AdjacentMines { get; set; }

    public int? ClaimedByPlayer { get; set; }
}
