using System.Diagnostics.CodeAnalysis;

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
        /// Exécuter un cycle
        /// </summary>
        /// <param name="tableau_initial"></param>
        public static bool[,] Cycle_de_jeu(bool[,] tableau_initial, Mode_de_jeu mode_de_jeu)
        {
            bool[,] tableau_suivant = (bool[,])tableau_initial.Clone();
            bool[,] tableau_voisins;
            int compteur;

            for (int x = 0; x < tableau_initial.GetLength(0); x++)
            {
                for (int y = 0; y < tableau_initial.GetLength(1); y++)
                {
                    tableau_voisins = Tableau_voisins(tableau_initial, x, y);
                    compteur = 0;

                    foreach (var élément in tableau_voisins)
                        compteur += élément ? 1 : 0;

                    if (tableau_initial[x, y] == true)
                        compteur--;

                    tableau_suivant[x, y] = mode_de_jeu switch
                    {
                        Mode_de_jeu.Day_and_Night => Calcul_day_and_night(tableau_initial[x, y], compteur),
                        _ or Mode_de_jeu.Jeu_de_la_vie => Calcul_jeu_de_la_vie(tableau_initial[x, y], compteur),
                    };

                }
            }

            return tableau_suivant;
        }

        /// <summary> 
        /// Si tableau_initial[x, y] et compteur n’est pas égale à 2 et compteur n’est pas égale à 3 faire
        /// tableau_suivant[x, y] = false
        /// Sinon si
        /// </summary>
        /// <param name="cellule"></param>
        /// <param name="compteur"></param>
        /// <returns></returns>
        private static bool Calcul_jeu_de_la_vie(bool cellule, int compteur)
        {
            if (cellule && compteur != 2 && compteur != 3)
                return false;
            else if (!cellule && compteur == 3)
                return true;
            else
                return cellule;
        }

        private static bool Calcul_day_and_night(bool cellule, int compteur)
        {
            if (cellule && compteur != 3 && compteur != 6 && compteur != 7 && compteur != 8)
                return false;
            else if (!cellule && compteur != 0 && compteur != 1 && compteur != 2 && compteur != 5 && compteur != 9)
                return true;
            else
                return cellule;
        }

        /// <summary>
        /// Créer la matrice de voisinage de la cellule indexée
        /// </summary>
        /// <param name="tableau_initial"></param>
        /// <param name="index_x"></param>
        /// <param name="index_y"></param>
        /// <returns>Le tableau des voisins</returns>
        private static bool[,] Tableau_voisins(bool[,] tableau_initial, int index_x, int index_y)
        {
            // Pas de test sur l'existence de la cellule dans le gameboard car cela sera gérer
            // par des clics à la souris sur les cellules directement
            var neighbourhood_matrix = new bool[3, 3];

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    // Put 0 if the index is outside the bounds of the gameboard
                    if (index_x - 1 + x < 0 || index_x - 1 + x > tableau_initial.GetLength(0) - 1
                        || index_y - 1 + y < 0 || index_y - 1 + y > tableau_initial.GetLength(1) - 1)
                        neighbourhood_matrix[x, y] = false;
                    // Put the value of the gameboard in the neighbourhood_matrix
                    else
                        neighbourhood_matrix[x, y] = tableau_initial[index_x - 1 + x, index_y - 1 + y];
                }
            }

            return neighbourhood_matrix;
        }

        /// <summary>
        /// Initialiser aléatoirement le plateau de jeu
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
