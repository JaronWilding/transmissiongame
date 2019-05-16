using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectKeyboard : MonoBehaviour
{
    
    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool _buttonSelected;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0 && _buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            _buttonSelected = true;
        }
    }

    private void OnDisable()
    {
        _buttonSelected = false;
    }
}
