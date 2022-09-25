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
            jeu_de_la_vie.TableauActualis� += Jeu_de_la_vie_TableauActualis�;
            jeu_de_la_vie.D�finir_tableau(tableau_initial);
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

        public void Modifier_tableau(object sender, RoutedEventArgs e)
        {
            var fen�tre_propri�t�_du_tableau = new Propri�t�_du_tableau_Window()
            {
                Lignes = Grille_de_jeu.Lignes,
                Colonnes = Grille_de_jeu.Colonnes,
            };
            fen�tre_propri�t�_du_tableau.Show();
        }

        public void Effacer_tableau(object sender, RoutedEventArgs e)
        {
            var nouveau_tableau = new bool[tableau_initial.GetLength(0), tableau_initial.GetLength(1)];
            D�finir_tableau(nouveau_tableau);
        }

        public void G�n�reration_al�atoire_tableau(object sender, RoutedEventArgs e)
        {
            var nouveau_tableau = Autocell.Initialiser_al�atoirement_le_tableau(tableau_initial);
            D�finir_tableau(nouveau_tableau);
        }

        public void Lire_tableau(object sender, RoutedEventArgs e)
        {
            if (jeu_de_la_vie.EstD�marrer)
            {
                jeu_de_la_vie.Arr�ter();
                Lecture_tableau_Btn.Content = "\uF00B";
                ToolTip.SetTip(Lecture_tableau_Btn, "D�marrer simulation");
            }
            else
            {
                jeu_de_la_vie.D�marrer();
                Lecture_tableau_Btn.Content = "\uEFD8";
                ToolTip.SetTip(Lecture_tableau_Btn, "Arr�ter simulation");
            }

            // D�sactive une partie de l'affichage pour �viter des erreurs
            Menu_gauche_Panel.IsEnabled =
            Mode_de_jeu_Combo_box.IsEnabled =
            Cycle_Number_box.IsEnabled =
            Menu_doite_Panel.IsEnabled = !jeu_de_la_vie.EstD�marrer;
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
                Grille_de_jeu.Sauvegarder_tableau(r�sulta);
        }

        public void Mode_de_jeu_Combo_box_Selection_changed(object sender, SelectionChangedEventArgs e)
        {
            jeu_de_la_vie?
                .D�finir_mode_de_jeu((Autocell.Mode_de_jeu)Mode_de_jeu_Combo_box.SelectedIndex);
        }

        public void Cycle_Number_box_Value_changed(object sender, int e)
        {
        }

        public void Speed_Number_box_Value_changed(object sender, int e)
        {
            jeu_de_la_vie.D�finir_la_vitesse((1000 * 100) / e);
        }

        private void Jeu_de_la_vie_TableauActualis�(object? sender, bool[,] �tat_actuel)
        {
            //var cellules = new List<Cellule>();

            /*for (int y = 0; y < �tat_actuel.GetLength(0); y++)
                for (int x = 0; x < �tat_actuel.GetLength(1); x++)
                    cellules.Add(new(�tat_actuel[y, x], x, y));*/

            Grille_de_jeu.D�ssiner(�tat_actuel);
        }

        private void D�finir_tableau(bool[,] tableau)
        {
            tableau_initial = tableau;
            jeu_de_la_vie.D�finir_tableau(tableau_initial);
        }
        #endregion
    }
}
