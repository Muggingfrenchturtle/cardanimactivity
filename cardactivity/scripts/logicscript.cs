using Godot;
using System;

public partial class logicscript : Node2D
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        //connect the shake signal from each card in the card group to thr screenshake function

        foreach (cardscript card in GetTree().GetNodesInGroup("card"))
        {
            //card.cardWasClicked += screenshake; //connects each card's signal to the screeshake function.
        }
    }

	public void screenshake()
	{
		GD.Print("shaking");
        Tween tweener = GetTree().CreateTween();

        /*
        tweener.TweenCallback(Callable.From(shakerand));
        tweener.TweenCallback(Callable.From(shakerand));
        tweener.TweenCallback(Callable.From(shakerand));
        tweener.TweenCallback(Callable.From(shakerand));
        tweener.TweenCallback(Callable.From(shakerand));
        tweener.TweenCallback(Callable.From(shakeReset));
        */
        var shaketime = 0.1;
        var intensity = 15;
        tweener.TweenProperty(this, "position:x", GD.Randi() % intensity, shaketime);
        tweener.TweenProperty(this, "position:x", GD.Randi() % intensity, shaketime);
        tweener.TweenProperty(this, "position:x", GD.Randi() % intensity, shaketime);
        tweener.TweenProperty(this, "position:x", GD.Randi() % intensity, shaketime);
        tweener.TweenProperty(this, "position:x", GD.Randi() % intensity, shaketime);
        tweener.TweenProperty(this, "position",Vector2.Zero, shaketime);

        

    }

	public void shakerand()
	{
        GD.Print("shakerand");
        GlobalPosition += new Vector2(GD.Randi() % 20, GD.Randi() % 20);
    }
	public void shakeReset() //back to normal
	{
		GlobalPosition = new Vector2(0, 0);

    }
}
