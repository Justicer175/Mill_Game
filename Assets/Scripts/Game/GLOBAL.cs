using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLOBAL : MonoBehaviour
{
    public static GLOBAL instance;


    [System.NonSerialized] public int randomTest = 12;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
