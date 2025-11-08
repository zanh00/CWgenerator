using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrosswordsPuzzleGenerator.Utilities.Validation;

namespace CrosswordsPuzzleGenerator.ViewModels
{
    partial class CWSettingViewModel : ObservableValidator, INotifyPropertyChanged
    {
		public CWSettingViewModel(int gridSize)
		{
			GridSize = gridSize;
		}

        private int _wordsToFit;

        [Range(1, 15, ErrorMessage = "Number of words must be between 1 and 15")]
        public int WordsToFit
        {
            get => _wordsToFit;
            set => SetProperty(ref _wordsToFit, value, true); // <- 'true' triggers validation
        }

        private int _gridSize;
		public int GridSize
		{
			get { return _gridSize; }
			set 
			{
				if (_gridSize != value)
				{
					if (value < 25)
					{
						_gridSize = value;
					}
					else
					{
						_gridSize = 25;
					}

					OnPropertyChanged(nameof(GridSize));
				}
			}
		}

		[ObservableProperty]
		private ObservableCollection<string> _collectionNames;

		[ObservableProperty]
		private string _selectedCollectionName;
		

		public Action? GenerateRequested {  get; set; }

		[RelayCommand]
		private void Generate()
		{
			GenerateRequested?.Invoke();
		}

		[RelayCommand]
		public void IncreaseGridSize()
		{
			GridSize++;
		}

        [RelayCommand]
		public void DecreaseGridSize()
		{
			if (GridSize > 1)
			{
				GridSize--;
			}
		}

		[RelayCommand]
		private void IncreaseWordsToFit()
		{
			WordsToFit++;
			OnPropertyChanged(nameof(WordsToFit));
		}

        [RelayCommand]
        private void DecreaseWordsToFit()
        {
            if (WordsToFit > 1)
            {
                WordsToFit--;
				OnPropertyChanged(nameof(WordsToFit));
            }
        }

		public void UpdateCollectionNames(ObservableCollection<string> collectionNames)
		{
			CollectionNames = collectionNames;
            OnPropertyChanged(nameof(CollectionNames));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
