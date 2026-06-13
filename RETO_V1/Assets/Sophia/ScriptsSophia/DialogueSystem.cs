using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class DialogueSystem : MonoBehaviour
{
    [Header("Componentes de UI")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject ctaButton;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject finalImage; 

    [Header("Componentes de Video")]
    [SerializeField] private VideoPlayer foodVideoPlayer;

    [Header("Configuración de Diálogo")]
    [SerializeField] private float typingSpeed = 0.03f;

    
    private string[] dialogueLines = new string[]
    {
        "¡Bienvenido a L’Umbria! Aquí ofrecemos una experiencia fresca y consciente, con un menú diverso que combina alta cocina internacional e ingredientes sustentables...",
        "Nuestra cocina es un laboratorio vivo operado por los talentosos estudiantes de Ciencias Culinarias, quienes transforman productos locales en platillos excepcionales...",
        "Te invitamos a disfrutar de sabores con un verdadero propósito. Si quieres explorar más, puedes usar el botón de abajo para revisar el sitio web y el menú completo."
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
        dialoguePanel.SetActive(true);
        ctaButton.SetActive(false);
        
        
        if (finalImage != null)
        {
            finalImage.SetActive(false);
        }

        if (foodVideoPlayer != null)
        {
            foodVideoPlayer.Stop(); 
        }

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

        
        if (finalImage != null)
        {
            finalImage.SetActive(true);
        }

        
        if (foodVideoPlayer != null)
        {
            foodVideoPlayer.Play();
        }
    }
}