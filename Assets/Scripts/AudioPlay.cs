using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
internal sealed class AudioPlay : MonoBehaviour
{
    private AudioSource _audioSource;

    #region DataSound Definition

    [SerializeField] private List<DataSound> dataSounds = new List<DataSound>();

    [Serializable]
    private sealed class DataSound
    {
        public string name;
        public AudioClip clip;
    }

    public AudioClip GetClip(string nameClip)
    {
        return dataSounds.Where(dataSound => dataSound.name == nameClip).
            Select(dataSound => dataSound.clip).FirstOrDefault();
    }

    #endregion

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play(string nameClip)
    {
        var audioClip = GetClip(nameClip);
        if (audioClip) {
            _audioSource.PlayOneShot(audioClip);
        }
        else {
            Debug.Log("Clip " + nameClip + " not found or not defined");
        }
    }
}