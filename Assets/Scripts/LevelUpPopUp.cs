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
        List<SkillTable> selectedSkillTables = new List<SkillTable>();
        List<SkillEnchantTable> selectedSkillEnchantTables = new List<SkillEnchantTable>();
        List<SelectStatTable> selectedStatTable = new List<SelectStatTable>();
        
        // 스킬이 없으면 일단 무조건 스킬을 뽑아야 함
        if (SkillManager.instance.playerSkills.Count == 0)
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
                    SkillManager.instance.playerSkills.Add(e.name, new CharacterSkill(e, new List<SkillEnchantTable>()));
                    selectedSkillTables.Add(e);
                }); // 전투 공식 이후 구현
            }
        }
        
        // 스킬이 하나라도 있으면 스킬 or 스텟 업그레이드가 뜸
        else
        {   
            for (int i = 0; i < 3; i++)
            {
                // 스킬이 3개면 더이상 스킬이 안뜸
                if (SkillManager.instance.playerSkills.Count == 3)
                {
                    int totalCount = 0;
                    totalCount += Datas.GameData.DTSkillEnchantData.Count;
                    totalCount += Datas.GameData.DTSelectStatData.Count;
                    
                    int value = Random.Range(0, totalCount);
                    
                    // 플레이어 스킬 카운트보다 작으면
                    if (value < Datas.GameData.DTSkillEnchantData.Count)
                    {
                        var playerSkill = SkillManager.instance.playerSkills.ToList()
                            [Random.Range(0, SkillManager.instance.playerSkills.Count)].Value;

                        var list = Datas.GameData.DTSkillEnchantData.ToList();
                        int randomIndex = Random.Range(0, list.Count);
                        var pick = list[randomIndex];

                        int count = 0;
                        // 다시 뽑는다
                        while (playerSkill.SkillEnchantTables.Count(item => item.index == pick.Value.index) >=
                               pick.Value.maxCnt && selectedSkillEnchantTables.Contains(pick.Value))
                        {
                            pick = Datas.GameData.DTSkillEnchantData.ElementAt(Random.Range(0, Datas.GameData.DTSkillEnchantData.Count));

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
                        upgradePrefab.upgradeButton.onClick.AddListener(() => 
                        {
                            SkillManager.instance.playerSkills.ElementAt(Random.Range(0, SkillManager.instance.playerSkills.Count)).
                                Value.SkillEnchantTables.Add(pick.Value);
                            Destroy(this.gameObject);
                        }); // 전투 공식 이후 구현
                        selectedSkillEnchantTables.Add(pick.Value);
                        // 아이콘 배치 필요
                    }

                    else
                    {
                        var list = Datas.GameData.DTSelectStatData.ToList();
                        int randomIndex = Random.Range(0, list.Count);
                        var pick = list[randomIndex];
                        
                        var upgradeHistory = SkillManager.instance.playerUpgradeHistory;

                        int count = 0;
                        while (selectedStatTable.Contains(pick.Value))
                        {
                            pick = list[Random.Range(0, list.Count)];
                            count++;
                            if (count > 100)
                            {
                                break;
                            }
                        }
                        
                        //예외처리 필요 캐릭터가 업그레이드를 풀까지 돌렸을 때 예외처리
                        upgradeHistory.SelectStatTable.Add(pick.Value);
                        
                        var upgradePrefab = Instantiate(upgradeContentsPrefab, upgradeContentsParent).GetComponent<UpgradeContentsUI>();
                        upgradePrefab.upgradeIcon.sprite = Resources.Load<Sprite>(pick.Value.iconPath);
                        
                        upgradePrefab.upgradeTitleText.text = pick.Value.name;
                        upgradePrefab.upgradeDescriptionText.text = pick.Value.description;
                        upgradePrefab.upgradeButton.onClick.AddListener(() => { Destroy(this.gameObject);}); // 전투 공식 이후 구현
                        selectedStatTable.Add(pick.Value);
                    }
                }
                
                // 스킬이 1개 또는 2개면 스킬 또는 스탯 업그레이드가 뜸
                else if (SkillManager.instance.playerSkills.Count == 1 
                         || SkillManager.instance.playerSkills.Count == 2)
                {
                    int totalCount = 0;
                    totalCount += Datas.GameData.DTSkillEnchantData.Count;
                    totalCount += Datas.GameData.DTSelectStatData.Count;
                    totalCount += Datas.GameData.DTSkillData.Count;
                    
                    int value = Random.Range(0, totalCount);
                    
                    if (value < Datas.GameData.DTSkillEnchantData.Count)
                    {
                        Debug.Log("1");
                        var playerSkill = SkillManager.instance.playerSkills.ToList()
                            [Random.Range(0, SkillManager.instance.playerSkills.Count)].Value;

                        var list = Datas.GameData.DTSkillEnchantData.ToList();
                        int randomIndex = Random.Range(0, list.Count);
                        var pick = list[randomIndex];
                        
                        // 다시 뽑는다
                        int count = 0;
                        while (playerSkill.SkillEnchantTables.Count(item => item.index == pick.Value.index) >=
                               pick.Value.maxCnt && selectedSkillEnchantTables.Contains(pick.Value))
                        {
                            pick = Datas.GameData.DTSkillEnchantData.ElementAt(Random.Range(0, Datas.GameData.DTSkillEnchantData.Count));

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
                        upgradePrefab.upgradeButton.onClick.AddListener(() => 
                        {
                            SkillManager.instance.playerSkills.ElementAt(Random.Range(0, SkillManager.instance.playerSkills.Count)).
                                Value.SkillEnchantTables.Add(pick.Value);
                            Destroy(this.gameObject);
                        }); // 전투 공식 이후 구현
                        selectedSkillEnchantTables.Add(pick.Value);

                        // 아이콘 배치 필요
                    }

                    else
                    {
                        if (value - Datas.GameData.DTSkillEnchantData.Count < Datas.GameData.DTSelectStatData.Count)
                        {
                            Debug.Log("2");
                            var list = Datas.GameData.DTSelectStatData.ToList();
                            int randomIndex = Random.Range(0, list.Count);
                            var pick = list[randomIndex];
                        
                            int count = 0;
                            while (selectedStatTable.Contains(pick.Value))
                            {
                                pick = list[Random.Range(0, list.Count)];
                                count++;
                                if (count > 100)
                                {
                                    break;
                                }
                            }
                            
                            var upgradeHistory = SkillManager.instance.playerUpgradeHistory;
                            
                            //예외처리 필요 캐릭터가 업그레이드를 풀까지 돌렸을 때 예외처리
                            upgradeHistory.SelectStatTable.Add(pick.Value);
                        
                            var upgradePrefab = Instantiate(upgradeContentsPrefab, upgradeContentsParent).GetComponent<UpgradeContentsUI>();
                            upgradePrefab.upgradeIcon.sprite = Resources.Load<Sprite>(pick.Value.iconPath);
                        
                            upgradePrefab.upgradeTitleText.text = pick.Value.name;
                            upgradePrefab.upgradeDescriptionText.text = pick.Value.description;
                            upgradePrefab.upgradeButton.onClick.AddListener(() => { Destroy(this.gameObject);}); // 전투 공식 이후 구현
                            selectedStatTable.Add(pick.Value);
                        }
                        
                        // 스킬에서 뽑기
                        else
                        {
                            var list = Datas.GameData.DTSkillData.ToList();
                            int randomIndex = Random.Range(0, list.Count);
                            var pick = list[randomIndex];
                            bool isUnique = false;
                            //예외처리 필요 캐릭터가 업그레이드를 풀까지 돌렸을 때 예외처리
                            var playerSkills = SkillManager.instance.playerSkills.ToList();

                            int count = 0;

                            // TODO 현재 스킬이 3개 뿐이라.. 스킬이 2개인데, 하나를 뽑은 상태에서 또 여기 빠질 경우  while문을 빠져나갈 수가 없음.
                            // 
                            while (!isUnique)
                            {
                                pick = list[Random.Range(0, list.Count)];
                                isUnique = playerSkills.All(ps => ps.Value.SkillTable.index != pick.Value.index) && !selectedSkillTables.Contains(pick.Value);

                                count++;
                                if (count > 100)
                                {
                                    return;
                                }
                            }
                            
                            
                            var upgradePrefab = Instantiate(upgradeContentsPrefab, upgradeContentsParent).GetComponent<UpgradeContentsUI>();
                            upgradePrefab.upgradeIcon.sprite = Resources.Load<Sprite>(pick.Value.icon);
                        
                            upgradePrefab.upgradeTitleText.text = pick.Value.name;
                            upgradePrefab.upgradeDescriptionText.text = pick.Value.description;
                            upgradePrefab.upgradeButton.onClick.AddListener(() =>
                            {
                                SkillManager.instance.playerSkills.Add(pick.Value.name, new CharacterSkill(pick.Value, new List<SkillEnchantTable>()));
                                Destroy(this.gameObject);
                            }); // 전투 공식 이후 구현
                            selectedSkillTables.Add(pick.Value);
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
