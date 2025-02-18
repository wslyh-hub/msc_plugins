using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using HarmonyLib;

namespace Mcs_Refresh_NpcLove
{
    [BepInPlugin("wslyh","msc_refresh_npclove","1.0.1")]
    public class MscPluginNpcLove : BaseUnityPlugin
    {
        void Start()
        {
            Logger.LogInfo("[msc refresh npclove]");
            Harmony.CreateAndPatchAll(typeof(MscPluginNpcLove),null);
        }
        void Update()
        {
          
        }


        public static void  refresh_npc_love(JiaoYi.NpcLove curr_npc)
        {
            if(curr_npc != null)
            {
                int npc_id = JiaoYi.JiaoYiUIMag.Inst.NpcId;

                jsonData.instance.MonstarCreatInterstingType(npc_id);

                Text[] list = curr_npc.Parent.GetComponentsInChildren<Text>();

                for (int i = 0; i < list.Length; i++)
                {
                    list[i].gameObject.SetActive(false);
                }
                curr_npc.InitNpcLove(npc_id);
            }
        }


        public static void add_click(JiaoYi.NpcLove curr_npc)
        {
            Image love_img = curr_npc.GetComponentInChildren<Image>(); 

            EventTrigger.Entry click = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };

            click.callback.AddListener(delegate (BaseEventData data)
            {
                refresh_npc_love(curr_npc);
            });

            love_img.gameObject.AddComponent<EventTrigger>().triggers.Add(click);
        }
    
        [HarmonyPostfix, HarmonyPatch(typeof(JiaoYi.JiaoYiUIMag), "InitNpcData")]
        private static void JiaoYiUIMag_InitNpcData_Patch()
        {
            JiaoYi.NpcLove curr_npc = JiaoYi.JiaoYiUIMag.Inst.GetComponentInChildren<JiaoYi.NpcLove>();
            if (curr_npc != null)
            {
                add_click(curr_npc);
            }
           
        }


        [HarmonyPostfix, HarmonyPatch(typeof(UINPCZengLi), "Start")]
        private static void UINPCJiaoHu_Start_Patch(UINPCZengLi __instance)
        {
                Image[] images = __instance.NPCXingQu.GetComponentsInChildren<Image>();

                EventTrigger.Entry click = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerClick
                };

                click.callback.AddListener(delegate (BaseEventData data)
                {
                    UINPCData curr_npc = UINPCJiaoHu.Inst.NowJiaoHuNPC;
                    int npc_id = curr_npc.ID;
                    jsonData.instance.MonstarCreatInterstingType(npc_id);
                    __instance.NPCXingQu.RefreshUI();
                });

                for (int i = 0; i < 2; i++)
                {
                    images[i].gameObject.AddComponent<EventTrigger>().triggers.Add(click); 
                }

        }
    }
}
