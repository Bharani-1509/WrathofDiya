using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Slider healthSlider;
    public Enemy enemy;

    void Start()
    {
        healthSlider.maxValue = enemy.MaxHealth;
        healthSlider.value = enemy.CurrentHealth;
    }

    void Update()
    {
        healthSlider.value = enemy.CurrentHealth;
    }
}
