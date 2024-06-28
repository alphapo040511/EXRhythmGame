using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class NoteLoad : MonoBehaviour
{
    public GameObject Note;
    public bool Play;
    public int NoteCount;
    public AudioSource Music, Tab;

    public float Delay;

    public int Perfect, Great, Nice, Good, Bad, Miss;

    public TextMeshProUGUI TypeText;

    private int i;
    private float PlayTime;
    private SaveNoteData saveNoteData = new SaveNoteData();
    private string path = Path.Combine(Application.dataPath, "SaveData.json");

    // Start is called before the first frame update
    void Start()
    {
        string jsonData = File.ReadAllText(path);
        saveNoteData = JsonUtility.FromJson<SaveNoteData>(jsonData);
        Debug.Log(saveNoteData.NoteType.Count);

        //for(int i = 0; i < saveNoteData.NoteType.Count; i ++)
        //{
        //    GameObject temp = Instantiate(Note);
        //    temp.GetComponent<Note>().NoteLoad = gameObject;
        //    temp.GetComponent<Note>().NoteType = saveNoteData.NoteType[i];
        //    temp.GetComponent<Note>().ArrivalTime = saveNoteData.ArrivalTime[i];
        //}
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0)) { ClickStart(); }
        if (Play)
            PlayTime += Time.fixedDeltaTime;

        if (i < saveNoteData.NoteType.Count && NoteCount < 14)
        {
            GameObject temp = Instantiate(Note,new Vector3(0,0,50),Quaternion.identity);
            temp.GetComponent<Note>().NoteLoad = gameObject;
            temp.GetComponent<Note>().NoteType = saveNoteData.NoteType[i];
            temp.GetComponent<Note>().ArrivalTime = saveNoteData.ArrivalTime[i] - PlayTime + Delay;
            NoteCount++;
            i++;
        }
    }

    void ClickStart()
    {
        if (!Play)
        {
            double curDspTime = AudioSettings.dspTime;
            Music.PlayScheduled(curDspTime + 1f);
            Play = true;
        }
    }

    public void AddPoint(int i)
    {
        switch(i)
        {
            case 0:
                Perfect++;
                TypeText.text = "Perfect";
                break;

            case 1:
                Great++;
                TypeText.text = "Great";
                break;

            case 2:
                Nice++;
                TypeText.text = "Nice";
                break;

            case 3:
                Good++;
                TypeText.text = "Good";
                break;

            case 4:
                Bad++;
                TypeText.text = "Bad";
                break;

            case 5:
                Miss++;
                TypeText.text = "Miss";
                break;
        }
    }
}
