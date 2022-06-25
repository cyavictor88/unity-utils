using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MenuManager : VisualElement
{


    SOCurrentScene currentScene = Resources.Load<SOCurrentScene>("SO/CurrentSceneSO"); 
    SOListOfScenes listOfScenes = Resources.Load<SOListOfScenes>("SO/ListOfScenesSO"); 


    public new class UxmlFactory : UxmlFactory<MenuManager, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            Debug.Log("init menu");

        }
    }


    public MenuManager()
    {
        // this.RegisterCallback<GeometryChangedEvent>(vicinit);
        this.RegisterCallback<GeometryChangedEvent>(init);

    }

    public DropdownField dfscenes;

    private void init(GeometryChangedEvent evt)
    {
        dfscenes = this.Q<DropdownField>("dfscenes");
        dfscenes.choices =listOfScenes.scenesStrs;

        string curdfstr = dfscenes.text;


        if(SceneManager.GetActiveScene().buildIndex==0)
        {
            currentScene.currentSceneIndex=-1;
           listOfScenes.scenesStrs = new List<string> { "Base Page","LineRenderer with Collider","Custom Mesh Line 3D", "Orb - Mesh Line", "Orb - LR"};

        }

        if (currentScene.currentSceneIndex>-1)
        {
            dfscenes.index=currentScene.currentSceneIndex;
        }






        dfscenes.RegisterValueChangedCallback(changeScene);

    }

    void changeScene(ChangeEvent<string> evt)
    {
        // if (evt.previousValue.Contains("Scene"))
            // return;




        currentScene.currentSceneString = evt.newValue;
        int index = listOfScenes.scenesStrs.IndexOf(evt.newValue);
        currentScene.currentSceneIndex = index;
        Debug.Log( "valchanged:-----------> "+evt.previousValue + " -------------> "+evt.newValue + " "+index );


        if( SceneManager.GetActiveScene().buildIndex!=index )
        {
            // SceneManager.LoadScene(currentScene.currentSceneString);
            SceneManager.LoadScene(index);

            // SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));
        }
        
        
        
        
        
        // dfscenes.label = "Scene"+index.ToString();
        // dfscenes.UnregisterCallback<ChangeEvent<string>>(changeScene);

        // dfscenes.UnregisterValueChangedCallback(changeScene);

        // SceneManager.LoadScene(currentSceneSO.currentSceneString);


        
    }



}