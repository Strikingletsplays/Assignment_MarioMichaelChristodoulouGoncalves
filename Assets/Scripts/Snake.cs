using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

    //Emun for direction
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }


    //Snake Instance
    public static Snake instance;

    //Movement Variables
    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private List<SnakeMovePosition> snakeMovePositionList;
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
        gridMoveDirection = Direction.Up;

        snakeMovePositionList = new List<SnakeMovePosition>();
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
            if (gridMoveDirection != Direction.Down)               //If snake is not going Down
            {
                gridMoveDirection = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Direction.Up)               //If snake is not going Up
            {
                gridMoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection != Direction.Right)               //If snake is not going Right
            {
                gridMoveDirection = Direction.Left;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection != Direction.Left)               //If snake is not going Left
            {
                gridMoveDirection = Direction.Right;
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

            SnakeMovePosition previousSnakeMovePosition = null;
            if(snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector;
            switch (gridMoveDirection)
            {
                default:
                case Direction.Right: gridMoveDirectionVector = new Vector2Int(1, 0); break;
                case Direction.Left: gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Up: gridMoveDirectionVector = new Vector2Int(0, 1); break;
                case Direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
            }
            gridPosition += gridMoveDirectionVector;

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
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);

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
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
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

        //Manualy add new instances to the list
        foreach(SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }


    //Classes!!
    public class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAsset.instance.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;                           //Set Order of body parts (prevents visual glitch)
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            //angle to rotate sprite after position is set.
            float angle;
            switch (snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up:      //Going Up
                    switch (snakeMovePosition.GetPreviusDirection())
                    {
                        default:                    
                            angle = 0;
                            break;
                        case Direction.Left:        //if Previusly was going left
                            angle = 45;
                            break;
                        case Direction.Right:        //if Previusly was going Right
                            angle = -45;
                            break;
                    }
                    break;
                case Direction.Down:    //Going Down
                    switch (snakeMovePosition.GetPreviusDirection())
                    {
                        default:
                            angle = 180;
                            break;
                        case Direction.Left:        //if Previusly was going left
                            angle = 180 + 45;
                            break;
                        case Direction.Right:        //if Previusly was going Right
                            angle = 180 -45;
                            break;
                    }
                    break;
                case Direction.Left:    //Going Left
                    switch (snakeMovePosition.GetPreviusDirection())
                    {
                        default:
                            angle = -90;
                            break;
                        case Direction.Down:        //if Previusly was going Down
                            angle = -45;
                            break;
                        case Direction.Up:        //if Previusly was going Up
                            angle = 45;
                            break;
                    }
                    break; ;
                case Direction.Right:   //Going to the right
                    switch (snakeMovePosition.GetPreviusDirection())
                    {
                        default: 
                            angle = 90; 
                            break;
                        case Direction.Down:        //if Previusly was going down
                            angle = 45;
                            break;
                        case Direction.Up:        //if Previusly was going Up
                            angle = -45;
                            break;
                    }
                    break;

            }
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
    //Keeps track of previus move of snake
    public class SnakeMovePosition
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        //Constructor
        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;

        }

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
