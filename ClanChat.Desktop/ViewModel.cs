using System.ComponentModel;

namespace ClanChat.Desktop
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string _clanNameText;

        public string ClanNameText
        {
            get { return _clanNameText; }
            set
            {
                if (_clanNameText != value)
                {
                    _clanNameText = value;
                    OnPropertyChanged(nameof(ClanNameText));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
