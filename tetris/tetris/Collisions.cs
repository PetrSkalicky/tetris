using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace tetris
{
    class Collissions                                                                   //třída pro vyhodnocení kolizí
    {
        bool bottom;
        bool left;
        bool right;
        bool nearLeft;
        bool nearRight;
        bool landed;
        bool collission;
        bool exit;
        bool gameOver;

        public bool checkBottom(Drawing dr)                     //kontrola, zda blok dopadl na dno
        {
            for (int y = 0; y < dr.rows; y++)                                              //cykly projedou celé hrací pole
            {
                for (int x = 0; x < dr.columns; x++)
                {
                    if (dr.Grid[x, y] != 0 && y == dr.rows - 1)                               //pokud se některá část bloku nachází v posledním řádku od shora, blok dopadl na dno
                    {
                        bottom = true;                                           //a metoda vrací true
                        y = dr.rows - 1;                                                   //ukončí nadřazený cyklus y
                        break;                                                          //ukončí cyklus x
                    }
                    else
                        bottom = false;                                          //jinak vrací false
                }
            }
            return bottom;
        }

        public bool checkLanded(Drawing dr)    //kontrola, zda blok dopadl na usazené bloky 
        {
            for (int y = 0; y < dr.rows - 1; y++)                                          //cykly projedou celé hrací pole                                                                                                                
            {
                for (int x = 0; x < dr.columns; x++)
                {
                    if (dr.Grid[x, y] != 0 && dr.BaseGrid[x, y + 1] != 0)                     //srovnávají se hodnoty v poli padajícího bloku s hodnotami v poli usazených bloků
                    {                                                                   //pokud se v řádku pod padajícím blokem nachází nějaká část již usazených bloků
                        landed = true;                                                  //padající blok dopadl a metoda vrací true
                        y = dr.rows - 1;                                                   //ukončí nadřazený cyklus y
                        break;                                                          //ukončí cyklus x
                    }
                    else
                        landed = false;
                }
            }
            return landed;
        }

        public bool checkLeft(Block currentBlock, int[,] grid, int[,] baseGrid)         //metoda pro vyhodnocení kolize před posunem vlevo
        {
            if (currentBlock.StartColumn < 1)                                        //kontrola, zda je nalevo hranice pole                                                                       
                left = true;
            else
            {
                for (int y = currentBlock.StartRow; y < currentBlock.StartRow + currentBlock.ActualOrientation.Count; y++)//kontrola, zda je nalevo usazený blok
                {
                    for (int x = currentBlock.StartColumn; x < currentBlock.StartColumn + currentBlock.ActualOrientation[0].Count(); x++)
                    {
                        if (grid[x, y] != 0 && baseGrid[x - 1, y] != 0)
                        {
                            left = true;                                                                                             //má-li nastat kolize, vrátí true
                            y = currentBlock.StartRow + currentBlock.ActualOrientation.Count - 1;                                        //ukončí nadřazený cyklus y
                            break;                                                                                                          //ukončí cyklus x
                        }
                        else
                            left = false;
                    }
                }
            }
            return left;

        }

        public bool checkNearLeft(int StartColumn, List<string> ActualOrientation)   //speciálně pro blok "linka" se hlídá blízkost hranic ob jeden sloupec. Metoda pro levou stranu.
        {                                                                               //důvodem je fakt, že tento typ bloku při rotaci zasahuje do vzdálenějších pozic
            if (StartColumn == 1)
                nearLeft = true;
            else
                nearLeft = false;                                                //má-li nastat kolize, vrátí true
            return nearLeft;
        }

        public bool checkNearRight(int StartColumn, List<string> ActualOrientation, int columns)  //metoda pro pravou stranu
        {
            if (StartColumn == columns - 2)
                nearRight = true;                                                //má-li nastat kolize, vrátí true
            else
                nearRight = false;
            return nearRight;
        }



        public bool checkRight(Block currentBlock, int columns, int[,] grid, int[,] baseGrid)           //metoda pro vyhodnocení kolize před posunem vpravo
        {
            if (currentBlock.StartColumn + currentBlock.ActualOrientation[0].Length > columns - 1)   //kontrola, zda je napravo hranice pole
                right = true;
            else
            {
                for (int y = currentBlock.StartRow; y < currentBlock.StartRow + currentBlock.ActualOrientation.Count; y++)//kontrola, zda je napravo usazený blok
                {
                    for (int x = currentBlock.StartColumn; x < currentBlock.StartColumn + currentBlock.ActualOrientation[0].Count(); x++)
                    {
                        if (grid[x, y] != 0 && baseGrid[x + 1, y] != 0)
                        {
                            right = true;                                                                                            //má-li nastat kolize, vrátí true
                            y = currentBlock.StartRow + currentBlock.ActualOrientation.Count - 1;                                        //ukončí nadřazený cyklus y
                            break;                                                                                                          //ukončí cyklus x
                        }
                        else
                            right = false;
                    }
                }
            }
            return right;
        }

        public bool checkRotation(Block currentBlock, Drawing dr)                                              //kontrola, zda je místo pro rotaci (např. mezi usazenými bloky)
        {
            exit = false;
            for (int y = currentBlock.StartRow; y < currentBlock.StartRow + currentBlock.TryOrientation.Count; y++)
            {
                for (int x = currentBlock.StartColumn; x < currentBlock.StartColumn + currentBlock.TryOrientation[0].Count(); x++)
                {
                    if (exit)
                        break;
                    if (y > dr.rows - 1 || x > dr.columns - 1 || x < 0)
                    {
                        collission = true;
                        exit = true;
                        break;
                    }
                    if (dr.TryGrid[x, y] != 0 && dr.BaseGrid[x, y] != 0)
                    {
                        collission = true;
                        exit = true;                                        //ukončí nadřazený cyklus y
                        break;                                              //ukončí cyklus x
                    }
                    else
                        collission = false;
                }
            }
            return collission;
        }

        public bool checkGameOver(Drawing dr)             //konec hry nastane, když je zaplněné některé z prostředních políček v prvních 2 řadách - nový blok není kam umístit
        {
            exit = false;
            for (int y = 0; y < 2; y++)
            {
                if (exit)
                    break;
                for (int x = 3; x < dr.columns - 4; x++)
                {
                    if (dr.BaseGrid[x, y] != 0)
                    {
                        gameOver = true;
                        exit = true;
                        break;
                    }
                    else
                        gameOver = false;
                }
            }
            return gameOver;
        }
    }
}
