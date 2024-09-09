using System.Collections;
using UnityEngine;

public class TestAvoidWater : MonoBehaviour
{
    [SerializeField] private bool testWater;
    [SerializeField] private float waterDropTimer, timeBetweenSpawn;
    [SerializeField] private GameObject[] waterDrops;
    private bool isDoing;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) testWater = !testWater;
        if (testWater && !isDoing)
            StartCoroutine(WaterDropTest());
    }

    private IEnumerator WaterDropTest()
    {
        isDoing = true;
        for (int i = 0; i < waterDrops.Length; i++)
        {  
            waterDrops[i].SetActive(true);
            StartCoroutine(HideGameObject(waterDrops[i]));
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
        yield return new WaitForSeconds(1);
        isDoing = false;
        //Debug.Log("done");
    }

    private IEnumerator HideGameObject(GameObject water)
    {
        yield return new WaitForSeconds(waterDropTimer);
        water.SetActive(false);
    }
    
}
