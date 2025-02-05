using UnityEngine;

public class FuelBoostScript : MonoBehaviour
{
    public float fuel = 100;
    public float boost_decrease_speed = 2f;
    public float boost_regen_speed = 1f;
    public bool burnout = false;
    public bool boosting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!boosting)
        {
            if (fuel >= 100)
            {
                fuel = 100;
            }
            else if (!burnout)
            {
                fuel += Time.deltaTime * boost_regen_speed;
            }
            else
            {
                fuel += Time.deltaTime * boost_regen_speed/2f;
            }
        }
    }

    public bool boost()
    {
        if (burnout && fuel <= 25)
        {
            boosting = false;
            return false;
        }
        else if (fuel >= 0)
        {
            fuel -= Time.deltaTime * boost_decrease_speed;
            burnout = false;
            boosting = true;
            return true;
        }
        else
        {
            fuel = 0;
            Debug.Log("Burnout");
            burnout = true;
            boosting = false;
            return false;
        }
    }

    public void refuel()
    {
        fuel = 100;
        burnout = false;
        boosting = false;
    }
}
