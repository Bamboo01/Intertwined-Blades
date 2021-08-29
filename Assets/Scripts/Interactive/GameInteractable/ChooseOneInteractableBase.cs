using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChooseOneInteractableBase : InteractableBase
{
    static List<ChooseOneInteractableBase> listofThings = new List<ChooseOneInteractableBase>();
    public static UnityAction chooseOneAction;
    private void Start()
    {
        listofThings.Add(this);
    }

    public override void OnLoadCompleteEvent()
    {
        foreach(var a in listofThings)
        {
            a.enabled = false;
        }
        chooseOneAction?.Invoke();
        Destroy(gameObject);
    }
}
