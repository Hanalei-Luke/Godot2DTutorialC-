using Godot;
using System;
public class Player : Area2D
{
	
	[Export]
	public float speed = 400f;

	Vector2 ScreenSize = Vector2.Zero;

	AnimatedSprite animSprite;

	CollisionShape2D collision;

	Vector2 startPos;

	[Signal]
	public delegate void HitSignal();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		startPos = Position;
		// Hide();
		animSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		collision = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	//Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		//Sets players direction
		Vector2 direction = Vector2.Zero;
		if(Input.IsActionPressed("move_right"))
		{
			direction.x += 1;
		}
		if(Input.IsActionPressed("move_left"))
		{
			direction.x += -1;
		}
		if(Input.IsActionPressed("move_down"))
		{
			direction.y += 1;
		}
		if(Input.IsActionPressed("move_up"))
		{
			direction.y += -1;
		}

		//moves the player within screen size
		Position += direction * speed * delta;
		Vector2 clampedPos = new Vector2(0,0);
		clampedPos.x = Mathf.Clamp(Position.x,0,ScreenSize.x);
		clampedPos.y = Mathf.Clamp(Position.y,0,ScreenSize.y);
		Position = clampedPos;

		if(direction.Length() > 0)
		{
			direction = direction.Normalized();
			animSprite.Play();
		}
		else
		{
			animSprite.Stop();
		}

		if(direction.x != 0)
		{
			animSprite.Animation = "right";
			animSprite.FlipH = direction.x < 0;
			animSprite.FlipV = false;
		}
		else if(direction.y != 0)
		{
			animSprite.Animation = "up";
			animSprite.FlipV = direction.y > 0;
		}

	}
	
	void onPlayerBodyEntered(RigidBody2D body)
	{
		GD.Print("collision detected!");
		Hide();
		collision.SetDeferred("disabled", true);
		EmitSignal(nameof(HitSignal));
	}

	public void SetupPlayer()
	{
		Position = startPos;
		Show();
		collision.SetDeferred("disabled", false);
	}

}
