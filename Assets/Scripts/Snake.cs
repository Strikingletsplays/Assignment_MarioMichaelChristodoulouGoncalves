using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    //Snake Instance
    public static Snake instance;

    //Movement Variables
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private List<Vector2Int> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;

    //Main Score
    public int Score = 0;

    //Snake Stats
    private int snakeBodySize;
    


    private void Awake()
    {
        instance = this;
        gridPosition = new Vector2Int(10, 10);
        gridMoveTimerMax = .3f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1, 0);

        snakeMovePositionList = new List<Vector2Int>();
        snakeBodyPartList = new List<SnakeBodyPart>();
        snakeBodySize = 0;
    }

    private void Update()
    {
        SnakeMovement();
        GridMovement();
    }

    private void SnakeMovement()
    {
        //Change Snakes Direction
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection.y != -1)               //If snake is not going Down
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection.y != 1)               //If snake is not going Up
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection.x != 1)               //If snake is not going Right
            {
                gridMoveDirection.x = -1;
                gridMoveDirection.y = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection.x != -1)               //If snake is not going Left
            {
                gridMoveDirection.x = 1;
                gridMoveDirection.y = 0;
            }
        }
    }
    private void GridMovement()
    {
        //Increase timer
        gridMoveTimer += Time.deltaTime;
        //if timer exceeds the time set, move snake, and reset more timer
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            snakeMovePositionList.Insert(0, gridPosition);

            gridPosition += gridMoveDirection;

            //Let the LevelGrid know that snake ate food
            bool snakeAteFood = LevelGrid.instance.DidSnakeEatFood(gridPosition);
            if (snakeAteFood)
            {
                //Snake ate some food, lets grow
                snakeBodySize++;
                CreateSnakeBody();
            }

            //Check if snakeBodySize is bigger than the snakeMocePositionList, and stop drawing snake trail. (remove last possition from the list)
            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            /*for (int i = 0; i < snakeMovePositionList.Count; i++)
            {
                Vector2Int snakeMovePossition = snakeMovePositionList[i];
                spawn sprite of snakes body part (snakeMovePossition)
                remove sprite before snake moves again

            }*/

            //Move Snake Head
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90);

            //Move Snake Body
            UpdateSnakeBodyParts();
        }    
    }

    private void CreateSnakeBody()
    {
        //Create new gameobject body part, and add it to the list
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }
    

    //FIX!!
    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetGridPosition(snakeMovePositionList[i]);
        }
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    //Return full list of positions occupied by the snake.
    public List<Vector2Int> GetSnakeGridPositionList()
    {
        //Declare a new list 
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };

        gridPositionList.AddRange(snakeMovePositionList);
        return gridPositionList;
    }

    public class SnakeBodyPart
    {
        private Vector2Int gridPosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAsset.instance.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;                           //Set Order of body parts (prevents visual glitch)
            transform = snakeBodyGameObject.transform;
        }

        public void SetGridPosition(Vector2Int gPosition)
        {
            gridPosition = gPosition;
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
        }
    }
}
