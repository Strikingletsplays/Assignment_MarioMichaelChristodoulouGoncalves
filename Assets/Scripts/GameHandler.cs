using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {


    private void Awake() {
        LevelGrid.instance = new LevelGrid(19, 19);                             //Create a 19x19 Grid So food dosent spawn on the edge on the grid
        LevelGrid.instance.SpawnFood(Random.Range(0, 10));                      //Spawn the first food Prefab
    }

}
