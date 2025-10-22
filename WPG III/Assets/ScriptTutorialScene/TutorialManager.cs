using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text dialogueText;
    public TMP_Text objectiveText;
    //public GameObject nextButton;

    [Header("Scene References")]
    public GameObject noodlePack;
    public GameObject pot;
    public GameObject buInah;
    public GameObject ghost;
    public GameObject sesajen;
    public GameObject trashBin;
    public BuInahDialogue buInahDialogue;
    public BuInahMove buInahMove; // Tambahan: untuk gerakan Bu Inah keluar

    private int stepIndex = 0;
    private bool waitingForPlayer = false;

    void Start()
    {
        StartCoroutine(StartTutorial());
    }

    IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(1f);
        NextStep();
    }

    // -------------------------------
    // STEP PROGRESSION LOGIC
    // -------------------------------
    public void NextStep()
    {
        stepIndex++;
        StopAllCoroutines();
        StartCoroutine(HandleStep(stepIndex));
    }

    IEnumerator HandleStep(int step)
    {
        switch (step)
        {
            // STEP 1 - Buka mie dan masak
            case 1:
                yield return StartCoroutine(buInahDialogue.Speak("Bu Inah", "Oke Minto, sekarang ibu tak ngajarin kamu cara masak mienya."));
                yield return StartCoroutine(buInahDialogue.Speak("Bu Inah", "Pertama kamu buka bungkus mienya terus masukin ke panci."));
                noodlePack.SetActive(true);
                objectiveText.text = "Buka bungkus mie dan masukkan ke panci.";
                waitingForPlayer = true;
                break;

            // STEP 2 - Tunggu mie matang
            case 2:
                yield return StartCoroutine(buInahDialogue.Speak("Bu Inah", "Bagus, sekarang kita tunggu beberapa saat."));
                objectiveText.text = "Tunggu mie matang...";
                waitingForPlayer = true;
                break;

            // STEP 3 - Kasih ke Bu Inah
            case 3:
                yield return StartCoroutine(buInahDialogue.Speak("Bu Inah", "Mie-nya sudah matang? Ambil dan kasih ke ibu."));

                objectiveText.gameObject.SetActive(true);
                objectiveText.text = "Ambil mie matang lalu serahkan ke Bu Inah.";
                waitingForPlayer = true;
                break;

            // STEP 4 - Setan muncul
            case 4:
                yield return StartCoroutine(buInahDialogue.Speak ("Bu Inah", "Hmmm enak, sip."));

                // Spawn hantu lewat GhostSpawner
                var ghostSpawner = FindObjectOfType<TutorialGhostSpawner>();
                if (ghostSpawner != null)
                    ghostSpawner.SpawnGhost();

                yield return StartCoroutine(buInahDialogue.Speak("Minto", "BU?! Itu apaan?"));
                yield return StartCoroutine(buInahDialogue.Speak("Bu Inah", "Ah elah biasa itu mah."));
                yield return StartCoroutine(buInahDialogue.Speak("Bu Inah", "Kalau setan muncul berarti ada yang ngirim sajen sekitar sini."));
                yield return StartCoroutine(buInahDialogue.Speak("Bu Inah", "Kalau setan muncul, pelanggan takut dan gak mau dateng."));  
                yield return StartCoroutine(buInahDialogue.Speak("Bu Inah", "Jadi usahain kamu cari sajennya terus buang ke tong sampah sebelah warung."));

                objectiveText.gameObject.SetActive(true);
                objectiveText.text = "Cari sesajen dan buang ke tong sampah. Hindari tertangkap setan!";
                waitingForPlayer = true;
                break;

            // STEP 5 - Sesajen dibuang, setan hilang
            case 5:
                ghost.SetActive(false);
                yield return StartCoroutine(buInahDialogue.Speak("Bu Inah", "Sip kerja bagus le, intinya begitu."));
                yield return StartCoroutine(buInahDialogue.Speak("Bu Inah", "Layani pelanggan, masak mie, kalau setan muncul maka pelanggan gak akan datang."));
                yield return StartCoroutine(buInahDialogue.Speak("Bu Inah", "Cari sesajen lalu buang ke tong sampah agar setan pergi dan pelanggan datang kembali."));    
                yield return StartCoroutine(buInahDialogue.Speak("Bu Inah", "Emang, awalnya ibu sanggup aja, cuman makin lama ibu makin capek."));
                yield return StartCoroutine(buInahDialogue.Speak("Bu Inah", "Yaudah ibu mau pergi dulu, kamu ati-ati ya!"));

                objectiveText.gameObject.SetActive(true);
                objectiveText.text = "Bu Inah pergi. Bersiaplah melayani pelanggan.";

                yield return new WaitForSeconds(1.5f);

                // Bu Inah pergi
                if (buInahMove != null)
                {
                    buInahMove.StartLeaving();
                }
                else
                {
                    Debug.LogWarning("BuInahMove belum di-assign di TutorialManager!");
                }
                break;

            // STEP 6 - Mulai gameplay utama
            /*case 6:
                objectiveText.text = "Layani 10 pelanggan.";
                if (customerSpawner) customerSpawner.SetActive(true);
                yield return Speak("Game dimulai. Layani pelanggan sebanyak mungkin!");
                nextButton.SetActive(true);
                break;*/
        }
    }

    public void HideObjective()
    {
        if (objectiveText != null)
            objectiveText.gameObject.SetActive(false);
    }

    // -------------------------------
    // Pemanggil dari event mekanik lain
    // -------------------------------
    public void OnPlayerAction(string actionName)
    {
        if (!waitingForPlayer) return;

        switch (stepIndex)
        {
            case 1:
                if (actionName == "mie_dimasak")
                {
                    waitingForPlayer = false;
                    NextStep();
                }
                break;

            case 2:
                if (actionName == "mie_matang")
                {
                    waitingForPlayer = false;

                    HideObjective();

                    NextStep();
                }
                break;

            case 3:
                if (actionName == "mie_diserahkan")
                {
                    waitingForPlayer = false;

                    HideObjective();

                    NextStep();
                }
                break;

            case 4:
                if (actionName == "sesajen_dibuang")
                {
                    waitingForPlayer = false;

                    HideObjective();

                    NextStep();
                }
                break;
        }
    }

    // -------------------------------
    // Fungsi dialog Bu Inah
    // -------------------------------
    IEnumerator Speak(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(1f);
    }

    // -------------------------------
    // Tombol Lanjut (opsional)
    // -------------------------------
    /*public void OnNextButton()
    {
        SceneManager.LoadScene("GameScene");
    }*/
}
