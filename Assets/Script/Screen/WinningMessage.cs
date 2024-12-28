using System.Collections.Generic;
using UnityEngine;

public class WinningMessage : MonoBehaviour
{
    private static List<string> winningMessages = new List<string>
    {
        "Gacorr! Sekarang kamu jadi Crazy Rich kerajaan Tuyul!",
        "Selamat! Kamu tidak menjadi botak dan temen tuyul",
        "Kamu menang! Bahkan tuyul sekarang jadi fans kamu.",
        "Pertarungan selesai, sekarang tuyul akan menuruti permintaanmu!",
    };

    public static string GetRandomWinningMessage()
    {
        return winningMessages[Random.Range(0, winningMessages.Count)];
    }
}
