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

            Définir_la_vitesse(1000);
        }
        #endregion

        #region Variables
        private readonly Timer timer;
        private bool[,]? tableau;
        private Autocell.Mode_de_jeu mode_de_jeu;

        public event EventHandler<bool[,]>? TableauActualisé;
        #endregion

        #region Properties
        public bool EstDémarrer { get; private set; }
        #endregion

        #region Methods

        /// <summary>
        /// Définit un tableau pour la génération
        /// </summary>
        /// <param name="tableau"></param>
        public void Définir_tableau(bool[,] tableau)
        {
            if (EstDémarrer)
                Arrêter();

            this.tableau = tableau;
            TableauActualisé?.Invoke(this, tableau);
        }

        public void Définir_mode_de_jeu(Autocell.Mode_de_jeu mode_de_jeu) =>
            this.mode_de_jeu = mode_de_jeu;

        /// <summary>
        /// Démarre/Reprend la génération
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Démarrer()
        {
            if (tableau == null)
                throw new Exception("La génération à était démarrer sans aucun tableau donnée. (On peut pas généré du vide)");

            timer.Start();
            EstDémarrer = true;
        }

        /// <summary>
        /// Arrête la génération
        /// </summary>
        public void Arrêter()
        {
            timer.Stop();
            EstDémarrer = false;
        }

        /// <summary>
        /// Définit la vitesse du jeu en milisecondes
        /// </summary>
        /// <param name="milisecondes"></param>
        public void Définir_la_vitesse(double milisecondes) => timer.Interval = milisecondes;

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            tableau = Autocell.Cycle_de_jeu(tableau!, mode_de_jeu);

            TableauActualisé?.Invoke(this, tableau);
        }
        #endregion
    }
}
