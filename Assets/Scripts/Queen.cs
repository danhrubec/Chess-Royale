using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece {

    override public bool IsMoveValid(Grid grid, Vector3 orig, Vector3 dest) {
        moveMade = true;
        return IsPathClear(grid, orig, dest);
    }


    //function for checking validsquares. Returns a list of the valid vector3 positions
    public List<Vector3> ValidSquares(Grid grid, Vector3 startingPos)
    {
        List<Vector3> valid = new List<Vector3>();

        for (int i = 0; i < grid.originalBoardSize; i++)
        {
            for (int j = 0; j < grid.originalBoardSize; j++)
            {
                Vector3 potentialDest = new Vector3(i, j);
                Vector3 result = potentialDest - startingPos;

                string dir = GetDirection(startingPos, potentialDest);

                if(dir.Equals("up right") || dir.Equals("down right") || dir.Equals("down left") || dir.Equals("up left") || dir.Equals("up") || dir.Equals("down") || dir.Equals("left") || dir.Equals("right"))
                {
                    if (IsPathClear(grid, startingPos, potentialDest) == true)
                    {
                        valid.Add(potentialDest);
                    }

                }

            }
        }

        return valid;
    }


    override public string GetName() { return "queen"; }

}
