using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Media;
using System.Diagnostics;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace tetris
{
    class Game : INotifyPropertyChanged                                  //interface pro binding
    {
        //bindované vlastnosti
        public string PauseButtonContent { get; set; }
        public string MusicButtonContent { get; set; }
        public string TimeLabelContent { get; set; }
        public string LevelLabelContent { get; set; }
        public string ScoreLabelContent { get; set; }
        public string HiScoreLabelContent { get; set; }
        public ImageBrush Background { get; set; }
        public Visibility Visib { get; set; }
        public Brush Brush { get { return brush; } }

        public event PropertyChangedEventHandler PropertyChanged;       //pro binding

        protected void notify(string propertyName)                      //notifikační metoda
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        DispatcherTimer timer1 = new DispatcherTimer();                 //instance timeru pro hru
        DispatcherTimer timer2 = new DispatcherTimer();                 //pro stopky
        Block currentBlock, nextBlock;                                  //instance padajícího bloku a následujícího bloku
        bool started;                                                   //pomocný indikátor stavu hry
        bool paused;
        Collissions col;                                                //instance tříd pro vyhodnocení kolizí 
        Movements mov;                                                  //pro pohyb bloku
        Drawing dr;                                                     //pro vykreslování
        string hiScoreStr;
        int hiScoreInt;
        int tr = 1;
        bool playerPaused;
        public int level;
        Stopwatch stopWatch = new Stopwatch();
        int min, sec;

        string path;

        bool bottom;                                             //true, pokud blok dopadne na dno hracího pole
        bool landed;                                                    //true, pokud blok dopadne na předešlé usazené bloky
        int score;                                                      //skóre
        int click = 0;                                                  //pomocný čítač kliků myši na tlačítko pause / resume
        int milisecs;                                                   //počet miulisekund pro interval timeru, který se bude postupně zkracovat
        bool gameOver;                                                  //true, pokud se pole zaplní až nahoru a nastane konec hry

        ColorAnimation ca = new ColorAnimation(Colors.Black, Color.FromRgb(200, 35, 10), TimeSpan.FromSeconds(2));
        SolidColorBrush brush = new SolidColorBrush(Colors.Black);

        public void NotifyAll()
        {
            notify("Visib");                                        //notifikace změny bindovaných vlastností
            notify("PauseButtonContent");
            notify("TimeLabelContent");
            notify("ScoreLabelContent");
            notify("Background");
        }

        public Game(Drawing dr)                          //konstruktor
        {
            this.dr = dr;
            Visib = Visibility.Hidden;
            PauseButtonContent = "Pause";
            TimeLabelContent = "Time";
            score = 0;
            ScoreLabelContent = string.Format("Score:\n{0:n0}", score);
            ImageBrush background = new ImageBrush();
            Background = background;


            NotifyAll();


            col = new Collissions();                                    //instance třídy kolizí
            mov = new Movements();                                      //instance třídy pro pohyby bloku
            dr.createEmptyGrids();                                      //metoda inicializace všech potřebných polí, výmaz Canvasu
            timer1.Tick += timer1_Tick;                                 //event handler intervalu časovače hry
            timer2.Tick += Timer2_Tick;                                 //a stopek
            timer2.Interval = TimeSpan.FromMilliseconds(999);


            //čtení HiScore ze souboru
            path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CSharpTetris";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            try
            {
                using (StreamReader sr = new StreamReader(path + @"\his"))
                {
                    hiScoreStr = sr.ReadLine();
                    hiScoreInt = Convert.ToInt32(hiScoreStr);
                }
            }
            catch
            {
            }
            HiScoreLabelContent = String.Format("HiScore:\n{0:n0}", hiScoreInt);
            notify("HiScoreLabelContent");
        }
        private void Timer2_Tick(object sender, EventArgs e)                //timer pro časomíru, zobrazuje čas od startu hry, měřený stopkami
        {
            min = (int)stopWatch.ElapsedMilliseconds / 1000 / 60;
            sec = (int)stopWatch.ElapsedMilliseconds / 1000 - min * 60;
            TimeLabelContent = String.Format("Time {0:D2}:{1:D2}", min, sec);
            notify("TimeLabelContent");
        }

        public void Start()    //při stisku tlačítka Start
        {
            Visib = Visibility.Hidden;
            notify("Visib");
            tr = 1;
            stopWatch.Reset();
            stopWatch.Start();
            timer2.Start();
            dr.createEmptyGrids();
            score = 0;
            milisecs = 999;
            gameOver = false;
            ScoreLabelContent = string.Format("Score:\n{0:n0}", score);
            PauseButtonContent = "Pause";
            TimeLabelContent = String.Format("Time {0:D2}:{1:D2}", min, sec);
            notify("ScoreLabelContent");
            notify("PauseButtonContent");
            notify("TimeLabelContent");
            currentBlock = new Block();                                     //instance padajícího bloku
            nextBlock = new Block();                                        //instance následujícího bloku
            started = true;                                                 //pomocná proměnná indikující stav, kdy je hra spuštěna
            timer1.Interval = TimeSpan.FromMilliseconds(milisecs);           //nastavení intervalu instance Timeru
            timer1.Start();                                                 //spuštění instance Timneru
            dr.updateGrid(currentBlock);                               //metoda aktualizace pole pro padající blok
            dr.udateNextGrid(nextBlock);                               //metoda aktualizace pole následujícího bloku
            dr.drawGrids();                                            //metoda pro vykreslení
            level = Convert.ToInt32(10 - milisecs / 100);               //výpočet dosaženého levelu

        }
        private void timer1_Tick(object sender, EventArgs e)                                                //hlavní logika hry: v pravidelných časových intervalech (timer pro hru)
        {
            if (started)                                                                                    //pokud je hra spuštěna
            {
                bottom = col.checkBottom(dr);                                                               //test, zda blok dosáhl dna
                landed = col.checkLanded(dr);                                                               //test, zda blok dopadl na usazené bloky

                if (!bottom && !landed && !gameOver)                                                        //pokud blok nedopadl na překážku, na dno a hra neskončila
                {
                    currentBlock.StartRow++;                                                                //posune se o řádek dolů (padá)
                    dr.updateGrid(currentBlock);                                                            //aktualizují se příslušná pole
                    dr.drawGrids();                                                                         //a vykreslí se
                }
                else
                {
                    timer1.Stop();                                                                          //pokud blok dopadl na překážku, zastaví se timer
                    bottom = false;                                                                         //"vynulují" se příslušné indikátory dopadu
                    landed = false;
                    int countLines = dr.copyToBaseGrid();                                                   //metoda pro spočtení zaplněných řádků
                    if (countLines > 0)                                                                     //pokud existují
                    {
                        score = score + countLines * 100 + (countLines - 1) * 50;                           //upraví se skóre: za každý plný řádek 100 bodů, navíc extra 50 bodů za vícenásobné řádky
                        milisecs = milisecs - 5;                                                            //zrychlí se timer
                        if (milisecs < 70)                                                                  //dolní mez pro timer, pod ni se již nezrychluje
                            milisecs = 70;
                        level = Convert.ToInt32(10 - milisecs / 100);
                        LevelLabelContent = "Level " + level;
                        notify("LevelLabelContent");

                        ScoreLabelContent = string.Format("Score:\n{0:n0}", score);                                 //aktualizace zobrazení skóre
                        notify("ScoreLabelContent");
                    }
                    gameOver = col.checkGameOver(dr);                                                           //metoda pro vyhodnocení konce hry
                    if (gameOver)                                                                               //pokud nastal
                    {
                        started = false;                                                                        //zobrazí se nápis a hra se zastaví
                        ScoreLabelContent = string.Format("Score:\n{0:n0}\nGAME OVER", score);
                        notify("ScoreLabelContent");
                        stopWatch.Stop();
                        timer1.Stop();
                        timer2.Stop();
                        if (score > hiScoreInt)                                                                 //je-li překonáno HiScore
                        {
                            hiScoreInt = score;
                            HiScoreLabelContent = String.Format("HiScore:\n{0:n0}", hiScoreInt);                //animuje se nápis "NEW!"
                            notify("HiScoreLabelContent");
                            ca.RepeatBehavior = new RepeatBehavior(10);
                            brush.BeginAnimation(SolidColorBrush.ColorProperty, ca);
                            notify("Brush");
                            Visib = Visibility.Visible;
                            notify("Visib");
                            using (StreamWriter sw = new StreamWriter(path + @"\his"))                          //zápis HiScore do souboru
                            {
                                hiScoreStr = hiScoreInt.ToString();
                                sw.WriteLine(hiScoreStr);
                                sw.Flush();
                            }
                        }
                    }
                    else
                    {
                        currentBlock = nextBlock;                                                           //jinak hra pokračuje: z následujícího bloku se stane současný blok
                        dr.updateGrid(currentBlock);                                                        //aktualizace polí
                        nextBlock = new Block();                                                            //aktualizace následujícího bloku
                        dr.udateNextGrid(nextBlock);
                        dr.drawGrids();                                                                     //vykreslení
                        timer1.Interval = TimeSpan.FromMilliseconds(milisecs);                              //úprava intervalu timeru a spuštění
                        timer1.Start();
                    }
                }
            }
        }

        public void pauseGame()
        {
            click++;                                                                                        //čítač kliků sleduje stav tlačítka
            if (click == 1 && started)
            {
                paused = true;
                PauseButtonContent = "Resume";                                                              //pokud je tačítko stisknuto poprvé, mění se na tlačítko Resume
                notify("PauseButtonContent");
                started = false;                                                                            //hra se pozastaví
                stopWatch.Stop();
                timer1.Stop();
                timer2.Stop();

            }
            else if (click == 2 && paused)                                                                             //následující stisk znamená obnovení hry a návrat tlačítka do původního stavu
            {
                paused = false;
                PauseButtonContent = "Pause";
                notify("PauseButtonContent");
                started = true;
                stopWatch.Start();
                timer1.Start();
                timer2.Start();
                click = 0;
            }
            else
                click = 0;
        }

        public void KeyDown(Key k)                          //zde se po stisku příslušných kláves volají metody pro pohyby bloku:
        {
            if (k == Key.Space)
                pauseGame();

            else if (started)
            {
                switch (k)
                {
                    case (Key.W):                                                            //rotace
                        mov.rotate(dr, currentBlock, col, bottom, landed);
                        break;
                    case (Key.S):                                                            //rychlý pád dolů
                        if (!bottom && !landed)
                            timer1.Interval = TimeSpan.FromMilliseconds(10);
                        else
                            timer1.Interval = TimeSpan.FromMilliseconds(milisecs);
                        break;
                    case (Key.A):                                                            //doleva
                        mov.moveLeft(dr, currentBlock, col, bottom);
                        break;
                    case (Key.D):                                                            //doprava
                        mov.moveRight(dr, currentBlock, col, bottom);
                        break;
                }
            }
        }
    }
}
