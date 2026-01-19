using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource pistolSound;
    [SerializeField] private AudioSource rocketLauncherSound;
    [SerializeField] private AudioSource shotgunSound;
    [SerializeField] private AudioSource rifleSound;
    [SerializeField] private AudioSource pistolEmptyingSound;
    [SerializeField] private AudioSource emptyGun;
    [SerializeField] private AudioSource reloadAmmo;
    [SerializeField] private AudioSource hoverSound;
    [SerializeField] private AudioSource dashSound;
    [SerializeField] private AudioSource hookSound;

    private void Awake()
    {
        Instance = this;
        /*
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        */
    }

    public void PlayPistolShootingSound()
    {
        pistolSound.Play();
    }

    public void PlayRocketLauncherShootingSound()
    {
        rocketLauncherSound.Play();
    }

    public void PlayRifleShootingSound()
    {
        rifleSound.Play();
    }

    public void PlayShotgunShootingSound()
    {
        shotgunSound.Play();
    }

    public void PlayPistolEmptyingSound()
    {
        pistolEmptyingSound.Play();
    }

    public void PlayEmptyGunSound()
    {
        emptyGun.Play();
    }

    public void PlayReloadAmmoSound()
    {
        reloadAmmo.Play();
    }

    public void PlayHoverSound()
    {
        hoverSound.Play();
    }

    public void PlayDashSound()
    {
        dashSound.Play();
    }

    public void PlayHookSound()
    {
        hookSound.Play();
    }
}
