using Godot;
using System;

public partial class cardspritedeckscript : Node
{
	public Texture2D[] cardsprites;

	private string spriteFolderPath = "res://sprites/";
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		cardsprites = new Texture2D[] //loads all the card sprites into an array once. having each card manually search for the file tanked performance based on past experiences.
									  //each card will get their sprites from here. based on the card's index number
		{
			(Texture2D)GD.Load(spriteFolderPath + "test1.png"),
            (Texture2D)GD.Load(spriteFolderPath + "test2.png"),
            (Texture2D)GD.Load(spriteFolderPath + "test3.png")
        };
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
