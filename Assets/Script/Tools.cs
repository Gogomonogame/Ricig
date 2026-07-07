using UnityEngine;

public static class Tools
{
    public static bool ChanceCalculation(int chance)
    {
        int randomNumber = Random.Range(0, 100);
        if(randomNumber <= chance ) return true;
        else return false;
    }


}
