using System;
using System.Windows.Input;
using MMIM.Enum;

namespace MMIM.Common
{
    public class Item : PropertyChangedBase
    {
        public Item(string name)
        {
            Name = name;
            _imageUri = new Uri(@"Icons\" + name + ".png", UriKind.Relative);
        }

        #region Fields

        public readonly string Name;
        private readonly Uri _imageUri;
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

        public Uri ImageUri
        {
            get { return _imageUri; }
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
