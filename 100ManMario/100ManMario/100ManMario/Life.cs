using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HundredManMario
{
    public class Life : INotifyPropertyChanged
    {
        private bool _alive = true;
        public bool Alive
        {
            get { return _alive; }
            set
            {
                _alive = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
