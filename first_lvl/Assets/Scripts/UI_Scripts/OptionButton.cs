using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    bool isActive = false;

    public void OpenPanel()
    {
        if(GameManager.instance != null && GameManager.instance.OptionsPanel != null)
        {
            isActive = !isActive;
            GameManager.instance.OptionsPanel.SetActive(isActive);
        }
    }
}
