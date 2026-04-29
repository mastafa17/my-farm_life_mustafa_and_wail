using UnityEngine;
using System.Collections;

public class TractorSound : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip startClip;
    public AudioClip idleClip;
    public AudioClip driveClip;

    public float startSoundTime = 2f;
    public float fadeTime = 0.3f;

    private bool engineOn;
    private bool playerInside;
    private AudioClip currentClip;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.volume = 1f;
    }

    void Update()
    {
        if (!playerInside || !engineOn) return;

        float move = Input.GetAxis("Vertical");

        if (Mathf.Abs(move) > 0.15f)
            ChangeClip(driveClip);
        else
            ChangeClip(idleClip);
    }

    public void EnterTractorSound()
    {
        playerInside = true;
        StartCoroutine(StartEngine());
    }

    public void ExitTractorSound()
    {
        playerInside = false;
        engineOn = false;
        currentClip = null;
        StopAllCoroutines();
        audioSource.Stop();
    }

    IEnumerator StartEngine()
    {
        engineOn = false;

        audioSource.Stop();
        audioSource.clip = startClip;
        audioSource.loop = false;
        audioSource.volume = 1f;
        audioSource.Play();

        yield return new WaitForSeconds(startSoundTime);

        engineOn = true;
        ChangeClip(idleClip);
    }

    void ChangeClip(AudioClip clip)
    {
        if (clip == null) return;
        if (currentClip == clip) return;

        currentClip = clip;
        StopCoroutineSafe();
        StartCoroutine(FadeSwitch(clip));
    }

    void StopCoroutineSafe()
    {
        // نخليها فارغة حتى ما نوقف StartEngine بالغلط
    }

    IEnumerator FadeSwitch(AudioClip clip)
    {
        float v = audioSource.volume;

        while (audioSource.volume > 0.05f)
        {
            audioSource.volume -= Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();

        while (audioSource.volume < 1f)
        {
            audioSource.volume += Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.volume = 1f;
    }
}