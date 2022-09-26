using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
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
            Définir_tableau(tableau_initial);
        }
        #endregion

        #region Variables
        Jeu_de_la_vie jeu_de_la_vie;
        bool[,] tableau_initial;
        int cycle_actuelle = 0;
        int cycle_maximum = 0;
        #endregion

        #region Methods
        public async void Charger_tableau(object sender, RoutedEventArgs e)
        {
            // TODO: refaire

            // Création du sélecteur de fichier
            var dialog_de_fichier = new OpenFileDialog()
            {
                Title = "Selection un modéle de tableau",
                Filters = new List<FileDialogFilter> {
                    new FileDialogFilter() { Extensions = new List<string> { "bmp", "csv" }, Name = "Tout format compatible" },
                    new FileDialogFilter() { Extensions = new List<string> { "bmp" }, Name = "Fichier Bitmap" },
                    new FileDialogFilter() { Extensions = new List<string> { "csv" }, Name = "Fichier Comma-separated values" },
                },
                AllowMultiple = false
            };

            // Afficher la fenétre de sélection de fichier
            var resulta = await dialog_de_fichier.ShowAsync(this);

            // Charger le fichier selectionner si il existe
            if (resulta != null && resulta.Length > 0)
            {
                if (File.Exists(resulta[0]))
                {
                    switch (Path.GetExtension(resulta[0]).ToLower())
                    {
                        case ".bmp":
                            var tableau_bmp = Charger_BMP(resulta[0]);
                            Ajouter_tableau(tableau_bmp, 0, 0, true);
                            break;

                        case ".csv":
                            var tableau_csv = Charger_CSV(resulta[0]);
                            Ajouter_tableau(tableau_csv, 0, 0, true);
                            break;

                        default:
                            Debug.WriteLine("L'extension n'est pas connus");
                            break;
                    }
                }
                else
                    Debug.WriteLine("Le fichier n'existe pas");
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
                Arrêter_la_génération();
            else
                Démarrer_la_génération();

        }
        public void Reinitialiser_tableau(object sender, RoutedEventArgs e)
        {
            Définir_tableau(tableau_initial);
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
            cycle_maximum = e;
        }

        public void Speed_Number_box_Value_changed(object sender, int e)
        {
            jeu_de_la_vie.Définir_la_vitesse(10000 / e);
        }

        private void Jeu_de_la_vie_TableauActualisé(object? sender, bool[,] état_actuel)
        {
            Grille_de_jeu.Déssiner(état_actuel);

            cycle_actuelle++;
            Dispatcher.UIThread.Post(() =>
            {
                Cycle_Text.Text = $"{cycle_actuelle} / {cycle_maximum}";

                if (cycle_maximum != 0 && cycle_actuelle > cycle_maximum)
                {
                    Arrêter_la_génération();
                    Lecture_tableau_Btn.IsEnabled = false;
                }
            });
        }
        #endregion
    }
}
