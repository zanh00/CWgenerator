#include "field.h"

Field::Field()
{
	letter = '/';
	wordIndex = -1;
	isTaken = false;
}

Field::~Field()
{
}

char Field::getLetter()
{
	return letter;
}

int Field::getWordIndex()
{
	return wordIndex;
}

bool Field::getIsTaken()
{
	return isTaken;
}

void Field::setLetter(char letter)
{
	this->letter = letter;
	isTaken = true;
}

void Field::setWordIndex(int wordIndex)
{
	this->wordIndex = wordIndex;
}
