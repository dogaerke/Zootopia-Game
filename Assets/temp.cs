using UnityEngine;

[ExecuteAlways]
public class temp : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        GetComponent<GroundTriggerHandler>().id = gameObject.name;
    }
}


