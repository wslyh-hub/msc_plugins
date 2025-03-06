using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using HarmonyLib;
using JSONClass;


namespace Msc_Fight_Info
{
    [BepInPlugin("wslyh", "msc_skill_damage_info", "1.0.1")]
    public class Skill_Info : BaseUnityPlugin
    {
        void Start()
        {
            Logger.LogInfo("[show skill-314 damage]");
            Harmony.CreateAndPatchAll(typeof(Skill_Info));
        }

        void Update()
        {

        }


        private static int Calc_Skill314_Damage(int buffcount, int limit, int skill_damage)
        {
            return (buffcount * 2 - (limit + 1) * 5) * limit / 2 + limit * skill_damage;
        }

        private static void Show_Skill314_Damage(YSGame.Fight.UIFightSkillTip __instance, GUIPackage.Skill skill)
        {
            KBEngine.Avatar player = PlayerEx.Player;
            int yan_huayan_buff_sum = player.buffmag.GetBuffSum(23) + player.buffmag.GetBuffSum(19);
            int skill_damage = JSONClass._skillJsonData.DataDict[skill.skill_ID].HP;

            int lingqi_sum = 0; for (int i = 0; i < 6; i++) lingqi_sum += PlayerEx.Player.cardMag[i];

            int limit = yan_huayan_buff_sum / 5; limit = limit > lingqi_sum ? lingqi_sum : limit;

            int damage = Calc_Skill314_Damage(yan_huayan_buff_sum, limit, skill_damage);

            __instance.SkillNameText.text = __instance.SkillNameText.text + " 面板伤害" + "(" + limit + "):" + damage;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(YSGame.Fight.UIFightSkillTip), "SetSkill")]
        private static void UIFightSkillTip_SetSkill_Patch(YSGame.Fight.UIFightSkillTip __instance, GUIPackage.Skill skill)
        {

            switch (skill.SkillID)
            {
                case 314:
                    Show_Skill314_Damage(__instance, skill);
                    break;

                default: break;
            }

        }

    }
}
