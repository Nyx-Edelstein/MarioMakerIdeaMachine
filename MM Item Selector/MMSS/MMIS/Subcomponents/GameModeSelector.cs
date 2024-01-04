using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using MMIM.Common;
using MMIM.Enum;

namespace MMIM.Subcomponents
{
    public class GameModeSelector : SubcomponentBase
    {
        #region Constructor 

        public GameModeSelector()
        {
            System.Enum.GetValues(typeof (GameModes))
                .OfType<GameModes>()
                .Select(x => new Item(@"games\" + x.ToString()))
                .ToList()
                .ForEach(item => GameModeItems.Add(item));
        }

        #endregion

        #region Fields

        private readonly ObservableCollection<Item> _gameModeItems = new ObservableCollection<Item>();

        #endregion

        #region Properties

        public ObservableCollection<Item> GameModeItems
        {
            get { return _gameModeItems; }
        }

        #endregion

        #region Methods

        public override void MakeRandomSelection(RNGCryptoServiceProvider rng)
        {
            if (GameModeItems.All(x => x.ToggleState != ItemToggleStates.Selected && x.ToggleState != ItemToggleStates.Enabled))
            {
                var rand = rng.GetRandomInt(1, 20);
                var numToPick = rand <= 10 ? 1
                    : rand <= 16 ? 2
                    : rand <= 19 ? 3
                    : 20;

                GameModeItems.Where(x => x.ToggleState == ItemToggleStates.Unset)
                    .OrderBy(x => rng.GetRandomInt(0, int.MaxValue))
                    .Take(numToPick)
                    .ToList()
                    .ForEach(x => x.ToggleState = ItemToggleStates.Selected);
            }

            GameModeItems.Where(x => x.ToggleState == ItemToggleStates.Unset)
                .ToList()
                .ForEach(item => item.ToggleState = ItemToggleStates.NotSelected);

            OnPropertyChanged();
        }

        public override void Reset()
        {
            GameModeItems.ToList().ForEach(item => item.Reset());
        }

        #endregion
    }
}
