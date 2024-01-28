using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LevelUpPopUp : UIBase
{
    public RectTransform upgradeContentsParent;
    public GameObject upgradeContentsPrefab;
    
    public List<Image> equipSkillImages;

    protected override void Initialize()
    {
        base.Initialize();

        //upgradeContentsPrefab.GetComponent<UpgradeContentsUI>();
    }

    protected override void Start()
    {

        Init();
        GameManager.Instance.PauseGame();
    }

    private static System.Random rng = new System.Random();
    public void UpgradeLogic()
    {
        // 스킬이 없으면 일단 무조건 스킬을 뽑아야 함
        if (GameManager.Instance.PlayerMove.playerSkills.Count == 0)
        {
            var selectedSkills = Datas.GameData.DTSkillData.Values.OrderBy(x => rng.Next())
                .Take(3)
                .ToList();
            
            foreach (var e in selectedSkills)
            {
                var upgradePrefab = Instantiate(upgradeContentsPrefab, upgradeContentsParent).GetComponent<UpgradeContentsUI>();
                upgradePrefab.upgradeIcon.sprite = Resources.Load<Sprite>(e.icon);
                        
                upgradePrefab.upgradeTitleText.text = e.name;
                upgradePrefab.upgradeDescriptionText.text = e.description;
                upgradePrefab.upgradeButton.onClick.AddListener(() =>
                {
                    Destroy(this.gameObject);
                    GameManager.Instance.PlayerMove.playerSkills.Add(e.name, new CharacterSkill(e, new List<SkillEnchantTable>()));
                }); // 전투 공식 이후 구현
            }
        }
        
        // 스킬이 하나라도 있으면 스킬 or 스텟 업그레이드가 뜸
        else
        {
            // 스킬이 3개면 더이상 스킬이 안뜸
            if (GameManager.Instance.PlayerMove.playerSkills.Count == 3)
            {
                int totalCount = 0;
                totalCount += Datas.GameData.DTSkillEnchantData.Count;
                totalCount += Datas.GameData.DTSelectStatData.Count;
                for (int i = 0; i < 3; i++)
                {
                    int value = Random.Range(0, totalCount);
                    
                    // 플레이어 스킬 카운트보다 작으면
                    if (value < Datas.GameData.DTSkillEnchantData.Count)
                    {
                        var playerSkill = GameManager.Instance.PlayerMove.playerSkills.ToList()
                            [Random.Range(0, GameManager.Instance.PlayerMove.playerSkills.Count)].Value;

                        var list = Datas.GameData.DTSkillEnchantData.ToList();
                        int randomIndex = Random.Range(0, list.Count);
                        var pick = list[randomIndex];
                        
                        // 다시 뽑는다
                        while (playerSkill.SkillEnchantTables.Count(item => item.index == pick.Value.index) >=
                               pick.Value.maxCnt)
                        {
                            pick = Datas.GameData.DTSkillEnchantData.ElementAt(Random.Range(0, Datas.GameData.DTSkillEnchantData.Count));
                        }
                        
                        var upgradePrefab = Instantiate(upgradeContentsPrefab, upgradeContentsParent).GetComponent<UpgradeContentsUI>();
                        upgradePrefab.upgradeIcon.sprite = Resources.Load<Sprite>(pick.Value.icon);
                        
                        upgradePrefab.upgradeTitleText.text = pick.Value.name;
                        upgradePrefab.upgradeDescriptionText.text = pick.Value.description;
                        upgradePrefab.upgradeButton.onClick.AddListener(() => { Destroy(this.gameObject);}); // 전투 공식 이후 구현
                        
                        // 아이콘 배치 필요
                    }

                    else
                    {
                        var list = Datas.GameData.DTSelectStatData.ToList();
                        int randomIndex = Random.Range(0, list.Count);
                        var pick = list[randomIndex];
                        
                        var upgradeHistory = GameManager.Instance.PlayerMove.playerUpgradeHistory;
                            
                        //예외처리 필요 캐릭터가 업그레이드를 풀까지 돌렸을 때 예외처리
                        upgradeHistory.SelectStatTable.Add(pick.Value);
                        
                        var upgradePrefab = Instantiate(upgradeContentsPrefab, upgradeContentsParent).GetComponent<UpgradeContentsUI>();
                        upgradePrefab.upgradeIcon.sprite = Resources.Load<Sprite>(pick.Value.iconPath);
                        
                        upgradePrefab.upgradeTitleText.text = pick.Value.name;
                        upgradePrefab.upgradeDescriptionText.text = pick.Value.description;
                        upgradePrefab.upgradeButton.onClick.AddListener(() => { Destroy(this.gameObject);}); // 전투 공식 이후 구현
                    }
                }
                
            }
            
            // 스킬이 1개 또는 2개면 스킬 또는 스탯 업그레이드가 뜸
            else if (GameManager.Instance.PlayerMove.playerSkills.Count == 1 
                     || GameManager.Instance.PlayerMove.playerSkills.Count == 2)
            {
                int totalCount = 0;
                totalCount += Datas.GameData.DTSkillEnchantData.Count;
                totalCount += Datas.GameData.DTSelectStatData.Count;
                totalCount += Datas.GameData.DTSkillData.Count;
                
                for (int i = 0; i < 3; i++)
                {
                    int value = Random.Range(0, totalCount);
                    
                    // 플레이어 스킬 카운트보다 작으면
                    if (value < Datas.GameData.DTSkillEnchantData.Count)
                    {
                        var playerSkill = GameManager.Instance.PlayerMove.playerSkills.ToList()
                            [Random.Range(0, GameManager.Instance.PlayerMove.playerSkills.Count)].Value;

                        var list = Datas.GameData.DTSkillEnchantData.ToList();
                        int randomIndex = Random.Range(0, list.Count);
                        var pick = list[randomIndex];
                        
                        // 다시 뽑는다
                        while (playerSkill.SkillEnchantTables.Count(item => item.index == pick.Value.index) >=
                               pick.Value.maxCnt)
                        {
                            pick = Datas.GameData.DTSkillEnchantData.ElementAt(Random.Range(0, Datas.GameData.DTSkillEnchantData.Count));
                        }
                        
                        var upgradePrefab = Instantiate(upgradeContentsPrefab, upgradeContentsParent).GetComponent<UpgradeContentsUI>();
                        upgradePrefab.upgradeIcon.sprite = Resources.Load<Sprite>(pick.Value.icon);
                        
                        upgradePrefab.upgradeTitleText.text = pick.Value.name;
                        upgradePrefab.upgradeDescriptionText.text = pick.Value.description;
                        upgradePrefab.upgradeButton.onClick.AddListener(() => { Destroy(this.gameObject);}); // 전투 공식 이후 구현
                        
                        // 아이콘 배치 필요
                    }

                    else
                    {
                        // 스킬 업그레이드에서 뽑기
                        if (value - Datas.GameData.DTSkillEnchantData.Count < Datas.GameData.DTSelectStatData.Count)
                        {
                            var list = Datas.GameData.DTSelectStatData.ToList();
                            int randomIndex = Random.Range(0, list.Count);
                            var pick = list[randomIndex];
                        
                            var upgradeHistory = GameManager.Instance.PlayerMove.playerUpgradeHistory;
                            
                            //예외처리 필요 캐릭터가 업그레이드를 풀까지 돌렸을 때 예외처리
                            upgradeHistory.SelectStatTable.Add(pick.Value);
                        
                            var upgradePrefab = Instantiate(upgradeContentsPrefab, upgradeContentsParent).GetComponent<UpgradeContentsUI>();
                            upgradePrefab.upgradeIcon.sprite = Resources.Load<Sprite>(pick.Value.iconPath);
                        
                            upgradePrefab.upgradeTitleText.text = pick.Value.name;
                            upgradePrefab.upgradeDescriptionText.text = pick.Value.description;
                            upgradePrefab.upgradeButton.onClick.AddListener(() => { Destroy(this.gameObject);}); // 전투 공식 이후 구현
                        }
                        
                        // 스킬에서 뽑기
                        else
                        {
                            var list = Datas.GameData.DTSkillData.ToList();
                            int randomIndex = Random.Range(0, list.Count);
                            var pick = list[randomIndex];
                            bool isUnique = false;
                            //예외처리 필요 캐릭터가 업그레이드를 풀까지 돌렸을 때 예외처리
                            var playerSkills = GameManager.Instance.PlayerMove.playerSkills.ToList();

                            int count = 0;
                            while (!isUnique)
                            {
                                randomIndex = Random.Range(0, list.Count);
                                pick = list[randomIndex];
                                isUnique = playerSkills.All(ps => ps.Value.SkillTable != pick.Value);

                                count++;
                                if (count > 100)
                                {
                                    break;
                                }
                            }
                            
                            var upgradePrefab = Instantiate(upgradeContentsPrefab, upgradeContentsParent).GetComponent<UpgradeContentsUI>();
                            upgradePrefab.upgradeIcon.sprite = Resources.Load<Sprite>(pick.Value.icon);
                        
                            upgradePrefab.upgradeTitleText.text = pick.Value.name;
                            upgradePrefab.upgradeDescriptionText.text = pick.Value.description;
                            upgradePrefab.upgradeButton.onClick.AddListener(() => { Destroy(this.gameObject);}); // 전투 공식 이후 구현
                        }
                    }
                    
                }
            }
        }
    }
    
    public void Init()
    {

        UpgradeLogic();
        
    }
    
    private void OnDestroy()
    {
        base.OnDisable();
        GameManager.Instance.ResumeGame();
    }
}
