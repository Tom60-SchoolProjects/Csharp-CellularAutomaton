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
            tableau_initial = new bool[100, 100];

            jeu_de_la_vie = new();
            jeu_de_la_vie.TableauActualisé += Jeu_de_la_vie_TableauActualisé;
            jeu_de_la_vie.Définir_tableau(tableau_initial);
        }
        #endregion

        #region Variables
        Jeu_de_la_vie jeu_de_la_vie;
        bool[,] tableau_initial;
        #endregion

        #region Methods
        public async void Charger_tableau(object sender, RoutedEventArgs e)
        {
            // TODO: refaire

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

        public void Modifier_tableau(object sender, RoutedEventArgs e)
        {
            var fenétre_propriété_du_tableau = new Propriété_du_tableau_Window()
            {
                Lignes = Grille_de_jeu.Lignes,
                Colonnes = Grille_de_jeu.Colonnes,
            };
            fenétre_propriété_du_tableau.Show();
        }

        public void Effacer_tableau(object sender, RoutedEventArgs e)
        {
            var nouveau_tableau = new bool[tableau_initial.GetLength(0), tableau_initial.GetLength(1)];
            Définir_tableau(nouveau_tableau);
        }

        public void Généreration_aléatoire_tableau(object sender, RoutedEventArgs e)
        {
            var nouveau_tableau = Autocell.Initialiser_aléatoirement_le_tableau(tableau_initial);
            Définir_tableau(nouveau_tableau);
        }

        public void Lire_tableau(object sender, RoutedEventArgs e)
        {
            if (jeu_de_la_vie.EstDémarrer)
            {
                jeu_de_la_vie.Arrêter();
                Lecture_tableau_Btn.Content = "\uF00B";
                ToolTip.SetTip(Lecture_tableau_Btn, "Démarrer simulation");
            }
            else
            {
                jeu_de_la_vie.Démarrer();
                Lecture_tableau_Btn.Content = "\uEFD8";
                ToolTip.SetTip(Lecture_tableau_Btn, "Arréter simulation");
            }

            // Désactive une partie de l'affichage pour éviter des erreurs
            Menu_gauche_Panel.IsEnabled =
            Mode_de_jeu_Combo_box.IsEnabled =
            Cycle_Number_box.IsEnabled =
            Menu_doite_Panel.IsEnabled = !jeu_de_la_vie.EstDémarrer;
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
                Grille_de_jeu.Sauvegarder_tableau(résulta);
        }

        public void Mode_de_jeu_Combo_box_Selection_changed(object sender, SelectionChangedEventArgs e)
        {
            jeu_de_la_vie?
                .Définir_mode_de_jeu((Autocell.Mode_de_jeu)Mode_de_jeu_Combo_box.SelectedIndex);
        }

        public void Cycle_Number_box_Value_changed(object sender, int e)
        {
        }

        public void Speed_Number_box_Value_changed(object sender, int e)
        {
            jeu_de_la_vie.Définir_la_vitesse((1000 * 100) / e);
        }

        private void Jeu_de_la_vie_TableauActualisé(object? sender, bool[,] état_actuel)
        {
            //var cellules = new List<Cellule>();

            /*for (int y = 0; y < état_actuel.GetLength(0); y++)
                for (int x = 0; x < état_actuel.GetLength(1); x++)
                    cellules.Add(new(état_actuel[y, x], x, y));*/

            Grille_de_jeu.Déssiner(état_actuel);
        }

        private void Définir_tableau(bool[,] tableau)
        {
            tableau_initial = tableau;
            jeu_de_la_vie.Définir_tableau(tableau_initial);
        }
        #endregion
    }
}
