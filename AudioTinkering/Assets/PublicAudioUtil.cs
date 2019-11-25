
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

public static class PublicAudioUtil
{

    //Unity has functions inside UnityEditor for playing audio (inside AudioUtil.cs) but it's not accessible normally.
    //This code uses reflection to find the methods we want (PlayClip and StopAllClips) and lets us call them via reflection.
    public static void PlayClip(AudioClip clip)
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "PlayClip",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new System.Type[] {
         typeof(AudioClip)
        },
        null
        );
        method.Invoke(
            null,
            new object[] {
         clip
        }
        );
    }
    public static void StopAllClips()
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass =
              unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "StopAllClips",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new System.Type[] { },
            null
        );
        method.Invoke(
            null,
            new object[] { }
        );
    }

} // class PublicAudioUtil