using CommunityToolkit.Mvvm.Input;
using CrosswordsPuzzleGenerator.Models;
using CrosswordsPuzzleGenerator.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace CrosswordsPuzzleGenerator.ViewModels
{
    internal partial class MainWIndowViewModel : INotifyPropertyChanged
    {
        const int initialGridSize = 10;

        private ObservableCollection<WordCollection> _wordCollections = new ();
        private WordCollectionService _wordCollectionService;

        public CWSettingViewModel Settings { get; set; }
        public CrosswordViewModel Crossword { get; set; }
        public MainWIndowViewModel()
        {
            Settings = new CWSettingViewModel(initialGridSize);
            Crossword = new CrosswordViewModel(initialGridSize);

            _wordCollectionService = new WordCollectionService();

            List<WordCollection> loadedCollections = _wordCollectionService.LoadCollections();

            foreach (WordCollection wordCollection in loadedCollections)
            {
                _wordCollections.Add(wordCollection);
            }

            Settings.PropertyChanged += OnSettingsChanged;
            Settings.GenerateRequested = OnGenerateRequested;
        }

        [RelayCommand]
        public void OpenEditCollections()
        {
            var window = new EditCollectionsView();
            var vm = new EditCollectionsViewModel(_wordCollections);
            window.DataContext = vm;

            window.ShowDialog();
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
