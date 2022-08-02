using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PanelManager.Instance.Init();
        PanelManager.Instance.Open<LoginPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
