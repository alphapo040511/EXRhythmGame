using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class NoteMaker : MonoBehaviour
{
    public AudioClip AudioFile;
    public AudioSource audioSource;
    public int BPM;
    public float FixTime;
    public GameObject NoteSet,NoteGroup;
    public Scrollbar Scrollbar;

    [SerializeField]
    private bool Play;
    private int AudioLength;
    private float NoteCount;
    private float SPB;
    private float ScrollValue;
    private GameObject NowNote;
    private NoteData LeftNote, RightNote;
    private string path = Path.Combine(Application.dataPath, "SaveData.Json");

    // Start is called before the first frame update
    void Start()
    {
        SaveNoteData saveNoteData = new SaveNoteData();
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            saveNoteData = JsonUtility.FromJson<SaveNoteData>(jsonData);
        }

        audioSource.clip = AudioFile;
        ScrollValue = 0;
        AudioLength = Mathf.CeilToInt(AudioFile.length);
        SPB = 60f / BPM;
        for (int j = 0; ; j++)
        {
            if (SPB * j >= AudioLength)
            {
                NoteCount = j;
                break;
            }
        }

        for (int i = 0; ; i++)
        {
            GameObject temp = Instantiate(NoteSet);
            temp.transform.position = new Vector3(0,0,i * 3 + FixTime / Time.fixedDeltaTime * (1 / (SPB / Time.fixedDeltaTime) / NoteCount));
            Debug.Log(FixTime / Time.fixedDeltaTime * 0.001f * 3);
            temp.transform.SetParent(NoteGroup.transform);

            temp.transform.GetChild(0).GetComponent<NoteData>().ArrivalTime = SPB * i + FixTime;
            temp.transform.GetChild(0).GetComponent<NoteData>().NoteNum = i * 2;
            if (saveNoteData.ActivateNoteNum.FindIndex(j => j == i * 2) != -1)
                temp.transform.GetChild(0).GetComponent<NoteData>().Activate = true;

            temp.transform.GetChild(1).GetComponent<NoteData>().ArrivalTime = SPB * i + FixTime;
            temp.transform.GetChild(1).GetComponent<NoteData>().NoteNum = i * 2 + 1;
            if (saveNoteData.ActivateNoteNum.FindIndex(j => j == i * 2 + 1) != -1)
                temp.transform.GetChild(1).GetComponent<NoteData>().Activate = true;


            if (SPB * i >= AudioLength)
            {
                break;
            }
        }
        Scrollbar.size = 1f / NoteCount * 20;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Play && Scrollbar.value >= 0 && Scrollbar.value < 1)
        {
            ScrollValue += 1 / (SPB / Time.fixedDeltaTime) / NoteCount;
            Scrollbar.value = ScrollValue;
        }


        Camera.main.transform.position = new Vector3(0, 5, Scrollbar.value * 3 * NoteCount - FixTime / Time.fixedDeltaTime * (1 / (SPB / Time.fixedDeltaTime) / NoteCount));
    }

    public void PlayButton()
    {
        if (Play)
        {
            Play = false;
            audioSource.Pause();
        }
        else
        {
            Play = true;
            audioSource.time = Scrollbar.value * AudioFile.length;
            ScrollValue = Scrollbar.value;
            audioSource.Play();
        }
    }

    public void SaveButton()
    {
        SaveNoteData saveNoteData = new SaveNoteData();
        for (int i = 0; i < NoteCount; i++)
        {
            NowNote = NoteGroup.transform.GetChild(i).gameObject;
            LeftNote = NowNote.transform.GetChild(0).GetComponent<NoteData>();
            RightNote = NowNote.transform.GetChild(1).GetComponent<NoteData>();

            if (LeftNote.Activate)
            {
                saveNoteData.ArrivalTime.Add(LeftNote.ArrivalTime);
                saveNoteData.NoteType.Add(LeftNote.NoteType);
                saveNoteData.ActivateNoteNum.Add(LeftNote.NoteNum);
            }

            if (RightNote.Activate)
            {
                saveNoteData.ArrivalTime.Add(RightNote.ArrivalTime);
                saveNoteData.NoteType.Add(RightNote.NoteType);
                saveNoteData.ActivateNoteNum.Add(RightNote.NoteNum);
            }
        }

        string jsonData = JsonUtility.ToJson(saveNoteData);
        File.WriteAllText(path, jsonData);

        Debug.Log("저장완료");
    }
}

[System.Serializable]
public class SaveNoteData
{
    public List<float> ArrivalTime = new List<float>();
    public List<int> NoteType = new List<int>();
    public List<int> ActivateNoteNum = new List<int>();
}