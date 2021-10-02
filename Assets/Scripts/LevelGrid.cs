using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid
{
    //LevelGrid Static Instance
    public static LevelGrid instance;

    //position to instantiate food obj
    private Vector2Int foodGridPosition;
    private GameObject foodGameObject;

    //Board dimensions to instantiate food in
    private int width;
    private int height;


    //Constructor
    public LevelGrid(int width, int height) 
    {
        this.width = width;
        this.height = height;
        SpawnFood();
    }

    private void SpawnFood() {
        //Check if food did not spawn on snake, if not spawn food, if yes re roll dise!
        do 
        {
            foodGridPosition = new Vector2Int(Random.Range(1, width), Random.Range(1, height));
        } while (Snake.instance.GetSnakeGridPositionList().IndexOf(foodGridPosition) != -1);

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAsset.instance.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    public bool DidSnakeEatFood(Vector2Int snakeGridPosition) {
        //check snake position if on food obj
        if (snakeGridPosition == foodGridPosition) 
        {
            Object.Destroy(foodGameObject);
            Snake.instance.Score++;
            UIManager.instance.UpdateScore();
            SpawnFood();
            return true;
        } 
        else 
        {
            return false;
        }
    }

}
