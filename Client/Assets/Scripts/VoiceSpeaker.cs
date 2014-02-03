// Voice Speaker  (c) ZJP

//

// Windows 32B >> Copy 'Voice_speaker.dll' in windows\system32 folder

// Windows 64B >> Copy 'Voice_speaker.dll' in windows\SysWOW64 folder

// Remember to release "Voice_speaker.dll" with your final project. It will be placed in the same folder as the EXE

//

// Voice Speaker  (c) ZJP //
using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class VoiceSpeaker : MonoBehaviour
{

    [DllImport("Voice_speaker.dll", EntryPoint = "VoiceAvailable")]
    private static extern int VoiceAvailable();

    [DllImport("Voice_speaker.dll", EntryPoint = "InitVoice")]
    private static extern void InitVoice();

    [DllImport("Voice_speaker.dll", EntryPoint = "WaitUntilDone")]
    private static extern int WaitUntilDone(int millisec);

    [DllImport("Voice_speaker.dll", EntryPoint = "FreeVoice")]
    private static extern void FreeVoice();

    [DllImport("Voice_speaker.dll", EntryPoint = "GetVoiceCount")]
    private static extern int GetVoiceCount();

    // Unity V4.x.x

    [DllImport("Voice_speaker.dll", EntryPoint = "GetVoiceName")]
    private static extern IntPtr GetVoiceName(int index);

    //  other Unity version

    // [DllImport ("Voice_speaker.dll", EntryPoint="GetVoiceName")]   private static extern string GetVoiceName(int index);



    [DllImport("Voice_speaker.dll", EntryPoint = "SetVoice")]
    private static extern void SetVoice(int index);

    [DllImport("Voice_speaker.dll", EntryPoint = "Say")]
    private static extern void Say(string ttospeak);

    [DllImport("Voice_speaker.dll", EntryPoint = "SayAndWait")]
    private static extern void SayAndWait(string ttospeak);

    [DllImport("Voice_speaker.dll", EntryPoint = "SpeakToFile")]
    private static extern int SpeakToFile(string filename, string ttospeak);

    [DllImport("Voice_speaker.dll", EntryPoint = "GetVoiceState")]
    private static extern int GetVoiceState();

    [DllImport("Voice_speaker.dll", EntryPoint = "GetVoiceVolume")]
    private static extern int GetVoiceVolume();

    [DllImport("Voice_speaker.dll", EntryPoint = "SetVoiceVolume")]
    private static extern void SetVoiceVolume(int volume);

    [DllImport("Voice_speaker.dll", EntryPoint = "GetVoiceRate")]
    private static extern int GetVoiceRate();

    [DllImport("Voice_speaker.dll", EntryPoint = "SetVoiceRate")]
    private static extern void SetVoiceRate(int rate);

    [DllImport("Voice_speaker.dll", EntryPoint = "PauseVoice")]
    private static extern void PauseVoice();

    [DllImport("Voice_speaker.dll", EntryPoint = "ResumeVoice")]
    private static extern void ResumeVoice();

    public int voice_nb = 1; // 0 = David, 1 = Hazel, 2 = Emily 16Hz, 3 = Zira

    void Start()
    {
        if (VoiceAvailable() > 0)
        {
            InitVoice(); // init the engine

            if (voice_nb > GetVoiceCount())
                voice_nb = 0;

            if (voice_nb < 0)
                voice_nb = 0;

            SetVoice(voice_nb);
            SetVoiceRate(1);
			
			Say("Welcome to VoKey");
			
			Say("Please enter your username and password to login.");

        }
        //Application.Quit();
    }

    void OnDisable()
    {
        if (VoiceAvailable() > 0)
        {
            FreeVoice();
        }
    }
}