using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace Jeux_de_la_vie.Avalonia
{
    internal class Paramètres_de_lapplication
    {
        public Size Taille_tableau { get; set; } = new(100, 100);
        public int Couleur_tableau { get; set; } = Color.White.ToArgb();
        public int Couleur_celulle { get; set; } = Color.Black.ToArgb();
        public string? Dernier_tableau;

        public static event EventHandler? Paramètre_changer;

        public static Paramètres_de_lapplication Actuelle
        {
            get
            {
                if (paramètres_de_lapplication_actuelle == null)
                    paramètres_de_lapplication_actuelle = Charger();

                return paramètres_de_lapplication_actuelle ?? new();
            }
        }
        private static Paramètres_de_lapplication? paramètres_de_lapplication_actuelle;

        private static Paramètres_de_lapplication? Charger()
        {
            try
            {
                var chemin_du_fichier_de_sauvegarde = Obtenir_le_chemin_du_fichier_de_sauvegarde();

                var flux = File.OpenRead(chemin_du_fichier_de_sauvegarde);
                return JsonSerializer.Deserialize<Paramètres_de_lapplication>(flux);
            }
            catch
            {
                return null;
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
                var json = JsonSerializer.Serialize(Actuelle, new JsonSerializerOptions()
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
    }
}
