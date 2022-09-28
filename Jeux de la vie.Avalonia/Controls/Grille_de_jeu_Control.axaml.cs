using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System.Drawing.Imaging;
using System.IO;
using SystemBitmap = System.Drawing.Bitmap;
using Graphics = System.Drawing.Graphics;
using SolidBrush = System.Drawing.SolidBrush;
using Avalonia.Threading;
using System.Collections.Generic;
using System;
using Avalonia.Media;
using Jeux_de_la_vie.Avalonia.Extensions;

namespace Jeux_de_la_vie.Avalonia.Controls
{
    public partial class Grille_de_jeu_Control : UserControl
    {
        #region Constructors
        public Grille_de_jeu_Control()
        {
            InitializeComponent();

            Taille_ligne = 1;
            Couleur_cellule = Colors.Black;
            Couleur_tableau = Colors.White;
            grille_source = new SystemBitmap(1, 1);
        }
        #endregion

        #region Variables
        private SystemBitmap grille_source;
        private Color Couleur_cellule;
        private Color Couleur_tableau;
        private bool En_cours_dutilisation;
        #endregion

        #region Properties
        private Bitmap Grille_de_jeu
        {
            get => Grille_de_jeu_Img.Source as Bitmap ?? new Bitmap(new MemoryStream());
            set => Grille_de_jeu_Img.Source = value;
        }

        public double Taille_ligne
        {
            get => Grille_de_jeu_Brd.BorderThickness.Top;
            private set =>
                Grille_de_jeu_Brd.BorderThickness = new Thickness(value);
        }

        public int Lignes => grille_source.Height;
        public int Colonnes => grille_source.Width;
        #endregion

        #region Methods
        public void Ajouter_tableau(string nom_du_fichier)
        {
            var nouveau_tableau = new SystemBitmap(nom_du_fichier);

            using (Graphics g = Graphics.FromImage(grille_source))
            {
                g.DrawImage(nouveau_tableau, 0, 0);;
            }

            Recharger_grille();
        }

        public void Nouveau_tableau(int taille_vertical, int taille_horizontal)
        {
            if (taille_vertical < 1)
                taille_vertical = 1;

            if (taille_horizontal < 1)
                taille_horizontal = 1;

            grille_source = new SystemBitmap(taille_vertical, taille_horizontal);

            using (var gfx = Graphics.FromImage(grille_source))
            using (var brush = new SolidBrush(Couleur_tableau.ToNativeColor()))
            {
                gfx.FillRectangle(brush, 0, 0, taille_vertical, taille_horizontal);
            }

            Recharger_grille();
        }

        public void Sauvegarder_tableau(string chemin_du_fichier) => Grille_de_jeu.Save(chemin_du_fichier);

        public void Définir_couleur(Color tableau, Color celulle)
        {
            System.Drawing.Color pixel;

            for (int x = 0; x < grille_source.Width; x++)
                for (int y = 0; y < grille_source.Height; y++)
                {
                    pixel = grille_source.GetPixel(x, y);

                    grille_source.SetPixel(
                        x, y,
                        pixel == Couleur_cellule.ToNativeColor() ? celulle.ToNativeColor() : tableau.ToNativeColor());
                }

            Couleur_tableau = tableau;
            Couleur_cellule = celulle;
        }

        public bool Déssiner(bool cellule, int position_vertical, int position_horizontal)
        {
            if (En_cours_dutilisation)
                return false;

            En_cours_dutilisation = true;

            grille_source.SetPixel(
                position_vertical,
                position_horizontal,
                cellule ? Couleur_cellule.ToNativeColor() : Couleur_tableau.ToNativeColor());

            En_cours_dutilisation = false;
            Recharger_grille();

            return true;
        }

        public bool Déssiner(bool[,] tableau)
        {
            if (En_cours_dutilisation)
                return false;

            En_cours_dutilisation = true;

            for (int x = 0; x < tableau.GetLength(0); x++)
                for (int y = 0; y < tableau.GetLength(1); y++)
                    grille_source.SetPixel(x, y,
                        tableau[x, y] ? Couleur_cellule.ToNativeColor() : Couleur_tableau.ToNativeColor());

            En_cours_dutilisation = false;
            Recharger_grille();

            return true;
        }

        private void Recharger_grille() => Dispatcher.UIThread.Post(() =>
        {
            if (En_cours_dutilisation)
                return;

            En_cours_dutilisation = true;
            using var memory = new MemoryStream();

            grille_source.Save(memory, ImageFormat.Bmp);

            memory.Position = 0;
            Grille_de_jeu = new Bitmap(memory);

            En_cours_dutilisation = false;
            return;
        });
        #endregion
    }
}
