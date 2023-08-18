using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private Enemy enemy;
    [SerializeField]
    public bool compAttack=false;

    [SerializeField]
    private Vector2 targetPos;

    public Slider healthSlider;
    public GameObject projectile;
    [SerializeField]
    private float projectileForce;
    

    
 

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        maxHealth = enemy.maxHealthProp;
        health = maxHealth;
        healthSlider.value = health / maxHealth;
    }

    public void Damage(float damage)
    {
        health = Mathf.Clamp(health - damage, 0f, maxHealth);
        if(health<=0f)
        {
            Die();
        }
        healthSlider.value = health / maxHealth;
    }
    void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator Attack()
    {
        int i = 0;
        GameObject _projectile;
        Rigidbody2D _projectileRB;
        Vector2 _projectileCurrentLoc;
        Vector2 targetPosA;
        if (compAttack== false && i == 0)
        {
            
            compAttack = true;
            targetPosA = new Vector2(enemy.targetPosProp.x, enemy.targetPosProp.y);
            _projectile = Instantiate(projectile, gameObject.transform);
            _projectileRB = _projectile.GetComponent<Rigidbody2D>();
            _projectileCurrentLoc = new Vector2(_projectileRB.transform.position.x, _projectileRB.transform.position.y);
            Vector2 diff = targetPosA - _projectileCurrentLoc;
            Vector2 ampDiff = diff.normalized;
            targetPos = ampDiff * projectileForce;
            i++;

            yield return new WaitForSeconds(attackSpeed);
            _projectile.SetActive(true);
            _projectileRB.AddForce(targetPos);
            compAttack = false;
            
               
            yield return new WaitForSeconds(4f);

            if (_projectile != null)
            {
                Destroy(_projectile);
            }
            yield break;
        }
             

    }

    public void methodStartAttack()
    {
        StartCoroutine("Attack");
    }
    

}
