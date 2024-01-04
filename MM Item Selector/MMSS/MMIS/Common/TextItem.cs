using System.Windows.Input;
using MMIM.Enum;

namespace MMIM.Common
{
    public class TextItem : PropertyChangedBase
    {
        public TextItem(string name)
        {
            _name = name;
        }

        #region Fields

        private readonly string _name;
        private ItemToggleStates _toggleState;

        #endregion

        #region Properties

        public ItemToggleStates ToggleState
        {
            get { return _toggleState; }
            set
            {
                _toggleState = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
        }

        #endregion

        #region Commands

        private ICommand _itemButtonCommand;

        public ICommand ItemButtonCommand
        {
            get { return _itemButtonCommand ?? (_itemButtonCommand = new Command(() => true, ItemButtonCommandExecute)); }
        }

        private void ItemButtonCommandExecute()
        {
            ToggleState = ToggleState == ItemToggleStates.Enabled ? ItemToggleStates.Disabled
                : ToggleState == ItemToggleStates.Disabled ? ItemToggleStates.Unset
                : ItemToggleStates.Enabled;

            OnPropertyChanged();
        }
        
        #endregion

        public void Reset()
        {
            if (ToggleState == ItemToggleStates.Selected || ToggleState == ItemToggleStates.NotSelected) ToggleState = ItemToggleStates.Unset;
        }
    }
}
