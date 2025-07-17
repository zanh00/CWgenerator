#include <iostream>
#include <string>
#include <fstream>
#include <random>

#include "field.h"
#include "wordInfo.h"

using namespace std;


/////////////////////////////// KONSTANTE ///////////////////////////////////////////
#define SizeX 15
#define SizeY 15
#define AlphabetStart 65
#define AlphabetEnd 90
//#define StBesed 7
#define MAX_WORD_LENGTH 12
#define MIN_WORD_LENGTH 3

////////////////////////////////// SPREMENLJIVKE ////////////////////////////////////
string fileName = "Nabor besed.txt";
string preberiBesedo;
int StBesed;


//string beseda[StBesed] = { "trojak", "rocnik", "hidrant", "kljuc", "nosilec", "pritrdilec", "zbiralecvode"};
string beseda[100];					// sem se vpi�ejo vse besede iz nabora besed
string izbraneBesede[10];			// besede, ki so izbrane iz nabora mo�nih besed 	
int startXposition;					//za�etna X koordinata
int startYposition;					//za�etna Y koordinata
int orientation;					//Kako bo beseda obrnjena: 1-navpi�no, 2-vodoravno, 3-navpi�no (obrnjeno), 4-vodoravno (obrnjeno)
Field matrix[SizeY][SizeX];
vector <WordInfo> selectedWords;	//dinami�ni vektor razreda, ki shranjuje koordinate vpisanih besed v matriko

////////////////////////////////  DEKLARAICJE FUNKCIJ /////////////////////////////
char randomLetter(int lowNumber, int highNumber);											//generira naklju�no �tevilo med low in high number
bool CheckCrossing(int orientation, int startXpos, int startYpos, int length, int& index);	// vrne true �e se beseda seka z �e katero vpisano besedo
bool CrossWords(int indexBesede, int orient, string tb);									//izvede kri�anje �e ustreza vsem pogojem - vrne true, �e je kri�anje uspelo ali false �e ni uspelo;
																							//tb - trenutna beseda, ki jo �elimo vpisati
bool CheckConditions(int orient, int startX, int startY, int ignore, int length);			//preveri �e beseda ki jo �elimo vpisati pa�e v polje in ne seka drugih besed - 
																							//jo kli�e funkcija CrossWords  
void writeToMatrix(int orient, int startX, int startY, int wordIndex);						//vpi�e besedo v matriko
bool CheckBoundaries(int orient, int startX, int startY, int length);						//preveri �e je beseda pa�e znotraj polja
void PrintMatrix();																			//iz�i�e matriko
string obrniBesedo(int indexBesede);														//obrne �eljeno besedo
bool InspectInputWord(string& prebranaBeseda);
void IzberiBesede(int stBesedZaIzbor);														//izbere besede iz naora besed
void GenerateCrossword(int stBesedZaVpis);

///////////////////////////////// MAIN //////////////////////////////////////////////
int main()
{
	int StBesedZaGenerator = 10;			// število besed ki se bo generiralo v križanke

	ifstream naborBesed(fileName);
	if (naborBesed.is_open())
	{
		int i = 0;
		while (getline(naborBesed, preberiBesedo))
		{
			if (InspectInputWord(preberiBesedo))		// Poskrbi za pravilni format besed in odstrani morebitne druge znake
			{											// �e je beseda dalj�a od 3 �rk in kraj�a od 12 bo spustlo skozi druga�e vrne false
				beseda[i] = preberiBesedo;
				i++;
			}
		}
		StBesed = i;								// Klko besed je bilo skupaj prebranih iz datoteke
	}
	else
	{
		cout << "Datoteke z nabororm besed ni mogoce odpreti";
		return 0;
	}
	naborBesed.close();
	
	if (StBesed < StBesedZaGenerator)				// V primeru da je iz nabora besed prebranih manj besed kot je prednastavljeno število besed ki se bojo 
	{												// generirala v križanki, se ta vrednost zmanjša na število besed prebranih iz nabora.
		StBesedZaGenerator = StBesed;				// Na primer: če je predvidneih 10 besed za generiranje (StBesedZaGenerator) iz nabora besed pa je prebranih manj
	}												// se mora to število seveda zmanjšati.

	IzberiBesede(StBesedZaGenerator);

	GenerateCrossword(StBesedZaGenerator); // generira križanko
	
	PrintMatrix(); // izpis končne matrike
	
}

///////////////////////////////////////// DEFINICIJA FUNKCIJ //////////////////////////////////////////////////////

/// <summary>
/// Generira naklju�no �tevilo med najni�jo in najvi�jo podano vrednostjo
/// </summary>
/// <param name="lowNumber"></param>
/// <param name="highNumber"></param>
/// <returns></returns>

