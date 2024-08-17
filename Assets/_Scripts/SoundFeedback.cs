using UnityEngine;

public enum SoundType
{
    click,
    place,
    remove,
    wrongPlacement
}

public class SoundFeedback : MonoBehaviour
{
    [SerializeField]
    private AudioClip clickSound, placeSound, removeSound, wrongPlacementSound;

    [SerializeField]
    private AudioSource audioSource;

    public void PlaySFX(SoundType type)
    {
        switch (type)
        {
            case SoundType.click:
                audioSource.PlayOneShot(clickSound);
                break;
            case SoundType.place:
                audioSource.PlayOneShot(placeSound);
                break;
            case SoundType.remove:
                audioSource.PlayOneShot(removeSound);
                break;
            case SoundType.wrongPlacement:
                audioSource.PlayOneShot(wrongPlacementSound);
                break;
            default:
                break;
        }
    }
}
