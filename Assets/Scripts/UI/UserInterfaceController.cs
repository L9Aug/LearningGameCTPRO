using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UserInterfaceController : MonoBehaviour
{
    public static UserInterfaceController UIC;

    public Image HealthBarImg;
    public Text AmmoCount;

    public Image ReloadUIImg;
    public Text ReloadText;

	void Start()
    {
        UIC = this;
        HideReloadUI();
    }

    public void OnHealthChanged(float Change, float Health, float MaxHealth)
    {
        HealthBarImg.fillAmount = Health / MaxHealth;
        if (Change < 0)
        {
            PlayerMetricsController.PMC.DamageTaken -= Change;
        }

        if (Health <= 0)
        {
            PlayerController.PC.IsDead = true;
        }
    }

    public void OnWeaponUpdate(int Magazine, int MagazineSize)
    {
        AmmoCount.text = Magazine + "/" + MagazineSize;

        if(Magazine <= MagazineSize / 6)
        {
            //display Reaload UI
            ReloadText.gameObject.SetActive(true);
        }
        if(Magazine == 0)
        {
            ReloadUIImg.gameObject.SetActive(true);
        }
    }

    public void AnimateReload()
    {
        ReloadUIImg.gameObject.SetActive(true);
        ReloadText.gameObject.SetActive(false);
        StartCoroutine(ReloadAnimation());
    }

    public void HideReloadUI()
    {
        ReloadUIImg.gameObject.SetActive(false);
        ReloadText.gameObject.SetActive(false);
    }

    IEnumerator ReloadAnimation()
    {
        
        while (PlayerController.PC.CurrentWeapon.reloading)
        {
            // animate reload symbol.
            ReloadUIImg.GetComponent<RectTransform>().Rotate(Vector3.forward, 90 * Time.deltaTime, Space.Self);
            yield return null;
        }
        ReloadUIImg.GetComponent<RectTransform>().localRotation = Quaternion.identity;
        HideReloadUI();
    }

}
