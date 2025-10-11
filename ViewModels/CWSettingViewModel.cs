using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;

namespace CrosswordsPuzzleGenerator.ViewModels
{
    partial class CWSettingViewModel : INotifyPropertyChanged
    {
		public CWSettingViewModel(int gridSize)
		{
			GridSize = gridSize;
		}

		private int _gridSize;

		private int _wordsToFit;

		public int WordsToFit
		{
			get { return _wordsToFit; }
			set { _wordsToFit = value; }
		}

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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
