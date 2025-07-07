using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ListOfNotesUI : MonoBehaviour
{
    [SerializeField]
    ARSession arSession;
    private ARAnchorManager arAnchorManager;
    private ARPlaneManager arPlaneManager;
    private LinkedList<GameObject> listOfNotes = new LinkedList<GameObject>();
    [SerializeField]
    private GameObject uiNotePrefab, note3DPrefab;
    private GameObject plusButton, currentObject;
    public delegate LinkedList<GameObject> GetListOfNotesDelegate();
    public static GetListOfNotesDelegate getListOfNotesDeleg;
    public delegate void RemoveNoteDelegate(GameObject noteToBeRemoved);
    public static RemoveNoteDelegate removeNoteDeleg;

    void Start()
    {
        arAnchorManager = GameObject.FindGameObjectWithTag("origin").GetComponent<ARAnchorManager>();
        arPlaneManager = GameObject.FindGameObjectWithTag("origin").GetComponent<ARPlaneManager>();
        plusButton = gameObject.transform.GetChild(0).gameObject;
        currentObject = plusButton;
        FillListOfNotes();
        getListOfNotesDeleg = GetListOfNotes;
        removeNoteDeleg = RemoveNote;
    }

    private void OnARSessionStateChanged(ARSessionStateChangedEventArgs args)
    {
        Debug.Log($"ListOfNotesUI: ARSession state changed to {args.state}");

        // Verificăm dacă sesiunea AR este în starea de tracking stabil
        if (args.state == ARSessionState.SessionTracking)
        {
            // Dezabonează-te de la eveniment pentru a preveni apeluri multiple
            ARSession.stateChanged -= OnARSessionStateChanged;
            Debug.Log("ListOfNotesUI: ARSession is tracking. Initiating Cloud Anchor resolution.");

            // Acum putem iniția popularea listei de note și rezolvarea ancorelor
            FillListOfNotes();
        }
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

                Debug.Log("Inainte de Coroutine");

                Debug.Log("Cloud anchor id resolve " + note.anchorUniqueId);
                StartCoroutine(ResolveCloudAnchorProcess(note.anchorUniqueId, note));

                Debug.Log("Dupa Coroutine");
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

    private IEnumerator ResolveCloudAnchorProcess(string cloudAnchorIdToResolve, Note note)
    {
        while(ARSession.state != ARSessionState.SessionTracking)
        {
            Debug.Log("No session for resolve!");
            yield return null;
        }

        while(arPlaneManager.trackables.count <= 0)
        {
            Debug.Log("No planes for resolve!");
            yield return null;
        }

        Debug.Log("Cloud anchor id resolve " + cloudAnchorIdToResolve);
        ResolveCloudAnchorPromise promise = arAnchorManager.ResolveCloudAnchorAsync(cloudAnchorIdToResolve);

        while (promise.State.Equals(PromiseState.Pending))
        {
            yield return null;
        }

        Debug.Log($"Resolve Promise State: {promise.State.ToString()}");
        Debug.Log($"Resolve Cloud Anchor State: {promise.Result.CloudAnchorState.ToString()}"); 


        if (promise.State.Equals(PromiseState.Done) && promise.Result.Anchor != null) 
        {
            Debug.Log(promise.Result.CloudAnchorState.ToString());
            ARCloudAnchor arCloudAnchor= promise.Result.Anchor;
            GameObject note3D =  Instantiate(note3DPrefab, arCloudAnchor.transform);
            note3D.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            AssociatedDetails associatedDetails = note3D.GetComponent<AssociatedDetails>();
            associatedDetails.SetNoteData(note);
            Debug.Log("Notita Instantiata cu succes! " + cloudAnchorIdToResolve );
        }
        else
        {
            Debug.Log("Cloud anchor id resolve 2" + cloudAnchorIdToResolve);
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
