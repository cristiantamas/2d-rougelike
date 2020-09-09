using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    /* Use a singleton to avoid having multiple instances of this type of object*/
    public static GameManager instance = null;

    public float turnDelay = .1f;
    public float levelStartDelay = 2f;

    private Text levelText;
    private GameObject levelImage;
    private bool doingSetup; /* Setting up the board and prevent the player from moving while doing so */

    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    private int level = 1;

    private List<Enemy> enemies;
    private bool enemiesMoving;

    [HideInInspector] public bool playersTurn = true;

    void Awake(){
        
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        
        boardScript = GetComponent<BoardManager>();
        enemies = new List<Enemy>();

        InitGame();
    }

    private void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();
    }

    /* Initialise game */
    void InitGame(){

        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear(); 
        boardScript.SetupScene(level);
    }

    private void HideLevelImage(){
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver(){
        levelText.text = "After " + level + " days you died...\n\n Git good noob";
        levelImage.SetActive(true);
        enabled = false;
    }

    private void Update(){
        if (playersTurn || enemiesMoving || doingSetup)
            return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script){
        enemies.Add(script);
    }

    IEnumerator MoveEnemies(){
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if(enemies.Count == 0){
            yield return new WaitForSeconds(turnDelay);
        }

        for(int i = 0; i < enemies.Count; i++){
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}
