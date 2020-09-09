using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/* TO-DO:
 *  - MAKE LEVEL GENRATION MORE INTERESTING
 *  - SPAWN EXIT IN DIFFERENT PLACES TO MAKE IT HARDER TO REACH
 *  - MAYBE A DIFFERENT IDEA FOR SPAWNING ENEMIES
 *  - MAYBE DECREASE FOOD SPAWNING OVER LEVELS TO MAKE IT HARDER
 *  - MAYBE INCREASE BOARD SIZE TO MAKE IT HARDER
 */

public class BoardManager : MonoBehaviour{
    
    [Serializable]
    public class Count{

        public int maximum;
        public int minimum;

        public Count(int max, int min){
            maximum = max;
            minimum = min;
        }
    }

    /* Game board size */
    public int columns = 8;
    public int rows = 8;

    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    
    /* Game objects that will be spawn */
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] foodTiles;
    public GameObject[] wallTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] enemyTiles;

    private Transform boardHolder; /* Keep hierarchy of objects clean */
    private List<Vector3> gridPositions = new List<Vector3>();

    /* Initialise the list of Grid positions
     * We use columns - 1 and rows - 1 so we can keep
     * the border empty, avoiding to make impossible levels
     */
    private void InitialiseList(){

        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
            for (int y = 1; y < rows - 1; y++)
                gridPositions.Add(new Vector3(x, y, 0f));
    }
    
    /* Instantiate the walls of the level and the floor */
    private void BoardSetup(){
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
            for (int y = -1; y < rows + 1; y++){
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                /* Check if we are in a position where an outer wall should be put */
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                /* Game object of floor or outer wall to be added to the boardHolder*/
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject; /* Quaternion.identity -> no rotation */
                instance.transform.SetParent(boardHolder);
            
            }
    }

    /* Return a random position from the gridPositions list to place an object there */
    Vector3 RandomPosition(){
        int randomIndex = Random.Range(0, gridPositions.Count);
        
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex); // Remove position so that we won't place two elements on the same spot

        return randomPosition;
    }

    /* Spawn a tile at a random position */
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum){
        int objectCount = Random.Range(minimum, maximum + 1);

        for(int i = 0; i < objectCount; i++){
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)]; /* Choose a random tile to be instantiated */

            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }


    /* Setup a scene for a level */
    public void SetupScene(int level){
        BoardSetup();
        InitialiseList();

        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        /* Make a logarithmic function based on level number to spawn number of enemies */
        int enemiesCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemiesCount, enemiesCount);

        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
