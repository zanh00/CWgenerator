using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace CrosswordsPuzzleGenerator.ViewModels
{
    internal class MainWIndowViewModel : INotifyPropertyChanged
    {
        const int initialGridSize = 8;

        public CWSettingViewModel Settings { get; set; }
        public CrosswordViewModel Crossword { get; set; }
        public MainWIndowViewModel()
        {
            Settings = new CWSettingViewModel(initialGridSize);
            Crossword = new CrosswordViewModel(initialGridSize);

            Settings.PropertyChanged += OnSettingsChanged;
        }

        private void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Settings.GridSize))
            {
                Crossword.GenerateEmpty(Settings.GridSize);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
