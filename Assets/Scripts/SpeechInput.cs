using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using UnityEngine.SceneManagement;

public class SpeechInput : MonoBehaviour
{   
    public AudioMsg audioMsg;
    public SetNavigationTarget setNavigationTarget;
    async public void StartApplication(){
        Debug.Log("inside the method: StartApplication");
        setNavigationTarget.setGuide();
    }
    public void QuitApplication(){
        Debug.Log("inside the method: Quit");
        Application.Quit();
    }
    public void goto212(){
        Debug.Log("inside the method: goto212");
        // setNavigationTarget.SetCurrentNavigationTarget(1);
    }
    
    
    async public void HeyLuna(){
        Debug.Log("inside the HeyLuna");
        await audioMsg.PlayAudio("YES");
        AzureSpeechToText();
      // StartCoroutine(StartRecording());
    }
  
    async public void AzureSpeechToText(){
        var speechConfig = SpeechConfig.FromSubscription("5876eecac25c41879c6933b36005b459", "eastus");
        speechConfig.SpeechRecognitionLanguage = "en-US";

        using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        Debug.Log("Speak into your microphone.");
        var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
        await OutputSpeechRecognitionResult(speechRecognitionResult);

    }
    private async Task OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
    {
        Debug.Log("inside the OutputSpeechRecognitionResult: ");
        switch (speechRecognitionResult.Reason)
        {
            case ResultReason.RecognizedSpeech:
                Console.WriteLine($"RECOGNIZED: Text={speechRecognitionResult.Text}");
                await Chatterbot(speechRecognitionResult.Text);
                break;
            case ResultReason.NoMatch:
                Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                break;
            case ResultReason.Canceled:
                var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                    Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                }
                break;
        }
    }

   private async Task Chatterbot(string text){
        Debug.Log("input from  user: "+ text);
        string apiUrl = "https://chatgpt-navbot.azurewebsites.net/api/chat";
        string jsonData = "{\"msg\":\"" + text + "\"}";
        HttpRequest httpRequest = new HttpRequest();
        string response = await httpRequest.CallApiAndGetResponse(apiUrl, jsonData);
        // Debug.Log("API response: " + response);
        response = response.Replace("\n", "");
        var res = response;
        await audioMsg.PlayAudio(res);
        if (response.Contains("212")){
            // Debug.Log("goto212");
            setNavigationTarget.SetCurrentNavigationTarget(1);
        }
        if (response.Contains("214")){
            // Debug.Log("goto214");
            setNavigationTarget.SetCurrentNavigationTarget(2);
        }
        
    }
    public void goto214(){
        Debug.Log("inside the method: goto214");
        // setNavigationTarget.SetCurrentNavigationTarget(2);
    }
    public void goto216(){
        Debug.Log("inside the method: goto216");
        setNavigationTarget.SetCurrentNavigationTarget(3);
    }
}




    // private IEnumerator StartRecording()
    // {
    //     // Start audio input, providing the device's microphone as the audio source
    //     var audioClip = Microphone.Start(null, true, 5, AudioSettings.outputSampleRate);

    //     // Wait for the specified duration (5 seconds in this example)
    //     yield return new WaitForSeconds(5f);

    //     // Stop recording audio
    //     Microphone.End(null);

    //     // Save the recorded audio clip
    //     recordedClip = AudioClip.Create("RecordedClip", audioClip.samples, audioClip.channels, audioClip.frequency, false);
    //     float[] data = new float[audioClip.samples * audioClip.channels];
    //     audioClip.GetData(data, 0);
    //     recordedClip.SetData(data, 0);
        
    //     // Process the recorded audio clip (e.g., send it to your speech recognition service)
    //     SpeechToText(recordedClip);
    // }