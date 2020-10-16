using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ProgressBar : MonoBehaviour
{

    public Image ActionProgressBar;

    private Character_Base character;

    private void Awake()
    {
        character = GetComponentInParent<Character_Base>();
    }
    private void Update()
    {
        if (ActionProgressBar != null)
        {
            if(character.currentAction && character.currentAction.useActionProgressBar)
            {
                float percent=character.currentAction.GetProgress();
                print(percent);
                if (percent <= 1)
                {
                    ActionProgressBar.fillAmount = percent;
                }
                else
                {
                    ActionProgressBar.fillAmount = 1;
                }
            }
            else
            {
                ActionProgressBar.fillAmount = 0;
            }
        }
        else
        {
            ActionProgressBar.fillAmount = 0;
        }
    }
}
