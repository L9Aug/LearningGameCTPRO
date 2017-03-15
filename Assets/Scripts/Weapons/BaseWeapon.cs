using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseWeapon : MonoBehaviour {

    public enum ProjectileType { SolidShot }

    public Transform Muzzle;
    public float Damage;
    public int ProjectilesPerShot = 1;
    public int MagazineSize;
    public int Magazine = 0;
    public float RateOfFire;
    public float ReloadTime;
    public float MuzzleVelocity;
    public float Spread;
    public float AimedSpread;
    public float WeaponRange;
    public int AimedZoom;
    public bool isAimed;
    public ProjectileType BulletType;

    public bool IsEquiped = false;
    public bool reloading = false;

    /// <summary>
    /// First Element is the location for the player. Second Element is the location for the AI.
    /// </summary>
    [Tooltip("First Element is the location for the player.\nSecond Element is the location for the AI.")]
    public List<Vector3> EquipLocation = new List<Vector3>();
    public Vector3 AimLocation;

    public delegate Vector3 GetTarget();
    public delegate void UpdateWeapon(int magazine, int magazineSize);
    public delegate void ReloadCallbackFunc();
    public List<UpdateWeapon> WeaponUpdates = new List<UpdateWeapon>();
    public List<ReloadCallbackFunc> ReloadBeginCallback = new List<ReloadCallbackFunc>();
    public List<ReloadCallbackFunc> ReloadEndCallback = new List<ReloadCallbackFunc>();

    protected float reloadTimer = 0;
    protected float ammoRemaining = 0;
    protected float cooldownTimer = 0;

    protected bool OnCooldown = false;

	// Use this for initialization
	void Awake ()
    {
        Magazine = MagazineSize;
        if (IsEquiped)
        {
            OnWeaponEquip();
        }
	}

    public void OnWeaponEquip()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
        IsEquiped = true;
        CallWeaponUpdates();
    }

    public void OnWeaponUnEquip()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().enabled = true;
        IsEquiped = false;
    }

    public virtual bool Fire(GetTarget targetFunc)
    {
        if (!reloading)
        {
            if (!OnCooldown)
            {
                if(Magazine > 0)
                {
                    for (int i = 0; i < ProjectilesPerShot; ++i)
                    {
                        Vector3 bulletEndPos = targetFunc();
                        bulletEndPos += ((isAimed) ? AimedSpread : Spread) * Random.insideUnitSphere;

                        BaseProjectile nProjectile = Instantiate<BaseProjectile>(Resources.Load<BaseProjectile>("Projectiles/" + BulletType.ToString()));
                        nProjectile.transform.position = Muzzle.position;
                        nProjectile.transform.LookAt(bulletEndPos);
                        nProjectile.SetUpProjectile(MuzzleVelocity, Damage);
                        
                    }
                    FiredGun();
                    return true;
                }
                else
                {
                    Reload();
                }
            }
        }
        return false;
    }

    protected virtual void DrawBulletPath(Vector3 StartPos, Vector3 EndPos)
    {
        BulletTrail bt = Instantiate<BulletTrail>(Resources.Load<BulletTrail>("BulletTrail"));
        bt.SetupBulletTrail(StartPos, EndPos, 1f / RateOfFire);
    }

    protected virtual void FiredGun()
    {
        OnCooldown = true;
        --Magazine;
        CallWeaponUpdates();
        StartCoroutine(CooldownTick());
    }

    protected void CallWeaponUpdates()
    {
        for(int i = 0; i < WeaponUpdates.Count; ++i)
        {
            if(WeaponUpdates[i] != null)
            {
                WeaponUpdates[i](Magazine, MagazineSize);
            }
        }
    }

    public virtual bool Reload()
    {
        if (!reloading && !OnCooldown && Magazine != MagazineSize)
        {
            reloading = true;
            StartCoroutine(RealodTick());
            CallReloadBeginCallback();
            return true;
        }
        return false;
    }

    protected virtual IEnumerator CooldownTick()
    {
        float timeBetweenShots = 1f / RateOfFire;
        while (OnCooldown)
        {
            yield return null;
            cooldownTimer += Time.deltaTime;
            if(cooldownTimer >= timeBetweenShots)
            {
                OnCooldown = false;
                cooldownTimer = 0;
            }
        }
    }

    protected virtual IEnumerator RealodTick()
    {
        while (reloading)
        {
            yield return null;
            reloadTimer += Time.deltaTime;
            if(reloadTimer >= ReloadTime)
            { 
                reloading = false;
                reloadTimer = 0;
                Magazine = MagazineSize;
                CallWeaponUpdates();
                CallReloadEndCallback();
            }
        }
    }

    void CallReloadBeginCallback()
    {
        for(int i = 0; i < ReloadBeginCallback.Count; ++i)
        {
            if(ReloadBeginCallback[i] != null)
            {
                ReloadBeginCallback[i]();
            }
            else
            {
                ReloadBeginCallback.RemoveAt(i);
                --i;
            }
        }
    }

    void CallReloadEndCallback()
    {
        for (int i = 0; i < ReloadEndCallback.Count; ++i)
        {
            if (ReloadEndCallback[i] != null)
            {
                ReloadEndCallback[i]();
            }
            else
            {
                ReloadEndCallback.RemoveAt(i);
                --i;
            }
        }
    }
}
