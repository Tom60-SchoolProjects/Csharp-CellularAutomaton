using Avalonia.Controls;
using Avalonia.Interactivity;
using Jeux_de_la_vie.Avalonia.Windows;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Jeux_de_la_vie.Avalonia
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Grille_de_jeu.Nouveau_tableau(150, 100);

            for (int i = 0; i < 100; i++)
            {
                Grille_de_jeu.D�ssiner(i, 0);
                Grille_de_jeu.D�ssiner(i, i);
                Grille_de_jeu.D�ssiner(0, i);
            }
        }

        public async void Charger_tableau(object sender, RoutedEventArgs e)
        {
            // Cr�ation du s�lecteur de fichier
            var dialog_de_fichier = new OpenFileDialog()
            {
                Title = "Selection un tableau au format BMP",
                Filters = new List<FileDialogFilter> { new FileDialogFilter() { Extensions = new List<string> { "bmp" }, Name = "Fichier Bitmap" } },
                AllowMultiple = false
            };

            // Afficher la fen�tre de s�lection de fichier
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
            var fen�tre_propri�t�_du_tableau = new Propri�t�_du_tableau_Window()
            {
                Lignes = (int)Grille_de_jeu.Grille_de_jeu.Size.Height,
                Colonnes = (int)Grille_de_jeu.Grille_de_jeu.Size.Width,
            };
            fen�tre_propri�t�_du_tableau.Show();
        }

        public async void Lire_tableau(object sender, RoutedEventArgs e)
        {

        }

        public async void Sauvegarder_tableau(object sender, RoutedEventArgs e)
        {
            var dialog_de_dossier = new SaveFileDialog()
            {
                Title = "Selectionnez un emplacement o� sauvegarder le tableau",
                DefaultExtension = ".bmp",
                Filters = new List<FileDialogFilter> { new FileDialogFilter() { Extensions = new List<string> { "bmp" }, Name = "Fichier Bitmap" } },
                InitialFileName = "Mon tableau",
            };
            var r�sulta = await dialog_de_dossier.ShowAsync(this);

            if (r�sulta != null)
                Grille_de_jeu.Grille_de_jeu.Save(r�sulta);
        }

        public async void Cycle_Number_box_Value_changed(object sender, int e)
        {

        }

        public async void Speed_Number_box_Value_changed(object sender, int e)
        {
        }
    }
}
