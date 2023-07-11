using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private bool resetingMouse = false;

    void Update()
    {
        if(resetingMouse)
        {
            Cursor.lockState = CursorLockMode.None;


            resetingMouse = false;
        }

        if(Input.GetMouseButtonDown(1))
        {
            ResetMousePositon();
        }
    }

    public void ResetMousePositon()
    {
        Cursor.lockState = CursorLockMode.Locked;

        resetingMouse = true;
    }
}
