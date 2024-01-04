using System.Linq;
using System.Security.Cryptography;
using MMIM.Common;
using MMIM.Enum;

namespace MMIM.Subcomponents
{
    public class NumItemsSlider : PropertyChangedBase
    {
        #region Fields

        private static int _numItems = 12;
        private static int _numGround;
        private static int _numPowerups;
        private static int _numUtilities;
        private static int _numEnemies;
        private static int _numHazards;

        #endregion

        #region Constants

        public int MIN_ITEMS
        {
            get { return 1; }
        }

        public int MAX_ITEMS
        {
            get { return 24; }
        }

        public int MAX_GROUND
        {
            get { return Minimum(NumItems, ItemCategories.Ground.Count()); }
        }

        public int MAX_POWERUPS
        {
            get { return Minimum(NumItems, ItemCategories.Powerup.Count()); }
        }

        public int MAX_UTILITIES
        {
            get { return Minimum(NumItems, ItemCategories.Utility.Count()); }
        }

        public int MAX_ENEMIES
        {
            get { return Minimum(NumItems, ItemCategories.Enemy.Count()); }
        }

        public int MAX_HAZARDS
        {
            get { return Minimum(NumItems, ItemCategories.Hazard.Count()); }
        }

        #endregion

        #region Properties

        private int SumOfSubSelections
        {
            get { return NumGround + NumPowerups + NumEnemies + NumHazards + NumUtilities; }
        }

        public int NumItems
        {
            get { return _numItems; }
            set
            {
                _numItems = Bound(MIN_ITEMS, value, MAX_ITEMS);
                Synchronize();
                OnPropertyChanged(null);
            }
        }

        public int NumGround
        {
            get { return _numGround; }
            set
            {
                if (SumOfSubSelections - NumGround + value > NumItems) return;
                _numGround = Bound(0, value, MAX_GROUND);
                OnPropertyChanged();
            }
        }

        public int NumPowerups
        {
            get { return _numPowerups; }
            set
            {
                if (SumOfSubSelections - NumPowerups + value > NumItems) return;
                _numPowerups = Bound(0, value, MAX_POWERUPS);
                OnPropertyChanged();
            }
        }

        public int NumUtilities
        {
            get { return _numUtilities; }
            set
            {
                if (SumOfSubSelections - NumUtilities + value > NumItems) return;
                _numUtilities = Bound(0, value, MAX_UTILITIES);
                OnPropertyChanged();
            }
        }

        public int NumEnemies
        {
            get { return _numEnemies; }
            set
            {
                if (SumOfSubSelections - NumEnemies + value > NumItems) return;
                _numEnemies = Bound(0, value, MAX_ENEMIES);
                OnPropertyChanged();
            }
        }

        public int NumHazards
        {
            get { return _numHazards; }
            set
            {
                if (SumOfSubSelections - NumHazards + value > NumItems) return;
                _numHazards = Bound(0, value, MAX_HAZARDS);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        private static int Bound(int min, int value, int max)
        {
            return value <= min
                ? min
                : value >= max
                ? max
                : value;
        }

        private static int Minimum(int a, int b)
        {
            return a < b ? a : b;
        }

        private static readonly RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private void Synchronize()
        {
            while (SumOfSubSelections > NumItems)
            {
                var rand = rng.GetRandomInt(1, SumOfSubSelections);
                if (NumGround > 0 && rand <= NumGround) NumGround -= 1;
                else if (NumPowerups > 0 && rand <= NumPowerups) NumPowerups -= 1;
                else if (NumUtilities > 0 && rand <= NumUtilities) NumUtilities -= 1;
                else if (NumEnemies > 0 && rand <= NumEnemies) NumEnemies -= 1;
                else NumHazards -= 1;
            }
        }

        #endregion
    }
}
