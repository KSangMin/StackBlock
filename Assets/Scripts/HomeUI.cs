using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : BaseUI
{
    Button startButton;
    Button exitButton;

    public override void Init()
    {
        startButton = transform.Find("StartButton").GetComponent<Button>();
        exitButton = transform.Find("ExitButton").GetComponent<Button>();

        startButton.onClick.AddListener(OnClickStartButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    protected override UIState GetUIState()
    {
        return UIState.Home;
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
