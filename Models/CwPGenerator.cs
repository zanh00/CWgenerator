using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace CrosswordsPuzzleGenerator.Models;

public readonly struct Position
{
    public int X { get; }
    public int Y { get; }

    public Position(int x, int y) : this()
    {
        X = x;
        Y = y;
    }
}

enum WordOrientation
{
    eVERTICAL, eHORIZONTAL, eVERTICAL_REVERSE, eHORIZONTAL_REVERSE
}

internal class CwPGenerator
{
    private readonly int _gridSize;
    private readonly Random _random;
    private Field[,] _matrix;
    private List<WordInfo> _insertedWords = new List<WordInfo>();
    private List<string> _wordsToInsert;


    public CwPGenerator(int gridSize, IEnumerable<string> wordsToInsert)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(gridSize);

        _gridSize = gridSize;
        _random = new Random();
        _matrix = new Field[gridSize, gridSize];
        _wordsToInsert = wordsToInsert.ToList() ?? throw new ArgumentNullException(nameof(wordsToInsert));

    }

    /// <summary>
    /// Checks if a fields starting at position X and Y spanning for a specifed length already belong to any inserted word.
    /// </summary>
    /// <param name="orientation">
    /// Word orientation.
    /// </param>
    /// <param name="startPosition">
    /// Start X and Y coordinate.
    /// </param>
    /// <param name="length">
    /// Length of the word - number of fileds to check.
    /// </param>
    /// <returns>
    /// Index of insertedWords if any of the checked filed belongs to inserted word (crossing found), -1 otherwise.
    /// </returns>
    private int CheckPotentialCrossing(WordOrientation orientation, Position startPosition, int length)
    {
        int returnIndex = -1;

        switch (orientation)
        {       
            case WordOrientation.eVERTICAL:
            case WordOrientation.eVERTICAL_REVERSE:

                for (int i = 0; i < length; i++)
                {
                    if (_matrix[startPosition.Y + i, startPosition.X].BelongsToWord)
                    {
                        returnIndex = _matrix[startPosition.Y + i, startPosition.X].WordIndex;
                    }
                }
                break;

            case WordOrientation.eHORIZONTAL:
            case WordOrientation.eHORIZONTAL_REVERSE:

                for (int i = 0; i < length; i++)
                {
                    if (_matrix[startPosition.Y, startPosition.X + i].BelongsToWord)
                    {
                        returnIndex = _matrix[startPosition.Y, startPosition.X + i].WordIndex;
                    }
                }
                break;

            default:
                break;
        }

        return returnIndex;
    }

    private void InsertWord(WordOrientation orientation, Position startPosition, string wordToInsert, int wordIndex)
    {
        switch (orientation)
        {
            case WordOrientation.eVERTICAL:
            case WordOrientation.eVERTICAL_REVERSE:

                for (int i = 0; i < wordToInsert.Length; i++)
                {
                    _matrix[startPosition.Y + i, startPosition.X].Character = wordToInsert[i];
                    _matrix[startPosition.Y + i, startPosition.X].WordIndex = wordIndex;
                }

                break;

            case WordOrientation.eHORIZONTAL:
            case WordOrientation.eHORIZONTAL_REVERSE:

                for (int i = 0; i < wordToInsert.Length; i++)
                {
                    _matrix[startPosition.Y, startPosition.X + i].Character = wordToInsert[i];
                    _matrix[startPosition.Y, startPosition.X + i].WordIndex = wordIndex;
                }

                break;
            default:
                break;
        }   
        
    }

    bool IsWithinOuterBounds(WordOrientation orientation, Position position, int length)
    {
        bool retVal = true;

        switch (orientation)
        {
            case WordOrientation.eVERTICAL:
            case WordOrientation.eVERTICAL_REVERSE:
                
                if (position.Y + length > _gridSize)
                {
                    retVal = false;
                }

                break;

            case WordOrientation.eHORIZONTAL: 
            case WordOrientation.eHORIZONTAL_REVERSE:

                if(position.X + length > _gridSize)
                {
                    retVal =  false;
                }

                break;
        }

        return retVal;
    }
}
