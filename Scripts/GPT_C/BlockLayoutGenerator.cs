using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BlockLayoutGenerator : MonoBehaviour
{
    public GameObject bigBlockPrefab;
    public GameObject smallBlockPrefab;
    public Slider sizeSlider;
    public Slider distanceSlider;
    public Slider SmallBlockDistance;
    public Slider smallBlockSizeSlider;

    private GameObject bigBlock;
    private List<GameObject> smallBlocks = new List<GameObject>();
    private float  smallBlockDistance;
    private float distance;
    private float smallBlockSize;

    private void Start()
    {
        sizeSlider.onValueChanged.AddListener(OnSizeSliderValueChanged);
        distanceSlider.onValueChanged.AddListener(OnDistanceSliderValueChanged);
        SmallBlockDistance.onValueChanged.AddListener(OnSmallBlockDistanceChanged);
        smallBlockSizeSlider.onValueChanged.AddListener(OnSmallBlockSizeSliderValueChanged);

        GenerateBigBlock();
        GenerateSmallBlocks();
    }

    private void GenerateBigBlock()
    {
        if (bigBlock != null)
            Destroy(bigBlock);

        float size = sizeSlider.value;
        bigBlock = Instantiate(bigBlockPrefab, transform.position, Quaternion.identity);
        bigBlock.transform.parent = this.transform ;
        bigBlock.transform.localScale = new Vector3(size, 1f, size);
        bigBlock.SetActive(true);
    }

    private void GenerateSmallBlocks()
    {
        // 删除之前生成的小方块
        foreach (GameObject block in smallBlocks)
        {
            Destroy(block);
        }
        smallBlocks.Clear();

        // 获取大区域的四个角的坐标
        float size = sizeSlider.value;
        float halfSize = size / 2f;
        Vector3 center = transform.position;
        Vector3 topLeft = center + new Vector3(-halfSize, 0f, halfSize);
        Vector3 topRight = center + new Vector3(halfSize, 0f, halfSize);
        Vector3 bottomLeft = center + new Vector3(-halfSize, 0f, -halfSize);
        Vector3 bottomRight = center + new Vector3(halfSize, 0f, -halfSize);

        // 沿着大区域的边缘生成小区域
        // 上边
        GenerateSmallBlocksOnEdge(topLeft, topRight);
        // 右边
        GenerateSmallBlocksOnEdge(topRight, bottomRight);
        // 下边
        GenerateSmallBlocksOnEdge(bottomRight, bottomLeft);
        // 左边
        GenerateSmallBlocksOnEdge(bottomLeft, topLeft);
    }

    private void GenerateSmallBlocksOnEdge(Vector3 start, Vector3 end)
    {
        // 计算边缘的长度和方向
        float length = Vector3.Distance(start, end);
        Vector3 direction = (end - start).normalized;

        // 计算小区域的数量
        int count = Mathf.FloorToInt((length) / (smallBlockSize + smallBlockDistance));

        // 计算垂直于边缘的偏移量
        Vector3 offset = Vector3.Cross(direction, Vector3.up) *(smallBlockSize/2+ distance);

        // 从起点开始，沿着边缘生成小区域
        for (int i = 0; i < count; i++)
        {
            // 计算小区域的位置
            Vector3 position = start + direction * (smallBlockDistance + smallBlockSize) * i + offset+ direction*smallBlockSize/2;

            // 实例化一个小方块
            GameObject smallBlock = Instantiate(smallBlockPrefab, position, Quaternion.identity);
            smallBlock.transform.localScale = new Vector3(smallBlockSize, 1f, smallBlockSize);
            smallBlock.SetActive(true);
            smallBlock.transform.parent = this.transform;
            // 添加到列表中
            smallBlocks.Add(smallBlock);
        }
    }


    private void OnSizeSliderValueChanged(float value)
    {
        GenerateBigBlock();
        GenerateSmallBlocks();
    }

    private void OnDistanceSliderValueChanged(float value)
    {
        distance = value;
        GenerateSmallBlocks();
    }

    private void OnSmallBlockDistanceChanged(float value)
    {
        smallBlockDistance = value;
        GenerateSmallBlocks();
    }

    private void OnSmallBlockSizeSliderValueChanged(float value)
    {
        smallBlockSize = value;
        GenerateSmallBlocks();
    }
}
