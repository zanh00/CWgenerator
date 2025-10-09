using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordsPuzzleGenerator.ViewModels
{
    class CrosswordViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<CellViewModel> Cells { get; set; } = new();

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



        public void GenerateEmpty(int gridSize)
        {
            Cells.Clear();
            int total = gridSize * gridSize;

            for (int i = 0; i < total; i++)
            {
                Cells.Add(new CellViewModel { Character = ' ' });
            }

            OnPropertyChanged(nameof(Cells));
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class CellViewModel
    {
        public char Character { get; set; }
    }
}
