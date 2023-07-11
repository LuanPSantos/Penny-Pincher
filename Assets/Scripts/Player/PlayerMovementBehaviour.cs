using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementBehaviour : MonoBehaviour
{

    [SerializeField]
    private float turnSpeed = 100f;
    [SerializeField]
    private float moveSpeedFacor = 50f;

    [SerializeField]
    private CursorManager cursorManger;

    private Vector2 mouseIniticalPositon;

    void Awake()
    {
        mouseIniticalPositon = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    void Start()
    {
        cursorManger.ResetMousePositon();
    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * turnSpeed * Time.deltaTime);
        }

        var direction = (Vector2) Input.mousePosition - mouseIniticalPositon;

        transform.Translate(Vector2.up * direction.magnitude / moveSpeedFacor * Time.deltaTime);
    }
}
