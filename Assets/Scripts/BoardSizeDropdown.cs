using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSizeDropdown : MonoBehaviour
{
    public void HandleInputData(int val) {

        if (val == 0) {
            GoBetween.boardSize = 16;
        }

        if (val == 1) {
            GoBetween.boardSize = 24;
        }

        if (val == 2) {
            GoBetween.boardSize = 32;
        }

    }
}
