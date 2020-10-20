using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClickButtonSound : MonoBehaviour
{
    public AudioClip sound;

    private Button Button => GetComponent<Button>();
    private AudioSource AudioSource => GetComponent<AudioSource>();

    // Start is called before the first frame update
    void Start() {
        gameObject.AddComponent<AudioSource>();
        AudioSource.clip = sound;
        AudioSource.playOnAwake = false;
    }

    void Play() {
        AudioSource.PlayOneShot(sound);
    }
}