using System.Drawing;

namespace Jeux_de_la_vie.Avalonia
{
    public struct Cellule
    {
        #region Constructors
        public Cellule(bool en_vie, Point possition)
        {
            En_vie = en_vie;
            Possition = possition;
        }

        public Cellule(bool en_vie, int x, int y)
        {
            En_vie = en_vie;
            Possition = new(x, y);
        }
        #endregion

        #region Variables
        public bool En_vie;
        public Point Possition;
        #endregion
    }
}
