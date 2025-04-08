using NUnit.Framework.Internal;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenAIController : MonoBehaviour
{
    private OpenAIAPI api;

    // Start is called before the first frame update
    void Start()
    {
        api = new OpenAIAPI("OPENAI_KEY");
        Testfunc();
    }

    private async void Testfunc()
    {
        var chat = api.Chat.CreateConversation();
        chat.Model = "gpt-4o-mini";
        chat.AppendUserInput("Is this an animal? Dog");

        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response); // "Yes"
    }
}