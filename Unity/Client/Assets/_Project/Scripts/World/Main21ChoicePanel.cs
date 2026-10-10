using System;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using UnityEngine;
using UnityEngine.InputSystem;
namespace ProjectLimitless.World
{
    /// <summary>Main21의 시간 제한 없는 선택 UI입니다. 기존 EventSystem과 패드 탐색을 재사용하고 종료 시 자기 입력 잠금만 해제합니다.</summary>
    public sealed class Main21ChoicePanel : MonoBehaviour
    {
        private InputAction cancel;private Action<int> selected;private int openedFrame;
        public static void Show(string title,string[] options,Action<int> onSelected)
        {
            var panel=new GameObject("Main21Choice",typeof(Main21ChoicePanel)).GetComponent<Main21ChoicePanel>();
            if(!WorldModalState.TryAcquire(panel)){Destroy(panel.gameObject);return;}
            panel.selected=onSelected;panel.openedFrame=Time.frameCount;PlayerController.SetMovementLocked(panel,true);
            var canvas=panel.gameObject.AddComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceOverlay;canvas.sortingOrder=300;
            panel.gameObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            var scaler=panel.gameObject.AddComponent<UnityEngine.UI.CanvasScaler>();scaler.uiScaleMode=UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1280,720);
            var background=Rect(panel.transform,"Panel",new Vector2(820,460),Vector2.zero);
            background.gameObject.AddComponent<UnityEngine.UI.Image>().color=new Color(.035f,.055f,.08f,.98f);
            Text(background,"Title",title,new Vector2(760,80),new Vector2(0,170));
            for(int i=0;i<options.Length;i++)
            {
                int index=i;var button=Rect(background,"Option"+i,new Vector2(720,62),new Vector2(0,90-i*72));
                button.gameObject.AddComponent<UnityEngine.UI.Image>().color=new Color(.15f,.23f,.32f);
                var b=button.gameObject.AddComponent<UnityEngine.UI.Button>();Text(button,"Text",options[i],new Vector2(680,54),Vector2.zero);
                b.onClick.AddListener(()=>panel.Select(index));if(i==0)b.Select();
            }
            panel.cancel=new InputAction("Main21ChoiceCancel",InputActionType.Button);panel.cancel.AddBinding("<Keyboard>/escape");panel.cancel.AddBinding("<Gamepad>/buttonEast");
            panel.cancel.performed+=_=>panel.Select(options.Length-1);panel.cancel.Enable();
        }
        public void Select(int index)
        {
            if(Time.frameCount<=openedFrame||selected==null)return;
            var callback=selected;selected=null;WorldModalState.Release(this);PlayerController.SetMovementLocked(this,false);
            gameObject.SetActive(false);Destroy(gameObject);callback(index);
        }
        internal static RectTransform Rect(Transform parent,string name,Vector2 size,Vector2 position)
        {
            var rect=new GameObject(name,typeof(RectTransform)).GetComponent<RectTransform>();rect.SetParent(parent,false);
            rect.anchorMin=rect.anchorMax=new Vector2(.5f,.5f);rect.sizeDelta=size;rect.anchoredPosition=position;return rect;
        }
        internal static UnityEngine.UI.Text Text(Transform parent,string name,string value,Vector2 size,Vector2 position)
        {
            var t=Rect(parent,name,size,position).gameObject.AddComponent<UnityEngine.UI.Text>();t.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            t.fontSize=26;t.color=Color.white;t.alignment=TextAnchor.MiddleCenter;t.text=value;t.raycastTarget=false;return t;
        }
        private void OnDestroy(){cancel?.Dispose();WorldModalState.Release(this);PlayerController.SetMovementLocked(this,false);}
    }
}
