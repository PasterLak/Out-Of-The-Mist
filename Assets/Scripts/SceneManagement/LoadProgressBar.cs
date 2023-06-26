
using UnityEngine;
using UnityEngine.UI;

public class LoadProgressBar : MonoBehaviour
{
    public Slider slider;

    private void Update()
    {
        slider.value = Loader.GetLoadingProgress();
    }
}
