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
            card.cardWasClicked += screenshake; //connects each card's signal to the screeshake function.
        }
    }

	public void screenshake()
	{
		//GD.Print("shaking");
        Tween tweener = GetTree().CreateTween();

       
        var shaketime = 0.1;
        var intensity = 15;
        tweener.TweenProperty(this, "position", new Vector2(GD.Randi() % intensity, GD.Randi() % intensity) , shaketime);
        tweener.TweenProperty(this, "position", new Vector2(GD.Randi() % intensity, GD.Randi() % intensity), shaketime);
        tweener.TweenProperty(this, "position", new Vector2(GD.Randi() % intensity, GD.Randi() % intensity), shaketime);
        tweener.TweenProperty(this, "position", new Vector2(GD.Randi() % intensity, GD.Randi() % intensity), shaketime);
        tweener.TweenProperty(this, "position", new Vector2(GD.Randi() % intensity, GD.Randi() % intensity), shaketime);
        tweener.TweenProperty(this, "position",Vector2.Zero, shaketime);
        //yes. this is absolutely horrible looking. but the tweencallbacks dont work as intended and i kinda want rand() to get called abuncha times, and i just wanna end this lol

        

    }

	

}
