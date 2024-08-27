using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    float barWidth;
    Vector2 indicatorMinMax;
    public Image target;
    public Image indicator;

    RectTransform indicatorRect;
    RectTransform targetRect;
    [SerializeField]
    private float startSpeed;
    public float speed;
    public float maxSpeed;

    public int level = 0;
    float timer = 0;
    public float holdTime = 1;

    float targetPosition;

    public RectTransform progress;
    public int maxLevels = 3;

    public TMPro.TMP_Text text;

    public Color goalColor = Color.red;
    public Color startColor = Color.black;

    private bool movingForward = true;
    private float lerpTime = 0f;

    #region Player
    PlayerCharacter playerCharacter;
    AnimatorControllerHandler controllerHandler;
    Animator anim;
    #endregion

    public GameObject rod;
    private GameObject rodI;
    public Vector3 rodDisplacement;
    public Vector3 rodRotation;

    #region UI
    GameObject bgGo;
    GameObject levelGo;
    #endregion

    public static FishingMinigame control;

	#region Win
	public List<GameObject> prizes = new List<GameObject>();
    bool hasWon;
    #endregion

    Vector3 outPosition;

	public enum GameMode
    {
        waiting,
        playing,
        won,
        lost
    }

    public GameMode gameMode = GameMode.waiting;

    public enum PressingState
    {
        idle,
        pressing,
        done
    }

    public PressingState state = PressingState.idle;

    public GameObject minigameCam;

    void Awake()
    {
        if (!control)
        {
            control = this;
        }

        foreach (Transform child in transform)
        {
            if (child.name == "Bg")
            {
                bgGo = child.gameObject;
            }
            if (child.name == "Level")
            {
                levelGo = child.gameObject;
            }
        }
    }

    public void StartGame(Vector3 outPos, GameObject playerSpawn, GameObject dummyCamera)
    {
        HideShowUI(true);

        gameMode = GameMode.waiting;
        ResetPressingState();
        progress.sizeDelta = new Vector2(0, progress.sizeDelta.y);
        text.text = "";
        speed = startSpeed;
        hasWon = false;
        level = 0;
        


        minigameCam.GetComponent<Camera>().depth = 99;
        outPosition = outPos;
        barWidth = bgGo.GetComponent<RectTransform>().sizeDelta.x;
        indicatorRect = indicator.GetComponent<RectTransform>();
        targetRect = target.GetComponent<RectTransform>();
        float indicatorWidth = indicatorRect.sizeDelta.x / 2;
        float validX = barWidth / 2 - indicatorWidth;
        indicatorMinMax = new Vector2(-validX, validX);

        target.color = startColor;
        SetTarget();

        startSpeed = speed;

        // Player
        playerCharacter = PlayerCharacter.control;
        playerCharacter.Lock();
        controllerHandler = playerCharacter.GetComponent<AnimatorControllerHandler>();
        controllerHandler.SetAnimatorController(AnimatorControllerHandler.ControllerType.minigame, 0);
        anim = playerCharacter.anim;

        playerCharacter.transform.position = playerSpawn.transform.position;
        playerCharacter.transform.eulerAngles = playerSpawn.transform.eulerAngles;


        // Hand
        GameObject hand = playerCharacter.GetComponent<PlayerPartsHandler>().playerHandL;
        rodI = Instantiate(rod, hand.transform);
        rodI.transform.localPosition = rodDisplacement;
        rodI.transform.localEulerAngles = rodRotation;

        // Camera
        minigameCam.transform.position = dummyCamera.transform.position;
        minigameCam.transform.eulerAngles = dummyCamera.transform.eulerAngles;
        minigameCam.GetComponent<CameraShake>().SetOriginalPosition(dummyCamera.transform.position);
    }

    void OnEnable()
    {
       PlayerCharacter.onTriggerEvent += CheckEvent;
    }


    void OnDisable()
    {
        PlayerCharacter.onTriggerEvent -= CheckEvent;
    }

    void CheckEvent()
    {
        switch (playerCharacter.eventName)
        {
            case "startFishing":
                gameMode = GameMode.playing;
                break;
            case "startEnd":
                StartEnd();               
                break;
            case "endEnd":
                QuitGame();
                break;
            default:
                Debug.Log($"Unrecognized event: ${playerCharacter.eventName}");
                break;
        }
    }

    void StartEnd()
    {
        // TODO: Spawn fish or do something idk
        if (rodI)
        {
            Destroy(rodI);
        }

        // If we won, instantiate fish
        if (hasWon)
        {
            // TODO: Get random prize from list, spawn it in player hand
        }
    }

    void QuitGame()
    {
        HideShowUI(false);        
        PlayerCharacter.control.Unlock();
        playerCharacter.transform.position = outPosition;
        GameObject[] gos = GameObject.FindGameObjectsWithTag("MinigameTrigger");
        foreach (GameObject go in gos)
        {
            if (go.GetComponent<FishingMinigame_Trigger>()) // TODO: make this apply to all minigame triggers
            {
                go.GetComponent<FishingMinigame_Trigger>().StopPlaying();
            }
        }
        minigameCam.GetComponent<Camera>().depth = -5;
        controllerHandler.SetAnimatorController(AnimatorControllerHandler.ControllerType.main, 0);
    }

    void SetTarget()
    {
        float targetWidth = targetRect.sizeDelta.x / 2;
        float validX = (barWidth / 2 - targetWidth);
        targetPosition = Random.Range(-validX, validX);
        targetRect.anchoredPosition = new Vector2(targetPosition, 0);
    }

    void Update()
    {
        if (gameMode != GameMode.playing) return;

        PingPongIndicator();

        if (Input.GetMouseButton(0))
        {
            CheckIndicatorPosition();
            anim.SetTrigger("Tap");
            CameraShake.Shake(.5f, speed / 4000);
        }
        else
        {
            ResetPressingState();
        }
    }

    void PingPongIndicator()
    {
        float duration = (indicatorMinMax.y - indicatorMinMax.x) / speed;
        lerpTime += Time.deltaTime / duration * (movingForward ? 1f : -1f);

        if (lerpTime >= 1f)
        {
            lerpTime = 1f;
            movingForward = false;
        }
        else if (lerpTime <= 0f)
        {
            lerpTime = 0f;
            movingForward = true;
        }

        float newXPosition = Mathf.Lerp(indicatorMinMax.x, indicatorMinMax.y, lerpTime);
        indicatorRect.anchoredPosition = new Vector2(newXPosition, indicatorRect.anchoredPosition.y);
    }

    void CheckIndicatorPosition()
    {
        if (state == PressingState.done) return;

        float targetBounds = targetRect.sizeDelta.x * 0.75f;
        bool isIndicatorInTarget = Mathf.Abs(indicatorRect.anchoredPosition.x - targetRect.anchoredPosition.x) <= targetBounds;

        if (isIndicatorInTarget)
        {
            if (state == PressingState.idle)
            {
                state = PressingState.pressing;
            }

            timer += Time.deltaTime;
            target.color = Color.Lerp(startColor, goalColor, timer / holdTime);

            if (timer >= holdTime)
            {
                GoToNextLevel();
            }
        }
        else
        {
            LostGame();
        }
    }

    void GoToNextLevel()
    {
        state = PressingState.done;
        SetTarget();
        level++;

        if (level >= maxLevels)
        {
            WinGame();
            return;
        }

        float newSize = barWidth * level / maxLevels;
        progress.sizeDelta = new Vector2(newSize, progress.sizeDelta.y);

        speed = Mathf.Lerp(startSpeed, maxSpeed, (float)level / maxLevels);
    }

    void LostGame()
    {
        HideElements();
        progress.sizeDelta = new Vector2(0, progress.sizeDelta.y);
        target.color = Color.red;
        gameMode = GameMode.lost;
        text.text = "You Lost";
        anim.SetBool("Success", false);
        anim.SetTrigger("FinishGame");
        hasWon = false;
    }

    void WinGame()
    {
        HideElements();
        float newSize = barWidth;
        progress.sizeDelta = new Vector2(newSize, progress.sizeDelta.y);
        gameMode = GameMode.won;
        text.text = "You Won";
        anim.SetBool("Success", true);
        anim.SetTrigger("FinishGame");
        hasWon = true;
    }

    void HideElements()
    {
        targetRect.sizeDelta = Vector2.zero;
        indicatorRect.sizeDelta = Vector2.zero;
    }

    void ResetPressingState()
    {
        state = PressingState.idle;
        timer = 0f;
        target.color = startColor;
    }

    void HideShowUI(bool value)
    {
        bgGo.SetActive(value);
        levelGo.SetActive(value);
    }
}
