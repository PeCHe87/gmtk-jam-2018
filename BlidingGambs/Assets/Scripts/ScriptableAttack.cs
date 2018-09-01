using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Tools/Attack", order = 1)]
public class ScriptableAttack : ScriptableObject
{
    public string name;

    public int damage;

    [Tooltip("Ammount of high beats between the telegraph and the attack")]
    public int preparationBeats;

    [Tooltip("Ammount of high beats between the attack and the next telegraph")]
    public int recoveryBeats;

    [Tooltip("Ammount of high beats of tolerance for the player to perform the correct combo")]
    public int toleranceBeats;
}
