#include "wordInfo.h"

WordInfo::WordInfo(string word, int orientation, int startX, int startY)
{
	this->word = word;
	this->orientation = orientation;
	this->startX = startX;
	this->startY = startY;
}

WordInfo::WordInfo(const WordInfo& data)
{
	word = data.word;
	orientation = data.orientation;
	startX = data.startX;
	startY = data.startY;
}

WordInfo::WordInfo()
{
	word = " ";
	orientation = 0;
	startX = 0;
	startY = 0;
}

WordInfo::~WordInfo()
{
}

string WordInfo::getWord()
{
	return word;
}

int WordInfo::getOrientation()
{
	return orientation;
}

int WordInfo::getStartX()
{
	return startX;
}

int WordInfo::getStartY()
{
	return startY;
}

void WordInfo::setWord(string word)
{
	this->word = word;
}

void WordInfo::setOrientation(int orientation)
{
	this->orientation = orientation;
}

void WordInfo::setStartX(int startX)
{
	this->startX = startX;
}

void WordInfo::setStartY(int startY)
{
	this->startY = startY;
}