using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritMask_Collect : MonoBehaviour
{
    bool activeMask = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activeMask && other.tag == "Player")
        {
            activeMask = false;

            other.GetComponent<SpiritMask>().CollectedSpiritMask();

            Destroy(this.gameObject); // Temp?
        }
    }
}
