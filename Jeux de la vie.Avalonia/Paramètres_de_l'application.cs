using Avalonia.Media;
using System;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace Jeux_de_la_vie.Avalonia
{
    internal class Paramètres_de_lapplication
    {
        #region Variables
        /// <summary> Y </summary>
        public static uint Taille_tableau_vertical
        {
            get => instance._Taille_tableau_vertical;
            set => instance._Taille_tableau_vertical = value;
        }
        /// <summary> X </summary>
        public static uint Taille_tableau_horizontal
        {
            get => instance._Taille_tableau_horizontal;
            set => instance._Taille_tableau_horizontal = value;
        }
        public static uint Couleur_tableau
        {
            get => instance._Couleur_tableau;
            set => instance._Couleur_tableau = value;
        }
        public static uint Couleur_celulle
        {
            get => instance._Couleur_celulle;
            set => instance._Couleur_celulle = value;
        }
        public static string? Dernier_tableau
        {
            get => instance._Dernier_tableau;
            set => instance._Dernier_tableau = value;
        }

        public static event EventHandler? Paramètre_changer;
        #endregion

        #region Properties
        public uint _Taille_tableau_vertical { get; set; } = 100;
        public uint _Taille_tableau_horizontal { get; set; } = 100;
        public uint _Couleur_tableau { get; set; } = Colors.White.ToUint32();
        public uint _Couleur_celulle { get; set; } = Colors.Black.ToUint32();
        public string? _Dernier_tableau { get; set; }

        private static Paramètres_de_lapplication instance { get; set; } = new();
        #endregion

        #region Methods
        /// <summary>
        /// Charge le fichier de sauvegarde
        /// </summary>
        /// <returns></returns>
        public static bool Charger()
        {
            try
            {
                var chemin_du_fichier_de_sauvegarde = Obtenir_le_chemin_du_fichier_de_sauvegarde();

                if (!File.Exists(chemin_du_fichier_de_sauvegarde))
                    return false;

                var flux = File.OpenRead(chemin_du_fichier_de_sauvegarde);
                var nouvelle_instance = JsonSerializer.Deserialize<Paramètres_de_lapplication>(flux);

                if (nouvelle_instance == null)
                    return false;
                else
                    instance = nouvelle_instance;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Aplique les modifications et le notifis au abonnée
        /// </summary>
        /// <returns></returns>
        public static bool Sauvegarder()
        {
            try
            {
                var chemin_du_fichier_de_sauvegarde = Obtenir_le_chemin_du_fichier_de_sauvegarde();
                var json = JsonSerializer.Serialize(instance, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                });

                File.WriteAllText(chemin_du_fichier_de_sauvegarde, json);

                Paramètre_changer?.Invoke(null, new EventArgs());
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string Obtenir_le_chemin_du_fichier_de_sauvegarde()
        {
            string fileName = "Paramètres.json";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = Path.Combine(baseDirectory, fileName);
            string path = Path.GetFullPath(relativePath);

            return path;
        }
        #endregion
    }
}
