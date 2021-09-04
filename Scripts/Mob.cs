using Godot;
using System;

public class Mob : RigidBody2D
{

    AnimatedSprite animSprite;

    [Export]
    public float min_speed = 150f;

    [Export]
    public float max_speed = 250f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        animSprite.Play();

        string[] mobTypes = animSprite.Frames.GetAnimationNames();
        Random rand = new Random();
        animSprite.Animation = mobTypes[rand.Next() % mobTypes.Length];
    }

    void onVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }

    void Kill()
    {
        QueueFree();
    }

}
