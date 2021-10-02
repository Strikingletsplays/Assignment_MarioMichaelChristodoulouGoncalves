using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid instance;                                                                       //LevelGrid Static Instance

    private Vector2Int foodGridPosition;                                                                    //Position of chosen foodObject
    private GameObject foodGameObject;                                                                      //Chosen foodObject

    private int width;                                                                                      //Board dimensions
    private int height;

    public bool FoodObjIsSpawned;                                                                           //Variable to check if Food is spawned

    string LastsFoodEaten = "";                                                                             //Keep track of witch Food type was last eaten
    public int Multiplier = 1;                                                                              //Multiplier of food streak

    public LevelGrid(int width, int height)                                                                 //Constructor
    {
        FoodObjIsSpawned = false;
        this.width = width;
        this.height = height;
    }

   
    public void SpawnFood(int number)                                                                       //Function to spawn random type of food
    {
        do                                                                                                  //Check if food possition is on snake, if not spawn food, if yes re roll dise!
        {
            foodGridPosition = new Vector2Int(Random.Range(1, width), Random.Range(1, height));
        } while (Snake.instance.GetSnakeGridPositionList().IndexOf(foodGridPosition) != -1);

        foodGameObject = SpawnObjFood.instance.SpawnFoodObjs(number);
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    public bool DidSnakeEatFood(Vector2Int snakeGridPosition) {
        if (snakeGridPosition == foodGridPosition)                                                          //check snake position if on food obj
        {
            if (LastsFoodEaten == foodGameObject.GetComponent<FoodStats>().name)                            //Check is Streak Begins
            {
                Multiplier++;
                Snake.instance.Score += Multiplier * foodGameObject.GetComponent<FoodStats>().Points;
            }
            else                                                                                            //Streak ended :(
            {
                Multiplier = 1;
                Snake.instance.Score += foodGameObject.GetComponent<FoodStats>().Points;
            }
            LastsFoodEaten = foodGameObject.GetComponent<FoodStats>().name;
            Object.Destroy(foodGameObject);
            UIManager.instance.UpdateScore();
            FoodObjIsSpawned = false;
            SpawnFood(Random.Range(0, 10));
            return true;
        } 
        else 
        {
            return false;
        }
    }

    public void CheckBorders(Vector2Int gridPosition)
    {
        if(gridPosition.x < 1 || gridPosition.y < 1 || gridPosition.x > width || gridPosition.y > height)
        {
            Time.timeScale = 0;                                                                             //Freeze timescale
            EndScore.instance.EnableEndCanvas();                                                            //Enable Death UI
            EndScore.instance.ShowEndScore();                                                               //Update EndScore
        }
    }
}
