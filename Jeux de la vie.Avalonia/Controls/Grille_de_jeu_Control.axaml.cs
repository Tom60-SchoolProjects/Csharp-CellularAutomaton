using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using SystemBitmap = System.Drawing.Bitmap;
using Color = System.Drawing.Color;
using Graphics = System.Drawing.Graphics;
using SolidBrush = System.Drawing.SolidBrush;

namespace Jeux_de_la_vie.Avalonia.Controls
{
    public partial class Grille_de_jeu_Control : UserControl
    {
        #region Constructors
        public Grille_de_jeu_Control()
        {
            InitializeComponent();

            //grille_source = new Bitmap();
            taille_cellule = new Size(4, 4);
            Taille_ligne = 1;
            Couleur_cellule = Color.Black;
            Couleur_tableau = Color.White;

            Nouveau_tableau(100, 100);
        }
        #endregion

        #region Variables
        private Size taille_cellule;
        private SystemBitmap grille_source;
        private Color Couleur_cellule;
        private Color Couleur_tableau;
        #endregion

        #region Properties
        private double taille_vertical => Taille_ligne * 2 + taille_cellule.Height * Grille_de_jeu.Size.Height;
        private double taille_horizontal => Taille_ligne * 2 + taille_cellule.Width * Grille_de_jeu.Size.Width;

        public Bitmap Grille_de_jeu
        {
            get => Grille_de_jeu_Img.Source as Bitmap ?? new Bitmap(new MemoryStream());
            private set =>
                Grille_de_jeu_Img.Source = value;
        }

        public double Taille_ligne
        {
            get => Grille_de_jeu_Brd.BorderThickness.Top;
            private set =>
                Grille_de_jeu_Brd.BorderThickness = new Thickness(value);
        }
        #endregion

        #region Methods
        public void Ajouter_tableau(string nom_du_fichier)
        {
            grille_source = new SystemBitmap(nom_du_fichier);

            Recharger_grille();
        }

        public void Nouveau_tableau(int taille_vertical, int taille_horizontal)
        {
            grille_source = new SystemBitmap(taille_vertical, taille_horizontal);
            using (var gfx = Graphics.FromImage(grille_source))
            using (var brush = new SolidBrush(Couleur_tableau))
            {
                gfx.FillRectangle(brush, 0, 0, taille_vertical, taille_horizontal);
            }

            Recharger_grille();
        }

        public void Déssiner(int position_vertical, int position_horizontal)
        {
            grille_source.SetPixel(
                position_vertical,
                position_horizontal,
                Couleur_cellule);

            Recharger_grille();
        }

        private void Recharger_grille()
        {
            using (var memory = new MemoryStream())
            {
                grille_source.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;

                Grille_de_jeu = new Bitmap(memory);
            }
        }
        #endregion
    }
}
