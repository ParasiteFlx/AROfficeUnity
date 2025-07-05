using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEditUI : MonoBehaviour
{
    private LinkedList<GameObject> listOfNotesUI;
    private AssociatedDetails associatedDetails;

    private void Start()
    {
        associatedDetails =this.gameObject.GetComponent<AssociatedDetails>();
    }

    public void TriggerUI()
    {
        listOfNotesUI = ListOfNotesUI.getListOfNotesDeleg();

        Note associatedNote = associatedDetails.GetAssociatedNote();

        Debug.Log(associatedNote.title + " " + associatedNote.id);

        foreach (GameObject node in listOfNotesUI)
        {
            Note nodeAssociatedDetails = node.GetComponent<AssociatedDetails>().GetAssociatedNote();

            Debug.Log(nodeAssociatedDetails.title + " " + nodeAssociatedDetails.id);

            if (nodeAssociatedDetails != null)
            {
                if (associatedNote.id != nodeAssociatedDetails.id)
                {
                    node.SetActive(false);
                }
                else
                {
                    node.SetActive(true);               
                    node.GetComponent<EditNote>().Transition();
                }
            }
            else
            {
                Debug.Log("E nul node!");
            }
        }
     
    }
}
