using System;
using System.Collections.Generic;
using System.Text;

namespace tetris
{
    class Block                                                                 //základní třída, reprezentjící typ bloku a jeho chování ve hře
    {
        public int StartColumn { get; set; }                                    //od které pozice se začne kreslit blok
        public int StartRow { get; set; }
        public int OrigStartColumn { get; set; }

        public List<string> ActualOrientation { get; set; }
        public List<string> TryOrientation { get { return tryOrientation; } }

        public int ColorNr { get; set; }
        public String Type { get; set; }                                        //typ bloku

        Random random = new Random();                                           //instance pro náhodný výběr

        List<string> orientation1, orientation2, orientation3, orientation4; //dostupné orientace bloku
        List<string> tryOrientation;

        int shift1, shift2, shift3, shift4;                                     //základní korekce polohy při rotaci mezi jednotlivými orientacemi bloku
        int leftShift1, leftShift2, leftShift3, leftShift4;                     //korekce polohy bloku v případě blízkosti levé hranice
        int rightShift1, rightShift2, rightShift3, rightShift4;                 //a pravé hranice.

        int nearLeftShift, nearRightShift;                                      //dodatečné korekce pro blok typu Line, který při rotaci mění záběr o více než 1 sloupec

        public Block()                                                          //konstruktor
        {
            int colorNr = 0;                                                    //číslo barvy bloku
            Type = randomizeBlock();                                            //metoda pro náhodný výběr typu bloku
            orientation1 = new List<string>();                                  //nové isntance seznamů pro předdefinované orientace bloků
            orientation2 = new List<string>();
            orientation3 = new List<string>();
            orientation4 = new List<string>();
            //pro každý typ bloku
            switch (Type)                                                       //následují definice základních 4 orientací
            {
                case ("zet-left"):
                    colorNr = 1;                                                //číslo barvy
                    orientation1.Add("110");                                    //definice tvaru bloku formou seznamu posloupností znaků pro každý řádek: 1 = kostka přítomna, 0 = kostka nepřítomna
                    orientation1.Add("011");
                    shift1 = 1;                                                 //posun při rotaci po směru hodinových ručiček, aby byla respektován Pivot point
                    leftShift1 = 0;                                             //dodatečné posuny při levé a pravé hranici hracího pole
                    rightShift1 = 0;

                    orientation2.Add("01");
                    orientation2.Add("11");
                    orientation2.Add("10");
                    shift2 = -1;
                    leftShift2 = 1;
                    rightShift2 = 0;

                    orientation3.Add("000");
                    orientation3.Add("110");
                    orientation3.Add("011");
                    shift3 = 0;
                    leftShift3 = 0;
                    rightShift3 = 0;

                    orientation4.Add("01");
                    orientation4.Add("11");
                    orientation4.Add("10");
                    shift4 = 0;
                    leftShift4 = 0;
                    rightShift4 = -1;

                    nearLeftShift = 0;

                    StartColumn = 3;
                    StartRow = 0;
                    break;

                case ("zet-right"):
                    colorNr = 2;

                    orientation1.Add("011");
                    orientation1.Add("110");
                    shift1 = 1;
                    leftShift1 = 0;
                    rightShift1 = 0;

                    orientation2.Add("10");
                    orientation2.Add("11");
                    orientation2.Add("01");
                    shift2 = -1;
                    leftShift2 = 1;
                    rightShift2 = 0;

                    orientation3.Add("000");
                    orientation3.Add("011");
                    orientation3.Add("110");
                    shift3 = 0;
                    leftShift3 = 0;
                    rightShift3 = 0;

                    orientation4.Add("10");
                    orientation4.Add("11");
                    orientation4.Add("01");
                    shift4 = 0;
                    leftShift4 = 0;
                    rightShift4 = -1;

                    nearLeftShift = 0;

                    StartColumn = 3;
                    StartRow = 0;
                    break;

                case ("key"):
                    colorNr = 3;
                    orientation1.Add("010");
                    orientation1.Add("111");
                    shift1 = 1;
                    leftShift1 = 0;
                    rightShift1 = 0;

                    orientation2.Add("10");
                    orientation2.Add("11");
                    orientation2.Add("10");
                    shift2 = -1;
                    leftShift2 = 1;
                    rightShift2 = 0;

                    orientation3.Add("000");
                    orientation3.Add("111");
                    orientation3.Add("010");
                    shift3 = 0;
                    leftShift3 = 0;
                    rightShift3 = 0;

                    orientation4.Add("01");
                    orientation4.Add("11");
                    orientation4.Add("01");
                    shift4 = 0;
                    leftShift4 = 0;
                    rightShift4 = -1;

                    nearLeftShift = 0;

                    StartColumn = 3;
                    StartRow = 0;
                    break;

                case ("square"):
                    colorNr = 4;
                    orientation1.Add("11");
                    orientation1.Add("11");
                    shift1 = 0;
                    leftShift1 = 0;
                    rightShift1 = 0;

                    orientation2.Add("11");
                    orientation2.Add("11");
                    shift2 = 0;
                    leftShift2 = 0;
                    rightShift2 = 0;

                    orientation3.Add("11");
                    orientation3.Add("11");
                    shift3 = 0;
                    leftShift3 = 0;
                    rightShift3 = 0;

                    orientation4.Add("11");
                    orientation4.Add("11");
                    shift4 = 0;
                    leftShift4 = 0;
                    rightShift4 = 0;

                    nearLeftShift = 0;

                    StartColumn = 4;
                    StartRow = 0;
                    break;

                case ("el-left"):

                    colorNr = 5;
                    orientation1.Add("100");
                    orientation1.Add("111");
                    shift1 = 1;
                    leftShift1 = 0;
                    rightShift1 = 0;

                    orientation2.Add("11");
                    orientation2.Add("10");
                    orientation2.Add("10");
                    shift2 = -1;
                    leftShift2 = 1;
                    rightShift2 = 0;

                    orientation3.Add("000");
                    orientation3.Add("111");
                    orientation3.Add("001");
                    shift3 = 0;
                    leftShift3 = 0;
                    rightShift3 = 0;

                    orientation4.Add("01");
                    orientation4.Add("01");
                    orientation4.Add("11");
                    shift4 = 0;
                    leftShift4 = 0;
                    rightShift4 = -1;
                    nearLeftShift = 0;

                    StartColumn = 3;
                    StartRow = 0;
                    break;

                case ("el-right"):

                    colorNr = 6;
                    orientation1.Add("001");
                    orientation1.Add("111");
                    shift1 = 1;
                    leftShift1 = 0;
                    rightShift1 = 0;

                    orientation2.Add("10");
                    orientation2.Add("10");
                    orientation2.Add("11");
                    shift2 = -1;
                    leftShift2 = 1;
                    rightShift2 = 0;

                    orientation3.Add("000");
                    orientation3.Add("111");
                    orientation3.Add("100");
                    shift3 = 0;
                    leftShift3 = 0;
                    rightShift3 = 0;

                    orientation4.Add("11");
                    orientation4.Add("01");
                    orientation4.Add("01");
                    shift4 = 0;
                    leftShift4 = 0;
                    rightShift4 = -1;

                    nearLeftShift = 0;

                    StartColumn = 3;
                    StartRow = 0;
                    break;

                case ("line"):
                    colorNr = 7;
                    orientation1.Add("0000");
                    orientation1.Add("1111");
                    shift1 = 2;
                    leftShift1 = 0;
                    rightShift1 = 0;

                    orientation2.Add("1");
                    orientation2.Add("1");
                    orientation2.Add("1");
                    orientation2.Add("1");
                    shift2 = -2;
                    leftShift2 = 2;
                    rightShift2 = -1;

                    orientation3.Add("0000");
                    orientation3.Add("0000");
                    orientation3.Add("1111");
                    shift3 = 1;
                    leftShift3 = 0;
                    rightShift3 = 0;

                    orientation4.Add("1");
                    orientation4.Add("1");
                    orientation4.Add("1");
                    orientation4.Add("1");
                    shift4 = -1;
                    leftShift4 = 1;
                    rightShift4 = -2;

                    nearLeftShift = 1;                      //dodatečné korekce polohy speciálně pro tento typ bloku
                    nearRightShift = -1;

                    StartColumn = 3;                     //výchozí poloha po vytvoření
                    StartRow = 0;
                    break;
            }
            ColorNr = colorNr;
            ActualOrientation = orientation1;       //nastavení výchozí orientace
        }

