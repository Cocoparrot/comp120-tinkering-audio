using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralAudioSource))]
public class ProceduralAudioSourceEditor : Editor
{


    /*
     * Author: Bradley Bath
     * Description: The custom inspector for ProceduralAudioSource
     * Issues:
     *      Created audio won't play. This seems to be because AudioUtil.PlayClip will not play any clips that aren't actually imported into the project (DialogueSound.wav will play correctly if you        set audioSource.audio to that).
     *  
     */

    //Creates a texture of a audio waveform that can be drawn to the inspector.
    ProceduralAudioSource audioSource;
    Texture2D WaveForm;


    int Frequency;
    bool playingSound;

    /// <summary> This method returns a generated AudioClip based on given arguments
    /// </summary>
    /// <param name="frequency">
    ///     The frequency of the generated audio
    /// </param>
    private AudioClip CreateToneAudioClip(float frequency)
    {
        int sampleFreq = 44000;

        float[] samples = new float[44000];
        //This creates a 'triangle' shaped wave from instead of a sine wave.
        for (int i = 0; i < samples.Length; i++)
            samples[i] = Mathf.Repeat(i * frequency / sampleFreq, 1) * 2f - 1f;

        AudioClip ac = AudioClip.Create("Audio", samples.Length, 1, sampleFreq, false);
        ac.SetData(samples, 0);

        return ac;
    }

    private void OnEnable()
    {
        WaveForm = null;
    }

    public override void OnInspectorGUI()
    {
        //Draw custom inspector:

        //Get ProceduralAudioSource (target is the selected object, we need to convert it to ProceduralAudioSource
        audioSource = (ProceduralAudioSource)target;
        //Draw the default inspector of ProceduralAudioSource (such as any public or serialized fields)
        DrawDefaultInspector();

        //Created a label with the text "Frequency"
        GUILayout.Label("Frequency");
        //Create a slider for frequency with the slider going between values 1 and 24000 (hz)
        Frequency = EditorGUILayout.IntSlider(Frequency, 1, 24000);

        //Call the PaintWaveformSpectrum function if audioSource and audioSource.audio exists
        if (audioSource && audioSource.audio)
        {
            //This causes a memory leak it seems. Need to find a good way to draw the WaveForm that isn't on every frame (which causes memory usage to go up) 

            //WaveForm = PaintWaveformSpectrum(audioSource.audio, 1, 300, 100, Color.yellow);
        }


        //Create a toggle box for playing audio. Here I use a ternary operator to change the text of it based on what playingSound is on one line.
        playingSound = EditorGUILayout.Toggle(!playingSound ? "Play audio" : "Stop audio", playingSound);
        //Created waveform preview if audioSource and audioSource.audio exist.
        if (audioSource && audioSource.audio)
        {
            GUILayout.Label("Waveform");
            //Box containing the waveform texture.
            GUILayout.Box(WaveForm, GUILayout.Width(300), GUILayout.Height(100));
        }
    }

    private void OnValidate()
    {
    }

    //Think of this function as Update() if you're unfamiliar with Editor/custom inspector scripts.
    private void OnSceneGUI()
    {
        //Set audioSource.audio to the object returned by CreateToneAudio
        if(audioSource)
            audioSource.audio = CreateToneAudioClip(Frequency);

        //Play or stop sound based on the boolean playingSound (since this if statement is one line, we don't need to use brackets.
        if (playingSound)
            PublicAudioUtil.PlayClip(audioSource.audio);
        else
            PublicAudioUtil.StopAllClips();
    }

    /// <summary> This method returns a Texture2D of an audio's waveform 
    ///    which can then be drawn to the inspector
    /// </summary>
    /// <param name="audio">
    ///     The audio clip to draw the waveform of
    /// </param>
    /// <param name="col">
    /// Color of the waveform
    /// </param>
    /// <param name="height">
    /// Height of the waveform texture
    /// </param>
    /// <param name="saturation">
    /// Saturation of the waveform texture
    /// </param>
    /// <param name="width">
    /// Width of the waveform texture
    /// </param>
    Texture2D PaintWaveformSpectrum(AudioClip audio, float saturation, int width, int height, Color col)
    {
        //Create empty texture with correct width, height and format.
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        //Create a float array with the length of the audio's samples.
        float[] samples = new float[audio.samples];
        //Create a float array with the length of the texture's width
        float[] waveform = new float[width];
        //Get data of the audio.
        audio.GetData(samples, 0);
        int packSize = (audio.samples / width) + 1;
        int sample = 0; 
        for (int i = 0; i < audio.samples; i += packSize)
        {
            waveform[sample] = Mathf.Abs(samples[i]);
            sample++;
        }
        //Background of image
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tex.SetPixel(x, y, Color.black);
            }
        }
        //Drawing the actual 'wave' of the waveform
        for (int x = 0; x < waveform.Length; x++)
        {
            for (int y = 0; y <= waveform[x] * ((float)height * .75f); y++)
            {
                tex.SetPixel(x, (height / 2) + y, col);
                tex.SetPixel(x, (height / 2) - y, col);
            }
        }
        //Apply the texture and return it.
        tex.Apply();

        return tex;
    }
}
