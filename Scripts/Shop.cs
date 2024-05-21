using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint standardTurret;
    public TurretBlueprint missleTurret;
    public TurretBlueprint laserBeamer;
    private BuildManager buildManager;
    public ParticleSystem particleSystem;
    public RectTransform focusEffect;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }
    
    public void SelectStandardTurret(RectTransform buttonTrans)
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(standardTurret, focusEffect.gameObject);
        particleSystem.Play();
        focusEffect.position = buttonTrans.position;
        focusEffect.gameObject.SetActive(true);
    }

    public void SelectMissileTurret(RectTransform buttonTrans)
    {
        Debug.Log("Missile Turret Selected");
        buildManager.SelectTurretToBuild(missleTurret, focusEffect.gameObject);
        particleSystem.Play();
        focusEffect.position = buttonTrans.position;
        focusEffect.gameObject.SetActive(true);
    }

    public void SelectLaserBeamer(RectTransform buttonTrans)
    {
        Debug.Log("Laser Beamer Selected");
        buildManager.SelectTurretToBuild(laserBeamer, focusEffect.gameObject);
        particleSystem.Play();
        focusEffect.position = buttonTrans.position;
        focusEffect.gameObject.SetActive(true);
    }
}