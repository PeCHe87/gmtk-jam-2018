using UnityEngine;

public class HealthController : MonoBehaviour
{
    public static System.Action<int> OnDamage;
    public static System.Action OnDead;

    [SerializeField] private int _maxHealth;
    [SerializeField] private int _health;
	
    public void Damage(int amountOfDamage)
    {
        _health = Mathf.Clamp(_health - amountOfDamage, 0, _maxHealth);

        if (_health > 0)
            OnDamage(_health);
        else
            OnDead();
    }
}
