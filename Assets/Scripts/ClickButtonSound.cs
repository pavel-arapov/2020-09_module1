using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClickButtonSound : MonoBehaviour
{
    public AudioClip sound;

    private Button Button => GetComponent<Button>();
    private AudioSource AudioSource => GetComponent<AudioSource>();

    [SerializeField] private AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start() {
        // adding AudioSource component to the Button
        gameObject.AddComponent<AudioSource>();
        AudioSource.clip = sound;
        AudioSource.playOnAwake = false;
        // _audioMixer = Resources.Load("AudioMixer") as AudioMixer;
        AudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effects")[0];
        // creating onClick Listener to play the sound 
        Button.onClick.AddListener(() => {
            AudioSource.PlayOneShot(sound);
        });
    }
}