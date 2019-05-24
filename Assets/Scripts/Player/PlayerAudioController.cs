using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource walkSource;
    [SerializeField] private AudioSource swordSource;
    [SerializeField] private AudioSource gunSource;

    [SerializeField] [FormerlySerializedAs("clips")]
    private AudioClip[] walkingClips;

    [SerializeField] private AudioClip[] swordClips;
    [SerializeField] private AudioClip[] gunClips;

    public void PlayWalkSound()
    {
        Play(walkingClips, walkSource, true);
    }

    public void PlaySwordSound()
    {
        Play(swordClips, swordSource, true);
    }

    public void PlayGunSounds()
    {
        Play(gunClips, gunSource, false);
    }

    private void Play(AudioClip[] audioClips, AudioSource source, bool blocking)
    {
        if (!source.isPlaying | !blocking)
        {
            int index = Random.Range(0, audioClips.Length);
            source.clip = audioClips[index];
            source.Play();
        }
    }
}