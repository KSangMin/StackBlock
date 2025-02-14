using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIState
{
    Home,
    Game,
    Score
}

public class UIManager : Singleton<UIManager>
{
    UIState curState = UIState.Home;
    HomeUI homeUI;
    GameUI gameUI;
    ScoreUI scoreUI;

    Stack stack;

    private void Awake()
    {
        base.Awake();
        stack = FindObjectOfType<Stack>();
        homeUI = GetComponentInChildren<HomeUI>();
        homeUI?.Init();
        gameUI = GetComponentInChildren<GameUI>();
        gameUI?.Init();
        scoreUI = GetComponentInChildren<ScoreUI>();
        scoreUI?.Init();

        ChangeState(UIState.Home);
    }

    public void ChangeState(UIState state)
    {
        curState = state;
        homeUI?.SetActive(curState);
        gameUI?.SetActive(curState);
        scoreUI?.SetActive(curState);
    }

    public void OnClickStart()
    {
        ChangeState(UIState.Game);
        stack.Restart();
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else

#endif

    }

    public void UpdateScore()
    {
        gameUI.SetUI(stack.Score, stack.Combo, stack.MaxCombo);
    }

    public void SetScreUI()
    {
        scoreUI.SetUI(stack.Score, stack.MaxCombo, stack.BestScore, stack.BestCombo);
        ChangeState(UIState.Score);
    }
}
