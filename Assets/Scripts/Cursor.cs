using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    int layerMask;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        layerMask = LayerMask.GetMask("Ground");
    }

    
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if(!Physics.Raycast(ray, out hit, 1000, layerMask))
            spriteRenderer.enabled = false;
        else
        {
            transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            spriteRenderer.enabled = true;
        }
    }
}
