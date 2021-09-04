using Godot;
using System;

public class Main : Node
{
    [Export]
    public PackedScene mob_scene;

    PathFollow2D mobSpawnLocation; 

    int score;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        HUD _hud = GetNode<HUD>("HUD")as HUD;
        _hud.GameStarted += new HUD.StartGameDelegate(GameStarted);
        GetNode<Area2D>("Player").Hide();
    }

    //Called when the mob timer times out
    void OnMobTimerTimeout()
    {
        //I assume you could have this on a singleton rather than having to create a new one everytime you need one in a script?
        Random rand = new Random();

        //Handles all the mob spawning biz
        mobSpawnLocation.UnitOffset = rand.Next();

        //Creating the mob node from the packed scene
        RigidBody2D mob = mob_scene.Instance()as RigidBody2D;
        Mob mobScript = mob as Mob;
        AddChild(mob);
        mob.Position = mobSpawnLocation.Position;

        //Set the direction
        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
        direction += rand.Next(Mathf.RoundToInt(-Mathf.Pi / 4), Mathf.RoundToInt(Mathf.Pi / 4));
        mob.Rotation = direction;

        //Sets the speed
        Vector2 velocity = new Vector2(rand.Next(Mathf.RoundToInt(mobScript.min_speed) , Mathf.RoundToInt(mobScript.max_speed)), 0);
        mob.LinearVelocity = velocity.Rotated(direction);
    
    }

    public void gameOver()
    {
        GetNode<Timer>("ScoreTimer").Stop();
        GetNode<Timer>("MobTimer").Stop();

        HUD hudScript = GetNode<CanvasLayer>("HUD")as HUD;
        hudScript.ShowMessage("Game Over! \n Final Score: " + score.ToString(),true);

        GetNode<Label>("HUD/ScoreLabel").Text = "Dodge the creeps!";
        GetNode<Button>("HUD/Button").Text = "play again?";
        GetNode<Button>("HUD/Button").Show();
        
        GetNode<AudioStreamPlayer>("Music").Stop();
        GetNode<AudioStreamPlayer>("DeathSound").Play();

    }

    public void OnScoreTimerTimeout()
    {
        score += 1;
        HUD _hud = GetNode<HUD>("HUD")as HUD;
        _hud.UpdateScore(score);
    }

    public void GameStarted()
    {
        score = 0;
        GetNode<Label>("HUD/ScoreLabel").Text = "0";
        
        HUD hudScript = GetNode<CanvasLayer>("HUD")as HUD;
        hudScript.ShowMessage("Get ready...");

        Player playerScript = GetNode<Area2D>("Player")as Player;
        playerScript.SetupPlayer();

        GetTree().CallGroup("mobs","Kill");

        GetNode<Timer>("StartTimer").Start();
    }

    public void OnStartTimerTimeout()
    {
        GetNode<Timer>("ScoreTimer").Start();
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("StartTimer").Stop();
        GetNode<AudioStreamPlayer>("Music").Play();
    }

}
 