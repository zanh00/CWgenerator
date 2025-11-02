using CrosswordsPuzzleGenerator.Models;
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
        const int initialGridSize = 10;

        private List<WordCollection> _wordCollections = new List<WordCollection>();
        private WordCollectionService _wordCollectionService;

        public CWSettingViewModel Settings { get; set; }
        public CrosswordViewModel Crossword { get; set; }
        public MainWIndowViewModel()
        {
            Settings = new CWSettingViewModel(initialGridSize);
            Crossword = new CrosswordViewModel(initialGridSize);

            _wordCollectionService = new WordCollectionService();
            _wordCollections = _wordCollectionService.LoadCollections();

            Settings.PropertyChanged += OnSettingsChanged;
            Settings.GenerateRequested = OnGenerateRequested;
        }

        private void OnGenerateRequested()
        {
            List<string> words = new List<string> { "prva", "druga", "tretja", "peta", "sedma" };

            Crossword.GenerateCW(Settings.GridSize, words);
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
