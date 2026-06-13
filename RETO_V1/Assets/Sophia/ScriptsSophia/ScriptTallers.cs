using System.Collections;
using UnityEngine;
using TMPro;

public class ScriptTallers : MonoBehaviour
{
    [Header("Componentes de UI")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject ctaButton;
    [Header("Configuración")]
    [SerializeField] private float typingSpeed = 0.03f;

    
    private string[] dialogueLines = new string[]
    {
        "Te damos la bienvenida al área de talleres. Este espacio interactivo representa el componente práctico y de co-creación del Congreso Internacional ICEO...",
        "A través de dinámicas guiadas, nuestra comunidad colabora con expertos para desarrollar competencias clave en sustentabilidad y tecnología ambiental...",
        "Si quieres ver los horarios de los talleres, dale un vistazo a la agenda con el botón de abajo."
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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            if (dialogueEnded) return;

            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLineIndex];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    void StartDialogue()
    {
        ctaButton.SetActive(false); 
        currentLineIndex = 0;
        dialogueEnded = false;
        StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        dialogueText.text = "";

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

        if (currentLineIndex < dialogueLines.Length)
        {
            StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueEnded = true;
        isTyping = false;
        
        
        if (ctaButton != null)
        {
            ctaButton.SetActive(true);
        }
    }
}