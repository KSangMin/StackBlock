using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : BaseUI
{
    TextMeshProUGUI scoreText;
    TextMeshProUGUI comboText;
    TextMeshProUGUI maxComboText;

    public override void Init()
    {
        scoreText = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        comboText = transform.Find("ComboText").GetComponent<TextMeshProUGUI>();
        maxComboText = transform.Find("MaxComboText").GetComponent<TextMeshProUGUI>();
    }

    protected override UIState GetUIState()
    {
        return UIState.Game;
    }

    public void SetUI(int score, int combo, int max)
    {
        scoreText.text = score.ToString();
        comboText.text = combo.ToString();
        maxComboText.text = max.ToString();
    }
}
