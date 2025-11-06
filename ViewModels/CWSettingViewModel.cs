using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CrosswordsPuzzleGenerator.ViewModels
{
    partial class CWSettingViewModel : ObservableObject, INotifyPropertyChanged
    {
		public CWSettingViewModel(int gridSize)
		{
			GridSize = gridSize;
		}


		private int _wordsToFit;
		public int WordsToFit
		{
			get { return _wordsToFit; }
			set { _wordsToFit = value; }
		}

		private int _gridSize;
		public int GridSize
		{
			get { return _gridSize; }
			set 
			{
				if (_gridSize != value)
				{
					_gridSize = value;
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
        private void IncreaseWordsToFit() => WordsToFit++;

        [RelayCommand]
        private void DecreaseWordsToFit()
        {
            if (WordsToFit > 1)
            {
                WordsToFit--;
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
