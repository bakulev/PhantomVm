using System.ComponentModel;

namespace PhantomCodeTest.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ru.dz.phantom.system.boot _boot;

        private string _text = "test text 2";
        public string Text
        {
            get => _text;
            set { _text = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text))); }
        }

        public MainViewModel()
        {
            var userBootObject = new ru.dz.phantom.system.boot();
            var systemBoot = new @internal.bootstrap();
            userBootObject.startup(systemBoot);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
