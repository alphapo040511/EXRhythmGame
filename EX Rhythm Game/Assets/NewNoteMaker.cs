using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NewNoteMaker : MonoBehaviour
{

    public AudioClip AudioFile;
    public AudioSource audioSource;
    public int BPM;
    public GameObject NoteGroup,Note;
    public Scrollbar Scrollbar;

    public float NoteCount,NowNoteCount;

    [SerializeField]
    private bool Play;
    private int AudioLength;

    private float SPB;
    private float ScrollValue;
    private NewNoteData NowNote;
    private string path = Path.Combine(Application.dataPath, "NewSaveData.Json");
    private double StartTime;

    // Start is called before the first frame update
    void Start()
    {
        NewSaveNoteData saveNoteData = new NewSaveNoteData();
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            saveNoteData = JsonUtility.FromJson<NewSaveNoteData>(jsonData);

            for (int i = 0; i < saveNoteData.NoteType.Count; i++)
            {
                GameObject temp = Instantiate(Note);
                temp.transform.position = saveNoteData.ActivateNotePos[i];
                temp.transform.SetParent(NoteGroup.transform);
                temp.GetComponent<NewNoteData>().ArrivalTime = saveNoteData.ArrivalTime[i];
                temp.GetComponent<NewNoteData>().NoteType = saveNoteData.NoteType[i];
            }
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

        Scrollbar.size = 1f / NoteCount * 20;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject temp = Instantiate(Note);
            temp.transform.position = new Vector3(-2.5f, 0, Camera.main.transform.position.z);
            temp.transform.SetParent(NoteGroup.transform);
            temp.GetComponent<NewNoteData>().ArrivalTime = Scrollbar.value * AudioFile.length;
            temp.GetComponent<NewNoteData>().NoteType = 0;
            NowNoteCount++;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject temp = Instantiate(Note);
            temp.transform.position = new Vector3(2.5f, 0, Camera.main.transform.position.z);
            temp.transform.SetParent(NoteGroup.transform);
            temp.GetComponent<NewNoteData>().ArrivalTime = Scrollbar.value * AudioFile.length;
            temp.GetComponent<NewNoteData>().NoteType = 1;
            NowNoteCount++;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Play && Scrollbar.value >= 0 && Scrollbar.value < 1 && StartTime <= AudioSettings.dspTime)
        {
            ScrollValue += 1 / (SPB / Time.fixedDeltaTime) / NoteCount;
            Scrollbar.value = ScrollValue;
        }

        Camera.main.transform.position = new Vector3(0, 5, Scrollbar.value * 3 * NoteCount);
    }

    public void PlayButton()
    {
        if (Play)
        {
            Play = false;
            audioSource.Stop();
        }
        else
        {
            Play = true;
            audioSource.time = Scrollbar.value * AudioFile.length;
            ScrollValue = Scrollbar.value;
            StartTime = AudioSettings.dspTime + 1f;
            audioSource.PlayScheduled(StartTime);
        }
    }

    public void SaveButton()
    {
        NewSaveNoteData saveNoteData = new NewSaveNoteData();
        for (int i = 0; i < NowNoteCount; i++)
        {
            NowNote = NoteGroup.transform.GetChild(i).GetComponent<NewNoteData>();
            saveNoteData.ArrivalTime.Add(NowNote.ArrivalTime);
            saveNoteData.NoteType.Add(NowNote.NoteType);
            saveNoteData.ActivateNotePos.Add(NowNote.transform.position);
        }

        string jsonData = JsonUtility.ToJson(saveNoteData);
        File.WriteAllText(path, jsonData);

        Debug.Log("저장완료");
    }

    [System.Serializable]
    public class NewSaveNoteData
    {
        public List<float> ArrivalTime = new List<float>();
        public List<int> NoteType = new List<int>();
        public List<Vector3> ActivateNotePos = new List<Vector3>();
    }
}
