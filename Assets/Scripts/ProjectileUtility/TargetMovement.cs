using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public delegate void PositionChangedHandler();

    public static event PositionChangedHandler OnPositionChanged;

    private Vector3 position;
    
    void Start()
    {
        this.position = this.transform.position;
    }
    
    void Update()
    {
        if (this.transform.position != this.position)
            if (OnPositionChanged != null)
            {
                OnPositionChanged.Invoke();
                this.position = this.transform.position;    
            }
    }
}
