using UnityEngine;

// Enumeraci�n para los tipos de prenda
public enum TipoPrenda
{
    ParteSuperior,
    ParteInferior,
    Calzado,
    Otro
}

// Clase para representar una prenda
[System.Serializable]
public class Prenda
{
    public string nombre; // Nombre de la prenda
    public Texture imagen; // Imagen de la prenda (puedes utilizar Texture o string para la ruta de la imagen)
    public TipoPrenda tipo; // Tipo de prenda (superior, inferior, calzado, etc.)
    // Otros atributos como color, talla, estilo, etc., pueden ser agregados seg�n tus necesidades
}

// Clase para manejar una colecci�n de prendas
public class ColeccionPrendas : MonoBehaviour
{
    public Prenda[] prendas; // Array que contiene todas las prendas

    // Puedes inicializar y manejar las prendas aqu� en este script
    void Start()
    {
        // Aqu� podr�as asignar valores a las prendas, por ejemplo:
        prendas = new Prenda[]
        {
            //new Prenda { nombre = "Camisa Azul", imagen = /* Asignar la textura o ruta de la imagen */, tipo = TipoPrenda.ParteSuperior },
            //new Prenda { nombre = "Pantal�n Vaquero", imagen = /* Asignar la textura o ruta de la imagen */, tipo = TipoPrenda.ParteInferior },
            // Puedes a�adir m�s prendas seg�n sea necesario
        };
    }

    // Supongamos que llenas la lista de prendas en alg�n momento
    void LlenarPrendas()
    {
        // Llenar la lista de prendas...
    }

    // M�todo para guardar las prendas usando PlayerPrefs
    public void GuardarPrendas()
    {
        // Serializamos las prendas a JSON
        string prendasJSON = JsonUtility.ToJson(prendas);

        // Guardamos la cadena JSON en PlayerPrefs
        PlayerPrefs.SetString("PrendasGuardadas", prendasJSON);

        // Guardamos los PlayerPrefs para asegurarnos de que se almacenen
        PlayerPrefs.Save();
    }

    // M�todo para cargar las prendas desde PlayerPrefs
    public void CargarPrendas()
    {
        // Obtenemos la cadena JSON almacenada en PlayerPrefs
        string prendasJSON = PlayerPrefs.GetString("PrendasGuardadas");

        // Si hay datos almacenados
        if (!string.IsNullOrEmpty(prendasJSON))
        {
            // Deserializamos las prendas desde la cadena JSON
            prendas = JsonUtility.FromJson<Prenda[]>(prendasJSON);
        }
        else
        {
            Debug.Log("No hay datos de prendas guardados.");
        }
    }
}


