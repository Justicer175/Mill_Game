using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
            bool didHit = Physics.Raycast(mouse, out info, 500.0f);
            if (didHit)
            {
                Debug.Log(info.point);
            }
            else
            {
                Debug.Log("miss");
            }
        }
    }
}
