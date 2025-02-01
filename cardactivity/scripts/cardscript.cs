using Godot;
using System;

public partial class cardscript : Marker2D
{
	[Export]
	private Texture2D[] cardTexture = new Texture2D[2]; //index 0 is the back, index 1 is the front 

    [Export]
	public int cardNum = 0; //the card's "id number"

	private cardspritedeckscript cardspritedeckscript;
    
	private bool isFacingUp = false;

	private Vector2 defaultSpriteScale;
    private Vector2 defaultScale;

    private float hoverscaleMultiplier = 1.1f; //scale will be multiplied by this number during hovering

    //private bool isFlipping = false; //prevents hover scaling code from firing while the card is flipping

    private bool isHovering = false;

    private bool shakeReady = true; //only shake the screen once, cuz its disorienting.

    private float postFlipXVal; //used in card flipping. yes. we need this in process. putting it in cardflip() will only run this once, which isnt gonna work.

    [Signal]
    public delegate void cardWasClickedEventHandler(); //a signal that will emit once the card is clicked, alerting logicscript to start shaking the screen.

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		defaultSpriteScale = new Vector2(GetNode<Sprite2D>("Sprite2D").Scale.X, GetNode<Sprite2D>("Sprite2D").Scale.Y);
        defaultScale = Scale;

		cardspritedeckscript = GetNode<cardspritedeckscript>(@"/root/logicnode/cardspriteDeck");


        cardTexture[0] = (Texture2D)GD.Load("res://sprites/cardback.png");

        GetNode<Sprite2D>("Sprite2D").Texture = cardTexture[0]; //shows cardback on existence
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isFacingUp == true)
		{
            cardTexture[1] = cardspritedeckscript.cardsprites[cardNum]; //the card's sprite will change depending on the card's ID number. 

			GetNode<Sprite2D>("Sprite2D").Texture = cardTexture[1];
        }
		else 
		{
            GetNode<Sprite2D>("Sprite2D").Texture = cardTexture[0];
        }


        
        /*
        if (isHovering == true) ////used in card flipping. yes. we need this in process. putting it in cardflip() will only run this once, which isnt gonna work.
                                //prevents inconsistent x value when removing the mouse from the card while flipping
        {
            postFlipXVal = hoverscaleMultiplier; //if hovering, card X value goal will be based on its hover size
        }
        else
        {
            postFlipXVal = 0; //if the mouse is off, it will be based on its non-hover size
        }
        */




    }

    


    //###################################################################### SIGNAL FUNCTIONS ###########################################################################
    //https://forum.godotengine.org/t/how-would-i-tell-whether-a-mouse-click-input-is-inside-an-area-2d/9852/2

	public void ifMouseHover()
	{
        isHovering = true;
        
        Tween tweenerr = GetTree().CreateTween();

        tweenerr.TweenProperty(this, "scale", new Vector2(defaultScale.X * hoverscaleMultiplier, defaultScale.Y * hoverscaleMultiplier), 0.1f);

        
    }

	public void ifMouseExit()
	{
        isHovering = false;

        Tween tweenerr = GetTree().CreateTween();

        tweenerr.TweenProperty(this, "scale", new Vector2(defaultScale.X, defaultScale.Y), 0.1f);
        
    }

    public void cardFlip(Node viewport, InputEvent inpEvent, int shape_idx)
	{
        //flipTime = true;

        if (inpEvent.IsActionPressed("leftClick")) //using that inputevent parameter from the area2d signal : https://docs.godotengine.org/en/stable/tutorials/inputs/input_examples.html
                                                   //nvm the normal way works.
        {

            Tween tweener = GetTree().CreateTween(); //tweens : https://docs.godotengine.org/en/stable/classes/class_tween.html

            float flipSpeed = 0.5f;

            



            //tweener.TweenCallback(Callable.From(ifMouseHover)); //automatically hover scale during flipping. the card should be hoverscaled anyways

            //tweener.TweenCallback(Callable.From(isFlippingToggle)); //toggles bool that prevents the hover scaling code from running during flipping

            tweener.TweenProperty(GetNode<Sprite2D>("Sprite2D"), "scale:x", 0, flipSpeed).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.In); //"flips" the card halfway by setting scale x to 0 
                                                                                                                                 //only use 1 axis as the property https://docs.godotengine.org/en/stable/classes/class_tween.html#class-tween-method-tween-property

            tweener.TweenCallback(Callable.From(changeflip)); //this only calls changeflip (which changes the sprite texture) after the tweenproperty above is done.

            tweener.TweenProperty(GetNode<Sprite2D>("Sprite2D"), "scale:x", defaultSpriteScale.X, flipSpeed).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out); //finishes flipping. hoverscalemultiplier is used because the mouse will be on the card when it clicks.

            //tweener.TweenCallback(Callable.From(isFlippingToggle)); //toggles bool to allow hover scaling to work again.

            //tweener.TweenCallback(Callable.From(ifMouseExit)); //automatically hover scale down after flipping. cuz i couldnt be bothered to fix the "the card dosent go down if you move your mouse away while the card is flipping" problem. its like those sinks with the buttons that automatically close the valve when you leave it alone.

            if (shakeReady == true)
            {
                EmitSignal(SignalName.cardWasClicked);//send signal to screenshake
                shakeReady = false; //only shake the screen once, cuz its disorienting.
            }
        }
        


    }
    public void changeflip()
    {
        isFacingUp = !isFacingUp;

        cardNum = (int)(GD.Randi() % 3); //randomizes card face each time you flip.
    }
    /*
    public void isFlippingToggle()
    {
        isFlipping = !isFlipping;
    }
    */

    //###################################################################### SIGNAL FUNCTIONS ###########################################################################
}
