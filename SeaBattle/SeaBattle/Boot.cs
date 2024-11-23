using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    internal class Boot
    {
        static void Main(string[] args)
        {
            SeaBattleGame game = new SeaBattleGame();
            game.GameProcess();
        }
    }
}
