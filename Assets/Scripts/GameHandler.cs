using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {


    private void Start() {
        LevelGrid.instance = new LevelGrid(19, 19);
    }

}
