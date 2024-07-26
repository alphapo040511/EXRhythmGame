using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGameManager : MonoBehaviour
{
    public SequenceData sequenceData;               //������ ������
    public NoteManager noteManager;                 //��Ʈ �Ŵ���
    public float playbackSpeed = 1.0f;              //��� �ӵ�
    private bool notesGenerated = false;            //��Ʈ ���� �Ϸ� �÷���

    // Start is called before the first frame update
    void Start()
    {
        if (sequenceData == null)
        {
            Debug.LogError("������ ������ ����");
            return;
        }

        sequenceData.LoadFromJson();                    //JSON ������ ������ �ε�

        if(sequenceData.trackNotes == null || sequenceData.trackNotes.Count == 0 )
        {
            InitializeTrackNotes();
        }

        //�Ŵ����� ������ �����͸� �����ͼ� ���� ��Ų��.
        noteManager.audioClip = sequenceData.audioClip;
        noteManager.bpm = sequenceData.bpm;
        noteManager.SetSpeed(playbackSpeed);

        GenerateNotes();
        noteManager.Initialize();
    }

    //Ʈ�� ��Ʈ �ʱ�ȭ
    private void InitializeTrackNotes()
    {
        sequenceData.trackNotes = new List<List<int>>();
        for (int i = 0; i < sequenceData.numberOfTracks; i++)
        {
            sequenceData.trackNotes.Add(new List<int>());
        }
    }

    //��Ʈ ����
    private void GenerateNotes()
    {
        if (notesGenerated) return;                     //��Ʈ�� �̹� ���� �Ǿ��ٸ� �ߺ� ���� ����

        noteManager.notes.Clear();                      //��Ʈ �Ŵ����� �����Ͽ� ��Ʈ �ʱ�ȭ

        for(int trackIndex = 0; trackIndex < sequenceData.trackNotes.Count; trackIndex++)       //��Ʈ Ʈ�� ��
        {
            for (int beatIndex = 0; beatIndex < sequenceData.trackNotes[trackIndex].Count; beatIndex++)     //�ش� Ʈ���� ��Ʈ
            {
                int noteValue = sequenceData.trackNotes[trackIndex][beatIndex];
                if (noteValue != 0)
                {
                    float startTime = beatIndex * 60f / sequenceData.bpm;
                    float duration = noteValue * 60f / sequenceData.bpm;
                    Note note = new Note(trackIndex, startTime, duration);
                    noteManager.notes.Add(note);
                }
            }
        }

        notesGenerated = true;

    }

    //��� �ӵ� ����
    public void SetPlayBackSpeed(float speed)
    {
        playbackSpeed = speed;
        noteManager.SetSpeed(speed);                    //���ǵ带 �޾Ƽ� ��Ʈ �Ŵ����� ����
    }

    //JSON �����Ϳ��� ������ ������ �ε�
    public void LoadSequenceDataFromJson()
    {
        sequenceData.LoadFromJson();

        if (sequenceData.trackNotes == null || sequenceData.trackNotes.Count == 0)
        {
            InitializeTrackNotes();
        }

        //�Ŵ����� ������ �����͸� �����ͼ� ���� ��Ų��.
        noteManager.audioClip = sequenceData.audioClip; 
        noteManager.bpm = sequenceData.bpm;
        noteManager.SetSpeed(playbackSpeed);

        notesGenerated = false;                         //���ο� �����͸� �ε� �����Ƿ� ��Ʈ ����� ���
        GenerateNotes();
        noteManager.Initialize();
    }
}
