using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public struct BlockStruct
{
    public Sprite sprite;
    public GameObject model;
    public Material mat;
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Random Objects")]
    [SerializeField] private GameObject[] blocks;
    [SerializeField] private Material[] blockColors;
    [SerializeField] private Sprite[] blockSprites;

    [Header("Levels")]
    [SerializeField] private GameObject[] levelPrefabs;

    [Header("Canvas")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject ustPanel;
    [SerializeField] private GameObject restartPanel;
    [SerializeField] private GameObject nextLevelPanel;
    [SerializeField] private GameObject settingsPanel;
    public TextMeshProUGUI percentile;
    public TextMeshProUGUI levelText;

    [Header("Images")]
    [SerializeField] private Image[] images = new Image[3];

    [SerializeField] private Camera cam;

    public Transform createPos;

    public Vector3 camFirstPos;

    public bool isFell;
    public bool cameraBack;

    public int objectCount = 4;
    public int calculate;
    private int myLevel = 0;

    public List<GameObject> collectBlocks = new List<GameObject>();

    public List<BlockStruct> blockList = new List<BlockStruct>();
    Dictionary<Sprite, List<GameObject>> spriteObject = new Dictionary<Sprite, List<GameObject>>();

    private GameObject lastSpawned;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ustPanel.SetActive(true);
        calculate = 0;
        percentile.text = "% " + calculate.ToString();

        levelText.text = "LEVEL " + (myLevel + 1);
        cameraBack = false;
        camFirstPos = cam.transform.position;

        Instantiate(levelPrefabs[myLevel], new Vector3(0, 0, 0.01f), Quaternion.identity);
        CreateBlockImageDictionary();
        BlockListCreator();
        PrepareObjects();


        Time.timeScale = 0;
        isFell = false;

    }

    private void Update()
    {
        if (cameraBack == true)
        {
            cam.transform.position = camFirstPos;
            cameraBack = false;
        }
    }
    private void CreateBlockImageDictionary()
    {
        int index = 0;
        for (int i = 0; i < 5; i++)
        {
            List<GameObject> bList = new List<GameObject>();

            for (int a = 0; a < 2; a++)
            {
                bList.Add(blocks[index]);
                index++;
            }
            spriteObject.Add(blockSprites[i], bList);
        }
        spriteObject[blockSprites[2]].Add(blocks[10]);
        spriteObject[blockSprites[2]].Add(blocks[11]);
    }
    private void BlockListCreator()
    {
        for (int i = 0; i < objectCount; i++)
        {
            BlockStruct bs = new BlockStruct();
            bs.sprite = blockSprites[Random.Range(0, blockSprites.Length)];

            Material mat = blockColors[Random.Range(0, blockColors.Length)];

            bs.mat = mat;
            List<GameObject> blockObjects = spriteObject[bs.sprite];

            bs.model = blockObjects[Random.Range(0, blockObjects.Count)];
            bs.model.GetComponent<Renderer>().material = mat;

            blockList.Add(bs);
        }

    }
    public void SpawnBlock()
    {
        lastSpawned = Instantiate(blockList[0].model, createPos.position, Quaternion.identity);
        lastSpawned.GetComponent<Renderer>().material = blockList[0].mat;
        collectBlocks.Add(lastSpawned);
    }

    public void PrepareObjects()
    {
        if (blockList.Count < 3) BlockListCreator();

        for (int i = 0; i < images.Length; i++)
        {
            BlockStruct b = blockList[i];
            Image img = images[i];

            img.sprite = b.sprite;
            img.color = b.mat.color;
        }
    }



    public void PlayButton()
    {
        Time.timeScale = 1;
        startPanel.SetActive(false);
        Blocks.canPlay = true;

        SpawnBlock();

    }
    public void RestartButton()
    {
        Time.timeScale = 1;
        Blocks.canPlay = true;
        WriteText();

        DestroyObjects();
        GameObject.FindGameObjectWithTag("Level").SetActive(false);

        //        Destroy(levelPrefabs[myLevel].gameObject);
        if (myLevel == 0)
        {
            Instantiate(levelPrefabs[myLevel], new Vector3(0, 0, 0.01f), Quaternion.identity);
            SpawnBlock();

        }
        else
        {
            Instantiate(levelPrefabs[myLevel], new Vector3(0, 0, 0.01f), Quaternion.identity);
            SpawnBlock();
        }

        cam.transform.position = camFirstPos;

        restartPanel.SetActive(false);


    }
    public void OpenRestart()
    {
        restartPanel.SetActive(true);
        Time.timeScale = 0;
        Blocks.canPlay = false;

    }
    public void NextLevel()
    {
        GameObject.FindGameObjectWithTag("Level").SetActive(false);
        objectCount += 2;
        myLevel += 1;
        levelText.text = "LEVEL " + (myLevel + 1);

        cameraBack = true;

        WriteText();
        DestroyObjects();
        if (myLevel >= 3)
        {
            myLevel = 0;
        }
        Instantiate(levelPrefabs[myLevel], new Vector3(0, 0, 0.01f), Quaternion.identity);

        Blocks.blockFallSpeed += 0.2f;
        Time.timeScale = 1;
        Blocks.canPlay = true;

        SpawnBlock();

        nextLevelPanel.SetActive(false);

    }

    public void OpenNextLevelPanel()
    {
        nextLevelPanel.SetActive(true);
        Time.timeScale = 0;
        Blocks.canPlay = false;
    }
    void WriteText()
    {
        calculate = 0;
        percentile.text = "% " + calculate.ToString();
    }

    private void DestroyObjects()
    {
        for (int i = 0; i < collectBlocks.Count; i++)
        {
            Destroy(collectBlocks[i]);
        }
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        if (startPanel.activeSelf)
        {
            startPanel.SetActive(false);
        }
        else if (restartPanel.activeSelf)
        {
            restartPanel.SetActive(false);
        }
        Time.timeScale = 0;
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        startPanel.SetActive(true);
        Time.timeScale = 1;

    }
}
