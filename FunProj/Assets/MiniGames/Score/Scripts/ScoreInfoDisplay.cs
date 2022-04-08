using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class ScoreInfoDisplay : MonoBehaviourPun
{
    public bool ready1, ready2,ready3;
    [SerializeField] Text nameText;
    [SerializeField] Text scoreText;
    PhotonView view;
    [SerializeField] Transform BarTransform;
    [SerializeField] float maxScore;
    [SerializeField] float maxBarScale;
    [SerializeField] float oldScoreBarSpeed;
    [SerializeField] ScoreInfoDisplay[] displays;
    public GameObject Player;

    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject FadeOut, FadeIn, WheelCanvas,WheelButtons;
    [SerializeField] CreateNJoinRooms RoomJoiner;
    [SerializeField] Transform[] OtherBars;

    [SerializeField] Text[] OtherScores;

    ProportionalCamera cam;
    CameraFollow camfollow;
    [SerializeField] Camera secondcam;
    bool once;

    Image ScaleBarImg;

    [SerializeField] GameObject WinnerCanvas;

    [SerializeField] Transform OGCamPos;
    private void Start()
    {
        Time.timeScale = 1;

        ScaleBarImg = BarTransform.GetComponentInChildren<Image>();
        ScaleBarImg.color = new Color(ScaleBarImg.color.r, ScaleBarImg.color.g, ScaleBarImg.color.b, 0);
        scoreText.text = "";
        nameText.text = "";

        camfollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ProportionalCamera>();
        view = GetComponent<PhotonView>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.CompareTag("Player"))
        {
            ScaleBarImg.color = new Color(ScaleBarImg.color.r, ScaleBarImg.color.g, ScaleBarImg.color.b, 1);
            Player = collision.gameObject;

            if (collision.GetComponent<PhotonView>().IsMine)
            {
                view.RPC("SetName", RpcTarget.All, PhotonNetwork.NickName);
                // PlayerPrefs.SetInt("OldScore", 3000);
                //  PlayerPrefs.SetInt("NewScore", Random.Range(4000, 10000));

               


                view.RPC("SetBar", RpcTarget.All, PlayerPrefs.GetInt("OldScore"), oldScoreBarSpeed, false);

            }

        }
    }

    

    [PunRPC]
    void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetNewBar()
    {
        if (Player && Player.GetComponent<PhotonView>().IsMine)
        {
          
            view.RPC("SetBar", RpcTarget.All, PlayerPrefs.GetInt("NewScore") + PlayerPrefs.GetInt("OldScore"), oldScoreBarSpeed * .75f, true);

        }
    }


    [PunRPC]
    void SetBar(int score, float BarSpeed, bool FastSlow)
    {


        if (FastSlow)
        {
            StartCoroutine(RiseBarFastNumerator(score, BarSpeed));

        }
        else
        {
            StartCoroutine(RiseBarNumerator(score, BarSpeed));
        }

    }
    //Normal
    IEnumerator RiseBarNumerator(int score, float BarSpeed)
    {

        if(score > 0)
        {



            float desiredScale = (score * maxBarScale) / maxScore;
            
            desiredScale += 1.322668f;

            while (BarTransform.localScale.y < desiredScale)
        {
            yield return null;
            if (BarTransform.transform.localScale.y + BarSpeed * Time.deltaTime > desiredScale)
            {
                BarTransform.transform.localScale = new Vector3(1, desiredScale, 1);
            }
            else
            {
                BarTransform.transform.localScale = new Vector3(1, BarTransform.transform.localScale.y + BarSpeed * Time.deltaTime, 1);
            }

            scoreText.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(scoreText.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition.x, spawnPosition.position.y + 220);
            if (Player)
            {
                Player.transform.position = new Vector3(spawnPosition.position.x - 0.7570001f, spawnPosition.position.y - 50f, spawnPosition.position.z);
            }


            float scorecalc = (BarTransform.transform.localScale.y * maxScore) / maxBarScale;
            int scoreint = (int)scorecalc;
            if (scoreint > score)
            {
                scoreint = score;
            }

            scoreText.text = scoreint.ToString();

            int primebar = CalculatePrime();
               
            cam.targetY = (OtherBars[primebar].localScale.y * cam.y) / maxBarScale;

        }

        }
      


        ready1 = true;
        bool allReady = false;
        while (!allReady)
        {
            allReady = true;
            for (int i = 0; i < displays.Length; i++)
            {
                if (displays[i].Player != null && displays[i].ready1 == false)
                {
                    allReady = false;
                }
            }
            yield return null;

        }

    }


    //fast duh
    IEnumerator RiseBarFastNumerator(int score, float BarSpeed)
    {
        if (!Player)
        {
            yield break;
        }


        
        float desiredScale = (score * maxBarScale ) / maxScore;
        desiredScale += 1.322668f;

 

        float OGBarSpeed = BarSpeed;



     

        while (BarTransform.localScale.y < desiredScale)
        {
           
            if (BarSpeed > OGBarSpeed * .3f)
            {
                BarSpeed -= BarSpeed * .05f;
            }
            if (BarTransform.localScale.y * 1.2f > desiredScale)
            {
                BarSpeed = OGBarSpeed * .1f;
                if (BarTransform.localScale.y * 1.05f > desiredScale && BarSpeed > 1f)
                {
                    BarSpeed -= .25f;
                }
            }


            yield return null;
            if (BarTransform.transform.localScale.y + BarSpeed * Time.deltaTime > desiredScale)
            {
                BarTransform.transform.localScale = new Vector3(1, desiredScale, 1);
            }
            else
            {
                BarTransform.transform.localScale = new Vector3(1, BarTransform.transform.localScale.y + BarSpeed * Time.deltaTime, 1);
            }

            scoreText.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(scoreText.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition.x, spawnPosition.position.y + 220);

            Player.transform.position = new Vector3(spawnPosition.position.x - 0.7570001f, spawnPosition.position.y - 50f, spawnPosition.position.z);



            float scorecalc = (BarTransform.transform.localScale.y * maxScore) / maxBarScale;
            int scoreint = (int)scorecalc;
            if (scoreint > score)
            {
                scoreint = score;
            }

            scoreText.text = scoreint.ToString();


            int primebar = CalculatePrime();
           
            cam.targetY = (OtherBars[primebar].localScale.y * cam.y) / maxBarScale;

        }

        bool winner = false;
        bool RealWinner = false;

        int primebarr = CalculatePrime();
       

        if (OtherBars[primebarr] == BarTransform)
        {
            winner = true;
            int winnerscore = 0;
            int.TryParse(OtherScores[primebarr].text, out winnerscore);
            if (winnerscore >= 100)
            {
                RealWinner = true;
            }
        }

     


        yield return new WaitForSeconds(1);

        int parameter = 0;

        if (PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] != null)
        {
            parameter = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"];

        }
        ready2 = true;
        bool allReady = false;
        while (!allReady)
        {
            allReady = true;
            for (int i = 0; i < displays.Length; i++)
            {
                if (displays[i].Player != null && displays[i].ready2 == false)
                {
                    allReady = false;
                }
            }
            yield return null;

        }


        yield return new WaitForSeconds(.2f);
      


        PlayerPrefs.SetInt("OldScore", PlayerPrefs.GetInt("NewScore") + PlayerPrefs.GetInt("OldScore"));
        PlayerPrefs.SetInt("NewScore", 0);
        
          if(Player.GetComponent<PhotonView>().IsMine )
        {
            view.RPC("Emote", RpcTarget.All, parameter, winner);
        }
        

        StartCoroutine(ZoomingIn(RealWinner) );




    }


    public void EmoteExternal(bool winner, bool topplayer)
    {
        if(topplayer)
        {
            bool checkwinnerr = GetWinner();
            if(!checkwinnerr)
            {
                return;
            }
        }


        int parameter = 0;

        if (PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] != null)
        {
            parameter = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"];

        }

        view.RPC("Emote", RpcTarget.All, parameter, winner);

    }

    int CalculatePrime()
    {
        if(Player == null)
        {
            return 0;
        }

        int primebar=0;
        int resultA = 0, ResultB = 0;

        int.TryParse(OtherScores[0].text, out ResultB);

        for (int i = 0; i < OtherScores.Length; i++)
        {
            int.TryParse(OtherScores[i].text, out resultA);

            if (resultA > ResultB)
            {
                primebar = i;

                int.TryParse(OtherScores[i].text, out ResultB);
            }
        }
        return primebar;

    }

    private void Update()
    {


        
        if(ready3)
        {
           bool allReady = true;
            for (int i = 0; i < displays.Length; i++)
            {
                if (displays[i].Player != null && displays[i].ready3 == false)
                {
                    allReady = false;
                }
            }
          
            if (allReady && PhotonNetwork.IsMasterClient && !once && Player && Player.GetComponent<PhotonView>().IsMine)
            {
                once = true;



                // RoomJoiner.LoadLeve("Firescape2");
                RoomJoiner.LoadRandomLeve();

            }
        }
      
    }








