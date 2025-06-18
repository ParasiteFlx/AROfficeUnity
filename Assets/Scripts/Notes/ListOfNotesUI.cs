using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;

public class ListOfNotesUI : MonoBehaviour
{
    private LinkedList<GameObject> listOfNotes = new LinkedList<GameObject>();
    [SerializeField]
    private GameObject uiNotePrefab;
    private GameObject plusButton, currentObject;
    public delegate LinkedList<GameObject> GetListOfNotesDelegate();
    public static GetListOfNotesDelegate getListOfNotesDeleg;
   

    void Start()
    {
        plusButton = gameObject.transform.GetChild(0).gameObject;
        currentObject = plusButton;
        FillListOfNotes();
        getListOfNotesDeleg = GetListOfNotes;
 
    }

    private void FillListOfNotes()
    {
        List<Note> notesData = Notes.getNotesListDeleg();
       
        if(notesData.Count > 0 )
        {
            foreach (Note note in notesData)
            {
                GameObject newNoteUI = Instantiate(uiNotePrefab);
                newNoteUI.transform.SetParent(this.gameObject.transform);
                AssociatedDetails associatedDetails = newNoteUI.GetComponent<AssociatedDetails>();
                associatedDetails.SetNoteData(note);
                newNoteUI.transform.SetPositionAndRotation(plusButton.transform.position, plusButton.transform.rotation);
                listOfNotes.AddLast(newNoteUI);
            }
        }
        listOfNotes.AddLast(plusButton);

        if (listOfNotes.Count > 1 && listOfNotes.First.Value != plusButton) 
        {
            currentObject.gameObject.SetActive(false);
            currentObject = listOfNotes.First.Value;
            currentObject.gameObject.SetActive(true);
        }


    }

    public LinkedList<GameObject> GetListOfNotes()
    {
        return listOfNotes;
    }

    public void AddNote()
    {
        GameObject newNoteUI = Instantiate(uiNotePrefab);
        newNoteUI.transform.SetParent(this.gameObject.transform);
        AssociatedDetails associatedDetails = newNoteUI.GetComponent<AssociatedDetails>();
        associatedDetails.SetNoteData(Notes.getTempNoteDeleg());
        newNoteUI.transform.SetPositionAndRotation(plusButton.transform.position, plusButton.transform.rotation);
        listOfNotes.AddBefore(listOfNotes.Last, newNoteUI);
        currentObject = newNoteUI;
    }

   

    public void SwitchRight()
    {      
       LinkedListNode <GameObject> previousNode = listOfNotes.Find(currentObject).Previous;
       if(previousNode!= null )
        {
            currentObject.gameObject.SetActive(false);
            currentObject = previousNode.Value;
            currentObject.gameObject.SetActive(true);
        }
    }

    public void SwitchLeft()
    {
        LinkedListNode<GameObject> nextNode = listOfNotes.Find(currentObject).Next;
        if (nextNode != null)
        {
            currentObject.gameObject.SetActive(false);
            currentObject = nextNode.Value;
            currentObject.gameObject.SetActive(true);
        }
    }

}
