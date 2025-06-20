using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventMiddleMan : MonoBehaviour
{
    private string originalInputFieldText;
    private string currentInputFieldText;

    public void SetOriginalInputFieldText(string originalText)
    {
        originalInputFieldText = originalText;
        Debug.Log("Orignal Input Field Text setat : " + originalInputFieldText);
    }

    public void SetCurrentInputFieldText(string currentText)
    {
        if (currentText != originalInputFieldText)
        {
            currentInputFieldText = currentText;
            Debug.Log("Current Input Field Text setat : " + currentInputFieldText);
        }
    }

    public void EndEditEventTriggered()
    {
        Debug.Log("EndEditEventTriggered!");
        this.gameObject.GetComponent<TMP_InputField>().text = currentInputFieldText;
    }
}
