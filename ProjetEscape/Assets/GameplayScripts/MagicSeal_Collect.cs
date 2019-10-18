using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSeal_Collect : MonoBehaviour
{
    public string sealName;
    bool activeSeal;
    // Start is called before the first frame update
    void Start()
    {
        activeSeal = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activeSeal && other.tag == "Player")
        {
            activeSeal = false;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().CollectedSeal(sealName);

            Destroy(this.gameObject); //Temp?
        }
    }
}
