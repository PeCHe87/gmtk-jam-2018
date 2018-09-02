using UnityEngine;

public class HealthController : MonoBehaviour
{
    public System.Action<int> OnDamage;
    public System.Action OnDead;

    [SerializeField] private int _maxHealth;
    [SerializeField] private int _health;
	
    public int Health { get { return _health; } }

    public int MaxHealth { get { return _maxHealth; } }

    public void Damage(int amountOfDamage)
    {
        _health = Mathf.Clamp(_health - amountOfDamage, 0, _maxHealth);

        if (_health > 0)
        {
            if (OnDamage != null)
                OnDamage(_health);
        }
        else if (OnDead != null)
            OnDead();
    }
}
