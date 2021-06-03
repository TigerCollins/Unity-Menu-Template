using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindToggleGroup : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;

    
    // Start is called before the first frame update
    void Start()
    {
        toggle.group = transform.parent.GetComponent<ToggleGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
