using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HW_CamController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<MonoBehaviour> Cameras;
    public Queue<MonoBehaviour> QueueList;
    public MonoBehaviour previous;
    public MonoBehaviour current;

    private void Awake()
    {
        foreach (MonoBehaviour cam in Cameras)
        {
            QueueList.Enqueue(cam);
        }
        current = QueueList.Dequeue();
    }
    void Start()
    {
        
    }

    private void OnSwitchCam(InputValue inputValue)
    {
        current.enabled = false;
        previous = current; 
        current = QueueList.Dequeue();
        current.enabled = true; 
    }
}
