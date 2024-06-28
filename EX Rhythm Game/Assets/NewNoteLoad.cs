using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using static NewNoteMaker;

public class NewNoteLoad : MonoBehaviour
{
    public GameObject Note;
    public bool Play,NoteMove,Pause;
    public int NoteCount;
    public AudioSource Music, Tab;

    public float Delay;

    public int Perfect, Great, Nice, Good, Bad, Miss;

    public TextMeshProUGUI TypeText;

    private int i;
    private float PlayTime, BeforePlayTime;
    private NewSaveNoteData saveNoteData = new NewSaveNoteData();
    private string path = Path.Combine(Application.dataPath, "NewSaveData.json");
    private double StartTime;
    private double musicTime, BeforeMusicTime;

    // Start is called before the first frame update
    void Start()
    {
        string jsonData = File.ReadAllText(path);
        saveNoteData = JsonUtility.FromJson<NewSaveNoteData>(jsonData);
        Debug.Log(saveNoteData.NoteType.Count);
        PlayTime = -1;
        StartTime = 1;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) { ClickStart(); }
        if (Input.GetMouseButtonDown(1)) { StopGame(); }
    }

    private void FixedUpdate()
    {


        if (Play)
            PlayTime = (float)(AudioSettings.dspTime - StartTime + BeforePlayTime);

        if (PlayTime >= BeforePlayTime && Pause && Play)
        {
            Pause = false;
            NoteMove = true;
        }

        if (i < saveNoteData.NoteType.Count && NoteCount < 14)
        {
            GameObject temp = Instantiate(Note, new Vector3(0, 0, 50), Quaternion.identity);
            temp.GetComponent<Note>().NoteLoad = gameObject;
            temp.GetComponent<Note>().NoteType = saveNoteData.NoteType[i];
            temp.GetComponent<Note>().ArrivalTime = saveNoteData.ArrivalTime[i] - PlayTime - 1 + Delay;
            NoteCount++;
            i++;
        }
    }

    void ClickStart()
    {
        if (!Play)
        {
            Music.time = (float)musicTime;
            StartTime = AudioSettings.dspTime + 1;
            Music.PlayScheduled(StartTime);
            Play = true;
        }
    }

    public void StopGame()
    {
        BeforePlayTime = PlayTime;
        Pause = true;
        NoteMove = false;
        musicTime = AudioSettings.dspTime - StartTime + BeforeMusicTime;
        BeforeMusicTime = musicTime;
        Music.Stop();
        Play = false;
    }

    public void Restart()
    {

    }

    public void AddPoint(int i)
    {
        switch (i)
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


