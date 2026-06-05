using UnityEngine;

// Enum ini ditaruh di luar class agar bisa diakses oleh semua script
public enum VarianMie
{
    MieUnguSambalMatah,
    MieGorengKeriting,
    MieGorengBiasa,
    BelumAdaIsi
}

public class MangkokData : MonoBehaviour
{
    // Variabel ini yang akan membedakan isi mangkuk
    public VarianMie isiMieSaatIni = VarianMie.BelumAdaIsi;
}