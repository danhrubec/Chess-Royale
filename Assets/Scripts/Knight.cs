using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece {

    public override bool IsMoveValid(Grid grid, Vector3 orig, Vector3 dest) { 
        
        if(this.enabled == false)
        {
            Debug.Log("Piece is disabled");
            return false;
        }

        Vector3 result = dest - orig;

        if ((Mathf.Abs(result.x) == 2 && Mathf.Abs(result.y) == 1) || (Mathf.Abs(result.y) == 2 && Mathf.Abs(result.x) == 1))
        {
            moveMade = true;
            return true;
        }

        return false;

    }

    //function for checking validsquares. Returns a list of the valid vector3 positions
    public List<Vector3> ValidSquares(Grid grid, Vector3 startingPos)
    {
        List<Vector3> valid = new List<Vector3>();

        for(int i = 0; i<grid.originalBoardSize;i++)
        {
            for(int j = 0; j < grid.originalBoardSize;j++)
            {
                Vector3 potentialDest = new Vector3(i, j);
                Vector3 result = potentialDest - startingPos;

                if ((Mathf.Abs(result.x) == 2 && Mathf.Abs(result.y) == 1) || (Mathf.Abs(result.y) == 2 && Mathf.Abs(result.x) == 1))
                {
                    valid.Add(potentialDest);
                }
            }
        }

        return valid;
    }

    private string name = "knight";
    override public string GetName() { return name; }

}
