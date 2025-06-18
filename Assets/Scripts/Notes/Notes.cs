using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Notes : MonoBehaviour
{
    private string filePath;
    private NotesList notesListWrapper;
    private List<Note> notesList;
    private Note temporaryNote;
    [SerializeField]
    private GameObject parentUI;
    public delegate void AddNoteDataDelegate(Note note);
    public static AddNoteDataDelegate addNoteDeleg;
    public delegate List<Note> GetNoteListDelegate();
    public delegate void SetNoteListDelegate(List<Note> updatedNoteList);
    public static SetNoteListDelegate setNoteListDeleg;
    public static GetNoteListDelegate getNotesListDeleg;
    public delegate void SetTempNoteDelegate(Note note);
    public static SetTempNoteDelegate setTempNoteDeleg;
    public delegate Note GetTempNoteDelegate();
    public static GetTempNoteDelegate getTempNoteDeleg;


    private void Awake()
    {
        filePath = Application.persistentDataPath + "/Notes.json";
        notesListWrapper = new NotesList();
        notesList = new List<Note>();
        setNoteListDeleg = SetNotesList;
        getNotesListDeleg = GetNotesList;
        LoadNotes();
        temporaryNote = new Note();
        setTempNoteDeleg = SetTemporaryNote;
        getTempNoteDeleg = GetTemporaryNote;
    }

    void Start()
    {       
        addNoteDeleg = addNoteData;       
        Debug.Log(filePath);       
        //TestNotes();
        //SaveNotes();
    }

    private void SaveNotes()
    {
        
        if(notesList.Count > 0 )
        {
            notesListWrapper.notes = notesList;
            string notes = JsonUtility.ToJson(notesListWrapper,true);
            File.WriteAllText(filePath, notes);
        }     
    }

    private void LoadNotes()
    {
        if(File.Exists(filePath))
        {
            string notes = File.ReadAllText(filePath);
            if (notes != null)
            {
                notesListWrapper = JsonUtility.FromJson<NotesList>(notes);
            }   
        }
        notesList = notesListWrapper.notes;
    }

    private void addNoteData(Note note)
    {
        note.id = notesList.Count + 1;
        notesList.Add(note);
        SaveNotes();
        LoadNotes();
    }

    public List<Note> GetNotesList()
    {
        return notesList;
    }

    public void SetNotesList(List<Note> updatedNotesList)
    {
        Debug.Log("Setat");
        notesList = updatedNotesList;
        SaveNotes();
        LoadNotes();
    }

    public void SetTemporaryNote(Note note)
    {
        temporaryNote = note;
    }

    public Note GetTemporaryNote()
    {
        return temporaryNote;
    }
}

[System.Serializable]
public class Note
{
    public int id;
    public string title;
    public string content;
    public string anchorUniqueId;

    public Note(int id, string title, string content, string anchorUniqueId)
    {
        this.id = id;
        this.title = title;
        this.content = content;
        this.anchorUniqueId = anchorUniqueId;
    }

    public Note( string title, string content, string anchorUniqueId)
    {      
        this.title = title;
        this.content = content;
        this.anchorUniqueId = anchorUniqueId;
    }

    public Note(string title, string content)
    {
        this.title = title;
        this.content = content;       
    }

    public Note()
    {
      
    }
}

[System.Serializable]

public class NotesList
{
    public List<Note> notes;

    public NotesList()
    {
        notes = new List<Note>();
    }
}