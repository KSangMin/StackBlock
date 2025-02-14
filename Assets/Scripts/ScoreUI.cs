using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : BaseUI
{
    TextMeshProUGUI score;
    TextMeshProUGUI combo;
    TextMeshProUGUI bestScore;
    TextMeshProUGUI bestCombo;

    Button startButton;
    Button exitButton;

    public override void Init()
    {
        score = transform.Find("Score").GetComponent<TextMeshProUGUI>();
        combo = transform.Find("Combo").GetComponent<TextMeshProUGUI>();
        bestScore = transform.Find("BestScore").GetComponent<TextMeshProUGUI>();
        bestCombo = transform.Find("BestCombo").GetComponent<TextMeshProUGUI>();

        startButton = transform.Find("StartButton").GetComponent<Button>();
        exitButton = transform.Find("ExitButton").GetComponent<Button>();

        startButton.onClick.AddListener(OnClickStartButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    protected override UIState GetUIState()
    {
        return UIState.Score;
    }

    public void SetUI(int score, int combo, int bestscore, int bestcombo)
    {
        this.score.text = score.ToString();
        this.combo.text = combo.ToString();
        this.bestScore.text = bestscore.ToString();
        this.bestCombo.text = bestcombo.ToString();
    }

    void OnClickStartButton()
    {
        UIManager.Instance.OnClickStart();
    }

    void OnClickExitButton()
    {
        UIManager.Instance.OnClickExit();
    }
}
