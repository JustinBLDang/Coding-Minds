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

    // Start is called before the first frame update
    void Start()
    {
        api = new OpenAIAPI("OpenAI Key");
        TestFunc();
    }

    public async void TestFunc()
    {
        var result = await api.Chat.CreateChatCompletionAsync(new ChatRequest() {
            Model = "gpt-4o-mini", 
            Temperature = 0.1, 
            Messages = new ChatMessage[]
            {
                new ChatMessage(ChatMessageRole.System, "You are a prestigious Veterinarian that has been in the industry for many decades. You will diagnose the animals symptoms and provide solutions if they exist."),
                new ChatMessage(ChatMessageRole.User, "My cat has a runny nose and isn't as active as she use to be.")
            }
        });

        displayText.text = result.Choices[0].ToString();
    }
}
