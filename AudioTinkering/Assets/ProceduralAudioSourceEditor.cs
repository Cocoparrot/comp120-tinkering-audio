using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralAudioSource))]
public class ProceduralAudioSourceEditor : Editor
{


    //Creates a texture of a audio waveform that can be drawn to the inspector.
    ProceduralAudioSource audioSource;
    Texture2D WaveForm;


    int Frequency;
    bool playingSound;

    private AudioClip CreateToneAudioClip(float frequency)
    {
        int sampleFreq = 44000;

        float[] samples = new float[44000];
        for (int i = 0; i < samples.Length; i++)
            samples[i] = Mathf.Repeat(i * frequency / sampleFreq, 1) * 2f - 1f;

        AudioClip ac = AudioClip.Create("Audio", samples.Length, 1, sampleFreq, false);
        ac.SetData(samples, 0);

        return ac;
    }

    public override void OnInspectorGUI()
    {
        audioSource = (ProceduralAudioSource)target;
        DrawDefaultInspector();
        GUILayout.Label("Frequency");
        Frequency = EditorGUILayout.IntSlider(Frequency, 1, 24000);
        playingSound = EditorGUILayout.Toggle(!playingSound ? "Play audio" : "Stop audio", playingSound);
        if (audioSource && audioSource.audio)
        {
            GUILayout.Label("Waveform");
            GUILayout.Box(WaveForm, GUILayout.Width(300), GUILayout.Height(100));
        }

    }

    private void OnValidate()
    {
        Debug.Log("!!");
        Debug.Log(audioSource.audio);
    }

    private void OnSceneGUI()
    {
        if(audioSource)
            audioSource.audio = CreateToneAudioClip(Frequency);
        if (audioSource && audioSource.audio)
            WaveForm = PaintWaveformSpectrum(audioSource.audio, 1, 300, 100, Color.yellow);
        if (playingSound)
        {
            PublicAudioUtil.PlayClip(audioSource.audio);
        }
        else
        {
            PublicAudioUtil.StopAllClips();
        }
        if (audioSource && audioSource.audio)
        {
            SaveWavUtil.Save("Testing testing", audioSource.audio);
        }
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
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        float[] samples = new float[audio.samples];
        float[] waveform = new float[width];
        audio.GetData(samples, 0);
        int packSize = (audio.samples / width) + 1;
        int s = 0;
        for (int i = 0; i < audio.samples; i += packSize)
        {
            waveform[s] = Mathf.Abs(samples[i]);
            s++;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tex.SetPixel(x, y, Color.black);
            }
        }

        for (int x = 0; x < waveform.Length; x++)
        {
            for (int y = 0; y <= waveform[x] * ((float)height * .75f); y++)
            {
                tex.SetPixel(x, (height / 2) + y, col);
                tex.SetPixel(x, (height / 2) - y, col);
            }
        }
        tex.Apply();

        return tex;
    }
}
