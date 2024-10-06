using UnityEngine;
using UnityEngine.UI;

public class CharacterShopManager : MonoBehaviour
{
    public Button[] characterButtons; // Danh sách các button cho các nhân vật
    private Button selectedButton; // Button hiện tại được chọn

    public float selectedAlpha = 0.5f; // Độ mờ của ảnh khi được chọn
    public float defaultAlpha = 1f;    // Độ trong suốt mặc định

    private void Start()
    {
        foreach (Button button in characterButtons)
        {
            // Thêm sự kiện cho mỗi button
            button.onClick.AddListener(() => OnCharacterSelected(button));
        }
    }

    private void OnCharacterSelected(Button clickedButton)
    {
        // Nếu có button nào được chọn trước đó, trả về alpha mặc định
        if (selectedButton != null)
        {
            Image previousImage = selectedButton.GetComponentInChildren<Image>();
            SetImageAlpha(previousImage, defaultAlpha);
        }

        // Cập nhật button mới được chọn
        selectedButton = clickedButton;

        // Lấy Image con của button và làm mờ ảnh của nó
        Image selectedImage = clickedButton.GetComponentInChildren<Image>();
        Debug.Log("Image selected", selectedImage);
        SetImageAlpha(selectedImage, selectedAlpha);

        Debug.Log(clickedButton.name + " selected");
    }

    private void SetImageAlpha(Image image, float alphaValue)
    {
        // Thay đổi thuộc tính alpha của ảnh
        Color color = image.color;
        color.a = alphaValue; // Điều chỉnh giá trị alpha
        image.color = color;
    }
}
