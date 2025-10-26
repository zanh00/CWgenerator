using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordsPuzzleGenerator.Models
{
    internal class WordInfo
    {
        private string? _word;

        public string Word
        {
            get { return _word; }
            set { _word = value; }
        }

        private WordOrientation _orientation;

        public WordOrientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        private Position _startPosition;

        public Position StartPosition
        {
            get { return _startPosition; }
            set { _startPosition = value; }
        }




    }
}
