using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static System.TimeZoneInfo;

public class Options : MonoBehaviour
{
    private static Options instance;
    private string filePath;
    private static readonly int[] transitionTypes = { 0, 1, 2 }; // 0 = noTransitions; 1 = simplifiedTransitions; 2 = complexTransitions;
    private string[] transitionTexts = { "None", "Simple", "Complex" };
    private static List<int> transitionTypeList = new List<int>(transitionTypes);
    private int currentTransitionType = transitionTypeList[1];
    private int temporaryTransitionType;
    private CurrentOptions currentOptions = new CurrentOptions();
    private CurrentOptions defaultOptions = new CurrentOptions();

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

        filePath = Application.persistentDataPath + "/Options.json";
        DefaultOptions(defaultOptions);
        LoadOptions();
    }

    public static Options Instance()
    {
        return instance;
    }


    private void DefaultOptions(CurrentOptions defaultOptions)
    {
       defaultOptions.transitionType = transitionTypeList[1];
       defaultOptions.transitionTypeName = transitionTexts[1];
    }

    public void ResetDefaultOptions()
    {
        
        currentOptions = new CurrentOptions
        {
            transitionType = defaultOptions.transitionType,
            transitionTypeName = defaultOptions.transitionTypeName
        };
      
        SetCurrentTransitionType(defaultOptions.transitionType);
        SetTemporaryTransitionType();
        foreach(GameObject button in Transitions.Instance().activeMenu)
        {
            string buttonName = null;

            foreach(Transform child in button.transform)
            {
                if (child.childCount > 0)
                {
                    if (child.CompareTag("buttonName"))
                    {
                        buttonName = child.GetChild(0).GetComponent<TextMeshPro>().text;
                    }
                    else
                    {
                        if(buttonName!=null)
                        {
                            if (buttonName.Equals("Transition Type"))
                            {
                                child.GetChild(0).GetComponent<TextMeshPro>().text = currentOptions.transitionTypeName;
                            }                        
                        }
                    }
                }
            }
        }
    }

    public void SaveOptions()
    {
        currentTransitionType = temporaryTransitionType;
        currentOptions.transitionType = currentTransitionType;
        currentOptions.transitionTypeName = transitionTexts[currentTransitionType];
        string optionsData = JsonUtility.ToJson(currentOptions);
        System.IO.File.WriteAllText(filePath, optionsData);
    }

    public void LoadOptions()
    {
        if (System.IO.File.Exists(filePath))
        {
           // Debug.Log(filePath);
            string optionsData = System.IO.File.ReadAllText(filePath);
            if (optionsData != null)
            {
               currentOptions = OptionsDataCheck(optionsData);
               currentTransitionType = currentOptions.transitionType;
            }
            else
            {
                currentOptions = defaultOptions;
            }
        }
    }


    //In case a user changes the settings and inserts some wrong data.
    private CurrentOptions OptionsDataCheck(string optionsData)
    {
        CurrentOptions options = JsonUtility.FromJson<CurrentOptions>(optionsData);
        if (options.transitionType > 2)
        {
            options.transitionType = 0;
        }
        else if (options.transitionType < 0)
        {
            options.transitionType = 2;
        }
        else if (!(options.transitionType is int))
        {
            options.transitionType = 1;
        }

        bool correctName = false;
        foreach( string type in transitionTexts)
        {
            if (options.transitionTypeName.Equals(type))
            {
                correctName = true; 
            }
        }
        if (!correctName)
        {
            options.transitionTypeName = transitionTexts[options.transitionType];
        }
 
        return options;
    }

    public int GetCurrentTransitionType()
    {
        return currentTransitionType;
    }

    public void SetCurrentTransitionType(int transitionType)
    {
        currentTransitionType = transitionType;
    }

    public int GetTemporaryTransitionType()
    {
        return temporaryTransitionType;
    }

    public void SetTemporaryTransitionType()
    {
        temporaryTransitionType = currentTransitionType;
    }

    public void ChangeTransitionType(bool right)
    {
       
        if (right)
        {
     
            if (temporaryTransitionType < 2)
            {
               temporaryTransitionType++;

            }
            else
            {
               temporaryTransitionType = 0;
            }
        }
        else
        {
            
            if (temporaryTransitionType > 0)
            {
                temporaryTransitionType--;

            }
            else
            {
               temporaryTransitionType = 2;
            }
        }
    }

    public int GetTransitionType()
    {
        return currentOptions.transitionType;
    }

    public string GetTransitionTypeName()
    {
        return currentOptions.transitionTypeName;
    }
}

[System.Serializable]
public class CurrentOptions
{
    public int transitionType;
    public string transitionTypeName;   
}