        public void rotateBlock(bool left, bool right, bool nearLeft, bool nearRight, bool trial)
        {
            if (trial)
                OrigStartColumn = StartColumn;
            if (ActualOrientation == orientation1)                      //definice rotací ve směru hodinových ručiček
            {
                StartColumn = StartColumn + shift1;                     //testuje se poloha bloku,v blízkosti levé a pravé hranice se aplikují korekce polohy
                if (left)
                    StartColumn = StartColumn + leftShift1;
                if (right)
                    StartColumn = StartColumn + rightShift1;
                if (trial)                                              //metoda je volána buď v testovacím režimu, kdy ověřuje možnost rotace, nebo v ostrém režimu, kdy blok skutečně rotuje
                    tryOrientation = orientation2;                      //podle toho mění buď virtuální budoucí orientaci
                else
                    ActualOrientation = orientation2;                   //nebo skutečnou orientaci bloku
            }
            else if (ActualOrientation == orientation2)
            {
                StartColumn = StartColumn + shift2;
                if (left)
                    StartColumn = StartColumn + leftShift2;
                if (right)
                    StartColumn = StartColumn + rightShift2;
                if (nearLeft)
                    StartColumn = StartColumn + nearLeftShift;
                if (trial)
                    tryOrientation = orientation3;
                else
                    ActualOrientation = orientation3;
            }
            else if (ActualOrientation == orientation3)
            {
                StartColumn = StartColumn + shift3;
                if (left)
                    StartColumn = StartColumn + leftShift3;
                if (right)
                    StartColumn = StartColumn + rightShift3;
                if (trial)
                    tryOrientation = orientation4;
                else
                    ActualOrientation = orientation4;
            }
            else if (ActualOrientation == orientation4)
            {
                StartColumn = StartColumn + shift4;
                if (left)
                    StartColumn = StartColumn + leftShift4;
                if (right)
                    StartColumn = StartColumn + rightShift4;
                if (nearRight)
                    StartColumn = StartColumn + nearRightShift;
                if (trial)
                    tryOrientation = orientation1;
                else
                    ActualOrientation = orientation1;
            }
        }

        public string randomizeBlock()                              //metoda pro náhodný výběr typu bloku. Dle pravidel má výběr probíhat ze 7 virtuálních kapes, každá obsahuje 4 kusy stejného typu
        {
            string type = "";
            int typeNumber = random.Next(1, 29);                    //volí se tedy náhodné číslo v rozsahu 1 - 28, rozsah je rozdělen do sedmi skupin čísel po 4, každá skupina vrací určitý typ bloku
            switch (typeNumber)
            {
                case (1):
                case (2):
                case (3):
                case (4):
                    type = "zet-left";
                    break;
                case (5):
                case (6):
                case (7):
                case (8):
                    type = "zet-right";
                    break;
                case (9):
                case (10):
                case (11):
                case (12):
                    type = "key";
                    break;
                case (13):
                case (14):
                case (15):
                case (16):
                    type = "el-left";
                    break;
                case (17):
                case (18):
                case (19):
                case (20):
                    type = "el-right";
                    break;
                case (21):
                case (22):
                case (23):
                case (24):
                    type = "square";
                    break;
                case (25):
                case (26):
                case (27):
                case (28):
                    type = "line";
                    break;
            }
            return type;
        }
    }
}
