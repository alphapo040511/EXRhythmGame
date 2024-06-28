using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewNoteData : MonoBehaviour
{
    public int NoteType;
    public float ArrivalTime;
    public Material LeftColor, RightColor;

    private void Update()
    {
        if (NoteType == 0)
        {
            GetComponent<MeshRenderer>().material = LeftColor;
        }
        else if(NoteType == 1)
            GetComponent<MeshRenderer>().material = RightColor;
    }
}
