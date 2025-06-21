using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

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
    public delegate void RemoveNoteDataDelegate(Note note);
    public static RemoveNoteDataDelegate removeNoteDeleg;
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
       // TestNotes();
       // SaveNotes();
        LoadNotes();
        temporaryNote = new Note();
        setTempNoteDeleg = SetTemporaryNote;
        getTempNoteDeleg = GetTemporaryNote;
    }

    void Start()
    {       
        addNoteDeleg = AddNoteData;
        removeNoteDeleg = RemoveNoteData;
        Debug.Log(filePath);       
        //TestNotes();
        //SaveNotes();
    }

    private void SaveNotes()
    {
         notesListWrapper.notes = notesList;
         string notes = JsonUtility.ToJson(notesListWrapper,true);
         File.WriteAllText(filePath, notes);
            
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

    private void AddNoteData(Note note)
    {      
        notesList.Add(note);
        SaveNotes();
        LoadNotes();
    }

    private void RemoveNoteData(Note note)
    {
        int indexToRemove = -1;
        for (int i = 0; i < notesList.Count; i++)
        {
            if (notesList[i].id == note.id)
            {
                indexToRemove = i;
                break; 
            }
        }

        if (indexToRemove != -1)
        {
            notesList.RemoveAt(indexToRemove);         
        }
   
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

    private void TestNotes()
    {
        notesList.Add(new Note("test", "ceva"));
        notesList.Add(new Note("test2", "ceva2"));
        notesList.Add(new Note("test3", "ceva3"));
    }
}

[System.Serializable]
public class Note
{
    public string id;
    public string title;
    public string content;
    public string anchorUniqueId;

    public Note(string title, string content, string anchorUniqueId)
    {
        this.id = Guid.NewGuid().ToString(); 
        this.title = title;
        this.content = content;
        this.anchorUniqueId = anchorUniqueId;
    }

    public Note(string title, string content)
    {
        this.id = Guid.NewGuid().ToString();
        this.title = title;
        this.content = content;       
    }

    public Note()
    {
        this.id = Guid.NewGuid().ToString();
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