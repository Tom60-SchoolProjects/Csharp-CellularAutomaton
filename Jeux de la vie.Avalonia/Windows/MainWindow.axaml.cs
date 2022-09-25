using Avalonia.Controls;
using Avalonia.Interactivity;
using Jeux_de_la_vie.Avalonia.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Jeux_de_la_vie.Avalonia
{
    public partial class MainWindow : Window
    {
        #region Constructors
        public MainWindow()
        {
            InitializeComponent();

            Grille_de_jeu.Nouveau_tableau(100, 100);

            var tableau = new bool[100, 100];

            tableau[10, 10] = true;
            tableau[10, 11] = true;
            tableau[10, 12] = true;

            jeu_de_la_vie = new();
            jeu_de_la_vie.TableauActualisé += Jeu_de_la_vie_TableauActualisé;
        }
        #endregion

        #region Variables
        Jeu_de_la_vie jeu_de_la_vie;
        #endregion

        #region Methods
        public async void Charger_tableau(object sender, RoutedEventArgs e)
        {
            // Création du sélecteur de fichier
            var dialog_de_fichier = new OpenFileDialog()
            {
                Title = "Selection un tableau au format BMP",
                Filters = new List<FileDialogFilter> { new FileDialogFilter() { Extensions = new List<string> { "bmp" }, Name = "Fichier Bitmap" } },
                AllowMultiple = false
            };

            // Afficher la fenétre de sélection de fichier
            var resulta = await dialog_de_fichier.ShowAsync(this);

            // Charger le fichier selectionner si il existe
            if (resulta != null && resulta.Length > 0)
            {
                if (File.Exists(resulta[0]))
                    Grille_de_jeu.Ajouter_tableau(resulta[0]);
                else
                    Debug.WriteLine("File don't exist");
            }
        }

        public async void Modifier_tableau(object sender, RoutedEventArgs e)
        {
            var fenétre_propriété_du_tableau = new Propriété_du_tableau_Window()
            {
                Lignes = (int)Grille_de_jeu.Grille_de_jeu.Size.Height,
                Colonnes = (int)Grille_de_jeu.Grille_de_jeu.Size.Width,
            };
            fenétre_propriété_du_tableau.Show();
        }

        public async void Lire_tableau(object sender, RoutedEventArgs e)
        {
            if (jeu_de_la_vie.EstDémarrer)
            {
                jeu_de_la_vie.Arrêter();
                Lecture_tableau_Btn.Content = "\uF00B";
                ToolTip.SetTip(Lecture_tableau_Btn, "Démarrer simulation");
            }
            else
            {
                var tableau = Grille_de_jeu.Exporter_le_tableau();

                for (int y = 0; y < tableau.GetLength(0); y++)
                {
                    string line = "";
                    for (int x = 0; x < tableau.GetLength(1); x++)
                    {
                        line += tableau[y, x] ? "X" : "-";
                    }

                    Debug.WriteLine(line);
                }

                jeu_de_la_vie.Démarrer(tableau);
                Lecture_tableau_Btn.Content = "\uEFD8";
                ToolTip.SetTip(Lecture_tableau_Btn, "Arréter simulation");
            }
        }

        public async void Sauvegarder_tableau(object sender, RoutedEventArgs e)
        {
            var dialog_de_dossier = new SaveFileDialog()
            {
                Title = "Selectionnez un emplacement où sauvegarder le tableau",
                DefaultExtension = ".bmp",
                Filters = new List<FileDialogFilter> { new FileDialogFilter() { Extensions = new List<string> { "bmp" }, Name = "Fichier Bitmap" } },
                InitialFileName = "Mon tableau",
            };
            var résulta = await dialog_de_dossier.ShowAsync(this);

            if (résulta != null)
                Grille_de_jeu.Grille_de_jeu.Save(résulta);
        }

        public async void Cycle_Number_box_Value_changed(object sender, int e)
        {

        }

        public async void Speed_Number_box_Value_changed(object sender, int e)
        {
        }

        private void Jeu_de_la_vie_TableauActualisé(object? sender, bool[,] état_actuel)
        {
            for (int y = 0; y > état_actuel.GetLength(0); y++)
                for (int x = 0; x > état_actuel.GetLength(1); x++)
                {
                    Grille_de_jeu.Déssiner(état_actuel[y, x], x, y);
                }
        }
        #endregion
    }
}
