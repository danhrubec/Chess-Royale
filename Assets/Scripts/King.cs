using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece {
    // king may move 1 space in any direction
    override public bool IsMoveValid(Grid grid, Vector3 orig, Vector3 dest) {

        if (Mathf.Abs(dest.x - orig.x) == 1 && Mathf.Abs(dest.y - orig.y) == 0) { // horizontal
            moveMade = true;
            return true;
        } else if (Mathf.Abs(dest.x - orig.x) == 0 && Mathf.Abs(dest.y - orig.y) == 1) { // vertical
            moveMade = true;
            return true;
        } else if (Mathf.Abs(dest.x - orig.x) == 1 && Mathf.Abs(dest.y - orig.y) == 1) { // diagonal
            moveMade = true;
            return true;
        } else {
            return false;
        }

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

            

                if (Mathf.Abs(potentialDest.x - startingPos.x) == 1 && Mathf.Abs(potentialDest.y - startingPos.y) == 0)
                { // horizontal
                    valid.Add(potentialDest);
                }
                else if (Mathf.Abs(potentialDest.x - startingPos.x) == 0 && Mathf.Abs(potentialDest.y - startingPos.y) == 1)
                { // vertical
                    valid.Add(potentialDest);
                }
                else if (Mathf.Abs(potentialDest.x - startingPos.x) == 1 && Mathf.Abs(potentialDest.y - startingPos.y) == 1)
                { // diagonal
                    valid.Add(potentialDest);
                }
               
            }
        }

        return valid;
    }

    private string name = "king";
    override public string GetName() { return name; }

}
