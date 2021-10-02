using UnityEngine;
using LINQtoCSV;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class SpawnObjFood : MonoBehaviour
{

    public static SpawnObjFood instance;                                                                                //Instance of SpawnObjFood
    public GameObject[] foods;                                                                                          //The food Prefabs
    public Dictionary<string, GameObject> foodPrefabs = new Dictionary<string, GameObject>();                           //A dictionary to groop the food names with their gameobjects
    public TextAsset foodObjects;                                                                                       //This CSV file contains all the food that will be spawned in the scene

                                                                                                                        //Seting up LINQtoCSV library
    CsvFileDescription inputFileDescription = new CsvFileDescription                                                    //Function used to separate data from CSV file
    {
        SeparatorChar = ',',
        FirstLineHasColumnNames = true
    };

    private void Awake()
    {
        instance = this;                                                                                                //Set the instance of this class
        foreach (var obj in foods)                                                                                      //Add food prefabs to dictionary
        {
            foodPrefabs[obj.name] = obj;
        }
    }

    public GameObject SpawnFoodObjs(int random)                                                                         //Spawn one of the food objects
    {
        GameObject FinalFoodObj = null;
        using (var ms = new MemoryStream())
        {
            using (var txtWriter = new StreamWriter(ms))
            {
                using (var txtReader = new StreamReader(ms))
                {
                    txtWriter.Write(foodObjects.text);
                    txtWriter.Flush(); 
                    ms.Seek(0, SeekOrigin.Begin);                                                                       //Go to the very begining of the stream
                    CsvContext cc = new CsvContext();
                    cc.Read<FoodObjects>(txtReader, inputFileDescription)                                               //Read the data from the CSV
                        .ToList()
                        .ForEach(so =>
                        {
                            if (!LevelGrid.instance.FoodObjIsSpawned)
                            {
                                if (random >= 5 && so.PrefabName == "RedApple")                                         //Spawning RedApple if random number is >= 5
                                {
                                    LevelGrid.instance.FoodObjIsSpawned = true;
                                    //Create an instance of the named prefab
                                    GameObject copy = Instantiate(foodPrefabs[so.PrefabName]);                          //Instantiate Prefab
                                    copy.name = so.PrefabName;                                                          //Set Name
                                    copy.GetComponent<FoodStats>().Color = so.Color;                                    //Set Color
                                    copy.GetComponent<FoodStats>().Points = so.Points;                                  //Set Points
                                    FinalFoodObj = copy.gameObject;
                                }
                                else if (random <= 4 && so.PrefabName == "GreenApple")                                  //Spawning GreenApple if random number is <= 4
                                {
                                    LevelGrid.instance.FoodObjIsSpawned = true;
                                    GameObject copy = Instantiate(foodPrefabs[so.PrefabName]);                          //Instantiate Prefab
                                    copy.name = so.PrefabName;                                                          //Set Name
                                    copy.GetComponent<FoodStats>().Color = so.Color;                                    //Set Color
                                    copy.GetComponent<FoodStats>().Points = so.Points;                                  //Set Points
                                    FinalFoodObj = copy;
                                }
                            }
                        });
                }
            }
        }
        return FinalFoodObj;                                                                                            //Returned the random food object to spawn
    }
}
