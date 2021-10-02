using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public enum Direction                                                                                   //Emun for snake directions
    {
        Left,
        Right,
        Up,
        Down
    }

    public static Snake instance;                                                                           //Snake Instance
                                                                                                            //Snake Movement Variables 
    private Direction gridMoveDirection;                                                                    //Snake Movement Direction
    private Vector2Int gridPosition;                                                                        //Snake Movement Possition on grid                                                       
    private float gridMoveTimer;                                                                            //Timer for movement
    private float gridMoveTimerMax;                                                                         //Timer max value
    private List<SnakeMovePosition> snakeMovePositionList;                                                  //A list of all the positions the snake has been (based on Snake size)
    private List<SnakeBodyPart> snakeBodyPartList;                                                          //A list of all Snake Body Parts

    public int Score = 0;                                                                                   //Main Score

    private int snakeBodySize;                                                                              //Snake Size

    private void Awake()
    {
        instance = this;                                                                                    //Set instance to this
        gridPosition = new Vector2Int(10, 10);                                                              //Set Snake Position in middle of grid
        gridMoveTimerMax = .2f;                                                                             //Set Timer/Speed of snake
        gridMoveTimer = gridMoveTimerMax;                                                                   //Set gridMoveTimer to the max value
        gridMoveDirection = Direction.Up;                                                                   //Snake Start pointing Direction

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodyPartList = new List<SnakeBodyPart>();
        snakeBodySize = 0;
    }

    private void Update()
    {
        SnakeMovement();                                                                                    //Function to Detect player input (arrow keys)
        GridMovement();                                                                                     //Move Snake & Check if snake ate food
        if (DidSnakeDie())
        {
            Time.timeScale = 0;                                                                             //Freeze timescale
            EndScore.instance.EnableEndCanvas();                                                            //Enable Death UI
            EndScore.instance.ShowEndScore();                                                               //Update EndScore
        }
    }

    private void SnakeMovement()
    {                                                                                                       //Change Snakes Direction                                                                                                      
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection != Direction.Down)                                                        //If snake is not going Down
            {
                gridMoveDirection = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Direction.Up)                                                          //If snake is not going Up
            {
                gridMoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection != Direction.Right)                                                       //If snake is not going Right
            {
                gridMoveDirection = Direction.Left;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection != Direction.Left)                                                        //If snake is not going Left
            {
                gridMoveDirection = Direction.Right;
            }
        }
    }
    private void GridMovement()
    {
        gridMoveTimer += Time.deltaTime;                                                                    //Increase timer for snake step
        if (gridMoveTimer >= gridMoveTimerMax)                                                              //if times up, move snake, and reset timer
        {
            gridMoveTimer -= gridMoveTimerMax;

            SnakeMovePosition previousSnakeMovePosition = null;                                             //Variable to check last position
            if(snakeMovePositionList.Count > 0)                                                             //If moved at least once
            {
                previousSnakeMovePosition = snakeMovePositionList[0];                                       //Set previus Snake Position 
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);                            //Saving priviusPosition, gridPosition & moveDirection of snake.
            snakeMovePositionList.Insert(0, snakeMovePosition);                                             //Insert above object to the list the "snakeMovePosition"

            Vector2Int gridMoveDirectionVector;
            switch (gridMoveDirection)                                                                      //Converting emun values to Vector values and moving
            {
                default:
                case Direction.Right: gridMoveDirectionVector = new Vector2Int(1, 0); break;
                case Direction.Left: gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Up: gridMoveDirectionVector = new Vector2Int(0, 1); break;
                case Direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
            }
            gridPosition += gridMoveDirectionVector;

            LevelGrid.instance.CheckBorders(gridPosition);

            bool snakeAteFood = LevelGrid.instance.DidSnakeEatFood(gridPosition);                           //Find out if snake ate food
            if (snakeAteFood)
            {
                snakeBodySize++;                                                                            //if yes, Lets Grow!
                CreateSnakeBody();
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1)                                           //remove tail snake part from the list
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);                               //Move Snake Head
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);    //rotate Snake Head

            UpdateSnakeBodyParts();                                                                         //Move Snake Body
        }    
    }

    private void CreateSnakeBody()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));                                  //Create new GameObject body part, and add it to snakeBodyPartList
    }
    
    private void UpdateSnakeBodyParts()                                                                     //Move all snake body parts through the list
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);                            //Set history possitions of snake
        }
    }

    private float GetAngleFromVector(Vector2Int dir)                                                        //Rotate Snake Head
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }

    public List<Vector2Int> GetSnakeGridPositionList()                                                      //Return full list of positions occupied by the snake.
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };                        //Declare a new list 

        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)                              //Manualy add new instances to the list
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }

    public bool DidSnakeDie()                                                                               //Check if snake is dead
    {
        foreach(var obj in snakeMovePositionList)
        {
            if(obj.GetGridPosition() == gridPosition)                                                       //If current head position collided with any bodypart
            {
                return true;
            } 
        }
        return false;
    }

    public class SnakeBodyPart                                                                                //SnakeBodyPart class to store bodypart atributes
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAsset.instance.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;                     //Set Order of body parts (prevents visual glitch)
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)                                 //Set snakeMovePosition
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);
        }
    }
    public class SnakeMovePosition                                                                            //Keeps track of all mAtributes of snake
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)             //Constructor to set all atributes
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }
                                                                                                             //Geter functions
        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public Direction GetPreviusDirection()
        {
            if(previousSnakeMovePosition == null)
            {
                return Direction.Up;
            }
            else
            {
                return previousSnakeMovePosition.direction;
            }
            
        }
    }
}
