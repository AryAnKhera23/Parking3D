using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UserInput : MonoBehaviour
{
    public UnityAction OnMouseUp;
    public UnityAction OnMouseDown;
    public UnityAction OnMouseMove;
    private bool IsMouseDown;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IsMouseDown = true;
            OnMouseDown?.Invoke();
        }
        if(IsMouseDown)
        {
            OnMouseMove?.Invoke();
        }
        if (Input.GetMouseButtonUp(0))
        {
            IsMouseDown= false;
            OnMouseUp?.Invoke();
        }
    }
}
