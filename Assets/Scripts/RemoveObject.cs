using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.Space) && IsMouseOver())
        {
            Destroy(gameObject);
        }
    }

    private bool IsMouseOver(){
        Vector3 CameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 ObjectPos = transform.position;
        //Debug.Log(CameraPos);
        //Debug.Log(ObjectPos);
        float offL = 0.5F;
        if (Mathf.Abs(CameraPos.x-ObjectPos.x-offL)<0.5 && Mathf.Abs(CameraPos.y-ObjectPos.y+offL)<0.5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
