using Godot;
using System;

public partial class cardscript : Sprite2D
{
	[Export]
	private Texture2D[] cardTexture = new Texture2D[2]; //index 0 is the back, index 1 is the front 

    [Export]
	public int cardNum = 0; //the card's "id number"

	private cardspritedeckscript cardspritedeckscript;
    
	private bool isFacingUp = false;

	private Vector2 defaultScale;

    private bool flipTime = false;

    private int phasetracker = 0; //used for tracking phases in animations

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		defaultScale = new Vector2(Scale.X, Scale.Y);

		cardspritedeckscript = GetNode<cardspritedeckscript>(@"/root/logicnode/cardspriteDeck");


        cardTexture[0] = (Texture2D)GD.Load("res://sprites/cardback.png");

		this.Texture = cardTexture[0]; //shows cardback on existence
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isFacingUp == true)
		{
            cardTexture[1] = cardspritedeckscript.cardsprites[cardNum]; //the card's sprite will change depending on the card's ID number. 

			this.Texture = cardTexture[1];
        }
		else 
		{
            this.Texture = cardTexture[0];
        }





        

        if (flipTime == true)
        {
            float flipSpeed = 4f;

            //reduce x scale, when it reaches 0, switch texture and increase x once again

            if (Scale.X > 0 && phasetracker == 0) //"flip" the card halfway
            {
                
                Scale = Scale.Lerp(new Vector2(-1, Scale.Y), flipSpeed * (float)this.GetProcessDeltaTime()); //scales x to 0.
                                                                                                            //reminder : lerp uses "=" to change scale, NOT += or -=
                                                                                                            //getprocessdeltatime is a good way to get (float)delta when you are unable to get it due to not being in process.
                                                                                                            //set goal to -1 because setting it to 0 makes it not actually reach 0 due to interpolation wierdness.
                GD.Print("flipping..." + Scale.X + " " + phasetracker);
            }

            if (Scale.X <= 0 && phasetracker == 0)
            {
                isFacingUp = !isFacingUp; //toggles isfacingup once scalex = 0

                phasetracker += 1;

                GD.Print("flipped." + " " + phasetracker);
            }

            if (Scale.X < defaultScale.X && phasetracker == 1) //fully flip the card over
            {
                Scale = Scale.Lerp(new Vector2(defaultScale.X, Scale.Y), flipSpeed * (float)this.GetProcessDeltaTime());
            }

            if (Scale.X >= defaultScale.X && phasetracker == 1)
            {
                flipTime = false; //end the animation once its done flipping
                phasetracker = 0; //resets phasetracker
            }
        }
        
        

    }


    //###################################################################### SIGNAL FUNCTIONS ###########################################################################
    //https://forum.godotengine.org/t/how-would-i-tell-whether-a-mouse-click-input-is-inside-an-area-2d/9852/2

	public void ifMouseHover()
	{

	}

	public void ifMouseNotHover()
	{

	}

    public void cardFlip()
	{
        flipTime = true;


    }

    //###################################################################### SIGNAL FUNCTIONS ###########################################################################
}
