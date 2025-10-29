using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordsPuzzleGenerator.Models
{
    public class Cell
    {
		protected char _character;

		public char Character
		{
			get { return _character; }
			set { _character = value; }
		}

		protected bool _belongsToWord;

		public bool BelongsToWord
		{
			get { return _belongsToWord; }
			set { _belongsToWord = value; }
		}

		public Cell(char character, bool belongsToWord)
		{
			_character		= character;
			_belongsToWord	= belongsToWord;
		}

		public Cell()
		{
			_character		= ' ';
			_belongsToWord	= false;
		}

	}

    public class Field : Cell
    {
		private int _wordIndex;

		public int WordIndex
		{
			get { return _wordIndex; }
			set { _wordIndex = value; }
		}

		public Field() : base(' ', false)
		{
			_wordIndex		= -1;
		}

		public Field(char character, bool belongsToWord, int wordIndex) : base(character, belongsToWord)
        {
			_wordIndex = wordIndex;
        }
    }
}
