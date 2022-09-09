using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputMobile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    string buttonName;

    private void Start()
    {
        buttonName = gameObject.name;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Parmak dokundu: " + eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Parmak kaldýrýldý: " + eventData.position);

        switch (buttonName)
        {
            case "BaslamaButon":
                GameManager.instance.PlayButton();
                break;
            case "RestartButton":
                GameManager.instance.RestartButton();
                break;
            case "NextButton":
                GameManager.instance.NextLevel();
                break;
            case "AyarlarButon":
                GameManager.instance.OpenSettings();
                break;
            case "CloseButton":
                GameManager.instance.CloseSettings();
                break;
            default:
                break;
        }
    }


}