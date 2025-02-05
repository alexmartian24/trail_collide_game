using UnityEngine;
using UnityEngine.UI;

public class Fuel_Management : MonoBehaviour
{
    public FuelBoostScript blue_fuel;
    public FuelBoostScript red_fuel;
    public Text blue_fuel_UI;
    public Text red_fuel_UI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(blue_fuel_UI != null)
        {
            blue_fuel_UI.text = "Blue Fuel: " + (int) blue_fuel.fuel;
        }
        if (red_fuel_UI != null)
        {
            red_fuel_UI.text = "Red Fuel: " + (int) red_fuel.fuel;
        }
    }
}
