using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Input;
using MMIM.Common;
using MMIM.Enum;
using MMIM.Subcomponents;

namespace MMIM
{
    public class MainViewModel : PropertyChangedBase
    {
        #region Fields

        private static readonly GameModeSelector _gameModeSelector = new GameModeSelector();
        private static readonly CourseThemeSelector _courseThemeSelector = new CourseThemeSelector();
        private static readonly AutoScrollSelector _autoScrollSelector = new AutoScrollSelector();
        private static readonly TimerSettingsSelector _timerSettingsSelector = new TimerSettingsSelector();
        private static readonly GameTypeSelector _gameTypeSelector = new GameTypeSelector();
        private static readonly NumItemsSlider _numItemsSlider = new NumItemsSlider();
        private static readonly ItemsSelector _itemsSelector = new ItemsSelector(_numItemsSlider, 60);

        private readonly ObservableCollection<Item> _selectedItems = new ObservableCollection<Item>();

        #endregion

        #region Properties

        public GameModeSelector GameModeSelector
        {
            get { return _gameModeSelector; }
        }

        public CourseThemeSelector CourseThemeSelector
        {
            get { return _courseThemeSelector; }
        }

        public AutoScrollSelector AutoScrollSelector
        {
            get { return _autoScrollSelector; }
        }

        public TimerSettingsSelector TimerSettingsSelector
        {
            get { return _timerSettingsSelector; }
        }

        public ItemsSelector ItemsSelector
        {
            get { return _itemsSelector; }
        }

        public GameTypeSelector GameTypeSelector
        {
            get { return _gameTypeSelector; }
        }

        public ObservableCollection<Item> SelectedItems
        {
            get { return _selectedItems; }
        }

        public NumItemsSlider NumItemsSlider
        {
            get { return _numItemsSlider; }
        }

        #endregion

        #region Commands

        #region RandomizeCommand

        private ICommand _randomizeCommand;
        public ICommand RandomizeCommand
        {
            get { return _randomizeCommand ?? (_randomizeCommand = new Command(() => true, ExecuteRandomize)); }
        }

        private void ExecuteRandomize()
        {
            var rng = new RNGCryptoServiceProvider();

            GameModeSelector.MakeRandomSelection(rng);
            CourseThemeSelector.MakeRandomSelection(rng);
            AutoScrollSelector.MakeRandomSelection(rng);
            TimerSettingsSelector.MakeRandomSelection(rng);
            ItemsSelector.MakeRandomSelection(rng);
            GameTypeSelector.MakeRandomSelection(rng);

            SelectedItems.Clear();
            ItemsSelector.Items
                .Where(item => item.ToggleState == ItemToggleStates.Selected || item.ToggleState == ItemToggleStates.Enabled)
                .ToList()
                .ForEach(item => SelectedItems.Add(item));
        }

        #endregion

        #region ResetCommand

        private ICommand _resetCommand;

        public ICommand ResetCommand
        {
            get { return _resetCommand ?? (_resetCommand = new Command(() => true, ExecuteReset)); }
        }

        private void ExecuteReset()
        {
            GameModeSelector.Reset();
            CourseThemeSelector.Reset();
            AutoScrollSelector.Reset();
            TimerSettingsSelector.Reset();
            ItemsSelector.Reset();
            GameTypeSelector.Reset();
            SelectedItems.Clear();
        }

        #endregion

        #endregion
    }
}