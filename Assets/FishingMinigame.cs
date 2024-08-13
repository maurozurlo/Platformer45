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

    public enum GameMode
    {
        playing,
        won,
        lost
    }

    public GameMode gameMode = GameMode.playing;

    public enum PressingState
    {
        idle,
        pressing,
        done
    }

    public PressingState state = PressingState.idle;

    void Start()
    {
        barWidth = GetComponent<RectTransform>().sizeDelta.x;
        indicatorRect = indicator.GetComponent<RectTransform>();
        float indicatorWidth = indicatorRect.sizeDelta.x / 2;
        float validX = barWidth / 2 - indicatorWidth;
        indicatorMinMax = new Vector2(-validX, validX);
        targetRect = target.GetComponent<RectTransform>();
        target.color = startColor;
        SetTarget();
        startSpeed = speed;
    }

    void SetTarget() {
        float targetWidth = targetRect.sizeDelta.x / 2;
        float validX = (barWidth / 2 - targetWidth);
        targetPosition = Random.Range(-validX, validX);
        targetRect.anchoredPosition = new Vector2(targetPosition, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMode != GameMode.playing) return;

        PingPongIndicator();

        if (Input.GetMouseButton(0))
        {
            CheckIndicatorPosition();
        }
        else
        {
            state = PressingState.idle;
            timer = 0;
        }
    }

    void PingPongIndicator()
    {
        float duration = (indicatorMinMax.y - indicatorMinMax.x) / speed;
        lerpTime += (movingForward ? 1 : -1) * Time.deltaTime / duration;

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
        if (state == PressingState.done) return; // If we're done we don't need to execute any more code

        float targetBounds = targetRect.sizeDelta.x;
        if (Mathf.Abs(indicatorRect.anchoredPosition.x - targetRect.anchoredPosition.x) <= targetBounds * .75)
        {
            

            timer += Time.deltaTime;
            target.color = Color.Lerp(startColor, goalColor, timer / holdTime);
            if (state == PressingState.pressing && timer >= holdTime)
            {
                GoToNextLevel();

            }
            if (state == PressingState.pressing || state == PressingState.done) return;
            state = PressingState.pressing;
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
        float newSize = Mathf.Abs(barWidth / maxLevels * level);
        float newSpeed = maxSpeed / maxLevels + startSpeed;
        speed = newSpeed;

        
        if (level == maxLevels)
        {
            WinGame();
            return;
        }
        progress.sizeDelta = new Vector2(newSize, progress.sizeDelta.y);
    }



    void LostGame()
    {
        HideElements();
        progress.sizeDelta = new Vector2(0, progress.sizeDelta.y);
        target.color = Color.red;
        gameMode = GameMode.lost;
        text.text = "You Lost";
    }


    void WinGame() {
        HideElements();
        float newSize = barWidth;
        progress.sizeDelta = new Vector2(newSize, progress.sizeDelta.y);
        gameMode = GameMode.won;
        text.text = "You Won";
    }

    void HideElements()
    {
        targetRect.sizeDelta = Vector2.zero;
        indicatorRect.sizeDelta = Vector2.zero;
    }

    
}
