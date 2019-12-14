using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    bool gameEnd = true;
    public bool GameEnd { get { return gameEnd; } set { gameEnd = value; } }
    Text clockText;
    [SerializeField]
    float battleTimer = 180;
    float delta = 0;

    private void Start()
    {
        clockText = GetComponentInChildren<Text>();
        clockText.text = "180";
    }

    void Update()
    {
        if (gameEnd) return;
        delta = Time.deltaTime;
        flowTime(delta);
    }

    //タイマー時間が終わったら終了
    void flowTime(float d)
    {
        if (gameEnd) return;
        battleTimer -= d;
        clockText.text = Mathf.FloorToInt(battleTimer).ToString();
        if (battleTimer < 4)
        {
            gameEnd = true;
            battleTimer = 0;
            clockText.fontSize = 20;
            clockText.text = Mathf.FloorToInt(battleTimer).ToString();
            B_GameManager.instance.StartcountDown(false);
        }
    }
}
