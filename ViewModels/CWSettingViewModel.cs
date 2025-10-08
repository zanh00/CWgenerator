using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;

namespace CrosswordsPuzzleGenerator.ViewModels
{
    class CWSettingViewModel : INotifyPropertyChanged
    {
		private int _gridSize = 4;

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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
