using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    private RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit))
            {
                if (hit.collider.transform.tag == "Note")
                {
                    if (hit.collider.transform.GetComponent<NoteData>().Activate == true)
                        hit.collider.transform.GetComponent<NoteData>().Activate = false;
                    else
                        hit.collider.transform.GetComponent<NoteData>().Activate = true;
                }
            }
        }
    }
}
