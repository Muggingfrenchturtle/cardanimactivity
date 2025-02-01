using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class cardscript : MonoBehaviour
{
    public Sprite[] sprites;

    public bool isFacingUp = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isFacingUp == false)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = sprites[0];
        }
        else if (isFacingUp == true)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = sprites[1];
        }
        
    }

    private void OnMouseEnter()
    {
        var speed = 0.1f;
        transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), speed);
    }

    private void OnMouseExit()
    {
        var speed = 0.1f;
        transform.DOScale(new Vector3(1f, 1f, 1f), speed);
    }


    private void OnMouseDown()
    {
        var flipspeed = 0.5f;

        //transform.GetChild(0).DOScaleX(0, flipspeed).OnComplete(() => isFacingUp = !isFacingUp).OnComplete(() => transform.GetChild(0).DOScaleX(1f, flipspeed)); //the sprite change dosent work

        //https://github.com/Demigiant/dotween/issues/348
        //https://dotween.demigiant.com/documentation.php?api=DOVirtual.Delayed
        DOVirtual.DelayedCall(flipspeed, flip); //putting this in the oncomplete chain below dosent work for some reason. so i just have it change texture with a delay of flipspeed, making it change texture on cue.
        transform.GetChild(0).DOShakePosition(1, 0.2f); //yep. this dosent work in the oncomplete either
        transform.GetChild(0).DOScaleX(0, flipspeed).SetEase(Ease.InBack).OnComplete(() => DOVirtual.DelayedCall(0.1f,flip) /*//literally dosent work*/).OnComplete(() => transform.GetChild(0).DOScaleX(1f, flipspeed).SetEase(Ease.OutBack));

        



    }

    private void flip()
    {
        Debug.Log("help");
        isFacingUp = !isFacingUp;
    }
}
