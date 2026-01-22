using UnityEngine;
using TMPro;

public class UICollectible : MonoBehaviour
{
    public static UICollectible Instance;

    public TextMeshProUGUI carrotText;
    public TextMeshProUGUI cabbageText;
    public TextMeshProUGUI beetrootText;
    public TextMeshProUGUI otherText;

    void Awake()
    {
        Instance = this;
    }

    public void Refresh(int carrot, int cabbage, int beetroot, int other)
    {
        if (carrotText) carrotText.text = $"Carrot: {carrot}";
        if (cabbageText) cabbageText.text = $"Cabbage: {cabbage}";
        if (beetrootText) beetrootText.text = $"Beetroot: {beetroot}";
        if (otherText) otherText.text = $"Other Crops: {other}";
    }
}
