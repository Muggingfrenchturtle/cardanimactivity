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

    private int phasetracker = 0; //used for tracking phases in animations

    private float hoverscaleMultiplier = 1.1f; //scale will be multiplied by this number during hovering

    private bool isFlipping = false; //prevents hover scaling code from firing while the card is flipping

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





        
        /*
        if (flipTime == true)
        {
            float flipSpeed = 4f;

            //reduce x scale, when it reaches 0, switch texture and increase x once again

            if (Scale.X > 0 && phasetracker == 0) //"flip" the card halfway
            {
                
                Scale = Scale.Lerp(new Vector2(-1, Scale.Y), flipSpeed * (float)this.GetProcessDeltaTime()); //scales x to 0.
                                                                                                            //reminder : lerp uses "=" to change scale, NOT += or -=
                                                                                                            //getprocessdeltatime is a good way to get (float)delta when you are unable to get it due to not being in process.
                                                                                                            //set goal to -1 because setting it to 0 makes it not actually reach 0 due to interpolation wierdness. but just imagine its set to 0
                //GD.Print("flipping..." + Scale.X + " " + phasetracker);
            }

            if (Scale.X <= 0 && phasetracker == 0)
            {
                isFacingUp = !isFacingUp; //toggles isfacingup once scalex = 0

                phasetracker += 1;

                //GD.Print("flipped." + " " + phasetracker);
            }

            if (Scale.X < defaultScale.X && phasetracker == 1) //fully flip the card over
            {
                Scale = Scale.Lerp(new Vector2(defaultScale.X + 0.00001f, Scale.Y), flipSpeed * (float)this.GetProcessDeltaTime()); //defaultscale plussed 0.0001 for same reasons as the above.
                                                                                                                                    //the lerping never actually gets to 0, so we add a small bit of value so it actually gets to 0.
                                                                                                                                    //we set it to be as small as possible so that the card dosent appear a bit stretched. however, making this number bigger will make it so that you can flip it again sooner.
            }

            if (Scale.X >= defaultScale.X && phasetracker == 1)
            {
                flipTime = false; //end the animation once its done flipping
                phasetracker = 0; //resets phasetracker
            }
        }

        GD.Print("fliptime : " + flipTime + "phasetracker : " + phasetracker);
        */

    }

    


    //###################################################################### SIGNAL FUNCTIONS ###########################################################################
    //https://forum.godotengine.org/t/how-would-i-tell-whether-a-mouse-click-input-is-inside-an-area-2d/9852/2

	public void ifMouseHover()
	{
        if (isFlipping == false)
        {
            Tween tweenerr = GetTree().CreateTween();

            tweenerr.TweenProperty(this, "scale", new Vector2(defaultScale.X * hoverscaleMultiplier, defaultScale.Y * hoverscaleMultiplier), 0.1f);

        }

    }

	public void ifMouseExit()
	{
        if (isFlipping == false)
        {
            Tween tweenerr = GetTree().CreateTween();

            tweenerr.TweenProperty(this, "scale", new Vector2(defaultScale.X, defaultScale.Y), 0.1f);
        }
    }

    public void cardFlip(Node viewport, InputEvent inpEvent, int shape_idx)
	{
        //flipTime = true;

        if (inpEvent.IsActionPressed("leftClick")) //using that inputevent parameter from the area2d signal : https://docs.godotengine.org/en/stable/tutorials/inputs/input_examples.html
                                                   //nvm the normal way works.
        {
            float flipSpeed = 0.5f;

            Tween tweener = GetTree().CreateTween(); //tweens : https://docs.godotengine.org/en/stable/classes/class_tween.html

            tweener.TweenCallback(Callable.From(isFlippingToggle)); //toggles bool that prevents the hover scaling code from running during flipping

            tweener.TweenProperty(this, "scale:x", 0, flipSpeed).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.In); //"flips" the card halfway by setting scale x to 0 
                                                                                                                                 //only use 1 axis as the property https://docs.godotengine.org/en/stable/classes/class_tween.html#class-tween-method-tween-property

            tweener.TweenCallback(Callable.From(changeflip)); //this only calls changeflip (which changes the sprite texture) after the tweenproperty above is done.

            tweener.TweenProperty(this, "scale:x", defaultScale.X * hoverscaleMultiplier, flipSpeed).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out); //finishes flipping. hoverscalemultiplier is used because the mouse will be on the card when it clicks.

            tweener.TweenCallback(Callable.From(isFlippingToggle)); //toggles bool to allow hover scaling to work again.
        }
        


    }
    public void changeflip()
    {
        isFacingUp = !isFacingUp;
    }
    public void isFlippingToggle()
    {
        isFlipping = !isFlipping;
    }

    //###################################################################### SIGNAL FUNCTIONS ###########################################################################
}
