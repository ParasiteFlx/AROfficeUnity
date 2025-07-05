using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DoneButton : MonoBehaviour
{
    [SerializeField]
    CanvasGroup playUi, notePlacementControls;
    GameObject note3D;
    private ARAnchorManager arAnchorManager;
    public delegate void SetNote3DDelegate(GameObject note3DInstance);
    public static SetNote3DDelegate setNote3DDeleg;

    private void Start()
    {
        arAnchorManager = GameObject.FindGameObjectWithTag("origin").GetComponent<ARAnchorManager>();
        setNote3DDeleg = SetNote3D;
    }

    public void SetNote3D(GameObject noteToBeSet)
    {
        note3D = noteToBeSet;
    }

    public void AddAnchor()
    {
        ARAnchor arAnchor;
        arAnchor = note3D.AddComponent<ARAnchor>();
        StartCoroutine(HostCloudAnchorCoroutine(arAnchor));
        StartCoroutine(Transitions.Instance().CanvasFadeOut(notePlacementControls));
        StartCoroutine(Transitions.Instance().CanvasFadeIn(playUi));
      
    }

    private IEnumerator HostCloudAnchorCoroutine(ARAnchor arAnchorToHost)
    {
        HostCloudAnchorPromise promise = arAnchorManager.HostCloudAnchorAsync(arAnchorToHost, 1);
        Debug.Log("HostCloudAnchorState : " + promise.Result.CloudAnchorState.ToString());
        while (promise.State.Equals(PromiseState.Pending))
        {
            yield return null;
        }

        if(promise.State.Equals(PromiseState.Done) && promise.Result.CloudAnchorId!=null)
        {   
            string cloudAnchorId = promise.Result.CloudAnchorId;
            Debug.Log("cloud anchor id : " + cloudAnchorId);
            Note tempNote = Notes.getTempNoteDeleg();
            tempNote.anchorUniqueId = cloudAnchorId;
            Notes.setTempNoteDeleg(tempNote);
            Notes.addNoteDeleg(tempNote);
            note3D.GetComponent<AssociatedDetails>().SetNoteData(tempNote);
            Debug.Log("AnchorUniqueID generated and set! " + cloudAnchorId);
        }
        else
        {
            Debug.Log("HostCloudAnchorState 2: " + promise.Result.CloudAnchorState.ToString());
        }
    }

}
