using CrosswordsPuzzleGenerator.Models;
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

        public ObservableCollection<Cell> Cells { get; set; } = new();

        public CrosswordViewModel(int gridSize)
        {
            //GenerateEmpty(gridSize);
            List<string> words = new List<string> { "prva", "druga", "tretja"};
            GenerateCW(gridSize, words);
        }
        
        // For desing instance
        public CrosswordViewModel()
        {
            GenerateEmpty(8);
            GridSize = 8;
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



        public void GenerateEmpty(int gridSize)
        {
            Cells.Clear();
            int total = gridSize * gridSize;

            for (int i = 0; i < total; i++)
            {
                Cells.Add(new Cell());
            }

            OnPropertyChanged(nameof(Cells));
        }

        public void GenerateCW(int gridSize, List<string> words)
        {
            CwPGenerator gen = new CwPGenerator(gridSize, words);

            bool isSuccessfull = gen.GenerateCW();

            if (isSuccessfull)
            {
                List<Cell> cells = gen.GetResutl();
                Cells.Clear();

                foreach (Cell cell in cells)
                {
                    Cells.Add(cell);
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
