using System.Collections.Generic;
using UnityEngine;

public class ListOfNotesUI : MonoBehaviour
{
    private LinkedList<GameObject> listOfNotes = new LinkedList<GameObject>();
    [SerializeField]
    private GameObject uiNotePrefab;
    private GameObject plusButton, currentObject;
    public delegate LinkedList<GameObject> GetListOfNotesDelegate();
    public static GetListOfNotesDelegate getListOfNotesDeleg;
    public delegate void RemoveNoteDelegate(GameObject noteToBeRemoved);
    public static RemoveNoteDelegate removeNoteDeleg;

   

    void Start()
    {
        plusButton = gameObject.transform.GetChild(0).gameObject;
        currentObject = plusButton;
        FillListOfNotes();
        getListOfNotesDeleg = GetListOfNotes;
        removeNoteDeleg = RemoveNote;
 
    }

    private void FillListOfNotes()
    {
        List<Note> notesData = Notes.getNotesListDeleg();
       
        if(notesData.Count > 0 )
        {
            foreach (Note note in notesData)
            { 
               
                GameObject newNoteUI = Instantiate(uiNotePrefab, this.gameObject.transform, false);

                RectTransform newNoteUIRectTransform = newNoteUI.GetComponent<RectTransform>();
                RectTransform plusButtonRectTransform = plusButton.GetComponent<RectTransform>(); 

                newNoteUIRectTransform.anchoredPosition = plusButtonRectTransform.anchoredPosition;
                newNoteUIRectTransform.sizeDelta = plusButtonRectTransform.sizeDelta;
                newNoteUIRectTransform.pivot = plusButtonRectTransform.pivot;
                newNoteUIRectTransform.localScale = plusButtonRectTransform.localScale; 

                AssociatedDetails associatedDetails = newNoteUI.GetComponent<AssociatedDetails>();
                associatedDetails.SetNoteData(note);
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
        plusButton.SetActive(false);
        GameObject newNoteUI = Instantiate(uiNotePrefab, this.gameObject.transform,false);        
        AssociatedDetails associatedDetails = newNoteUI.GetComponent<AssociatedDetails>();
        associatedDetails.SetNoteData(Notes.getTempNoteDeleg());
        
        RectTransform newNoteUIRectTransform = newNoteUI.GetComponent<RectTransform>();
        RectTransform plusButtonRectTransform = plusButton.GetComponent<RectTransform>();
        newNoteUIRectTransform.anchoredPosition = plusButtonRectTransform.anchoredPosition;
        newNoteUIRectTransform.sizeDelta = plusButtonRectTransform.sizeDelta;
        newNoteUIRectTransform.pivot = plusButtonRectTransform.pivot; 
        newNoteUIRectTransform.localScale = plusButtonRectTransform.localScale;

        newNoteUI.SetActive(true);
        listOfNotes.AddBefore(listOfNotes.Last, newNoteUI);
        currentObject = newNoteUI;
    }

    public void RemoveNote(GameObject noteToBeRemoved)
    {
        noteToBeRemoved.SetActive(false);
        LinkedListNode<GameObject> nodeToBeRemoved = listOfNotes.Find(noteToBeRemoved);
        LinkedListNode<GameObject> previousNode = listOfNotes.Find(noteToBeRemoved).Previous;
        LinkedListNode<GameObject> nextNode = listOfNotes.Find(noteToBeRemoved).Next;

        if(previousNode!= null)
        {
            previousNode.Value.SetActive(true);
            currentObject = previousNode.Value;
            EditNote.setCurrentNoteUIDeleg(currentObject);
        }
        else
        {  if(nextNode!=null)
           {
                nextNode.Value.SetActive(true);
                currentObject = nextNode.Value;
                EditNote.setCurrentNoteUIDeleg(currentObject);
            }
            else
            {
                plusButton.SetActive(true);
                currentObject = plusButton;              
            }

        }

            listOfNotes.Remove(nodeToBeRemoved);
            Destroy(noteToBeRemoved.gameObject);
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
