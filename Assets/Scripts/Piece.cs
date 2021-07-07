using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// all pieces should inherit this class and override its IsMoveValid method so it can be called in MoveOnGrid; in Grid, pieces should be attached as scripts to GameObjects
abstract public class Piece : MonoBehaviour {

    public bool moveMade = false;
    public int currX = 0;
    public int currY = 0;
    // returns a string with the direction a piece wants to move
    protected string GetDirection(Vector3 orig, Vector3 dest) {

        Vector3 result = dest - orig;

        if (result.x >= 1) {
            if (result.y >= 1 && (result.y - result.x) == 0) {
                return "up right";
            }
            if (result.y <= -1 && (result.y + result.x) == 0) {
                return "down right";
            }
            if(result.y == 0)
            {
                return "right";
            }
        }

        if (result.x <= -1) {
            if (result.y >= 1 && (result.y + result.x) == 0) {
                return "up left";
            }
            if (result.y <= -1 && (result.y - result.x) == 0) {
                return "down left";
            }
            if(result.y == 0)
            {
                return "left";

            }
        }

        if (result.y >= 1 && result.x == 0) {
            return "up";
        }
        if (result.y <=1 && result.x == 0)
        {
            return "down";
        }
        return "not allowed";
    }
    //pieces override this method to get their name returned
    public abstract string GetName();

    // pieces override this method for their specific movement rules
    public abstract bool IsMoveValid(Grid grid, Vector3 orig, Vector3 dest);
    // checks if any intervening pieces prevent a piece from moving along a path
    protected bool IsPathClear(Grid grid, Vector3 orig, Vector3 dest) {

        string dir = GetDirection(orig, dest);
        Vector3 check = new Vector3(orig.x, orig.y, 0);

        if (dir.Equals("left")) {

            for (int i = (int)check.x; i > dest.x + 1; i--) {
                check = check + Vector3.left;
                if (!grid.IsSpaceClear(check)) {
                    return false;
                }
            }

            return true;

        } else if (dir.Equals("up")) {

            for (int i = (int)check.y; i < dest.y - 1; i++) {
                check = check + Vector3.up;
                if (!grid.IsSpaceClear(check)) {
                    return false;
                }
            }

            return true;

        } else if (dir.Equals("right")) {

            for (int i = (int)check.x; i < dest.x - 1; i++) {
                check = check + Vector3.right;
                if (!grid.IsSpaceClear(check)) {
                    return false;
                }
            }

            return true;

        } else if (dir.Equals("down")) {

            for (int i = (int)check.y; i > dest.y + 1; i--) {
                check = check + Vector3.down;
                if (!grid.IsSpaceClear(check)) {
                    return false;
                }
            }

            return true;

        } else if (dir.Equals("down left")) {

            for (int i = (int)check.x; i > dest.x + 1; i--) {
                check = check + Vector3.left + Vector3.down;
                if (!grid.IsSpaceClear(check)) {
                    return false;
                }
            }

            return true;

        } else if (dir.Equals("up left")) {

            for (int i = (int)check.x; i > dest.x + 1; i--) {
                check = check + Vector3.left + Vector3.up;
                if (!grid.IsSpaceClear(check)) {
                    return false;
                }
            }

            return true;

        } else if (dir.Equals("up right")) {

            for (int i = (int)check.x; i < dest.x - 1; i++) {
                check = check + Vector3.right + Vector3.up;
                if (!grid.IsSpaceClear(check)) {
                    return false;
                }
            }

            return true;

        } else if (dir.Equals("down right")) {

            for (int i = (int)check.x; i < dest.x - 1; i++) {
                check = check + Vector3.right + Vector3.down;
                if (!grid.IsSpaceClear(check)) {
                    return false;
                }
            }

            return true;

        } else {
            throw new System.Exception("Path specified is invalid");
        }

    }
}
