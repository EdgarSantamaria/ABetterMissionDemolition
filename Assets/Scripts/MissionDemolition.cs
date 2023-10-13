using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;  //a private Singleton

    [Header("Inscribed")]
    public Text uitLevel;           //the UIText_level Text
    public Text uitShots;           //the UIText_shots Text
    public Vector3 castlePos;       //the place to put the castles
    public GameObject[] castles;    //An array of the castles

    [Header("Dynamic")]
    public int level;               //The current level
    public int levelMax;            //The number of levels
    public int shotsTaken;
    public GameObject castle;       //The current castle
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; //FollowCam mode

    // Start is called before the first frame update
    void Start()
    {
        S = this;  //Define the Singleton

        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        //Get rid of the old castle if one exists
        if (castle != null)
        {
            Destroy(castle);
        }

        //Destroy old projectiles if they exist 
        Projectile.DESTROY_PROJECTILES();

        //Instantiate the new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        //Reset the goal
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;

        //Zoom out to show both
        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }

    void UpdateGUI()
    {
        //Show the data in the GUITexts
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();

        //Check for level end
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            //Change mode to stop checking for level end
            mode = GameMode.levelEnd;
            //Zoom out to show both
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);
            //Start the next level in 2 seconds
            Invoke("NextLevel", 2f);
        }

    }

    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
            shotsTaken = 0;
        }
        StartLevel();
    }

    //Static method that allows code anywhere to increment shotsTaken
    static public void SHOT_FIRED()
    {
        S.shotsTaken++;
    }

    //Static method that allows code anywhere to get a reference to S.castle
    static public GameObject GET_CASTLE()
    {
        return S.castle; 
    }
}
