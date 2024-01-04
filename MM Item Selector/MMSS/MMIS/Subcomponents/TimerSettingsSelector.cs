using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using MMIM.Common;
using MMIM.Enum;

namespace MMIM.Subcomponents
{
    public class TimerSettingsSelector : SubcomponentBase
    {
        #region Constructor

        public TimerSettingsSelector()
        {
            Settings.TimerSettings.Select(x => new TextItem(x))
                .ToList()
                .ForEach(item => Items.Add(item));
        }

        #endregion

        #region Fields

        private readonly ObservableCollection<TextItem> _items = new ObservableCollection<TextItem>();

        #endregion

        #region Properties

        public ObservableCollection<TextItem> Items
        {
            get { return _items; }
        }

        #endregion

        #region Methods

        public override void MakeRandomSelection(RNGCryptoServiceProvider rand)
        {
            if (Items.All(x => x.ToggleState != ItemToggleStates.Selected && x.ToggleState != ItemToggleStates.Enabled))
            {
                var selectedItem = Items.Where(item => item.ToggleState == ItemToggleStates.Unset)
                    .OrderBy(x => rand.GetRandomInt(0, int.MaxValue))
                    .FirstOrDefault();

                if (selectedItem != null) selectedItem.ToggleState = ItemToggleStates.Selected;                
            }

            Items.Where(item => item.ToggleState == ItemToggleStates.Unset)
                    .ToList()
                    .ForEach(item => item.ToggleState = ItemToggleStates.NotSelected);

            OnPropertyChanged();
        }

        public override void Reset()
        {
            Items.ToList().ForEach(item => item.Reset());
            OnPropertyChanged();
        }

        #endregion
    }
}
