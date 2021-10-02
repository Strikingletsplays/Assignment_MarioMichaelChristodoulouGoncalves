using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAsset : MonoBehaviour
{
    //Static Instance
    public static GameAsset instance;

    private void Awake()
    {
        instance = this;
    }

    //Snake Parts
    public Sprite snakeHeadSprite;
    public Sprite snakeBodySprite;

    //Food Sprite
    public Sprite foodSprite;


    
}
