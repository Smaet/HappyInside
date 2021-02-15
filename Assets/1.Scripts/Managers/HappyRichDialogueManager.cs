using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Doozy.Engine.UI;
using UnityEngine.UI;

public class HappyRichDialogueManager : MonoBehaviour
{

    public UIView uiView;
    public UIView uiView_Result;
    public DialogueDatabase dialogueDatabase;
    public DialogueSystemController dialogueSystemController;
    public DialogueSystemTrigger dialogueSystemTrigger;

    public Image image_Background;


    public void StartDialogue()
    {
        uiView.Show();
        //어느 대화로 할지 추가
        //dialogueSystemTrigger.conversation = "???";

        StartCoroutine(DelayDialogue());
    }

    public void StartConversationEvent(Transform _transform)
    {
        Debug.Log("Start Coversation!!!");
    }

    public void EndConversatiionEvent(Transform _transform)
    {
        Debug.Log("End Coversation!!!");
        ShowResult();
    }

    IEnumerator DelayDialogue()
    {
        yield return new WaitForSeconds(0.5f);

        //DialogueManager.instance.StartConversation("MiniGame", dialogueSystemTrigger.conversationActor);
        DialogueManager.instance.StartConversation("New Conversation 1", dialogueSystemTrigger.conversationActor);
       

        //dialogueSystemTrigger.TryStart(dialogueSystemTrigger.conversationActor);
    }

    public void ShowResult()
    {
        uiView_Result.Show();
    }

    public void CloseResult()
    {
        uiView_Result.Hide();

        EndDialogue();
    }

    public void EndDialogue()
    {
        uiView.Hide();
    }

    public void OnIncrement()
    {
        print("미니게임 한 횟수 : " + dialogueDatabase.GetItem("MiniGameCount"));
    }
}
