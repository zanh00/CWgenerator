using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
    private bool _generationSuccessful = false;


    public CwPGenerator(int gridSize, IEnumerable<string> wordsToInsert)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(gridSize);

        _gridSize = gridSize;
        _random = new Random();
        _matrix = new Field[gridSize, gridSize];

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                _matrix[x, y] = new Field();
            }
        }

        _wordsToInsert = wordsToInsert.ToList() ?? throw new ArgumentNullException(nameof(wordsToInsert));

    }

    /// <summary>
    /// Checks if a fields starting at position X and Y spanning for a specifed length already belong to any inserted word.
    /// IgnorePosition parameter can be used for when a crossed word is checked for any other potential crosses. Position of the 
    /// first cross can for this purpuse be ignored.
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
    /// <param name="ignorePosition">
    /// Position in the span to be ignored by the cross check. -1 if all fields should be checked.
    /// </param>
    /// <returns>
    /// Index of insertedWords if any of the checked filed belongs to inserted word (crossing found), -1 otherwise.
    /// </returns>
    private int CheckPotentialWordToCross(WordOrientation orientation, Position? startPosition, int length, int ignorePosition)
    {
        int returnIndex = -1;

        if(startPosition is not { } startPos)
            throw new ArgumentNullException(nameof(startPosition));

        switch (orientation)
        {       
            case WordOrientation.eVERTICAL:
            case WordOrientation.eVERTICAL_REVERSE:

                for (int i = 0; i < length; i++)
                {
                    if ((_matrix[startPos.Y + i, startPos.X].BelongsToWord) && (i != ignorePosition))
                    {
                        returnIndex = _matrix[startPos.Y + i, startPos.X].WordIndex;
                        break;
                    }
                }
                break;

            case WordOrientation.eHORIZONTAL:
            case WordOrientation.eHORIZONTAL_REVERSE:

                for (int i = 0; i < length; i++)
                {
                    if ((_matrix[startPos.Y, startPos.X + i].BelongsToWord) && (i != ignorePosition))
                    {
                        returnIndex = _matrix[startPos.Y, startPos.X + i].WordIndex;
                        break;
                    }
                }
                break;

            default:
                break;
        }

        return returnIndex;
    }

    private Position? CrossInsertedWord(WordInfo insertedWordInfo, WordOrientation wtbiOrinetation, string wordToBeInserted)
    {
        Position? wtbiStartPosition = null;
        int ignoreCoordinate = -1;

        // If both words have the same orientation we can not cross them.
        if(insertedWordInfo.Orientation == wtbiOrinetation)
        {
            return null;
        }

        for (int i = 0; i < insertedWordInfo.Word.Length; i++)
        {
            for (int j = 0; j < wordToBeInserted.Length; j++)
            {
                if (insertedWordInfo.Word[i] == wordToBeInserted[j])
                {
                    switch (insertedWordInfo.Orientation)
                    {
                        case WordOrientation.eVERTICAL:
                        case WordOrientation.eVERTICAL_REVERSE:

                            wtbiStartPosition = new Position(insertedWordInfo.StartPosition.X - j, insertedWordInfo.StartPosition.Y + i);
                            ignoreCoordinate = insertedWordInfo.StartPosition.X;

                            break;

                        case WordOrientation.eHORIZONTAL:
                        case WordOrientation.eHORIZONTAL_REVERSE:

                            wtbiStartPosition = new Position(insertedWordInfo.StartPosition.X + i, insertedWordInfo.StartPosition.Y - j);
                            ignoreCoordinate = insertedWordInfo.StartPosition.Y;

                            break;
                        default:
                            break;
                    }

                    if (!IsWithinOuterBounds(wtbiOrinetation, wtbiStartPosition, wordToBeInserted.Length))
                    {
                        // Word to be insereted would go out of limits, can not cross.
                        break;
                    }
                    else if (CheckPotentialWordToCross(wtbiOrinetation, wtbiStartPosition, wordToBeInserted.Length, ignoreCoordinate) != -1)
                    {
                        // Word conflicts with another word, can not croos either.
                        break;
                    }
                    else
                    {
                        return wtbiStartPosition;
                    }
                }
            }
        }

        // No valid matches found.
        return null;

    }

    private void InsertWord(WordOrientation orientation, Position startPosition, string wordToInsert, int wordIndex)
    {
        switch (orientation)
        {
            case WordOrientation.eVERTICAL:
            case WordOrientation.eVERTICAL_REVERSE:

                for (int i = 0; i < wordToInsert.Length; i++)
                {
                    _matrix[startPosition.Y + i, startPosition.X].Character     = wordToInsert[i];
                    _matrix[startPosition.Y + i, startPosition.X].WordIndex     = wordIndex;
                    _matrix[startPosition.Y + i, startPosition.X].BelongsToWord = true;
                }

                break;

            case WordOrientation.eHORIZONTAL:
            case WordOrientation.eHORIZONTAL_REVERSE:

                for (int i = 0; i < wordToInsert.Length; i++)
                {
                    _matrix[startPosition.Y, startPosition.X + i].Character     = wordToInsert[i];
                    _matrix[startPosition.Y, startPosition.X + i].WordIndex     = wordIndex;
                    _matrix[startPosition.Y, startPosition.X + i].BelongsToWord = true;
                }

                break;
            default:
                throw new NotImplementedException();
        }

        _insertedWords.Add(new WordInfo(wordToInsert, orientation, startPosition));

    }

    bool IsWithinOuterBounds(WordOrientation orientation, Position? position, int length)
    {
        bool retVal = true;

        if (position is not { } pos)
            throw new ArgumentNullException(nameof(position));

        if (pos.X < 0 || pos.Y < 0)
        {
            return false;
        }

        switch (orientation)
        {
            case WordOrientation.eVERTICAL:
            case WordOrientation.eVERTICAL_REVERSE:
                
                if (pos.Y + length > _gridSize)
                {
                    retVal = false;
                }

                break;

            case WordOrientation.eHORIZONTAL: 
            case WordOrientation.eHORIZONTAL_REVERSE:

                if(pos.X + length > _gridSize)
                {
                    retVal =  false;
                }

                break;
        }

        return retVal;
    }

    private WordOrientation GetRandomOrientation()
    {
        return (WordOrientation)_random.Next((int)WordOrientation.eVERTICAL, (int)WordOrientation.eHORIZONTAL_REVERSE);
    }

    private Position GetRandomPosition()
    {
        return new Position (_random.Next(_gridSize - 1), _random.Next(_gridSize - 1));
    }

    private char GetRandomLetter()
    {
        const int alphabetStart = 65;
        const int alphabetEnd = 90;

        return (char)_random.Next(alphabetStart, alphabetEnd);
    }

    private void FillEmptyFileds()
    {
        for (int row = 0; row < _gridSize; row++)
        {
            for (int column = 0; column < _gridSize; column++)
            {
                if (!_matrix[row, column].BelongsToWord)
                {
                    _matrix[row, column].Character = GetRandomLetter();
                }
            }
        }
    }

    public bool GenerateCW()
    {
        int iterations = 0;

        do
        {
            Position        startPosition       = GetRandomPosition();
            WordOrientation orientation         = GetRandomOrientation();
            string          word                = _wordsToInsert[_insertedWords.Count];
            bool            insertionSuccessful = false;

            if ((orientation == WordOrientation.eHORIZONTAL_REVERSE) || (orientation == WordOrientation.eVERTICAL_REVERSE))
            {
                word = new string(word.Reverse().ToArray());
            }

            if (!IsWithinOuterBounds(orientation, startPosition, word.Length))
            {
                // Word would not fit. Start from the begining.
                iterations++;
                continue;
            }

            int intersectedWordIndex = CheckPotentialWordToCross(orientation, startPosition, word.Length, -1);

            if (intersectedWordIndex != -1)
            {
                // Word intersects with an already inserted word...check if we can cross it.
                Position? newStartPosition = CrossInsertedWord(_insertedWords[intersectedWordIndex], orientation, word);

                if (newStartPosition != null)
                {
                    // Valid coordinates for a cross found -> word can be inserted
                    insertionSuccessful = true;
                    startPosition = (Position)newStartPosition;
                }
            }
            else
            {
                // Word doesn't intersect with any existing word -> word can be inserted
                insertionSuccessful = true;
            }

            if (insertionSuccessful)
            {
                InsertWord(orientation, startPosition, word, _insertedWords.Count);
                
            }
            else
            {
                iterations++;
            }

        } while ((_insertedWords.Count < _wordsToInsert.Count) && (iterations < 100));

        if (_insertedWords.Count == _wordsToInsert.Count)
        {
            _generationSuccessful = true;
            FillEmptyFileds();
        }
        else
        {
            _generationSuccessful = false;
        }

        return _generationSuccessful;
    }

    public List<Cell> GetResutl()
    {
        List<Cell> cells = new List<Cell>();

        if (!_generationSuccessful)
        {
            return cells;
        }

        for (int row = 0; row < _gridSize;  row++)
        {
            for (int column = 0; column < _gridSize; column++)
            {
                cells.Add(_matrix[row, column]);
            }
        }

        return cells;
    }
}
