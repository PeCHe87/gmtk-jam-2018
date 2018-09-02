using System;
using UnityEngine;

public class SoundState : MonoBehaviour
{
    [SerializeField] private EntityController _target;
    [Header("Sounds")]
    [SerializeField] private AudioClip _clipBadComboStep;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        _target.OnGoodComboStep += GoodComboStep;
        _target.OnBadComboStep += BadComboStep;
        _target.OnComboComplete += PerformCombo;
        _target.OnReceiveDamage += ReceiveDamage;
        _target.OnMissAttack += MissAttack;
        _target.OnPreAttack += PreAttack;
        _target.OnAttack += Attack;
    }

    private void GoodComboStep(int step, AudioClip clip)
    {
        Play(clip);
    }

    private void BadComboStep(ScriptableCombo combo)
    {
        Play(combo.clipBadStep);
    }

    private void PerformCombo(ScriptableCombo combo)
    {
        int clipIndex = combo.clipsFeedback.Length - 1;

        Play(combo.clipsFeedback[clipIndex]);
    }

    private void ReceiveDamage(ScriptableAttack attack)
    {
        //Play(attack.clipHit);
    }

    private void MissAttack(ScriptableAttack attack)
    {
        Play(attack.clipSwoosh);
    }

    private void PreAttack(ScriptableAttack attack)
    {
        PlayLoop(attack.clipPre);
    }

    private void Attack(ScriptableAttack attack)
    {
        Play(attack.clipHit);
    }

    private void OnDestroy()
    {
        _target.OnGoodComboStep -= GoodComboStep;
        _target.OnBadComboStep -= BadComboStep;
        _target.OnComboComplete -= PerformCombo;
        _target.OnReceiveDamage -= ReceiveDamage;
        _target.OnMissAttack -= MissAttack;
        _target.OnPreAttack -= PreAttack;
        _target.OnAttack -= Attack;
    }

    public void Play(AudioClip clip)
    {
        Debug.Log("SoundState::<b>Play</b> -- loop: " + audioSource.loop);
        audioSource.loop = false;
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play(); //audioSource.PlayOneShot(clip);
    }

    public void PlayLoop(AudioClip clip)
    {
        
        audioSource.loop = true;
        audioSource.clip = clip;
        Debug.Log("SoundState::<b>PlayLoop</b> -- loop: " + audioSource.loop);

        audioSource.Play();
    }
}
