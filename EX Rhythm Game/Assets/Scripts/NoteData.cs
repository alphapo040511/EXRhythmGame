using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteData : MonoBehaviour
{
    public bool Activate;
    public int NoteNum;
    public int NoteType;
    public float ArrivalTime;
    public Material White, Color;

    private void Update()
    {
        if(Activate)
        {
            GetComponent<MeshRenderer>().material = Color;
        }
        else
            GetComponent<MeshRenderer>().material = White;
    }
}
