using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Jeux_de_la_vie.Avalonia.Extensions;
using System;

namespace Jeux_de_la_vie.Avalonia.Windows
{
    public partial class Propriété_du_tableau_Window : Window
    {
        #region Constructors
        public Propriété_du_tableau_Window()
        {
            InitializeComponent();
            //Définir_les_événements();

            Appliquer_Button.IsEnabled = false;
            Message_derreur_Text.Text = "";
            Propriété_changé += Propriété_du_tableau_Window_Propriété_changé;

            Lignes = Paramètres_de_lapplication.Taille_tableau_horizontal;
            Colonnes = Paramètres_de_lapplication.Taille_tableau_vertical;
            Couleur_cellule = new SolidColorBrush(Color.FromUInt32(Paramètres_de_lapplication.Couleur_celulle));
            Couleur_tableau = new SolidColorBrush(Color.FromUInt32(Paramètres_de_lapplication.Couleur_tableau));
        }
        #endregion

        #region Variables
        private uint? lignes;
        private uint? colonnes;
        private IBrush? couleur_cellule;
        private IBrush? couleur_tableau;

        public event EventHandler<EventArgs>? Propriété_changé;
        #endregion

        #region Properties
        /// <summary> X </summary>
        public uint Lignes
        {
            get => lignes ?? 0;
            set
            {
                Taille_lignes_TextBox.Text = value.ToString();
                lignes = value;
            }
        }

        /// <summary> Y </summary>
        public uint Colonnes
        {
            get => colonnes ?? 0;
            set
            {
                Taille_colonnes_TextBox.Text = value.ToString();
                colonnes = value;
            }
        }

        public IBrush Couleur_cellule
        {
            get => couleur_cellule ?? Brushes.Black;
            set
            {
                Couleur_cellule_TextBox.Text = value.ToString();
                couleur_cellule = value;
                Couleur_cellule_Border.Background = value;
            }
        }
        public IBrush Couleur_tableau
        {
            get => couleur_tableau ?? Brushes.White;
            set
            {
                Couleur_tableau_TextBox.Text = value.ToString();
                couleur_tableau = value;
                Couleur_tableau_Border.Background = value;
            }
        }
        #endregion

        #region Methods
        public void Taille_lignes_TextBox_KeyUp(object? sender, KeyEventArgs e)
        {
            Appliquer_Button.IsEnabled = true;

            if (uint.TryParse(Taille_lignes_TextBox.Text, out var lignes))
            {
                this.lignes = lignes;
                Taille_lignes_TextBox.BorderBrush = Brushes.Gray;
            }
            else
                Error(Taille_lignes_TextBox, "La lignes contient des caractêre invalid");
        }

        public void Taille_colonnes_TextBox_KeyUp(object? sender, KeyEventArgs e)
        {
            Appliquer_Button.IsEnabled = true;

            if (uint.TryParse(Taille_colonnes_TextBox.Text, out var colonnes))
            {
                this.colonnes = colonnes;
                Taille_colonnes_TextBox.BorderBrush = Brushes.Gray;
            }
            else
                Error(Taille_colonnes_TextBox, "La colonnes contient des caractêre invalid");
        }

        public void Couleur_cellule_TextBox_KeyUo(object? sender, KeyEventArgs e)
        {
            Appliquer_Button.IsEnabled = true;

            try
            {
                couleur_cellule = Brush.Parse(Couleur_cellule_TextBox.Text);
                Couleur_cellule_TextBox.BorderBrush = Brushes.Gray;
                Couleur_cellule_Border.Background = couleur_cellule;
            }
            catch { Error(Couleur_cellule_TextBox, "La cellule contient des caractêre invalid"); }
        }

        public void Couleur_tableau_TextBox_KeyUp(object? sender, KeyEventArgs e)
        {
            Appliquer_Button.IsEnabled = true;

            try
            {
                couleur_tableau = Brush.Parse(Couleur_tableau_TextBox.Text);
                Couleur_tableau_TextBox.BorderBrush = Brushes.Gray;
                Couleur_tableau_Border.Background = couleur_tableau;
            }
            catch { Error(Couleur_tableau_TextBox, "Le tableau contient des caractêre invalid"); }
        }

        public void Appliquer_Button_Click(object sender, RoutedEventArgs e)
        {
            if (lignes != null)
                Paramètres_de_lapplication.Taille_tableau_horizontal = lignes.Value;
            if (colonnes != null)
                Paramètres_de_lapplication.Taille_tableau_vertical = colonnes.Value;
            if (couleur_cellule != null)
                Paramètres_de_lapplication.Couleur_celulle = couleur_cellule.ToColor().ToUint32();
            if (couleur_tableau != null)
                Paramètres_de_lapplication.Couleur_tableau = couleur_tableau.ToColor().ToUint32();

            Paramètres_de_lapplication.Sauvegarder();

            Propriété_changé?.Invoke(this, new EventArgs());

            Appliquer_Button.IsEnabled = false;
        }

        public void Fermer_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /*private void Définir_les_événements()
        {
            Taille_lignes_TextBox.AddHandler(TextInputEvent, Taille_lignes_TextBox_Input, RoutingStrategies.Tunnel);
            Taille_colonnes_TextBox.AddHandler(TextInputEvent, Taille_colonnes_TextBox_Input, RoutingStrategies.Tunnel);
            Couleur_cellule_TextBox.AddHandler(TextInputEvent, Couleur_cellule_TextBox_Input, RoutingStrategies.Tunnel);
            Couleur_tableau_TextBox.AddHandler(TextInputEvent, Couleur_tableau_TextBox_Input, RoutingStrategies.Tunnel);
        }*/

        private void Error(TextBox textBox, string message)
        {
            textBox.BorderBrush = Brush.Parse("Red");
            Message_derreur_Text.Text = message;

            Appliquer_Button.IsEnabled = false;
        }

        private void Propriété_du_tableau_Window_Propriété_changé(object? sender, EventArgs e)
        {
            /*Taille_lignes_TextBox.BorderBrush = Brushes.Gray;
            Taille_colonnes_TextBox.BorderBrush = Brushes.Gray;
            Couleur_cellule_TextBox.BorderBrush = Brushes.Gray;
            Couleur_tableau_TextBox.BorderBrush = Brushes.Gray;*/
        }
        #endregion
    }
}
