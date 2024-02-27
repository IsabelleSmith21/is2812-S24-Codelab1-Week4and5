using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class CoolCounter : MonoBehaviour
{
    public static CoolCounter instance; //singleton instance to make sure only one instance of the script exists

    public TextMeshProUGUI display;

    public int score; //public variable to store the current score

    private const string FILE_DIR = "/DATA/"; //creating the folder where the text file will be 
    private const string DATA_FILE = "hs.txt"; //creating the txt file, constants cause we dont want to mess w it
    private string FILE_FULL_PATH; //not a constant to depend on operating system 
    public int CoolScore //gets and sets the value
    {
        get { return score; }
        private set { score = value; }
    }

    public float timeleft = 5f;
    
    private string highScoresString = "";

    private bool isinGame = true;

    private List<int> highScores; //actual variable that we are manipulating 

    public List<int> HighScores //changing the lowercase variable like it's a function 
    {
        get
        {
            if (highScores == null) //&& File.Exists(FILE_FULL_PATH)) //check if high scores are null and if they are 
            {
                Debug.Log("got from file");
                highScores = new List<int>();
                if (File.Exists(FILE_FULL_PATH))
                {
                    highScoresString = File.ReadAllText(FILE_FULL_PATH); //pull the string from the text file and assign it to hs string
                    highScoresString = highScoresString.Trim(); //trimming the white space
                    string[] highScoreArray = highScoresString.Split("\n"); //split based on the seperator 

                    for (int i = 0; i < highScoreArray.Length; i++) //iterate through the array, translate it back to ints and add the current score to it:
                    {
                        int currentScore = Int32.Parse(highScoreArray[i]);
                        highScores.Add(currentScore);
                    }
                }
            }
          /*  else if (highScores == null)
            {
                Debug.Log("NOPE");
                highScores = new List<int>();
                highScores.Add(3);
                highScores.Add(2);
                highScores.Add(1);
                highScores.Add(0);
            }*/

            return highScores;
        }
    }

    //public int DestroyScore { get; private set; } //public varaible to store the destroy score 

    //reference to textmeshpro for displaying the final score 
    public TextMeshProUGUI CoolScoreText;

    void Awake() //the script instance is being loaded
    {
        if (instance == null) //ensures there is only one instance of the script
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject); //destroy duplicate instance 
        }
    }

    void Start()
    {
        FILE_FULL_PATH = Application.dataPath + FILE_DIR + DATA_FILE;//define what the full file will be based on the operating system 
        /*CoolScore = 0; //initalize scores and print debug messages 
        Debug.Log("CoolCounter initialized. CoolScore: " + CoolScore);
        DestroyScore = 0;
        Debug.Log("DestroyCounter initialized. DestroyScore:" + DestroyScore);*/
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cool")) //if this object collides with an object tagged Cool the cool 
            //score wil go up and that object will be destroyed and the destroy score will go up
        {
            CoolScore++;
            Debug.Log("Collision with Cool object detected. CoolScore: " + CoolScore);

            Destroy(collision.gameObject);
           // DestroyScore++;
          //  Debug.Log("Collision with Cool object detected. DestroyScore: " + DestroyScore);
        }

        if (collision.gameObject.CompareTag("NotCool")) //if this object collides with an object tagged Not Cool the
            //object will be destroyed and the destroy score will go up 
        {
            Destroy(collision.gameObject);
         //   DestroyScore++;
         //   Debug.Log("Collision with Cool object detected. DestroyScore: " + DestroyScore);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isinGame) //If game is running display score and time left assumes true 
        {
            display.text = "Score" + score + (int)timeleft;
        }
        else
        {
            display.text = "score:" + score +
                           "\nHigh Scores:\n" + highScoresString;
        }

        timeleft -= Time.deltaTime;
        if (timeleft <= 0 && isinGame)
        {
            isinGame = false;
            SceneManager.LoadScene("ResultScreen");
            SetHighScore();
        }
    }
    bool isHighScore(int score) //creates a fucntion that determines if the scores is or is not a high score 
    {
        for (int i = 0; i < HighScores.Count; i++) //iterates through every slot in the hs list and check against score value 
        {
            if (highScores[i] < score)
            {
                return true;
            }
        }

        return false;
    }

    void SetHighScore()
    {
        if (isHighScore(score)) //check if the current score is a hs and decide where to put it in the high score list 
        {
            int highScoreSlot = -1;//-1 so it includes slot 0 in the for loop 

            for (int i = 0; i < HighScores.Count; i++)
            {
                if (score > highScores[i])
                {
                    highScoreSlot = i;//iterate through each slot in the list and check if the score is higher if it is higher put it in that slot
                    break;
                }
            }
            
            highScores.Insert(highScoreSlot, score);

            highScores = highScores.GetRange(0, 3);

            string scoreBoardText = "";

            foreach (var highScore in highScores)
            {
                scoreBoardText += highScore + "\n";
            }

            highScoresString = scoreBoardText;
            
            File.WriteAllText(FILE_FULL_PATH, highScoresString);

        }
    }
}


