using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAsset : MonoBehaviour
{
    public static GameAsset instance;                           //Static Instance

    private void Awake()
    {
        instance = this;
    }
                                                                //Snake Parts
    public Sprite snakeHeadSprite;
    public Sprite snakeBodySprite;           
}
