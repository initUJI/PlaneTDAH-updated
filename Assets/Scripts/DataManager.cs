using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public class DataManager : MonoBehaviour
{
    public static DataManager instancia;
    public PartidaJugada[] datosPorPartida;
    public int ultimaPuntuacion;

    // Start is called before the first frame update
    void Awake()
    {
        if (instancia == null)
        {
            DontDestroyOnLoad(gameObject);
            instancia = this;
            Cargar();
        }
        else if (instancia != this)
        {
            Destroy(gameObject);
        }
    }
    public void Guardar()
    {
        try
        {
            string nombreFichero = "PartidGuardada";
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formateador = new BinaryFormatter();
            System.IO.FileStream fichero = File.Create(Application.persistentDataPath + "/" + nombreFichero + ".dat");

            DatosDelJuego datos = new DatosDelJuego();
            datos.allExercicesData = this.datosPorPartida;
            datos.lastPuntuacion = this.ultimaPuntuacion;

            formateador.Serialize(fichero, datos);
            fichero.Close();
        }
        catch (Exception e)
        {
            print("error: " + e);
        }
    }

    public void Cargar()
    {
        string nombreFichero = "PartidGuardada";
        if (File.Exists(Application.persistentDataPath + "/" + nombreFichero + ".dat"))
        {
            BinaryFormatter formateador = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + nombreFichero + ".dat", FileMode.Open);
            DatosDelJuego data = (DatosDelJuego)formateador.Deserialize(file);
            file.Close();
            this.datosPorPartida = data.allExercicesData;
            this.ultimaPuntuacion = data.lastPuntuacion;
        }
        else
        {
            print("El fichero no existe");
        }
    }

    public int comprobarPartidasDelDia()
    {
        DateTime today = DateTime.Now.Date;
        int result = 0;

        foreach(PartidaJugada partida in datosPorPartida)
        {
            if(partida.fechaEjercicio.Date == today.Date)
            {
                result++;
            }
        }

        return result;
    }

    internal void añadirPartidaGuardada(PartidaJugada partidaActual)
    {
        PartidaJugada[] resultado = new PartidaJugada[datosPorPartida.Length + 1];
        datosPorPartida.CopyTo(resultado, 1);
        resultado[0] = partidaActual;

        datosPorPartida = resultado;
    }

    internal void sustituirPartidaGuardada(PartidaJugada partidaNueva)
    {
        datosPorPartida[0] = partidaNueva;

    }
}
[Serializable]
public class PartidaJugada
{
    public DateTime fechaEjercicio;
    public string dificultad;
    public bool juegoTerminado;
    public int aciertos, fallos;


    public PartidaJugada (string dificultad)
    {
        this.fechaEjercicio = System.DateTime.Now;
        this.dificultad = dificultad;
        this.juegoTerminado = false;
        this.aciertos = 0;
        this.fallos = 0;
    }
}

[Serializable]
public class DatosDelJuego
{
    public PartidaJugada[] allExercicesData;
    public int lastPuntuacion;
}


