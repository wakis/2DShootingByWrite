using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameObj : MonoBehaviour
{
    static public int[] Score_TIme = new int[2];
    public GameObject scoreText;
    public Text scoreTextFront;
    [System.NonSerialized]
    public int score;
    public Text timeText;
    [System.NonSerialized]
    public float time;
    public TextMesh BOSSText;
    [System.NonSerialized]
    public float bossHP;
    public float scrollSpeed = 1f;
    public Vector3[] ScreenSize = new Vector3[2];
    
    public GameObject player;
    public GameObject Boss;
    public bool gameclear;
    public float clearaftime;
    Vector2 BosstextPos;
    [System.NonSerialized]
    public bool onConcentration;
    [System.NonSerialized]
    public float gradationTimePer;
    [System.NonSerialized]
    public float gameDeltaTime;
    [SerializeField]
    float TimePer;
    public float getTimePer { get { return TimePer; } }
    public PauseMenu pauseMenu;

    public bool onMenu;

    private void Awake()
    {
        onMenu = false;
           time = 0f;
        score = 500;
        gameclear = false;
        if (player == null && Boss == null)
        {
            foreach (var p_obj in FindObjectsOfType<Rigidbody2D>())
            {
                if (p_obj.tag == "Player")
                {
                    player = p_obj.gameObject;
                }
                else if (p_obj.tag == "BOSS")
                {
                    Boss = p_obj.gameObject;
                }
            }
        }
        BosstextPos = BOSSText.transform.position;
        //BosstextPos.y = ScreenSize[0].y * 0.8f;
        BOSSText.transform.position += new Vector3(0f, BosstextPos.y, 0f).normalized;
        gradationTimePer = 1f;
        onConcentration = false;
        gameDeltaTime = 0f;
    }
    private void Start()
    {
        ScreenSize[0] = Camera.main.ViewportToWorldPoint(new Vector3(0, 0,10));
        ScreenSize[1] = Camera.main.ViewportToWorldPoint(new Vector3(1, 1,10));
        Score_TIme[0] = Score_TIme[1] = 0;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            pauseMenu.gameObject.SetActive(true);
        }
        if (player == null || player.tag != "Player")
        {
            Awake();
        }
        if (Boss != null)
        {
            timeKeeper();
            if (Boss.transform.position.x<ScreenSize[1].x*1.1f)
            {
                if (BOSSText.transform.position.y< BosstextPos.y)
                {
                    BOSSText.transform.position -= new Vector3(0f, BosstextPos.y, 0f).normalized *gameDeltaTime*2f;
                }else if(BOSSText.transform.position.y != BosstextPos.y)
                {
                    BOSSText.transform.position = (Vector3)BosstextPos * 1f;
                }
            }
        }
        if (!gameclear)
        {
            scoreTextFront.text = "HP:" + score.ToString();
            if (score < 0f) scoreTextFront.text = "HP:" + "0";
            timeText.text = time.ToString("f2");
            if (Boss != null && Boss.GetComponent<eBossMove>())
            {
                if (bossHP != Boss.GetComponent<eBossMove>().getHP * 30 / Boss.GetComponent<eBossMove>().MAXHP)
                {
                    bossHP = Boss.GetComponent<eBossMove>().getHP * 30 / Boss.GetComponent<eBossMove>().MAXHP;
                    string s = "";
                    var strAddByLoop = new System.Text.StringBuilder();
                    for (int lp = 0; lp < bossHP; lp++)
                    {
                        s += "||";
                    }
                    BOSSText.text = s;
                }
                else if (Boss == null)
                {
                    clearaftime = 0f;
                    gameclear = true;
                }
            }else
            {
                if (Boss == null)
                {
                    clearaftime = 0f;
                    gameclear = true;
                }
            }

            if (score < 0f)
            {
                clearaftime += Time.deltaTime;
                Score_TIme[0] = score;
                Score_TIme[1] = (int)(time * 10f);
                player.GetComponent<player>().onPlay = false;
                if (clearaftime > 2f)
                {
                    SceneManager.LoadSceneAsync("Result");
                }
            }
        }
        else if(gameclear)
        {
            clearaftime += Time.deltaTime;
            Score_TIme[0] = score;
            Score_TIme[1] = (int)(time*10f);
            player.GetComponent<player>().onPlay = false;
            if (clearaftime > 1f) {
                BOSSText.text = "Game Clear\nPlease Push Any Button";
                if (Input.anyKeyDown)
                {
                    SceneManager.LoadSceneAsync("Result");
                }
            }
        }
    }

    void timeKeeper()
    {
        if (!onMenu) {
            if (onConcentration)
            {
                if (gradationTimePer > TimePer)
                {
                    gradationTimePer -= (1f - TimePer) * gradationTimePer * Time.deltaTime * 4f;
                }
                else if (gradationTimePer != TimePer)
                {
                    gradationTimePer = TimePer;
                }
            } else
            {
                if (gradationTimePer != 0f && gradationTimePer < 1f)
                {
                    if (gradationTimePer == 0f)
                    {
                        gradationTimePer += 0.1f;
                    }
                    else
                    {
                        gradationTimePer += TimePer * 1 / gradationTimePer * Time.deltaTime * 8f;
                    }
                }
                else if (gradationTimePer != 1f)
                {
                    gradationTimePer = 1f;
                }
            }
        }
        gameDeltaTime = gradationTimePer * Time.deltaTime;
        time += gameDeltaTime;
    }

    public void shockTime()
    {
        gradationTimePer = 0f;
    }
}