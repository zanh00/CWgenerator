#pragma once
#include <string>

using namespace std;

class WordInfo
{
	protected:
		string word;
		int orientation;
		int startX;
		int startY;

	public:
		WordInfo(string word, int orientation, int startX, int startY);
		WordInfo(const WordInfo& data);
		WordInfo();
		~WordInfo();
		string getWord();
		int getOrientation();
		int getStartX();
		int getStartY();
		void setWord(string word);
		void setOrientation(int orientation);
		void setStartX(int startX);
		void setStartY(int startY);
};
