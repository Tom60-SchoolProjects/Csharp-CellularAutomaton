using System;

namespace Jeux_de_la_vie
{
    [Obsolete]
    public class Automate
    {
        public static bool[,] Autocell(bool[,] état_actuel)
        {
            état_actuel = état_suivant(état_actuel, Verifier_voisin(état_actuel));

            return état_actuel;
        }

        public static int[,] Verifier_voisin(in bool[,] état_actuel)
        {
            var voisins = new int[état_actuel.GetLength(0), état_actuel.GetLength(1)];

            for (int y = 0; y < état_actuel.GetLength(0); y++)
                for (int x = 0; x < état_actuel.GetLength(1); x++)
                {
                    var i = Indices(x, y, état_actuel);

                    int nombre_voisins = 0;
                        nombre_voisins += état_actuel[i[2], i[0]] ? 1 : 0;
                    if (i[1] <= 100)
                        nombre_voisins += état_actuel[i[2], i[1]] ? 1 : 0;
                    nombre_voisins += état_actuel[i[3], i[0]] ? 1 : 0;
                    if (i[3] <= 100)
                        nombre_voisins += état_actuel[i[3], i[1]] ? 1 : 0;
                    nombre_voisins -= état_actuel[y, x] ? 1 : 0;

                    voisins[y, x] = nombre_voisins;
                }

            return voisins;
        }

        public static int[] Indices(int x, int y, in bool[,] état_actuel)
        {
            var i = new int[] { x - 1, x + 2, y - 1, y + 2 };

            if (x == 0)
                i[0] = x;

            if (x == état_actuel.GetLength(1))
                i[1] = x + 1;

            if (y == 0)
                i[2] = y;

            if (y == état_actuel.GetLength(0))
                i[3] = y + 1;

            return i;
        }

        public static bool[,] état_suivant(in bool[,] état_actuel, in int[,] voisins)
        {
            var suivant = new bool[état_actuel.GetLength(0), état_actuel.GetLength(1)];

            for (int y = 0; y < état_actuel.GetLength(0); y++)
                for (int x = 0; x < état_actuel.GetLength(1); x++)
                {
                    if (état_actuel[y, x] == true)
                    {
                        if (voisins[y, x] == 2)
                            suivant[y, x] = true;

                        else
                            suivant[y, x] = false;
                    }
                    else
                    {
                        if (voisins[y, x] == 3)
                            suivant[y, x] = true;
                        else
                            suivant[y, x] = false;
                    }
                }

            return suivant;
        }
    }
}