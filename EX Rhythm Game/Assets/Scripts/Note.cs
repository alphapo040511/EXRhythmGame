using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float Speed;
    public float ArrivalTime;
    public int NoteType;
    public GameObject NoteLoad;
    

    public Material LeftColor, RightColor;


    private bool used;
    private float PlayTime;

    private void Start()
    {
        if (NoteType == 0)
        {
            transform.position = new Vector3(-2,0,Speed * ArrivalTime);
            GetComponent<MeshRenderer>().material = LeftColor;
        }
        else if (NoteType == 1)
        {
            transform.position = new Vector3(2, 0, Speed * ArrivalTime);
            GetComponent<MeshRenderer>().material = RightColor;
        }
    }

    private void FixedUpdate()
    {

        if (NoteLoad.GetComponent<NewNoteLoad>().NoteMove)
        {
            PlayTime += Time.fixedDeltaTime;
            transform.Translate(-Vector3.forward * Speed * Time.fixedDeltaTime);

            if (Mathf.Abs(PlayTime - ArrivalTime) <= 0.1f)
            {
                if (NoteType == 0 && Input.GetKeyDown(KeyCode.F) || NoteType == 1 && Input.GetKeyDown(KeyCode.J))
                {
                    if (!used)
                    {
                        used = true;
                        CheckTiming();
                        Destroy(gameObject);
                        NoteLoad.GetComponent<NewNoteLoad>().NoteCount--;
                    }
                }
            }

            if (PlayTime - ArrivalTime > 0.1f && !used)
            {
                used = true;
                Destroy(gameObject);
                NoteLoad.GetComponent<NewNoteLoad>().NoteCount--;
                NoteLoad.GetComponent<NewNoteLoad>().AddPoint(5);
            }
        }
    }

    private void CheckTiming()
    {
        if (Mathf.Abs(PlayTime - ArrivalTime) <= 0.03f)
        {
            NoteLoad.GetComponent<NewNoteLoad>().AddPoint(0);
        }
        else if(Mathf.Abs(PlayTime - ArrivalTime) <= 0.04f)
        {
            NoteLoad.GetComponent<NewNoteLoad>().AddPoint(1);
        }
        else if (Mathf.Abs(PlayTime - ArrivalTime) <= 0.06f)
        {
            NoteLoad.GetComponent<NewNoteLoad>().AddPoint(2);
        }
        else if (Mathf.Abs(PlayTime - ArrivalTime) <= 0.07f)
        {
            NoteLoad.GetComponent<NewNoteLoad>().AddPoint(3);
        }
        else
        {
            NoteLoad.GetComponent<NewNoteLoad>().AddPoint(4);
        }

    }
}
