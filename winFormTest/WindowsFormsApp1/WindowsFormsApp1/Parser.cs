using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
namespace SkafRevengeModel
{
    static class Parser
    {
        public static void ParseChar(string fileName, out Point player, out Point skaf, out Model.Cell[,] map)
        {
            Point pl = new Point(-1, -1);
            Point sk = new Point(-1, -1);
            Model.Cell[,] tempMap;
            using (StreamReader f = new StreamReader(fileName))
            {
                var tokens = f.ReadLine().Split(' ');
                int x = int.Parse(tokens[0]);
                int y = int.Parse(tokens[1]);
                tempMap = new Model.Cell[y, x];
                for (int i = 0; i < y; i++)
                {
                    string line = f.ReadLine();
                    for (int j = 0; j < x; j++)
                    {
                        switch (line[j])
                        {
                            case 'F':
                                tempMap[i, j] = new Model.Cell(CellDomain.Free);
                                break;
                            case 'W':
                                tempMap[i, j] = new Model.Cell(CellDomain.Wall);
                                break;
                            case 'S':
                                sk = new Point(i, j);
                                tempMap[i, j] = new Model.Cell(CellDomain.Free);
                                break;
                            case 'P':
                                pl = new Point(i, j);
                                tempMap[i, j] = new Model.Cell(CellDomain.Free);
                                break;
                            default:
                                throw new Exception("Атата, не те символы");
                        }
                    }
                }
            }
            if (pl.X == -1) throw new Exception("Игрока не завезли");
            if (sk.X == -1) throw new Exception("Шкафа не нашлось. Вот бы в жизни так...");
            skaf = sk; player = pl; map = tempMap;
        }
    }
}
