using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;

namespace Jeux_de_la_vie.Avalonia.Windows
{
    public partial class Propriété_du_tableau_Window : Window
    {
        #region Constructors
        public Propriété_du_tableau_Window()
        {
            InitializeComponent();
            Définir_les_événements();

            Appliquer_Button.IsEnabled = false;
            Message_derreur_Text.Text = "";
            Propriété_changé += Propriété_du_tableau_Window_Propriété_changé;
        }
        #endregion

        #region Variables
        private int lignes;
        private int colonnes;
        private IBrush couleur_cellule;
        private IBrush couleur_tableau;

        public event EventHandler<EventArgs> Propriété_changé;
        #endregion

        #region Properties
        public int Lignes
        {
            get => lignes;
            set
            {
                Taille_lignes_TextBox.Text = value.ToString();
                lignes = value;
            }
        }

        public int Colonnes
        {
            get => colonnes;
            set
            {
                Taille_colonnes_TextBox.Text = value.ToString();
                colonnes = value;
            }
        }

        public IBrush Couleur_cellule
        {
            get => couleur_cellule;
            set
            {
                Couleur_cellule_TextBox.Text = value.ToString();
                couleur_cellule = value;
            }
        }
        public IBrush Couleur_tableau
        {
            get => couleur_tableau;
            set
            {
                Couleur_tableau_TextBox.Text = value.ToString();
                couleur_tableau = value;
            }
        }
        #endregion

        #region Methods
        public void Taille_lignes_TextBox_Input(object? sender, TextInputEventArgs e)
        {
            Appliquer_Button.IsEnabled = true;

            if (!int.TryParse(e.Text, out _))
                Error(Couleur_cellule_TextBox, "La lignes contient des caractêre invalid");
        }

        public void Taille_colonnes_TextBox_Input(object? sender, TextInputEventArgs e)
        {
            Appliquer_Button.IsEnabled = true;

            if (!int.TryParse(e.Text, out _))
                Error(Couleur_cellule_TextBox, "La colonnes contient des caractêre invalid");
        }

        public void Couleur_cellule_TextBox_Input(object? sender, TextInputEventArgs e)
        {
            Appliquer_Button.IsEnabled = true;

            try { Couleur_cellule_Border.Background = Brush.Parse(Couleur_cellule_TextBox.Text); }
            catch { Error(Couleur_cellule_TextBox, "La cellule contient des caractêre invalid"); }
        }

        public void Couleur_tableau_TextBox_Input(object? sender, TextInputEventArgs e)
        {
            Appliquer_Button.IsEnabled = true;

            try { Couleur_tableau_Border.Background = Brush.Parse(Couleur_tableau_TextBox.Text); }
            catch { Error(Couleur_tableau_TextBox, "Le tableau contient des caractêre invalid"); }
        }

        public void Appliquer_Button_Click(object sender, RoutedEventArgs e)
        {
            lignes = Convert.ToInt32(Taille_lignes_TextBox.Text);
            colonnes = Convert.ToInt32(Taille_colonnes_TextBox.Text);
            couleur_cellule = Brush.Parse(Couleur_cellule_TextBox.Text);
            couleur_tableau = Brush.Parse(Couleur_tableau_TextBox.Text);

            Paramètres_de_lapplication.Sauvegarder();

            Appliquer_Button.IsEnabled = false;
        }

        public void Fermer_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Définir_les_événements()
        {
            Taille_lignes_TextBox.AddHandler(TextInputEvent, Taille_lignes_TextBox_Input, RoutingStrategies.Tunnel);
            Taille_colonnes_TextBox.AddHandler(TextInputEvent, Taille_colonnes_TextBox_Input, RoutingStrategies.Tunnel);
            Couleur_cellule_TextBox.AddHandler(TextInputEvent, Couleur_cellule_TextBox_Input, RoutingStrategies.Tunnel);
            Couleur_tableau_TextBox.AddHandler(TextInputEvent, Couleur_tableau_TextBox_Input, RoutingStrategies.Tunnel);
        }

        private void Error(TextBox textBox, string message)
        {
            textBox.BorderBrush = Brush.Parse("Red");
            Message_derreur_Text.Text = message;

            Appliquer_Button.IsEnabled = false;
        }

        private void Propriété_du_tableau_Window_Propriété_changé(object? sender, EventArgs e)
        {
            Taille_lignes_TextBox.BorderBrush = null;
            Taille_colonnes_TextBox.BorderBrush = null;
            Couleur_cellule_TextBox.BorderBrush = null;
            Couleur_tableau_TextBox.BorderBrush = null;
        }
        #endregion
    }
}
