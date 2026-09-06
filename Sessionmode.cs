using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Sessionmode : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI sessioncounttext = null;
    [SerializeField] TMPro.TextMeshProUGUI timertext = null;
    [SerializeField] TMPro.TextMeshProUGUI highscoretext = null;
    [SerializeField] TMPro.TextMeshProUGUI gameoverhighscoretext = null;
    [SerializeField] TMPro.TextMeshProUGUI gameoverscoretext = null;
    [SerializeField] TMPro.TextMeshProUGUI gamepaustimetext = null;
    [SerializeField] TMPro.TextMeshProUGUI gamepausehighestscoretext = null;
    [SerializeField] TMPro.TextMeshProUGUI gamepausecurentscoretext = null;
    [SerializeField] TMPro.TextMeshProUGUI SwipeHintText = null;
    [SerializeField] RectTransform SwipeHintTexttransform = null;
    [SerializeField] GameObject howtoplaypanel = null;
    [SerializeField] GameObject pausemenupanel = null;
    [SerializeField] GameObject gameoverpanel = null;
    [SerializeField] Camera camera = null;
    [SerializeField] UnityEngine.UI.Image [] mazedots = null; 
    [SerializeField] Mazegenerator mazegen = null;
    [SerializeField] Contarollermovement contaroller = null;
    [SerializeField] GameObject timedot = null;
    [SerializeField] GameObject blockpanel = null;
    List<GameObject> activedots = new List<GameObject>();

    [SerializeField] int mazecount = 0;
    [SerializeField] int sessioncount= 1;
    int highscore = 0;
    

    [SerializeField] float remaningtime = 0;
    float giventime = 0;

    int remaingtimedot = 4;
    bool ismazecompaleted = false;
    bool issessioncompaleted= false;
    [SerializeField]bool starttimer = false;
    bool timeisover = false;

    private void Awake()
    {

        camera = GetComponent<Camera>();
        int randomsize = Random.Range(15, 20);
        mazegen.Setmazesize(randomsize);
        gameoverpanel.SetActive(false);
        if(PlayerPrefs.GetInt("RushFirstTime", 0) == 0) 
        {
            openhowtoplay();
        }
    


    }

    private void Start()
    {
        //AdManager.Instance.ShowBanner();
        setcamera();
        setthatimedot();
        giventime = remaningtime;
        mazecount++;
        timeisover = false;
        setmazecountdots();
        sessioncounttext.text = "" + sessioncount;
        timertext.text = "" + (int)giventime;
        starttimer = true;
      

    }


    private void Update()
    {
        if (!timeisover)
        {
            if (contaroller.getisfirstmove())
            {
                if (SwipeHintText.gameObject.activeSelf) { SwipeHintText.gameObject.SetActive(false); }
                if (starttimer)
                {
                  
                    chakemazcompaleted();
                    chaketimer();
                }
            }
            else
            {
                if (!SwipeHintText.gameObject.activeSelf) { SwipeHintText.gameObject.SetActive(true); }
                float alpha = (Mathf.Sin(Time.time * 1f) + 1f) / 2f;
                SwipeHintText.alpha = alpha;  // only this
            }
        }
        else 
        {
            gameover();
        }
        setmazecountdots();
        sessioncounttext.text = ""+sessioncount;
        highscoretext.text = "" + PlayerPrefs.GetInt("HighScore");
    }
    void setcamera() {
        int mazesize = mazegen.Getmazesize();
        float center = (mazesize - 1) / 2f;

        float aspectRatio = (float)Screen.width / Screen.height;
      
       

        if (aspectRatio <= 0.5)
        {
            transform.position = new Vector3(center, center + 1f, transform.position.z);
            float uiOffset = 0.5f;
            float sizeByWidth = (mazesize / 2f + uiOffset) / aspectRatio;
            float sizeByHeight = mazesize / 2f + uiOffset;
            camera.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);

            SwipeHintTexttransform.anchoredPosition = new Vector3(0, 370, 0);

        }
        else if (aspectRatio > 0.5)
        {
            transform.position = new Vector3(center  , center + 2.5f, transform.position.z);
            float uiOffset = 2f;
            float HightuiOffset = 8f;
            float sizeByWidth = (mazesize / 2f + uiOffset) / aspectRatio;
            float sizeByHeight = mazesize / 2f + HightuiOffset;
            camera.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);

            SwipeHintTexttransform.anchoredPosition = new Vector3(0, 170, 0);
        }

    }

    void chakemazcompaleted() 
    {
        if (contaroller.getisend() )
        {
            if (!ismazecompaleted && !issessioncompaleted)
            {
                if (mazecount < 3 )
                {
                    ismazecompaleted = true;
                    starttimer = false;
                    StartCoroutine(setnewmaze());
                }
                else 
                {
                    issessioncompaleted = true;
                    starttimer = false;
                    StartCoroutine(setnewsession());
                    sethighscore(sessioncount);

                }
            }
        }
    }

    IEnumerator setnewmaze() 
    {

        Debug.Log("setnewmaze START");
        yield return new WaitForSeconds(0.2f);
        contaroller.setisend();
        clearremainingdots();
        mazegen.settrygenratemaze();
        yield return new WaitForSeconds(0.1f); // wait fo
        setthatimedot();
        mazecount = mazecount +1;
        if(blockpanel.gameObject.activeSelf) blockpanel.gameObject.SetActive(false);
        ismazecompaleted = false;
        starttimer = true;
    }

    IEnumerator setnewsession() 
    {

        contaroller.setisend();
        yield return new WaitForSeconds(0.4f);
        blockpanel.gameObject.SetActive(true);
        sessioncount = sessioncount + 1;
        mazecount = 0;
        int randomsize = Random.Range(15, 20);
        mazegen.Setmazesize(randomsize);
        setcamera();
        remaingtimedot = 4;
        giventime = remaningtime+1;
        issessioncompaleted = false;
        starttimer = true;

    }

    void chaketimer()
    {
        
        if (starttimer) 
        {
            if (giventime > 1)
            {
                giventime = giventime - Time.deltaTime;
            }
            else 
            {
                starttimer = false;
                timeisover = true;
            }

            timertext.text  =""+(int)giventime ;
        
        }
    } 

    void gameover() 
    {
        if (timeisover) 
        {
            timertext.text = "" + (int)giventime;
            gameoverhighscoretext.text = ""+ PlayerPrefs.GetInt("HighScore");
            gameoverscoretext.text = "" + sessioncount;
            contaroller.setgameovertrue();
            gameoverpanel.SetActive(true);
        }
    }

    void sethighscore(int input)
    {
        if (highscore < input)
        { 
            
            PlayerPrefs.SetInt("HighScore", input);
            PlayerPrefs.Save();
            highscore = input;
            Debug.Log(highscore);
        }
    }

    
    void setthatimedot() 
    {      
        int timedotcount = 0;
        if (remaingtimedot > 2)
        {
            timedotcount = Random.Range(1, 3);
        }
        else
        {
            timedotcount = 1; 
        }

        for (int i = 0; i < timedotcount; i++)
        {
            Vector2 timedotpoint  =  mazegen.getdeadend();
            if (timedotpoint == Vector2.negativeInfinity) break;
            GameObject temptimedot  = Instantiate(timedot, timedotpoint, Quaternion.identity);
            temptimedot.GetComponent<Timedotsetup>().SetupSessionmode(this);
            activedots.Add(temptimedot);
            remaingtimedot -= 1;
        }

    }
    void clearremainingdots()
    {
        foreach (GameObject dot in activedots)
        {
            Destroy(dot);
        }
        activedots.Clear();
    }


    void setmazecountdots() 
    {
        if (mazecount > 0) 
        {
            UnityEngine.UI.Image mazedotimage = mazedots[mazecount-1];
            Color c = mazedotimage.color;
            c.a = 1f;
            mazedotimage.color = c;
        }
        if(mazecount == 0) 
        {
            foreach (var item in mazedots)
            {
                UnityEngine.UI.Image mazedotimage = item;
                Color c = mazedotimage.color;
                c.a = 0.3f;
                mazedotimage.color = c;
            }
        }
    }


    public void gotomainmenu() 
    {
      
        SceneManager.LoadScene("Mainsscreen");
    }



    public void openpausemenu() 
    {
        gamepausecurentscoretext.text = "" +sessioncount;
        gamepausehighestscoretext.text = "" + PlayerPrefs.GetInt("HighScore");
        gamepaustimetext.text = "" + (int)giventime;
        contaroller.setgameovertrue();
        pausemenupanel.SetActive(true);
       starttimer = false;
    }

    public void closepausemenu() 
    {
        contaroller.setgameoverfalse();
        pausemenupanel.SetActive(false);
        starttimer = true;
    }

    public void retry()
    {
        if (sessioncount > 5)
        {
            AdManager.Instance.ShowInterstitial(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    public void continuebywatchingad()
    {
        AdManager.Instance.ShowRewarded(() =>
        {
            gameoverpanel.SetActive(false);
            timeisover = false;
            giventime = 51f;
            starttimer = true;
            contaroller.setgameoverfalse();
        });

    }

    public void closehowtoplay()
    {
        if (PlayerPrefs.GetInt("RushFirstTime", 0) == 0)
        {
            PlayerPrefs.SetInt("RushFirstTime", 1);
            PlayerPrefs.Save();
        }
        howtoplaypanel.SetActive(false);
        starttimer = true;
    }

    public void openhowtoplay()
    {
        howtoplaypanel.SetActive(true);
        starttimer = false;
    }



    public void addtime(float input) 
    {
        giventime = giventime + input;
    }
    


}
