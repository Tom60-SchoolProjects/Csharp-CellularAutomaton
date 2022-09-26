using Avalonia.Controls;
using Avalonia.Interactivity;
using Jeux_de_la_vie.Avalonia.Windows;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Jeux_de_la_vie.Avalonia
{
    public partial class MainWindow : Window
    {
        private void Définir_tableau(bool[,] tableau)
        {
            tableau_initial = tableau;
            jeu_de_la_vie.Définir_tableau(tableau_initial);
            cycle_actuelle = 0;
            Reinitialiser_tableau_Btn.IsEnabled = false;
            Lecture_tableau_Btn.IsEnabled = true;
        }

        private void Démarrer_la_génération()
        {
            jeu_de_la_vie.Démarrer();
            Lecture_tableau_Btn.Content = "\uEFD8";
            ToolTip.SetTip(Lecture_tableau_Btn, "Arréter la génération");

            Reinitialiser_tableau_Btn.IsEnabled = true;

            Actualiser_le_menu();
        }

        private void Arrêter_la_génération()
        {
            jeu_de_la_vie.Arrêter();
            Lecture_tableau_Btn.Content = "\uF00B";
            ToolTip.SetTip(Lecture_tableau_Btn, "Démarrer la génération");

            Actualiser_le_menu();
        }

        private void Actualiser_le_menu()
        {
            // Désactive une partie de l'affichage pour éviter des erreurs
            Menu_gauche_Panel.IsEnabled =
            Mode_de_jeu_Combo_box.IsEnabled =
            Cycle_Number_box.IsVisible =
            Reinitialiser_tableau_Btn.IsEnabled =
            Menu_doite_Panel.IsEnabled = !jeu_de_la_vie.EstDémarrer;

            Cycle_Text_border.IsVisible = jeu_de_la_vie.EstDémarrer;
        }

        private void Ajouter_tableau(bool[,] tableau, int possition_vertical, int possition_horizontal, bool transparence = false)
        {
            var nouveau_tableau = tableau_initial;
            int vertical = tableau.GetLength(0);
            int horizontal = tableau.GetLength(1);

            if (vertical > tableau_initial.GetLength(0))
            {
                int surplus_vertical = vertical - tableau_initial.GetLength(0);
                vertical -= surplus_vertical;
            }

            if (horizontal > tableau_initial.GetLength(1))
            {
                int surplus_horizontall = horizontal - tableau_initial.GetLength(1);
                horizontal -= surplus_horizontall;
            }

            for (int x = 0; x < vertical; x++)
                for (int y = 0; y < horizontal; y++)
                    if ((transparence && tableau[x, y]) || !transparence)
                    nouveau_tableau[x + possition_vertical, y + possition_horizontal] =
                        tableau[x, y];

            Définir_tableau(nouveau_tableau);
        }

        private bool[,] Charger_BMP(string path)
        {
            var bmp = new Bitmap(path);
            var tableau = new bool[bmp.Width, bmp.Height];


            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                    tableau[x, y] = bmp.GetPixel(x, y).GetBrightness() < 0.5; // Si le pixel est sombre, c'est une cellule vivante

            return tableau;
        }

        private bool[,] Charger_CSV(string path)
        {
            bool[,] tableau;
            List<int> valeurs = new();

            using (TextFieldParser csvReader = new(path))
            {
                csvReader.CommentTokens = new string[] { "#" };
                csvReader.SetDelimiters(new string[] { ",", ";" });
                csvReader.HasFieldsEnclosedInQuotes = true;

                int taille_horizontal_maximum = 0;
                int taille_vertical_maximum = 0;

                while (!csvReader.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[]? colonnes = csvReader.ReadFields();

                    if (colonnes != null)
                    {
                        if (colonnes.Length > taille_horizontal_maximum)
                            taille_horizontal_maximum = colonnes.Length;

                        foreach (var colonne in colonnes)
                        {
                            if (int.TryParse(colonne, out var entier))
                                valeurs.Add(entier);
                            else
                                valeurs.Add(0);
                        }

                        taille_vertical_maximum++;
                    }
                }

                tableau = new bool[taille_horizontal_maximum, taille_vertical_maximum];

                int x = 0;
                int y = 0;
                foreach (var valeur in valeurs)
                {
                    if (x >= taille_horizontal_maximum)
                    {
                        y++;
                        x = 0;
                    }

                    tableau[x, y] = valeur == 1;

                    x++;
                }
            }

            return tableau;
        }
    }
}
