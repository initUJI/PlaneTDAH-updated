using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronesSpawns : MonoBehaviour
{
    public List<string[]> posicionamientosSpawn = new List<string[]>();

    void Awake()
    {
        //Aqui se definen los diferentes patrones a seguir
        string[] patron1 = {"4_0", "4_1", "4_2", "4_3", "4_4"};
        posicionamientosSpawn.Add(patron1);

        string[] patron2 = { "3_4", "3_3", "3_2", "3_1", "3_0" };
        posicionamientosSpawn.Add(patron2);

        string[] patron3 = { "3_4", "3_3", "3_2", "3_1", "3_0", "3_1", "3_2", "3_3", "3_2" };
        posicionamientosSpawn.Add(patron3);

        string[] patron4 = { "1_2", "2_2", "3_2", "4_2", "5_2", "4_2", "3_2", "2_2", "1_2" };
        posicionamientosSpawn.Add(patron4);

        string[] patron5 = { "3_4", "4_4", "5_3", "5_2", "4_1", "3_1", "2_2", "2_3", "3_4" };
        posicionamientosSpawn.Add(patron5);

        string[] patron6 = { "4_4", "3_4", "2_3", "2_2", "3_1", "4_1", "4_2", "4_3", "4_4" };
        posicionamientosSpawn.Add(patron6);

        string[] patron7 = {"1_3", "2_3", "3_3", "4_3", "5_3", "", "5_1", "4_1", "3_1", "2_1", "1_1" };
        posicionamientosSpawn.Add(patron7);

        string[] patron8 = { "1_3", "1_2", "1_1", "2_1", "3_1", "4_1", "5_1", "5_2", "5_3" };
        posicionamientosSpawn.Add(patron8);
    }
}
