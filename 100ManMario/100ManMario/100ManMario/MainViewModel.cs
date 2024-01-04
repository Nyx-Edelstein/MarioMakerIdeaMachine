using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HundredManMario
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Constructor

        public MainViewModel()
        {
            for (var i = 0; i < 100; i++)
                Lives.Add(new Life());

            for (var i = 0; i < 3; i++)
            {
                var emptyMushroom = new Life
                {
                    Alive = false
                };
                Mushrooms.Add(emptyMushroom);
            }   
        }


        #endregion

        #region Fields

        private readonly ObservableCollection<Life> _lives = new ObservableCollection<Life>();
        private readonly ObservableCollection<Life> _mushrooms = new ObservableCollection<Life>();
        private bool _showVideoPlayer;

        #endregion

        #region Properties

        public ObservableCollection<Life> Lives
        {
            get { return _lives; }
        }

        public ObservableCollection<Life> Mushrooms
        {
            get { return _mushrooms; }
        }

        public int NumLives
        {
            get { return Lives.Count(l => l.Alive); }
        }

        public bool ShowVideoPlayer
        {
            get { return _showVideoPlayer; }
            set
            {
                _showVideoPlayer = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        #region OneUpCommand

        private ICommand _oneUpCommand;

        public ICommand OneUpCommand
        {
            get { return _oneUpCommand ?? (_oneUpCommand = new Command(() => true, ExecuteOneUpCommand)); }
        }

        private void ExecuteOneUpCommand()
        {
            var firstEmpty = Mushrooms.FirstOrDefault(life => life.Alive == false);
            if (firstEmpty == null) return;
            firstEmpty.Alive = true;
            OnPropertyChanged();
        }

        #endregion

        #region AddLivesCommand

        private ICommand _addLivesCommand;

        public ICommand AddLivesCommand
        {
            get { return _addLivesCommand ?? (_addLivesCommand = new Command(() => true, ExecuteAddLivesCommand)); }
        }

        private void ExecuteAddLivesCommand()
        {
            var numMushrooms = Mushrooms.Count(m => m.Alive);
            var deadLives = Lives.Where(life => life.Alive == false);
            var toAdd = deadLives.Take(numMushrooms).ToList();
            var toRemove = Mushrooms.Where(m => m.Alive).Reverse().ToList();

            //Play Sound
            Task.Run(() =>
            {
                var soundPlayer = new System.Media.SoundPlayer(@"Sounds\1up.wav");
                soundPlayer.Load();
                for (var i = 0; i < numMushrooms; i++)
                {
                    soundPlayer.Play();
                    if (i < toAdd.Count) toAdd[i].Alive = true;
                    toRemove[i].Alive = false;
                    OnPropertyChanged();
                    Thread.Sleep(900);
                }
            });
        }

        #endregion

        #region DeathCommand

        private ICommand _deathCommand;

        public ICommand DeathCommand
        {
            get { return _deathCommand ?? (_deathCommand = new Command(() => true, ExecuteDeathCommand)); }
        }

        private static readonly Random rand = new Random();
        private void ExecuteDeathCommand()
        {
            var lastAlive = Lives.LastOrDefault(life => life.Alive);
            if (lastAlive == null) return;
            lastAlive.Alive = false;

            //Play Sound
            Task.Run(() =>
            {
                var random = rand.Next(1, 7);
                var soundPlayer = new System.Media.SoundPlayer(@"Sounds\die" + random + ".wav");
                soundPlayer.Load();
                soundPlayer.Play();

                if (NumLives == 0)
                {
                    Thread.Sleep(1500);
                    var soundPlayer2 = new System.Media.SoundPlayer(@"Sounds\gameover.wav");
                    soundPlayer2.Load();
                    soundPlayer2.Play();
                }
            });

            OnPropertyChanged();
        }

        #endregion

        #region ResetCommand

        private ICommand _resetCommand;

        public ICommand ResetCommand
        {
            get { return _resetCommand ?? (_resetCommand = new Command(() => true, ExecuteResetCommand)); }
        }

        private async void ExecuteResetCommand()
        {
            if (NumLives > 0)
            {
                //Play Win Video
                ShowVideoPlayer = true;
                var videoPlayer = MainWindow.VideoPlayer;
                videoPlayer.Play();

                await Task.Run(() =>
                {
                    Thread.Sleep(16500);
                });

                videoPlayer.Stop();
                videoPlayer.Position = new TimeSpan(0);
                ShowVideoPlayer = false;
            }

            Lives.ToList().ForEach(life => life.Alive = true);
            Mushrooms.ToList().ForEach(m => m.Alive = false);
            OnPropertyChanged();            
        }

        #endregion

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
