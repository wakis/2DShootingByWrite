using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameObj : MonoBehaviour
{
    static public int[] Score_TIme = new int[2];
    public TextMesh scoreText;
    [System.NonSerialized]
    public int score;
    public TextMesh timeText;
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
    private void Awake()
    {
        time = 0f;
        score = 0;
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

    }
    private void Start()
    {
        ScreenSize[0] = Camera.main.ViewportToWorldPoint(new Vector3(0, 0,10));
        ScreenSize[1] = Camera.main.ViewportToWorldPoint(new Vector3(1, 1,10));
        Score_TIme[0] = Score_TIme[1] = 0;
    }
    private void Update()
    {
        if (!gameclear)
        {
            if (player == null || player.tag != "Player")
            {
                Awake();
            }
            if (Boss != null)
            {
                time += Time.deltaTime;
            }
            scoreText.text = score.ToString();
            timeText.text = time.ToString("f2");
            if(Boss!=null&& bossHP != Boss.GetComponent<eBossMove>().getHP * 30 / Boss.GetComponent<eBossMove>().MAXHP)
            {
                bossHP = Boss.GetComponent<eBossMove>().getHP * 30 / Boss.GetComponent<eBossMove>().MAXHP;
                string s = "";
                var strAddByLoop = new System.Text.StringBuilder();
                for (int lp = 0; lp < bossHP; lp++)
                {
                    s += "||";
                }
                BOSSText.text = s;
            }else if (Boss==null)
            {
                clearaftime = 0f;
                gameclear = true;
            }
        }else if(gameclear)
        {
            clearaftime += Time.deltaTime;
            Score_TIme[0] = score;
            Score_TIme[1] = (int)(time*10f);
            player.GetComponent<player>().onPlay = false;
            if (Input.anyKeyDown&& clearaftime>1f)
            {
                BOSSText.text = "Game Clear\nPlease Push Any Button";
                SceneManager.LoadSceneAsync("Result");
            }
        }
    }
}
