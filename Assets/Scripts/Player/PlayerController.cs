using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    /// <summary>
    /// The player controller
    /// </summary>
    public static PlayerController PC;
    public BaseWeapon CurrentWeapon;
    public Transform PlayerBody;
    public float EquipDist;
    public float EquipRadius;
    public Text EquipText;

    private Camera PlayerCam;
    
    // Use this for initialization
	void Start ()
    {
	    PC = this;
        PlayerCam = transform.GetComponentInChildren<Camera>();
        GetComponent<Health>().HealthChangedActions.Add(UserInterfaceController.UIC.OnHealthChanged);
        OnWeaponPickup(CurrentWeapon);
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckWeapon();
        LookForWeapon();
	}

    void CheckWeapon()
    {
        if (Input.GetButton("Fire"))
        {
            CurrentWeapon.Fire(GetTargetBulletLocation);
        }

        if (Input.GetButton("Reload"))
        {
            CurrentWeapon.Reload();
        }
    }

    void LookForWeapon()
    {
        Ray ray = new Ray(PlayerCam.transform.position, PlayerCam.transform.forward);
        RaycastHit hit = new RaycastHit();
        LayerMask mask = 1<<9;
        EquipText.text = "";
        if (Physics.SphereCast(ray, EquipRadius, out hit, EquipDist, mask))
        {
            if (hit.collider.gameObject.GetComponent<BaseWeapon>().IsEquiped == false)
            {
                EquipText.text = ("Press 'F' to equip: " + hit.collider.name);
                if (Input.GetButton("Interact"))
                {
                    // drop current weapon.
                    CurrentWeapon.OnWeaponUnEquip();
                    CurrentWeapon.transform.SetParent(transform.parent, true);
                    CurrentWeapon.GetComponent<Rigidbody>().AddExplosionForce(5, CurrentWeapon.transform.position - (CurrentWeapon.transform.forward * 0.1f), 0, 0.5f, ForceMode.VelocityChange);

                    // equip targeted weapon.
                    CurrentWeapon = hit.collider.GetComponent<BaseWeapon>();
                    CurrentWeapon.OnWeaponEquip();
                    CurrentWeapon.transform.rotation = Quaternion.identity;
                    CurrentWeapon.transform.position = CurrentWeapon.EquipLocation[0];
                    CurrentWeapon.transform.SetParent(PlayerCam.transform, false);
                }
            }
        }
    }

    void OnWeaponPickup(BaseWeapon newWeapon)
    {
        //if (CurrentWeapon != null) Destroy(CurrentWeapon.gameObject);
        CurrentWeapon = newWeapon;
        CurrentWeapon.WeaponUpdates.Add(UserInterfaceController.UIC.OnWeaponUpdate);
        CurrentWeapon.OnWeaponEquip();
        CurrentWeapon.ReloadBeginCallback.Add(UserInterfaceController.UIC.AnimateReload);
        //CurrentWeapon.ReloadEndCallback.Add();
    }

    Vector3 GetTargetBulletLocation()
    {
        return PlayerCam.transform.position + (PlayerCam.transform.forward * CurrentWeapon.WeaponRange);
    }
}