char randomLetter(int lowNumber, int highNumber)
{
	random_device RDEVICE;
	default_random_engine RENGINE(RDEVICE());
	uniform_int_distribution<int>RGENERATION(lowNumber, highNumber);
	return RGENERATION(RENGINE);
}


bool CheckCrossing(int orientation, int startXpos, int startYpos, int length, int& index)
{

	switch (orientation) // glede na orientacijo 1 - navpi�no; 2 - vodoravno
	{
		case 1:
		case 3:
			for (int i = 0; i < length; i++)									// vsako polje v katero bi radi vpisali to besedo preverimo �e �e pripada kak�ni drugi besedi
			{
				if (matrix[startYpos + i][startXpos].getIsTaken() == 1)			// �e je polje �e zasedeno .getIsTaken() vrne 1
				{
					index = matrix[startYpos + i][startXpos].getWordIndex();	// pridobimo index besede s katero se seka
					return true;
				}
			}
			break;
		case 2: 
		case 4:
			for (int i = 0; i < length; i++)									// velja enako...zgolj druga orientacija
			{
				if (matrix[startYpos][startXpos + i].getIsTaken() == 1)
				{
					index = matrix[startYpos][startXpos + i].getWordIndex();
					return true;
				}					
			}
			break;
	}

	return false;
}

bool CrossWords(int indexBesede, int tb_orientation, string tb)
{
	string vb = selectedWords[indexBesede].getWord();					//pridobimo besedo vb - vpisana beseda s katero s tb (trenutna beseda) seka
	int x_vb = selectedWords[indexBesede].getStartX();					//pridobimo za�etno X koordinato vb
	int y_vb = selectedWords[indexBesede].getStartY();					//pridobimo za�etno Y koordinat vb
	int vb_length = vb.length();										//dol�ina vpisane besede
	int ignoreLetterCoordinate = 0;										//koordinata kjer se besedi sekata - jo kasneje ignoriramo ko preverjamo �e se seka �e z katero drugo besedo

	if (tb_orientation == selectedWords[indexBesede].getOrientation())	//�e imata besedi enako orientacijo (se prekrivata) - kri�anje ni mo�no
	{
		return false;
	}

	for (int i = 0; i < vb.length(); i++)								//
	{																	// Gremo skozi vse mo�ne kombinacije kri�anja (vse �rke ki se ujemajo v obeh besedah
		for (int j = 0; j < tb.length(); j++)							//
		{
			if (vb.c_str()[i] == tb.c_str()[j]) // �e najdeo �rko ki se ujema
			{
				switch (selectedWords[indexBesede].getOrientation())
				{
					case 1:												//navpi�na orientacija
					case 3:
						startYposition = y_vb + i; // konstanten - ker je vb navpi�na torej je tb vodoravna -> y = konst
						startXposition = x_vb - j; // spreminjajo�
						ignoreLetterCoordinate = x_vb;
						break;
					case 2:												//vodoravna orientacija
					case 4:
						startYposition = y_vb - j; //spreminjajo�
						startXposition = x_vb + i; //konstanten
						ignoreLetterCoordinate = y_vb;
						break;
				}
				
				if (CheckConditions(tb_orientation, startXposition, startYposition, ignoreLetterCoordinate, tb.length()))		//preveri �e glede na dolo�ene koordinate ustrezamo vsem pogojem
				{
					return true;										// kri�anje je uspelo - imamo nove koordinate
				}
			}
		}
	}

	return false;														// Kri�anje ni uspelo
}

bool CheckConditions(int orient, int startX, int startY, int ignore, int length)
{

	switch (orient)
	{
		case 1:
		case 3:
			if (startY + length > SizeY)								// �e je pride beseda izven polja
			{
				return false;
			}

			if (startY < 0)												// �e je pride beseda izven polja
			{
				return false;
			}

			for (int i = 0; i <= length; i++)							// Preverimo morebitno kri�anje z �e kak�no drugo besedo
			{
				if (startY + i != ignore)								// Ignoriramo tisto pozicijo v kateri se tb seka z �eljeno besedo
				{
					if (matrix[startY + i][startX].getIsTaken())		// Preveri zasedenost polja
					{
						return false;
					}
				}
				
			}
			break;
		case 2:															// Velja enako kot za navpi�no orientacijo
		case 4:
			if (startX + length > SizeX)
			{
				return false;
			}

			if (startX < 0)
			{
				return false;
			}

			for (int i = 0; i <= length; i++)
			{
				
				if (startX + i != ignore)
				{
					if (matrix[startY][startX + i].getIsTaken())
					{
						return false;
					}
				}			
			}
			break;
	}

	return true;
}

