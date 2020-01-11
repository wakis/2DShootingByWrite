using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class setResult : MonoBehaviour
{
    public TextMesh[] texter = new TextMesh[3];
    float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        texter[0].text = "Score : " + gameObj.Score_TIme[0].ToString();
        texter[0].gameObject.SetActive(false);
        texter[1].text = "Time : " + (gameObj.Score_TIme[1]/10).ToString();
        texter[1].gameObject.SetActive(false);
        texter[2].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 0.5f&& !texter[1].gameObject.activeSelf)
        {
            texter[1].gameObject.SetActive(true);
        }
        else if (time > 1f && !texter[0].gameObject.activeSelf)
        {
            texter[0].gameObject.SetActive(true);
        }
        else if (time > 1.5f && !texter[2].gameObject.activeSelf)
        {
            texter[2].gameObject.SetActive(true);
        }else
        {
            if (Input.anyKeyDown) SceneManager.LoadSceneAsync("Start");
        }
    }
}
