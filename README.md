Video Sample of Game: https://youtu.be/welSnze64bs

External tools/assets Used: 
LinqToCSV: Used for deserializing Food data from CSV file: more details here: 
https://github.com/mperdeck/LINQtoCSV
https://www.codeproject.com/Articles/25133/LINQ-to-CSV-library


Lets start from the Game Handler. In this script we Create a 19x19 grid that defines our level boundaries and Instantiate a food object. 

Inside the LevelGrid script, we check and prevent food objects spawned directly on the player. We also detect if the player has ate food objects and start food streaks! The last function checks if the player collides with the borders of the grid and end the level.

Snake Script, all the player controls, move timer, and checks for available moves are done here. We create the snake boddy, define gridMoveTimer and a gridMoveTimerMax used to set the speed of the snake steps. We also define the function GetSnakeGridPositionList, used to return a list of all the grid possitions the snake currently is. In addition we define two classes in this script, SnakeMovePosition to keep track of all atributes of the snake (previousSnakeMovePosition,gridPosition,direction), and the other (SnakeBodyPart) to hold snake body parts and their order (bodyIndex). Lastly we check if the snake hits its own tail and if yes, end the level.

SpawnObjFood script is responsible for deserializing the CSV file containing the two food object data and saving them in a list of "FoodObjects", a class created to define all atributes of the food objects. With the help of LinqtoCSV and by adding the food prefabs to a dictionary, we can Instantiate out prefabs using the PrefabName food atribute located inside our now deserialised CVS and save the other deserialized data to FoodStats. Using a random generated number as a parameter and a forEach loop going through the CSV prefabs, we can spawn the FoodObjects types randomly.

if the LinqtoCSV library didint exist, I would attempt to save all data information to a .txt file (or any other different format i.e .xml, .json) and attempt to deserialize it using a different method. (Method Unknown, I would have to research as i did to find LinqtoCSV).

GameAsset scripts has just the bodypart sprites defined and used inside of the Snake Script. (via the GameAsset instance)

UIManager,EndScore and Menu are Ui scripts responsible for displaying the correct UI elements and score numbers.