void writeToMatrix(int orient, int startX, int startY, int wordIndex)
{
	switch (orient)
	{
	case 1:
	case 3:
		for (int j = 0; j < izbraneBesede[wordIndex].length(); j++)								//gremo �rko po �rko
		{
			matrix[startY + j][startX].setLetter(izbraneBesede[wordIndex].c_str()[j]);				// Vpi�i �rko v polje
			matrix[startY + j][startX].setWordIndex(wordIndex);								// Nastavi index besede za to polje
		}
		selectedWords.push_back(WordInfo(izbraneBesede[wordIndex], orient, startX, startY));		// V vektor ki hrani �e vpisane besede dodaj trenutno besedo
		break;
	case 2:
	case 4:
		for (int j = 0; j < izbraneBesede[wordIndex].length(); j++)
		{
			matrix[startY][startX + j].setLetter(izbraneBesede[wordIndex].c_str()[j]);
			matrix[startY][startX + j].setWordIndex(wordIndex);
		}
		selectedWords.push_back(WordInfo(izbraneBesede[wordIndex], orient, startX, startY));
		break;
	}
}


bool CheckBoundaries(int orient, int startX, int startY, int length)
{
	switch (orient)										// Preveri �e je beseda izven polja
	{
	case 1:
	case 3:
		if (startY + length > SizeY)
		{
			return false;
		}
		break;
	case 2:
	case 4:
		if (startX + length > SizeX)
		{
			return false;
		}
		break;
	}

	return true;
}

void PrintMatrix()
{
	for (int i = 0; i < SizeY; i++)
	{
		for (int j = 0; j < SizeX; j++)
		{
			cout << matrix[i][j].getLetter() << " ";
		}
		cout << endl;

	}
	cout << endl;
}

string obrniBesedo(int indexBesede)
{
	string reversed;
	for (int i = izbraneBesede[indexBesede].length(); i > 0; i--)
	{
		reversed.push_back(izbraneBesede[indexBesede].c_str()[i - 1]);
	}
	return reversed;
}

bool InspectInputWord(string& prebranaBeseda)
{
	string beseda = prebranaBeseda;
	prebranaBeseda.clear();
	for (int i = 0; i < beseda.length(); i++)				// Preveri se vsak znak, vpi�ejo se samo �rke morebitni ostali zanki (napake pri vpisu v datoteko) se izpustijo
	{
		if (isalpha(beseda.c_str()[i]))
		{
			prebranaBeseda.push_back(beseda.c_str()[i]);
		}
	}
	for (int i = 0; i < prebranaBeseda.length(); i++)		// Pretvorimo vse v male �rke
	{
		prebranaBeseda[i] = tolower(prebranaBeseda.c_str()[i]);
	}

	if ((prebranaBeseda.length() > MAX_WORD_LENGTH) || (prebranaBeseda.length() < MIN_WORD_LENGTH))			// �e je beseda dalj�a ali kraj�a od dovoljene dol�ine se bo izpustila
	{
		return false;
	}
	else
	{
		return true;
	}
	
}

void IzberiBesede(int stBesedZaIzbor)
{
	vector <int> izbraneStevilke;													// shranjuje izbrane �tevilke da nebi ve�krat izbrali iste besede
	int trenutnaStevilka;															// dr�i vrednost trenutne izbrane �tevilke
	bool ponovitev = false;															// zastavica ki se postavi �e se generira �e ponovljena �tevilka
	for (int i = 0; i < stBesedZaIzbor; i++)										// izberemo tak�no �tevilo besed
	{
		trenutnaStevilka = randomLetter(0, StBesed - 1);							//generiramo naklju�no �tevilko med 0 in �tevilom besed prebranih iz nabora besed
		for (auto j = izbraneStevilke.begin(); j != izbraneStevilke.end(); ++j)
		{
			if (*j == trenutnaStevilka)												// �e smo to �tevilko �e imeli
			{
				ponovitev = true;
				i--;																// dekrementiramo �tevec saj ta beseda nebo vpisana ker je �e
				break;
			}
		}
		if (ponovitev)																// �e je ta �tevilka �e bila resetiramo zastavico, �e ne pa vpi�emo to besedo
		{
			ponovitev = false;
		}
		else
		{
			izbraneBesede[i] = beseda[trenutnaStevilka];
			izbraneStevilke.push_back(trenutnaStevilka);
		}
	}
}

