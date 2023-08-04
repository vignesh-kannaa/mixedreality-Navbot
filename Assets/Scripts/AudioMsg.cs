using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;

public class AudioMsg : MonoBehaviour
{
    private Animator anim;

    private void Start(){    
        anim = GetComponent<Animator>();
    }
    public async Task PlayAudio(string textMessage){   
        anim.SetFloat("Action", 0.5f); // action: 0.5 -> talking
        await AzureVoice(textMessage);
        anim.SetFloat("Action", 0f); // action: 0 -> idle
    }

    private async Task AzureVoice(string textMessage){
        var config = SpeechConfig.FromSubscription("5876eecac25c41879c6933b36005b459", "eastus");
        config.SpeechSynthesisVoiceName = "en-US-JennyNeural";
        using var synthesizer = new SpeechSynthesizer(config);
        await synthesizer.SpeakTextAsync(textMessage);
    }
}



// public string path = "Audio/";
    // AudioSource source;
    // void Awake()
    // {
    //     source = GetComponent<AudioSource>();
    // }
    // public void playAudio(string clipName)
// string filePath = path + clipName;
        // AudioClip clipToPlay = Resources.Load<AudioClip>(filePath);
        // // Debug.Log("clip to play:"+ clipToPlay.GetType());
        // if (clipToPlay != null)
        // {
        //     source.clip = clipToPlay;
        //     source.Play();
        // }else
        // {
        //     Debug.Log("Clip not found: " + clipName);
        // }