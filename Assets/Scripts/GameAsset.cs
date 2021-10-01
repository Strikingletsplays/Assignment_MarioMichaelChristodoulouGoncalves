using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAsset : MonoBehaviour
{
    //Static Instance
    public static GameAsset instance;

    //Snake Parts
    public Sprite snakeHeadSprite;


    private void Awake()
    {
        instance = this;
    }
}
