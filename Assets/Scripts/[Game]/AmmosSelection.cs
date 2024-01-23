using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmosSelection : MonoBehaviour
{
    private int currentAmmoIndex = 0; // Current selected ammo index

    public List<Ammos> ammos;

    public GunShooter gun;

    void SwitchToNextAmmo()
    {
        // Implement logic to switch to the next ammo type
        // Update the currentAmmoIndex accordingly
        currentAmmoIndex++;

        if (currentAmmoIndex > 2)
        {
            currentAmmoIndex = 0;
        }
    }

    void SwitchToPreviousAmmo()
    {
        // Implement logic to switch to the previous ammo type
        // Update the currentAmmoIndex accordingly
        currentAmmoIndex--;

        if (currentAmmoIndex < 0)
        {
            currentAmmoIndex = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            SwitchToNextAmmo();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            SwitchToPreviousAmmo();
        }

        DisplayUIAmmos();
    }

    private void DisplayUIAmmos()
    {
        for (int i = 0; i < ammos.Count; i++)
        {
            if (currentAmmoIndex == i)
            {
                ammos[currentAmmoIndex]._memeGif.enabled = true;
                gun.onShootSFX = ammos[currentAmmoIndex]._audioClip;
                gun.projectilePrefab = ammos[currentAmmoIndex].projectile;
            }
            else
            {
                ammos[i]._memeGif.enabled = false;
            }
        }


    }
}

[System.Serializable]
public class Ammos
{
    public Image _memeGif;
    public AudioClip _audioClip;
    public GameObject projectile;
}
