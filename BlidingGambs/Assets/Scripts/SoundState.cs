using UnityEngine;

public class SoundState : MonoBehaviour
{
    [SerializeField] private EntityController _target;
    [Header("Sounds")]
    [SerializeField] private AudioClip _clipDamage;
    [SerializeField] private AudioClip _clipBadComboStep;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        _target.OnGoodComboStep += GoodComboStep;
        _target.OnBadComboStep += BadComboStep;
        _target.OnComboComplete += PerformCombo;
    }

    private void GoodComboStep(int step, AudioClip clip)
    {
        Play(clip);
    }

    private void BadComboStep()
    {
        Play(_clipBadComboStep);
    }

    private void PerformCombo(ScriptableCombo combo)
    {
        int clipIndex = combo.clipsFeedback.Length - 1;

        Play(combo.clipsFeedback[clipIndex]);
    }

    private void OnDestroy()
    {
        _target.OnGoodComboStep -= GoodComboStep;
        _target.OnBadComboStep -= BadComboStep;
        _target.OnComboComplete -= PerformCombo;
    }

    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
