using CommunityToolkit.Mvvm.Input;
using CrosswordsPuzzleGenerator.Models;
using CrosswordsPuzzleGenerator.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

            Settings.UpdateCollectionNames(GetCollectionNames());

        }

        [RelayCommand]
        public void OpenEditCollections()
        {
            var window = new EditCollectionsView();
            var vm = new EditCollectionsViewModel(_wordCollections);
            window.DataContext = vm;

            window.ShowDialog();

            // After window closed, update collection names for combo box display
            Settings.UpdateCollectionNames(GetCollectionNames());
        }

        [RelayCommand]
        public void ExportCW()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Word Document (*.docx)|*.docx";

            if(saveFileDialog.ShowDialog() == true)
            {
                List<Cell> cells = Crossword.Cells.ToList();
                try
                {
                    CWExportService.CreateWordDocument(saveFileDialog.FileName, Settings.GridSize, cells);
                } catch (Exception ex)
                {
                    MessageBox.Show("Unable to save crossword to a file", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private ObservableCollection<string> GetCollectionNames()
        {
            ObservableCollection<string> collectionNames = new ObservableCollection<string>();

            foreach (WordCollection wordCollection in _wordCollections)
            {
                collectionNames.Add(wordCollection.Name);
            }

            return collectionNames;
        }

        private void OnGenerateRequested()
        {
            List<string> words = new();

            string selectedCollectionName = Settings.SelectedCollectionName;

            WordCollection selectedCollection = WordCollectionService.GetCollectionByName(selectedCollectionName, _wordCollections);

            if (selectedCollection != null)
            {
                int pickSize = Settings.WordsToFit;

                if (selectedCollection.Words.Count < pickSize)
                {
                    pickSize = selectedCollection.Words.Count;
                }

                words = GetRandomItems(selectedCollection.Words, pickSize);

                Crossword.GenerateCW(Settings.GridSize, words);
            }
            else
            {
                //TODO Dialog -> collection must be selected
            }
        }

        public static List<string> GetRandomItems(List<string> list, int count)
        {
            Random random = new();
            return list.OrderBy(_ => random.Next()).Take(count).ToList();
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
