using System;
using System.Collections.Generic;
using System.Text;

namespace tetris
{
    class Movements
    {
        bool collission;
        bool nearLeft, nearRight;

        public void rotate(Drawing dr, Block currentBlock, Collissions col, bool bottom, bool landed)
        {
            if (!bottom && !landed)                                                                            //pokud blok nedopadl
            {
                bool left = col.checkLeft(currentBlock, dr.Grid, dr.BaseGrid);
                bool right = col.checkRight(currentBlock, dr.columns, dr.Grid, dr.BaseGrid);
                if (currentBlock.Type == "line")
                {
                    nearLeft = col.checkNearLeft(currentBlock.StartColumn, currentBlock.ActualOrientation);
                    nearRight = col.checkNearRight(currentBlock.StartColumn, currentBlock.ActualOrientation, dr.columns);
                }
                currentBlock.rotateBlock(left, right, nearLeft, nearRight, true);//pokus o rotaci
                bool outOfRange = dr.TryUpdateGrid(currentBlock);
                if (!outOfRange)
                {
                    collission = col.checkRotation(currentBlock, dr);
                }
                currentBlock.StartColumn = currentBlock.OrigStartColumn;                              //návrat do původní pozice v ose x

                if (!collission && !outOfRange)
                {
                    currentBlock.rotateBlock(left, right, nearLeft, nearRight, false);//rotace
                    dr.updateGrid(currentBlock);
                    dr.drawGrids();
                }
            }
        }

        public void moveLeft(Drawing dr, Block currentBlock, Collissions col, bool bottom)
        {
            bool left = (col.checkLeft(currentBlock, dr.Grid, dr.BaseGrid));
            if (!left && !bottom)
            {
                currentBlock.StartColumn--;
                dr.updateGrid(currentBlock);
                dr.drawGrids();
            }
        }

        public void moveRight(Drawing dr, Block currentBlock, Collissions col, bool bottom)
        {
            bool right = (col.checkRight(currentBlock, dr.columns, dr.Grid, dr.BaseGrid));
            if (!right && !bottom)
            {
                currentBlock.StartColumn++;
                dr.updateGrid(currentBlock);
                dr.drawGrids();
            }
        }
    }
}
