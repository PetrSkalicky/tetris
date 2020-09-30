using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace tetris
{
    class Drawing
    {
        public int[,] Grid { get; set; }                        //2d pole pro padající bloky, pro usazené bloky a následující blok
        public int[,] BaseGrid { get; set; }
        public int[,] NextGrid { get; set; }
        public int[,] TryGrid { get; set; }                     //testovací pole pro určení budoucí pozice po rotaci, pro vyhodnocení možného nedostatku místa nebo vyjetí mimo hranice po rotaci bloku

        private int width = 30, height = 30;                    //šířka a výška kostky bloku v pixelech
        public int columns { get { return 10; } }               //počet sloupců, počet řad hracího pole
        public int rows { get { return 22; } }

        UIElementCollection c1, c2;                                         //Děti Canvasů pro kreslení
                                                                            //štětce pro kreslení:
        SolidColorBrush brush = new SolidColorBrush(Colors.Black);          //padajícího bloku      
        SolidColorBrush baseBrush;                                          //usazených bloků
        SolidColorBrush nextBrush = new SolidColorBrush(Colors.Black);      //následujícího bloku


        int count;                                              //pomocný čítač zaplněných políček v řadě
        List<int> filledRows;                                   //seznam zaplněných řad

        bool outOfRange;                                        //vyhodnocuje vyjetí mimo hranice při rotaci
        bool exit;                                              //pomocná proměnná pro opuštění nadřazeného cyklu

        public Drawing(UIElementCollection c1, UIElementCollection c2)                //konstruktor, předává Canvas.Children
        {
            this.c1 = c1;
            this.c2 = c2;
        }

        public void createEmptyGrids()                           //vytvoří prázdná pole pro padající blok, usazené bloky, následující blok
        {
            Grid = new int[columns, rows];
            BaseGrid = new int[columns, rows];
            TryGrid = new int[columns, rows + 2];
            NextGrid = new int[4, 4];
            c1.Clear();                                          //vymaže Canvas pro hrací pole
        }

        public bool TryUpdateGrid(Block currentBlock)                                              //vytvoří testovací hrací pole s aktuálním blokem, který má budoucí orientaci (rotaci) a polohu
        {
            exit = false;                                                                          //pomocná proměnná pro opuštění nadřazeného cyklu
            Array.Clear(TryGrid, 0, TryGrid.Length);                                                //výmaz pole

            for (int y = currentBlock.StartRow; y < currentBlock.StartRow + currentBlock.TryOrientation.Count; y++)
            {
                if (exit)
                    break;
                int x = currentBlock.StartColumn;
                if (x < 0 || x > columns - 1)                                                //pokud se část bloku při rotaci ocitne mimo hranice, test končí, proměnná se nastaví na true
                {
                    outOfRange = true;
                    exit = true;
                    break;
                }
                else
                {
                    outOfRange = false;
                    TryParseBlockLine(currentBlock, x, y);                             //volá metodu kreslení testovacího bloku po řádcích
                }
            }
            return outOfRange;
        }

        private void TryParseBlockLine(Block currentBlock, int x, int y)    //vytváří testovací blok po jednotlivých řádcích. V místě přítomnosti políčka bloku nastaví číslo dle předdef. barvy bloku
        {
            foreach (char c in currentBlock.TryOrientation[y - currentBlock.StartRow])
            {
                if (c == '1')
                {
                    if (x > 9)                                               //pokud se ocitne při generování test. bloku  políčko mimo hranice, končí test
                    {
                        outOfRange = true;
                        exit = true;
                        break;
                    }
                    else
                    {
                        TryGrid[x, y] = currentBlock.ColorNr;
                        outOfRange = false;
                    }
                }
                x++;
            }
        }

        public void updateGrid(Block currentBlock)                                                //vytvoří ostré hrací pole s aktuálním blokem, který má danou orientaci (rotaci) a polohu
        {
            Array.Clear(Grid, 0, Grid.Length);
            for (int y = currentBlock.StartRow; y < currentBlock.StartRow + currentBlock.ActualOrientation.Count; y++)
            {
                int x = currentBlock.StartColumn;
                parseBlockLine(currentBlock, x, y);                                //volá metodu generování bloku po řádcích
            }
        }

        private void parseBlockLine(Block currentBlock, int x, int y)    //vytváří blok po jednotlivých řádcích. V místě přítomnosti políčka bloku nastaví číslo dle předdefinované barvy bloku
        {
            foreach (char c in currentBlock.ActualOrientation[y - currentBlock.StartRow])
            {
                if (c == '1')
                    Grid[x, y] = currentBlock.ColorNr;
                x++;
            }
        }

        public void udateNextGrid(Block nextBlock)                              //vytvoří pole pro následující blok
        {
            Array.Clear(NextGrid, 0, NextGrid.Length);
            for (int y = 0; y < 2; y++)
            {
                int x = 0;
                parseNextBlockLine(nextBlock, x, y);
            }
        }

        private void parseNextBlockLine(Block nextBlock, int x, int y)    //vytváří blok po jednotlivých řádcích. V místě přítomnosti políčka bloku nastaví číslo dle předdefinované barvy bloku
        {
            {
                foreach (char c in nextBlock.ActualOrientation[y])
                {
                    if (c == '1')
                        NextGrid[x, y] = nextBlock.ColorNr;
                    x++;
                }
            }
        }

        public void drawGrids()                                                             //vykreslí kompletní stav hracího pole
        {
            drawGrid();
            drawBaseGrid();
            drawNextGrid();
        }

        public void drawGrid()                                                              //vykreslí hotové hrací pole s padajícím blokem v aktuální pozici a orientaci
        {
            c1.Clear();
            for (int y = 0; y < rows; y++)                                                  //cyklus pro vykreslení
            {
                for (int x = 0; x < columns; x++)
                {
                    if (Grid[x, y] != 0)
                    {
                        brush.Color = switchDrawingColor(Grid, x, y);
                        Rectangle rectangle = new Rectangle()
                        {
                            Fill = brush,
                            Stroke = Brushes.Black,
                            StrokeThickness = 1,
                            Height = height - 2,
                            Width = width - 2,
                            RadiusX = 2,
                            RadiusY = 2
                        };
                        Canvas.SetLeft(rectangle, x * (width - 1) + x + 2);                 //nastavení pozice
                        Canvas.SetTop(rectangle, y * (height - 1) + y + 2);
                        c1.Add(rectangle);                                                  //přidá Dítě Canvasu
                    }
                }
            }
        }

        public void drawBaseGrid()                                                      //metoda vykreslení pole usazených bloků
        {
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (BaseGrid[x, y] != 0)
                    {
                        baseBrush = new SolidColorBrush(Colors.Black);
                        baseBrush.Color = switchDrawingColor(BaseGrid, x, y);
                        Rectangle rectangle = new Rectangle()
                        {
                            Fill = baseBrush,
                            Stroke = Brushes.Black,
                            StrokeThickness = 1,
                            Height = height - 2,
                            Width = width - 2,
                            RadiusX = 2,
                            RadiusY = 2
                        };
                        Canvas.SetLeft(rectangle, x * (width - 1) + x + 2);
                        Canvas.SetTop(rectangle, y * (height - 1) + y + 2);
                        c1.Add(rectangle);
                    }
                }
            }
        }

        public void drawNextGrid()
        {
            c2.Clear();
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (NextGrid[x, y] != 0)
                    {
                        Rectangle rectangle = new Rectangle();
                        nextBrush.Color = switchDrawingColor(NextGrid, x, y);
                        rectangle.Fill = nextBrush;
                        rectangle.Stroke = Brushes.Black;
                        rectangle.StrokeThickness = 1;
                        rectangle.Height = height - 2;
                        rectangle.Width = width - 2;
                        rectangle.RadiusX = 2;
                        rectangle.RadiusY = 2;
                        Canvas.SetLeft(rectangle, x * (width - 1) + x + 2);
                        Canvas.SetTop(rectangle, y * (height - 1) + y + 2);
                        c2.Add(rectangle);
                    }
                }
            }
        }

        public int copyToBaseGrid()                                                    //metoda pro zkopírování usazeného bloku do pole usazených bloků. Dříve usazené bloky nepřepisuje.
        {
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (Grid[x, y] != 0)
                        BaseGrid[x, y] = Grid[x, y];
                }
            }
            filledRows = checkFilledRows();                                             //volá metodu pro kontrolu, zda je nějaká řada zaplněna. Čísla zaplněných řad se vrací jako seznam
            if (filledRows.Count > 0)                                                   //pokud seznam obsahuje alespoň jednu položku (je zaplněna nějaké řada)                                 
                deleteFilledRows();                                                     //volá se metoda pro odstranění plných řad
            return filledRows.Count;                                                    //metoda vrací počet odstraněných řad pro výpočet skóre
        }

        private List<int> checkFilledRows()                                              //metoda pro kontrolu, zda je nějaká řada zaplněna
        {
            count = 0;
            filledRows = new List<int>();                                               //vždy se generuje nový seznam pro čísla zaplněných řad
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (BaseGrid[x, y] != 0)                                            //pro každou řadu se spočítá počet zaplněných polí pomocí čítače count
                        count++;
                }
                if (count == columns)                                                   //pokud je čítač roven počtu sloupců pole, je řada vyhodnocena jako zaplněná
                    filledRows.Add(y);                                                  //číslo řady je přidáno do seznamu
                count = 0;
            }
            return filledRows;                                                          //metoda vrací seznam s čísly zaplněných řad
        }

        private void deleteFilledRows()                                                  //metoda pro odstranění plných řad
        {
            foreach (int rowNumber in filledRows)                                       //projde položky seznamu
            {
                int yy = rowNumber + 1;
                for (int y = 0; y < rowNumber + 1; y++)
                {
                    yy--;                                                               //postupuje odzadu (spodní řady nejdříve)
                    for (int x = 0; x < columns; x++)                                   //projde jednotlivá políčka řady (sloupce)
                    {
                        if (yy == 0)                                                    //první řada odshora se vždy vloží jako prázdná
                            BaseGrid[x, yy] = 0;
                        else
                        {
                            BaseGrid[x, yy] = BaseGrid[x, yy - 1];                      //u ostatních řad platí, že vyplněnou řadu nahradí řada nad ní
                        }
                    }
                }
            }
        }

        private Color switchDrawingColor(int[,] someGrid, int x, int y)                      //metoda přepnutí barvy štětce před vykreslením pole s padajícím blokem
        {
            Color outputColor = Colors.Black;
            switch (someGrid[x, y])
            {
                case (1):
                    outputColor = Colors.Crimson;
                    break;
                case (2):
                    outputColor = Color.FromRgb(0, 152, 0);
                    break;
                case (3):
                    outputColor = Colors.DarkViolet;
                    break;
                case (4):
                    outputColor = Color.FromRgb(225, 192, 0);
                    break;
                case (5):
                    outputColor = Color.FromRgb(20, 20, 225);
                    break;
                case (6):
                    outputColor = Colors.DarkOrange;
                    break;
                case (7):
                    outputColor = Colors.DarkTurquoise;
                    break;
            }
            return outputColor;
        }
    }
}
