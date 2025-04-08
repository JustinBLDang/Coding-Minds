using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI_API;
using TMPro;
using OpenAI_API.Chat;
using OpenAI_API.Models;

class ChatGPTScript : MonoBehaviour
{
    OpenAIAPI api;
    [SerializeField] TMP_Text displayText;
    string prompt =
        "You are a kind and helpful Judge for a Model UN speech. You will be given a script that you will give a letter grade with the following criteria: brevity, clarity, and the strength of the message. Provide feedback on each of the criteria if needed." +
        "Speeches should typically delivered in 30-90 seconds, with a clear introduction, main point (country policy), supporting information, and a concluding call to action.";

    // Start is called before the first frame update
    void Start()
    {
        api = new OpenAIAPI("OPEN_AI_KEY");
    }

    public async void GPTGradeScript(string message, float clipLength)
    {
        var result = await api.Chat.CreateChatCompletionAsync(new ChatRequest() {
            Model = "gpt-4o-mini", 
            Temperature = 0.1, 
            Messages = new ChatMessage[]
            {
                new ChatMessage(ChatMessageRole.System, prompt),
                new ChatMessage(ChatMessageRole.User, message + " The length of the speech is " + clipLength + ".")
            }
        });

        displayText.text = result.Choices[0].ToString();
        Debug.Log(result.Choices[0].ToString());
    }

    public async void GPTGradeAudio(string filePath, float clipLength)
    {
        string resultText = await api.Transcriptions.GetTextAsync(filePath);

        GPTGradeScript(resultText, clipLength);
    }
}
