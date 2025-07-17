#pragma once

#include <string>

using namespace std;

class Field
{
	private:
		char letter;
		int wordIndex;
		bool isTaken;

	public:
		Field();
		~Field();
		char getLetter();
		int getWordIndex();
		bool getIsTaken();
		void setLetter(char letter);
		void setWordIndex(int word);
		

};

