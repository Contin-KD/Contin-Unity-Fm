
using excellent;
using System;

namespace ConfigDefinition
{
    [ExcellentConfig]
    [Serializable]
    public class GlobalConfig
    {
        public string Name { get; set; }
        public float FloatValue { get; set; }
        public string StringValue { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class CharacterConfig
    {
        public string Name { get; set; }
        [ConfigComment(@"Creep=普通
Elite=精英
boss=头领")]
        public MonsterType MonsterType { get; set; }
        public string Model { get; set; }
        public string Animator { get; set; }
        public string ModelHQ { get; set; }
        public string AnimatorHQ { get; set; }
        public string AI { get; set; }
        public int GetHitFX { get; set; }
        public int StepSFX { get; set; }
        public int DeadSFX { get; set; }
        public int[] Skills { get; set; }
        public int[] Buffs { get; set; }
        public int AppearSFX { get; set; }
        public bool NonTarget { get; set; }
        public bool WanderIfNoTarget { get; set; }
        public float ModelScale { get; set; }
        public float Size { get; set; }
        public bool ImmuneAttackStiff { get; set; }
        public float ThinkMin { get; set; }
        public float ThinkMax { get; set; }
        public float SightRange { get; set; }
        public MainAttribute MainAttribute { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Dexterity { get; set; }
        public int Vitality { get; set; }
        public int StrGrow { get; set; }
        public int IntGrow { get; set; }
        public int DexGrow { get; set; }
        public int VitGrow { get; set; }
        public int Life { get; set; }
        public int Fury { get; set; }
        public float Damage { get; set; }
        public float Armor { get; set; }
        public float PhysicalResistance { get; set; }
        public float ColdResistance { get; set; }
        public float FireResistance { get; set; }
        public float LightningResistance { get; set; }
        public float PoisonResistance { get; set; }
        public float CriticalHit { get; set; }
        public float CriticalHitDamage { get; set; }
        public float Evasion { get; set; }
        public float Block { get; set; }
        public float MS { get; set; }
        public int[] ItemRewardGroup { get; set; }
        public int MonsterExperience { get; set; }

    }

    [Serializable]
    public enum MainAttribute
    {
        Strength,
        Intelligence,
        Dexterity,
        Vitality
    }

    [ExcellentConfig]
    [Serializable]
    public class CombatFormulaConfig
    {
        public float CoefficientA { get; set; }
        public float CoefficientB { get; set; }
        public float[] IntervalEvasion { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class PlayerConfig
    {
        public string Name { get; set; }
        public int[] Buffs { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class BulletConfig
    {
        public BulletMode Mode { get; set; }
        public FireMode FireMode { get; set; }
        public float[] FireArgs { get; set; }
        public float FixedPointRandomOffset { get; set; }
        public int FireCount { get; set; }
        public float FireInterval { get; set; }
        public FXBindBone FireBone { get; set; }
        public int[] FX { get; set; }
        public int[] HitFX { get; set; }
        public int SFX { get; set; }
        public int HitSFX { get; set; }
        public float Speed { get; set; }
        public float AngularSpeed { get; set; }
        public float Gravity { get; set; }
        public float FixedPointDuration { get; set; }
        public float Duration { get; set; }
        public bool CollideUnit { get; set; }
        public int MaxHitUnitCount { get; set; }
        [ConfigArray(3)]
        public string[] CollideUnitEffects { get; set; }
        public bool CollideTerrain { get; set; }
        [ConfigArray(3)]
        public string[] CollideTerrainEffects { get; set; }
    }

    [Serializable]
    public enum BulletMode
    {
        Normal,
        Dash,
        FixedPoint,
    }

    [ExcellentConfig]
    [Serializable]
    public class InterruptStackActionConfig
    {
        public InterruptStateType StateType { get; set; }
        public InterruptStackActionType Stiff { get; set; }
        public InterruptStackActionType Stun { get; set; }
        public InterruptStackActionType KnockFloat { get; set; }
        public InterruptStackActionType KnockDown { get; set; }
        public InterruptStackActionType GetUp { get; set; }
        public InterruptStackActionType Hold { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class BuffConfig
    {
        public string Name { get; set; }
        public bool IsDebuff { get; set; }
        public int[] ImmuneBuffs { get; set; }
        public int MaxStack { get; set; }
        public BuffSameStackActionType SameStackAction { get; set; }
        public float Duration { get; set; }
        public BuffTriggerType TriggerType { get; set; }
        public double[] TriggerValue { get; set; }
        public CharacterState TriggerTargetState { get; set; }
        public float[] TriggerProbability { get; set; }
        public float[] TriggerInterval { get; set; }
        public int TriggerCount { get; set; }
        public bool RemoveWhenReachTriggerCount { get; set; }
        public int[] FX { get; set; }
        public int[] TriggerFX { get; set; }
        public int AddSFX { get; set; }
        public int RemoveSFX { get; set; }
        [ConfigArray(2)]
        public BuffHitData[] TriggerHits { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class SkillConfig
    {
        public string Name { get; set; }
        public int Icon { get; set; }
        public SkillGroupType GroupType { get; set; }
        public int[] GroupSkills { get; set; }
        public bool MoveCast { get; set; }
        public float MoveSpeed { get; set; }
        public string Animation { get; set; }
        public string FX { get; set; }
        public string SFX { get; set; }
        [ConfigComment(@"[技能id，输入起点，连招起点]
[技能id,0,连招起点]
输入起点：在这个时间以后的输入才有效
连招起点：预输入最低会从这个点开始，在技能结束前都可以combo
")]
        public float[] SkillCombo { get; set; }
        public float Duration { get; set; }
        public int CDGroup { get; set; }
        public SkillTargetType InputTarget { get; set; }
        public CharacterRelationType InputTargetRelation { get; set; }
        public int[] SelfBuffs { get; set; }
        public SkillCastLimit CastLimit { get; set; }
        [ConfigArray(4)]
        public SkillHitData[] DetectHits { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class DungeonConfig
    {
        public int[] MapList { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class DungeonMapConfig
    {
        public string Scene { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class SkillCDConfig
    {
        public float CD { get; set; }
        public bool CanCharge { get; set; }
        public int MaxChargeCount { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class AttributeMapConfig
    {
        public AttributeType AttributeType { get; set; }
        public string AttributeDescribe { get; set; }
        public string EnglishMark { get; set; }
        public float LowerLimit { get; set; }
        public float UpperLimit { get; set; }
    }

    [Serializable]
    public enum AttributeType
    {
        Primary,
        Secondary,
        Third,
        fourth
    }

    [ExcellentConfig]
    [Serializable]
    public class ExperienceGrowthConfig
    {
        public int CharacterLv { get; set; }
        public int UpgradeExp { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class EquipmentQualityConfig
    {
        public int[] RandomPropertyInterval { get; set; }
        public int QualityWeight { get; set; }
        public int[] GemPlace { get; set; }
        public int Decomposition { get; set; }
        public int QualityEffect { get; set; }
        public bool Appraisal { get; set; }
        public float AdditionPercentage { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class EquipmentRandomAttributeConfig
    {
        [ConfigArray(2)]
        public RandomAttributeId[] RandomAttributeIds { get; set; }
        public string AttibuteShow { get; set; }
    }

    [Serializable]
    public class RandomAttributeId
    {
        public int AttributeId { get; set; }
        public float[] GrowInterval { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class StrengthenConfig
    {
        public int BODCost { get; set; }
        public int KKMACost { get; set; }
        public int StoneCost { get; set; }
        public float SuccessRate { get; set; }
        public float FailureReturnRate { get; set; }
        public int FailureReturnLv { get; set; }
        public float BasicAttributeBonus { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class EquipmentBasePropertyConfig
    {
        public ItemType ItemType { get; set; }
        public Jobs Jobs { get; set; }
        public Parts Parts { get; set; }
        public int LimitLevel { get; set; }
        public int[] LimitAttribute { get; set; }
        [ConfigArray(2)]
        public RandomAttributeData[] RandomAttributeDatas { get; set; }
        public int AttributeGroupId { get; set; }
        public int MaxGemSlot { get; set; }
        public float AttackSpeed { get; set; }
        public string Icon { get; set; }
        public string EquipDescribe { get; set; }
        public string EquipmentShowModel { get; set; }
        public string EquipmentHighQuality { get; set; }
        public string EquipmentDropModel { get; set; }
        public int[] EquipmentQualityInterval { get; set; }
    }

    [Serializable]
    public class RandomAttributeData
    {
        public int AttributeType { get; set; }
        public float[] AttributeInterval { get; set; }
    }

    [Serializable]
    public enum ItemType
    {
        Equipment,
        Gem,
        Item
    }
    [Serializable]
    public enum Jobs
    {
        All,
        TwoHandedSword,
    }
    [Serializable]
    public enum Parts
    {
        Weapon = 1,
        SecondWeapon = 2,
        TwoHanded = 3,
        Caps = 4,
        Clothes = 8,
        Trousers = 16,
        Shoes = 32,
        Gloves = 64,
        Ornaments = 128
    }

    [ExcellentConfig]
    [Serializable]
    public class RandomAttributeGroupConfig
    {
        public int RandomAttributeGroupId { get; set; }
        public int[] RandomAttributeRewardId { get; set; }
        public int[] RandomAttributeRewardWeight { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class DropRewardGroupConfig
    {
        public int GroupId { get; set; }
        public ItemTypeGroup ItemTypeGroup { get; set; }
        public int ItemId { get; set; }
        public int MinNum { get; set; }
        public int MaxNum { get; set; }
        public int Weight { get; set; }
    }
    [Serializable]
    public enum ItemTypeGroup
    {
        Equipment,
        Coins,
        Item,
        Materials
    }

    [ExcellentConfig]
    [Serializable]
    public class PropConfig
    {
        public string PropName { get; set; }
        public PropType PropType { get; set; }
        [ConfigComment(@"：Ordinary:
1,任务物品
2,材料
3,杂物
4,坐骑
5,钥匙
Consumable:
5,药剂
6,食物
7,配方
8,卷轴
Gem:
9,红色珠宝
10,黄色珠宝
11,蓝色珠宝
12,绿色珠宝
")]
        public int SubType { get; set; }
        public int[] UseEffect { get; set; }
        public int StackingLimit { get; set; }
        public bool Sell { get; set; }
        public bool Trade { get; set; }
        public bool Discard { get; set; }
        public int SellPrice { get; set; }
        public string PropModel { get; set; }
        public string PropIcon { get; set; }
        public string PropDescribe { get; set; }
        public int GetSource { get; set; }
    }
    [Serializable]
    public enum PropType
    {
        Ordinary,
        Consumable,
        Gem,
    }







    [ExcellentConfig]
    [Serializable]
    public class SpellFieldConfig
    {
        public int[] FX { get; set; }
        public int[] DetectFX { get; set; }
        public int SFX { get; set; }
        public int DetectSFX { get; set; }
        public float Duration { get; set; }
        public float CollideRadius { get; set; }
        public int MaxHitCount { get; set; }
        public float Delay { get; set; }
        public float Interval { get; set; }
        public SpellFieldDetectType DetectType { get; set; }
        public CharacterRelationType TargetRelation { get; set; }
        [ConfigArray(3)]
        public string[] Effects { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class FXConfig
    {
        public string Asset { get; set; }
        public FXPlayMode PlayMode { get; set; }
        public FXBindBone BindBone { get; set; }
        public bool FollowRotationX { get; set; }
        public bool FollowRotationY { get; set; }
        public bool FollowRotationZ { get; set; }
        public bool AutoRecycle { get; set; }
        public bool HardStop { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class AudioConfig
    {
        public string Asset { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class SpriteConfig
    {
        public string Asset { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class UIConfig
    {
        public string Asset { get; set; }
        public UIMode Mode { get; set; }
    }

    [ExcellentConfig]
    [Serializable]
    public class I18NConfig
    {
        public string key { get; set; }
        public string zh_cn { get; set; }
        public string zh_tw { get; set; }
        public string en_us { get; set; }
    }

    [Serializable]
    public enum UIMode
    {
        Normal,
        Modal,
    }

    [Serializable]
    public enum CharacterState
    {
        Unvitalized,
        Idle,
        Interrupt,
        Dead,
    }

    [Serializable]
    public enum FXBindBone
    {
        Root,
        WeaponL,
        WeaponR,
        HandL,
        HandR,
        FootL,
        FootR,
        Chest,
        Head,
        TopHead,
        BulletFire,
        VFX,
    }

    [Serializable]
    public enum MonsterType
    {
        None,
        Creep,
        Elite,
        Boss,
    }

    [Serializable]
    public enum SkillGroupType
    {
        None,
        Sequence,
        Charge,
        Normal,
    }

    [Serializable]
    public class SkillCastLimit
    {
        public float Distance { get; set; }
    }

    [Serializable]
    public class SkillHitData
    {
        [ConfigComment(@"检测时间")]
        public float Time { get; set; }
        [ConfigComment(@"检测类型
caster=施法者
inputarge=输入参数
Circle=圆形
Sector=扇形
Rectangle=矩形")]
        public SkillDetectType DetectType { get; set; }
        [ConfigComment(@"检测参数
caster=无
inputarge=无
Circle=半径
Sector=长；角度
Rectangle=宽；长")]
        public float[] DetectArgs { get; set; }
        [ConfigComment(@"检测中心类型
CasterPosition=施法者位置
InputPosition=输入位置
InputUnit=输入目标位置")]
        public SkillDetectCenterType DetectCenterType { get; set; }
        public float CenterOffset { get; set; }
        [ConfigComment(@"影响的目标
Hostile=敌对
Self=自己
Ally=友军
All=所有")]
        public CharacterRelationType TargetRelation { get; set; }
        [ConfigArray(4)]
        [ConfigComment(@"影响效果，参数后面：revert等于可还原。
伤害=Lua\Effect\damage；伤害值；参数2无用
范围内触发=Lua\Effect\spellfield；SpellFieldConfig的ID
击倒=Lua\Effect\knock_down；持续时间")]
        public string[] Effects { get; set; }
    }

    [Serializable]
    public enum SpellFieldDetectType
    {
        Unit,
        Position,
        RandomPosition,
    }

    [Serializable]
    public enum FXPlayMode
    {
        AtPosition,
        Bind,
    }

    [Serializable]
    public enum FireMode
    {
        Point,
        Sector,
        RandomSector,
    }

    [Serializable]
    public class BuffHitData
    {
        public SkillDetectType DetectType { get; set; }
        public float[] DetectArgs { get; set; }
        public CharacterRelationType TargetRelation { get; set; }
        public string Effect { get; set; }
    }

    [Serializable]
    public enum InterruptStackActionType
    {
        IgnoreSelf = 0,
        RemoveTarget = 1,
    }

    [Serializable]
    public enum InterruptStateType
    {
        None = 0,
        /// <summary>
        /// 僵直
        /// </summary>
        Stiff,
        /// <summary>
        /// 眩晕
        /// </summary>
        Stun,
        /// <summary>
        /// 击飞
        /// </summary>
        KnockFloat,
        /// <summary>
        /// 击倒
        /// </summary>
        KnockDown,
        /// <summary>
        /// 起身
        /// </summary>
        GetUp,
        /// <summary>
        /// 定身
        /// </summary>
        Hold,
    }


    [Serializable]
    public enum BuffSameStackActionType
    {
        None,
        RefreshTime,
        MultiInstance,
    }

    [Serializable]
    public enum BuffTriggerType
    {
        /// <summary>
        /// 空触发
        /// </summary>
        None,
        /// <summary>
        /// 周期触发
        /// </summary>
        PeriodTrigger,
        /// <summary>
        /// 添加时触发
        /// </summary>
        AddBuff,
        /// <summary>
        /// 移除Buff
        /// </summary>
        RemoveBuff,
    }

    [System.Flags]
    [Serializable]
    public enum SkillTargetType
    {
        None = 0,
        Position = 1 << 0,
        Unit = 1 << 1,
        UnitOrPosition = Unit | Position,
        Object = 1 << 2,
        All = -1,
    }


    [Serializable]
    public enum SkillDetectType
    {
        None = 0,
        InputArgs = 1,
        InputPosition = 2,
        Caster = 3,
        TriggerUnit = 4,
        Circle = 5,
        Sector = 6,
        Rectangle = 7,
    }

    [Serializable]
    public enum SkillDetectCenterType
    {
        None,
        CasterPosition = 1,
        InputPosition = 2,
        InputUnit = 3,
    }

    [Serializable]
    public enum CharacterRelationType
    {
        None = 0,
        Hostile = 1 << 0,
        Self = 1 << 1,
        Ally = 1 << 2,
        All = -1,
    }
}
