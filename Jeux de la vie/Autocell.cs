namespace Jeux_de_la_vie
{
    public static class Autocell
    {
        public enum Mode_de_jeu
        {
            Jeu_de_la_vie,
            Day_and_Night
        }

        /// <summary>
        /// Run a round
        /// </summary>
        /// <param name="gameboard_init"></param>
        /// <param name="table_size"></param>
        public static bool[,] Cycle_de_jeu(bool[,] tableau_initial, Mode_de_jeu mode_de_jeu)
        {
            var tableau_suivant = tableau_initial;
            bool[,] neighbourhood_matrix;
            int cpt;

            for (int x = 0; x < tableau_initial.GetLength(0); x++)
            {
                for (int y = 0; y < tableau_initial.GetLength(1); y++)
                {
                    neighbourhood_matrix = Neighbourhood_Matrix(tableau_initial, x, y);
                    cpt = 0;

                    foreach (var elem in neighbourhood_matrix)
                        cpt += elem ? 1 : 0;

                    if (tableau_initial[x, y] == true)
                        cpt--;

                    switch (mode_de_jeu)
                    {
                        case Mode_de_jeu.Jeu_de_la_vie:
                            if (tableau_initial[x, y] && cpt != 2 && cpt != 3)
                                tableau_suivant[x, y] = false;
                            else if (!tableau_initial[x, y] && (cpt == 3))
                                tableau_suivant[x, y] = true;
                            break;

                        case Mode_de_jeu.Day_and_Night:
                            if (tableau_initial[x, y] && cpt != 3 && cpt != 6 && cpt != 7 && cpt != 8)
                                tableau_suivant[x, y] = true;
                            else if (!tableau_initial[x, y] && (cpt != 0 && cpt != 1 && cpt != 2 && cpt != 5 && cpt != 9))
                                tableau_suivant[x, y] = false;
                            break;
                    }

                }
            }

            return tableau_suivant;
        }

        /// <summary>
        /// Create the neighbourhood matrix of the cell indexed
        /// </summary>
        /// <param name="gameboard"></param>
        /// <param name="table_size"></param>
        /// <param name="x_index"></param>
        /// <param name="y_index"></param>
        /// <returns>The neighbourhood matrix</returns>
        private static bool[,] Neighbourhood_Matrix(bool[,] gameboard, int x_index, int y_index)
        {
            // Pas de test sur l'existence de la cellule dans le gameboard car cela sera gérer
            // par des clics à la souris sur les cellules directement
            var neighbourhood_matrix = new bool[3, 3];

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    // Put 0 if the index is outside the bounds of the gameboard
                    if (x_index - 1 + x < 0 || x_index - 1 + x > gameboard.GetLength(0) - 1
                        || y_index - 1 + y < 0 || y_index - 1 + y > gameboard.GetLength(1) - 1)
                        neighbourhood_matrix[x, y] = false;
                    // Put the value of the gameboard in the neighbourhood_matrix
                    else
                        neighbourhood_matrix[x, y] = gameboard[x_index - 1 + x, y_index - 1 + y];
                }
            }

            return neighbourhood_matrix;
        }

        /// <summary>
        /// Randomly initialize the gameboard
        /// </summary>
        /// <param name="table"></param>
        /// <param name="table_size"></param>
        public static bool[,] Initialiser_aléatoirement_le_tableau(bool[,] table)
        {
            var random = new Random();

            for (int x = 0; x < table.GetLength(0); x++)
                for (int y = 0; y < table.GetLength(1); y++)
                    table[x, y] = random.Next(2) == 1;

            return table;
        }
    }
}
