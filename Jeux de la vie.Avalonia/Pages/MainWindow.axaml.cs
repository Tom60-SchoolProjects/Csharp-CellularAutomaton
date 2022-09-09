using Avalonia.Controls;
using Avalonia.Interactivity;
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

            /*Grille_de_jeu.Nouveau_tableau(100, 100);

            for (int i = 0; i < 100; i++)
            {
                Grille_de_jeu.Déssiner(i, 0);
                Grille_de_jeu.Déssiner(i, 1);
                Grille_de_jeu.Déssiner(i, 2);
                Grille_de_jeu.Déssiner(i, 3);
                Grille_de_jeu.Déssiner(i, 4);
                Grille_de_jeu.Déssiner(i, 5);
            }*/
        }

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
                    Grille_de_jeu.Charger_tableau(resulta[0]);
                else
                    Debug.WriteLine("File don't exist");
            }
        }
    }
}
