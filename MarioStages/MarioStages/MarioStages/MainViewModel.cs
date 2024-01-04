using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MarioStages
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Fields

        private const int FIRST_STAGE = 1;
        private const int LAST_STAGE = 6;
        private int _currentStage = 1;

        #endregion

        #region Properties

        public int CurrentStage
        {
            get { return _currentStage; }
            set
            {
                if (value <= FIRST_STAGE) _currentStage = FIRST_STAGE;
                else if (value >= LAST_STAGE) _currentStage = LAST_STAGE;
                else _currentStage = value;
                OnPropertyChanged();
            }
        }

        public Uri CurrentImage
        {
            get { return new Uri(@"Images\" + CurrentStage + ".png", UriKind.Relative); }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(null));
        }

        #endregion
    }
}
