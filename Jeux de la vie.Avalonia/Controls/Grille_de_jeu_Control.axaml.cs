using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.ComponentModel;

namespace Jeux_de_la_vie.Avalonia
{
    public partial class Grille_de_jeu_Control : UserControl, INotifyPropertyChanged
    {
        private Size taille_cellule;

        public new event PropertyChangedEventHandler? PropertyChanged;

        double taille_vertical => Taille_ligne * 2 + taille_cellule.Height * Grille_source.Size.Height;
        double taille_horizontal => Taille_ligne * 2 + taille_cellule.Width * Grille_source.Size.Width;
        public Bitmap? Grille_source
        {
            get => Grille_de_jeu_Img.Source as Bitmap;
            private set =>
                Grille_de_jeu_Img.Source = value;
        }
        public double Taille_ligne
        {
            get => Grille_de_jeu_Brd.BorderThickness.Top;
            private set =>
                Grille_de_jeu_Brd.BorderThickness = new Thickness(value);
        }

        public Grille_de_jeu_Control()
        {
            InitializeComponent();

            //grille_source = new Bitmap();
            taille_cellule = new Size(4, 4);
            Taille_ligne = 1;
        }

        public void Charger_tableau(string nom_du_fichier)
        {
            Grille_source = new Bitmap(nom_du_fichier);
        }

        /*public void Nouveau_tableau(int taille_vertical, int taille_horizontal)
        {
            Grille_source = new Bitmap(taille_vertical, taille_horizontal);
        }

        public void Déssiner(int position_vertical, int position_horizontal)
        {
            Grille_source.SetPixel(
                position_vertical,
                position_horizontal,
                Color.Black );
        }*/
    }
}
