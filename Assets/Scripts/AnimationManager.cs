using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;

    public Animator _playerAnimator;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SetToDesiredAnimation(string parameterName)
    {
        switch (parameterName)
        {
            case "Happy":
                _playerAnimator.SetTrigger("isExcited");
                SoundManager.instance.playSound("Happy");
                GameManager.instance.CheckExprerimentCompleted();
                break;
            case "Sad":
                _playerAnimator.SetTrigger("isAngry");
                GameManager.instance.CheckExprerimentCompleted();
                SoundManager.instance.playSound("Smell");
                break;
            default:
                Debug.LogWarning($"Animation parameter '{parameterName}' not found!");
                break;
        }
    }
}
