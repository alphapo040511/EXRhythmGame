using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDelete : MonoBehaviour
{
    public GameObject Maker;
    private RaycastHit hit;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.transform.tag == "Note")
                {
                    Destroy(hit.collider.gameObject);
                    Maker.GetComponent<NewNoteMaker>().NowNoteCount--;
                }
            }
        }
    }
}
