using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] GameObject shieldObj;
    public bool isAcrtivated = true;


    public void ShieldActivation(float shieldTime)
    {
        StartCoroutine(ShieldActivationCO(shieldTime));
    }
    
    IEnumerator ShieldActivationCO(float shieldTime)
    {
        
        isAcrtivated = true;
        GetComponent<Health>().ShieldManipulation(isAcrtivated);
        shieldObj.SetActive(isAcrtivated);
        yield return new WaitForSeconds(shieldTime);
        isAcrtivated = false;
        shieldObj.SetActive(isAcrtivated);
        GetComponent<Health>().ShieldManipulation(isAcrtivated);
    }
}
