using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("Componentes de UI")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject ctaButton; // El botón web completo
    [SerializeField] private GameObject dialoguePanel;

    [Header("Configuración de Diálogo")]
    [SerializeField] private float typingSpeed = 0.03f; // Velocidad del efecto typewriter

    // El arreglo con los 3 diálogos que redactamos
    private string[] dialogueLines = new string[]
    {
        "¡Bienvenido a L’Umbria! Aquí ofrecemos una experiencia fresca y consciente, con un menú diverso que combina alta cocina internacional e ingredientes sustentables.",
        "Nuestra cocina es un laboratorio vivo operado por los talentosos estudiantes de Ciencias Culinarias, quienes transforman productos locales en platillos excepcionales.",
        "Te invitamos a disfrutar de sabores con un verdadero propósito. Si quieres explorar más, puedes usar el botón de abajo para revisar nuestro sitio web y el menú completo."
    };

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool dialogueEnded = false;

    void Start()
    {
        StartDialogue();
    }

    void Update()
    {
        // Detecta si el jugador presiona Espacio, Enter o hace clic izquierdo para avanzar
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            if (dialogueEnded) return;

            if (isTyping)
            {
                // Si el texto se está escribiendo y el usuario presiona el botón, muestra todo el cuadro de inmediato
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLineIndex];
                isTyping = false;
            }
            else
            {
                // Si ya terminó de escribirse la línea actual, avanza a la siguiente
                NextLine();
            }
        }
    }

    void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        ctaButton.SetActive(false); // Asegura que el CTA esté oculto al inicio
        currentLineIndex = 0;
        dialogueEnded = false;
        StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        dialogueText.text = ""; // Limpia el cuadro anterior

        // Va agregando letra por letra
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void NextLine()
    {
        currentLineIndex++;

        // Si aún hay líneas disponibles en el arreglo
        if (currentLineIndex < dialogueLines.Length)
        {
            StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
        }
        else
        {
            // Fin del diálogo: Aquí activamos la interacción final
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueEnded = true;
        isTyping = false;
        
        // Activamos el botón de CTA justo al final del tercer diálogo
        if (ctaButton != null)
        {
            ctaButton.SetActive(true);
        }
    }
}