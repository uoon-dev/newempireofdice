using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] GameObject body;

    void Start()
    {
        body.SetActive(false);
    }

    public void HandlePowerUpBody()
    {
        body.SetActive(true);
        var powerUpBodyController = FindObjectOfType<PowerUpBodyController>();
        powerUpBodyController.AddPowerUpGage();
    }
}
