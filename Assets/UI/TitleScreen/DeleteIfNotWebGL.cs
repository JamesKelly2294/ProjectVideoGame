using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteIfNotWebGL : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_WEBGL
        Destroy(gameObject);
#endif
    }
}