IEnumerator ZoomingIn(bool realwinner)
    {
        Camera camfov = camfollow.GetComponent<Camera>();
        cam.active = false;

        //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        int primeplayer = CalculatePrime();
       
        camfollow.offset = new Vector3(9.39999962f, 136.100006f, -558.299988f);
        camfollow.target = displays[primeplayer].Player.transform;

        while (camfov.fieldOfView < 70)
        {
            yield return null;
            camfov.fieldOfView += 1;
        }
        yield return new WaitForSeconds(.25f);
        while (camfov.fieldOfView > 45)
        {
            yield return null;
            camfov.fieldOfView -= 3f;
        }

        if(realwinner)
        {
            StartCoroutine(WinnerNumerator());


        }
        else
        {
            yield return new WaitForSeconds(1.8f);
            FadeOut.SetActive(true);
            yield return new WaitForSeconds(1.4f);

            ready3 = true;
        }

      

    }

    IEnumerator WinnerNumerator()
    {
        yield return new WaitForSeconds(.3f);
        WinnerCanvas.SetActive(true);
        yield return new WaitForSeconds(3);
        FadeOut.SetActive(true);
        yield return new WaitForSeconds(2);
        FadeIn.SetActive(false);
        FadeIn.SetActive(true);
        yield return new WaitForSeconds(.1f);
        FadeOut.SetActive(false);
        WinnerCanvas.SetActive(false);

        WheelCanvas.SetActive(true);
        secondcam.gameObject.SetActive(true);
        camfollow.gameObject.SetActive(false);

        ActivateButtons();


    }

    public void ActivateButtons()
    {
        int primebar = CalculatePrime();


        if (OtherBars[primebar] == BarTransform && displays[primebar].Player.GetComponent<PhotonView>().IsMine)
        {

            WheelButtons.SetActive(true);
        }

    }

    public void killWinner()
    {
        bool win = GetWinner();
        if(win)
        {
            Player.GetComponent<PlayerController>().DeathALL();
        }
       
    }

    public bool GetWinnerView()
    {
        int primebar = CalculatePrime();

        if (OtherBars[primebar] == BarTransform && Player.GetComponent<PhotonView>().IsMine)
        {
         
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetWinner()
    {
        int primebar = CalculatePrime();

        if (OtherBars[primebar] == BarTransform)
        {

            return true;
        }
        else
        {
            return false;
        }
    }




    [PunRPC]
        void Emote(int playerNum, bool winner)
        {
            if (!Player)
            {
                return;
            }
      

            PlayerController controller = Player.GetComponent<PlayerController>();

        bool actualwinner = GetWinner();

            if (winner  && actualwinner)
            {
                switch (playerNum)
                {
                    case 0:


                        controller.animator.Play("Greet");
                        controller.face.Expression("happy");


                        break;
                    case 1:
                        controller.animator.Play("HappyEmoteNatasha");
                        controller.face.Expression("happy");

                        break;
                    case 2:
                        controller.animator.Play("SamHappyEmote");
                        controller.face.Expression("happy");
                        break;
                }
            }
            else
            {
                switch (playerNum)
                {
                    case 0:
                    
                        controller.animator.Play("FBoy");
                        controller.face.Expression("sus");


                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                }
            }




        }






    }



