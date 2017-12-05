using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour {

    List<Sprite> currentAnim;

    bool animating = false;
    SpriteRenderer spriter;

    int maxSpeed = 15;
    int speed;
    int currentFrame = 0;

    private void Start()
    {
        spriter = GetComponent<SpriteRenderer>();
        speed = maxSpeed;
    }

    private void Update()
    {
        if (animating)
        {
            speed--;
            if (speed <= 0)
            {
                currentFrame++;
                if (currentFrame > currentAnim.Count - 1)
                    currentFrame = 0;
                SetFrame(currentFrame);
                speed = maxSpeed;
            }
        }
    }

    public bool isAnimating()
    {
        return animating;
    }

    public void SetSpeed(int speed)
    {
        maxSpeed = speed;
    }

    public void SetAnimation(List<Sprite> anim)
    {
        currentAnim = anim;        
    }

    public List<Sprite> GetAnimation()
    {
        return currentAnim;
    }

    public void StartAnimation(List<Sprite> anim = null)
    {
        if (anim != null)
            currentAnim = anim;
        if (currentAnim == null)
        {
            Debug.LogError("Error: currentAnim is null. Please set an animation first.");
            return;
        }
        SetFrame(0);
        animating = true;
    }

    public void SetFrame(int frame)
    {
        spriter.sprite = currentAnim[frame];
    }

    public void StopAnimation()
    {
        SetFrame(0);
        animating = false;
    }

    public void ContinueAnimation()
    {
        animating = true;
    }

    public void PauseAnimation()
    {
        animating = false;
    }
}
