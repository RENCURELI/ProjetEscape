using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMessageEnableTrigger : MonoBehaviour
{
    public string messageToListenTo;

    // Start is called before the first frame update
    void Start()
    {
        Messenger.AddListener(messageToListenTo, EnableCollisionTrigger);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnableCollisionTrigger()
    {
        GetComponent<BoxCollider>().enabled = true;
    }
}
