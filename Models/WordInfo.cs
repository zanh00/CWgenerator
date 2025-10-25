using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordsPuzzleGenerator.Models
{
    enum CWOrientation
    {
        eVERTICAL, eHORIZONTAL, eVERTICAL_REVERSE, eHORIZONTAL_REVERSE 
    }
    internal class WordInfo
    {
        private string? _word;

        public string Word
        {
            get { return _word; }
            set { _word = value; }
        }

        private CWOrientation _orientation;

        public CWOrientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }





    }
}
