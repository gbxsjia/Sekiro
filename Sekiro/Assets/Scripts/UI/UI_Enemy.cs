using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Enemy : MonoBehaviour
{
    public GameObject AlertParent;
    public Image AlertBar;

    private Character_Base character;
    private AIController controller;

    [SerializeField]
    private SpriteRenderer ExecuteIcon;

    private void Awake()
    {
        character = GetComponentInParent<Character_Base>();
        controller = GetComponentInParent<AIController>();
    }
    private void Update()
    {
        if (AlertBar != null)
        {
            float percent = controller.GetAlertPercent();
            if (percent > 0)
            {
                AlertParent.SetActive(true);
                AlertBar.fillAmount = percent;
            }
            else
            {
                AlertParent.SetActive(false);
            }
        }
        ExecuteIcon.enabled = controller.canBeExecute;
    }
}
