using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsFuncs : MonoBehaviour
{
    public void DisableSomething(GameObject item){
        item.SetActive(false);
    }

    public void EnableSomething(GameObject item){
        item.SetActive(true);
    }
}