void GenerateCrossword(int stBesedZaVpis)
{
	int wordIndex = -1;						// indeks zaporedno vpisanih besed...Primer: prva vpisana beseda bo imela indeks 0 naslednja 1 ... 
											//se vpi�e za vsako vpisano �rko v matriko da se ve kateri besedi pripada.

	startXposition = randomLetter(0, SizeX - 1); //generiramo naklju�no X koordinato
	startYposition = randomLetter(0, SizeY - 1); //generiramo naklju�no Y koordinato
	orientation = randomLetter(1, 4);			 //generiramo naklju�no orientacijo

	/////////////////////////////////// VPIS PRVE BESEDE ///////////////////////////////////////////////////////////////////////
	switch (orientation)
	{
	case 1:															//�e je �eljena orientacija navpi�na
	case 3:															// ali navpi�na obrnjena
		if (orientation == 3)										// �e je orientacija navpi�na obrnjena potem obrni besedo
		{
			izbraneBesede[0] = obrniBesedo(0);
		}
		if (startYposition + izbraneBesede[0].length() > SizeY)			//�e  beseda nebi pasala v polje
		{
			while (startYposition + izbraneBesede[0].length() > SizeY)		//dokler beseda ne pa�e v polje tak dolgo generiraj nove koordinate
			{
				startYposition = randomLetter(0, SizeY);
			}
		}

		for (int i = 0; i < izbraneBesede[0].length(); i++)												//vpis besede v matriko
		{
			matrix[startYposition + i][startXposition].setLetter(izbraneBesede[0].c_str()[i]);
			matrix[startYposition + i][startXposition].setWordIndex(0);
		}
		selectedWords.push_back(WordInfo(izbraneBesede[0], orientation, startXposition, startYposition)); // shrani prvo besedo z njenjimi koordinatami 
		break;
	case 2:															 //�e je �eljena orientacija vodoravna
	case 4:															// ali vodoravna obrnjena
		if (orientation == 4)										// �e je orientacija vodoravna obrnjena potem obrni besedo
		{
			izbraneBesede[0] = obrniBesedo(0);
		}
		if (startXposition + izbraneBesede[0].length() > SizeX)			//�e je beseda nebi pasala v polje
		{
			while (startXposition + izbraneBesede[0].length() > SizeX)		//dokler beseda ne pa�e v polje tak dolgo generiraj nove koordinate
			{
				startXposition = randomLetter(0, SizeX);
			}
		}

		for (int i = 0; i < izbraneBesede[0].length(); i++)												// vpis besede v matriko
		{
			matrix[startYposition][startXposition + i].setLetter(izbraneBesede[0].c_str()[i]);
			matrix[startYposition][startXposition + i].setWordIndex(0);
		}
		selectedWords.push_back(WordInfo(izbraneBesede[0], orientation, startXposition, startYposition)); // shrani prvo besedo z njenjimi koordinatami
		break;
	}
	////////////////////////////////////// VPIS OSTALIH BESED /////////////////////////////////////////////////////

	for (int i = 1; i < stBesedZaVpis; i++)																	//gremo povrsti skozi vse besede ki jih �elimo vpisati
	{
		do																								//dokler nene beseda pa�e v pojle tak dolgo generiraj nove koordinate
		{
			startXposition = randomLetter(0, SizeX - 1);
			startYposition = randomLetter(0, SizeY - 1);
			orientation = randomLetter(1, 4);
		} while (!CheckBoundaries(orientation, startXposition, startYposition, izbraneBesede[i].length()));

		if (orientation == 3 || orientation == 4)														// �e je orientacija obrnjena potem obrni besedo
		{
			izbraneBesede[i] = obrniBesedo(i);
		}
		for(int stPoizkusov = 0; stPoizkusov < 100; stPoizkusov++)
		{
			if (CheckCrossing(orientation, startXposition, startYposition, izbraneBesede[i].length(), wordIndex)) //preverimo �e se beseda seka z �e katero vpisano besedo - vrne true �e se kri�a (pogoj izpolnjen)
			{
				if (CrossWords(wordIndex, orientation, izbraneBesede[i]))										    // kli�emo funkicjo ki poi��e tak�ne koordinate da se besedi pravilno kri�ata
				{																						    // �e ji to uspe vrne true, �e pa kri�anje ne uspe pa vrne false
					writeToMatrix(orientation, startXposition, startYposition, i);							// vpis v matriko
					break;
				}
				else																						// �e kri�anje ni uspelo potem ponovno generira koordinate 
				{
					if (orientation == 3 || orientation == 4)												// v primeru da je bila prej beseda obrnjena, kri�anje pa ni uspelo potem obrni besedo nazaj
					{
						izbraneBesede[i] = obrniBesedo(i);
					}
					do
					{
						startXposition = randomLetter(0, SizeX - 1);
						startYposition = randomLetter(0, SizeY - 1);
						orientation = randomLetter(1, 4);
					} while (!CheckBoundaries(orientation, startXposition, startYposition, izbraneBesede[i].length()));

					if (orientation == 3 || orientation == 4)
					{
						izbraneBesede[i] = obrniBesedo(i);
					}
				}

			}
			else																							// �e se beseda ne kri�a z nobeno besedo, se lahko ta potem kar vpi�e v matriko
			{
				writeToMatrix(orientation, startXposition, startYposition, i);
				break;
			}

		}
		//PrintMatrix(); //testni izpis matrike
	}
}
