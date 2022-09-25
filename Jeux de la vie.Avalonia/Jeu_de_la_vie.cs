using System;
using System.Timers;

namespace Jeux_de_la_vie.Avalonia
{
    internal class Jeu_de_la_vie
    {
        #region Constructors
        public Jeu_de_la_vie()
        {
            timer = new();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;

            tableau = new bool[0, 0];
        }
        #endregion

        #region Variables
        Timer timer;
        bool[,] tableau;
        bool estDémarrer;

        public event EventHandler<bool[,]>? TableauActualisé;
        #endregion

        #region Properties
        public bool EstDémarrer => estDémarrer;
        #endregion

        #region Methods
        /// <summary>
        /// Démarre le jeu
        /// </summary>
        public void Démarrer(bool[,] tableau_initial)
        {
            tableau = tableau_initial;

            timer.Start();
            estDémarrer = true;
        }

        /// <summary>
        /// Arrête le jeu
        /// </summary>
        public void Arrêter()
        {
            timer.Stop();
            estDémarrer = false;
        }

        /// <summary>
        /// Définit la vitesse du jeu en milisecondes
        /// </summary>
        /// <param name="milisecondes"></param>
        public void Définir_la_vitesse(double milisecondes) => timer.Interval = milisecondes;

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            tableau = Automate.Autocell(tableau);

            TableauActualisé?.Invoke(this, tableau);
        }
        #endregion
    }
}
