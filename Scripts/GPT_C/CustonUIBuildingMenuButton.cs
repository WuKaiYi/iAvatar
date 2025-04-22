
using UnityEngine;
using UnityEngine.UI;

using EasyBuildSystem.Features.Runtime.Buildings.Part;
using EasyBuildSystem.Features.Runtime.Buildings.Placer;
using EasyBuildSystem.Features.Runtime.Buildings.Manager;
using EasyBuildSystem.Packages.Addons.BuildingMenu;

public class CustonUIBuildingMenuButton : MonoBehaviour
{
    [SerializeField] RawImage m_UIBuildingThumbnail;
    [SerializeField] Text m_UIBuildingText;

    [SerializeField] Button m_UIBuildingButton;
    public Button UIBuildingButton { get { return m_UIBuildingButton; } }

    public void SetSlot(CustonUIBuildingMenu.CustonBuildingMenuCategory.Item item)
    {
        if (item.BuildingPart == null)
        {
            return;
        }

        m_UIBuildingThumbnail.texture = item.BuildingPart.GetGeneralSettings.Thumbnail;
        m_UIBuildingText.text = item.name;
    }
}
