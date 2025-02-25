using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Options : MonoBehaviour
{
    private static Options instance;
    private string filePath;
    private int[] transitionTypes = { 0, 1, 2 }; // 0 = noTransitions; 1 = simplifiedTransitions; 2 = complexTransitions;
    private List<int> transitionTypeList;
    public int currentTransitionType;
    private CurrentOptions currentOptions = new CurrentOptions();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Ensures persistence between scenes
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // Destroy duplicate instances
            Destroy(gameObject);
        }
    }

    public static Options Instance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath + "/Options.json";
        transitionTypeList = new List<int>(transitionTypes);
        currentTransitionType = transitionTypeList[1];
        DefaultOptions(currentOptions);
        LoadOptions();
        Debug.Log(filePath);
    }

    private void DefaultOptions(CurrentOptions currentOptions)
    {
        currentOptions.transitionType = currentTransitionType;
    }


    public void SaveOptions()
    {
        string optionsData = JsonUtility.ToJson(currentOptions);
        System.IO.File.WriteAllText(filePath, optionsData);
    }

    public void LoadOptions()
    {
        if (System.IO.File.Exists(filePath))
        {
            string optionsData = System.IO.File.ReadAllText(filePath);
            currentOptions = JsonUtility.FromJson<CurrentOptions>(optionsData);
        }
    }

    public int GetTransitionType()
    {
        return currentTransitionType;
    }

    public void SetTransitionType(bool right)
    {
        if (right)
        {
            if (currentTransitionType < 2)
            {
                currentTransitionType++;

            }
            else
            {
                currentTransitionType = 0;
            }
        }
        else
        {
            if (currentTransitionType > 0)
            {
                currentTransitionType--;

            }
            else
            {
                currentTransitionType = 2;
            }
        }
    }
}

[System.Serializable]
public class CurrentOptions
{
    public int transitionType;
}
