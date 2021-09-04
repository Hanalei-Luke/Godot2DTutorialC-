using Godot;
using System;

public class HUD : CanvasLayer
{

    public delegate void StartGameDelegate();

    public event StartGameDelegate GameStarted;

    public void onButtonPressed()
    {
        GameStarted?.Invoke();
        GetNode<Button>("Button").Hide();
    }

    public void UpdateScore(int score)
    {
         GetNode<Label>("ScoreLabel").Text =  score.ToString();
    }

    //This is kinda extra, just saves having to do extra calls for things
    public void ShowMessage(string message, bool dontHide = false, int timeToShow = 1)
    {
        GetNode<Label>("MessageLabel").Text = message;
        GetNode<Label>("MessageLabel").Show();
        if(timeToShow != 1)
        {
            GetNode<Timer>("MessageTimer").WaitTime = timeToShow;
        }
        if(!dontHide)
        {
            GetNode<Timer>("MessageTimer").Start();
        }
    }

    void onMessageTimerTimeout()
    {
        GetNode<Label>("MessageLabel").Hide();
    }

    public void ShowGameOver()
    {
        ShowMessage("game over!");
    }
}
