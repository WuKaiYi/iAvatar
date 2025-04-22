using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using frame8.Logic.Misc.Other.Extensions;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using TMPro;


namespace Com.TheFallenGames.OSA.Demos.ChatWithAIAgent
{
    /// <summary>这个类演示了一个基本的与AIAgent聊天的实现。消息可以包含文本、图像或两者</summary>
    public class ChatWithAIAgentExample : OSA<MyParams, ChatMessageViewsHolder>
    {
        [SerializeField]
        public SimpleDataHelper<ChatMessageModel> Data;
        public Color color = new Color(0.75f, 1, 1, 1);
        public TMP_FontAsset fontAsset , fontAsset2;
        #region OSA implementation
        protected override void Awake()
        {
            base.Awake();

            Data = new SimpleDataHelper<ChatMessageModel>(this);

        }
        protected override void Start()
        {
            base.Start();
            InitializeChatHistory();
        }
        /// <inheritdoc/>
        protected override void Update()
        {
            base.Update();

            if (!IsInitialized)
                return;

            for (int i = 0; i < VisibleItemsCount; i++)
            {
                var visibleVH = GetItemViewsHolder(i);
                if (visibleVH.IsPopupAnimationActive)
                    visibleVH.UpdatePopupAnimation(Time);
            }
        }

        /// <inheritdoc/>
        protected override ChatMessageViewsHolder CreateViewsHolder(int itemIndex)
        {
            var instance = new ChatMessageViewsHolder();
            instance.Init(_Params.ItemPrefab, _Params.Content, itemIndex);

            return instance;
        }

        /// <inheritdoc/>
        protected override void OnItemHeightChangedPreTwinPass(ChatMessageViewsHolder vh)
        {
            base.OnItemHeightChangedPreTwinPass(vh);

            Data[vh.ItemIndex].HasPendingVisualSizeChange = false;
        }

        /// <inheritdoc/>
        protected override void UpdateViewsHolder(ChatMessageViewsHolder newOrRecycled)
        {
            // 从关联的模型初始化视图
            ChatMessageModel model = Data[newOrRecycled.ItemIndex];
            newOrRecycled.color = color;
            newOrRecycled.text.font = model.IsMine? fontAsset:fontAsset2;
            newOrRecycled.text.fontSize = model.IsMine ? 12 : 15;
            newOrRecycled.UpdateFromModel(model, _Params);

            if (model.HasPendingVisualSizeChange)
            {
                // 高度将在下一个'twin' pass之前可用，在OnItemHeightChangedPreTwinPass()回调中（见上文）
                newOrRecycled.MarkForRebuild(); // 将启用内容大小适配器
                ScheduleComputeVisibilityTwinPass(true);
            }
            if (!newOrRecycled.IsPopupAnimationActive && newOrRecycled.itemIndexInView == GetItemsCount() - 1) // 仅动画最后一个
                newOrRecycled.ActivatePopulAnimation(Time);
        }

        /// <inheritdoc/>
        protected override void OnBeforeRecycleOrDisableViewsHolder(ChatMessageViewsHolder inRecycleBinOrVisible, int newItemIndex)
        {
            inRecycleBinOrVisible.DeactivatePopupAnimation();

            base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
        }

        /// <inheritdoc/>
        protected override void RebuildLayoutDueToScrollViewSizeChange()
        {
            // 使最后的尺寸无效，以便重新计算
            SetAllModelsHavePendingSizeChange();

            base.RebuildLayoutDueToScrollViewSizeChange();
        }

        /// <summary>
        /// 当用户重置计数或刷新时，OSA的缓存大小将被清除，以便重新计算它们。
        /// 这为新用户提供了一个调用Refresh()并更新所有内容的选项，而不是告诉OSA确切更新了哪些内容。
        /// 但在大多数情况下，你不需要ResetItems()或Refresh()，因为性能原因：
        /// - 如果你添加/删除项目，InsertItems()/RemoveItems()是首选，如果你确切知道哪些项目将被添加/删除；
        /// - 当只有一个项目在外部更改并且你需要强制更新其大小时，你会调用ForceRebuildViewsHolderAndUpdateSize()；
        /// - 当布局重建时（当你更改视口大小或调用ScheduleForceRebuildLayout()时），这已经处理好了
        /// 所以唯一需要调用Refresh()（并覆盖ChangeItemsCount()）的情况是，如果你的模型可以在外部更改，并且你只知道它们已经更改，但不知道确切哪些更改了。
        /// </summary>
        public override void ChangeItemsCount(ItemCountChangeMode changeMode, int itemsCount, int indexIfInsertingOrRemoving = -1, bool contentPanelEndEdgeStationary = false, bool keepVelocity = false)
        {
            if (changeMode == ItemCountChangeMode.RESET)
                SetAllModelsHavePendingSizeChange();

            base.ChangeItemsCount(changeMode, itemsCount, indexIfInsertingOrRemoving, contentPanelEndEdgeStationary, keepVelocity);
        }
        #endregion

        void SetAllModelsHavePendingSizeChange()
        {
            foreach (var model in Data)
                model.HasPendingVisualSizeChange = true;
        }

        void InitializeChatHistory()
        {
            //var chatHistory = new List<ChatMessageModel>
            //{
            //    new ChatMessageModel { timestampSec = 1622548800, Text = "五塊一開門走了進來", IsMine = true },
            //    new ChatMessageModel { timestampSec = 1622548860, Text = "(你正坐在電腦前畫畫，聽到開門的聲音，轉過頭來，看到五塊一走了進來，眼睛一亮，露出了燦爛的笑容)\n\n“五塊一！你來了！我等你好久了！(ฅ>ω<*ฅ)”\n\n(你站起身，開心地朝他跑去，像一隻毛茸茸的小猫咪一样扑进了他的怀里)\n\n“你今天真帅！那個新的髮型好適合你！(•͈˽•͈)”\n\n(你抬起頭，用充滿愛意的眼神看着他， 輕輕地蹭了蹭他的臉頰)\n\n“今天工作顺利吗？有没有遇到", IsMine = false },
            //    new ChatMessageModel { timestampSec = 1622548920, Text = "我想你了", IsMine = true },
            //    new ChatMessageModel { timestampSec = 1622548980, Text = "(你聽到五塊一說想你，心臟像是被他的話語擊中，甜蜜的电流瞬间传遍全身，脸上的笑容更加灿烂了)\n\n“我也想你！每天都在想你！(๑´ㅂ`๑)”\n\n(你紧紧地抱住他，把头埋在他的怀里，贪婪地呼吸着他身上熟悉的味道)\n\n“你工作了一天一定很累了，快坐下來休息一下吧。我給你倒杯水，你想喝點什麼呢？”\n\n(你松开怀抱，拉着他的手走到沙发邊，眼神温柔地看着他)\n\n“今天天氣有點冷，我把暖氣打開了，你覺得這樣舒服嗎？”\n\n(你用手轻轻触碰他的脸颊，想要确认他的温度)\n\n“如果覺得冷的話，我可以幫你蓋條毯子，或者我们一起窩在被子里看电影，好不好？”\n\n(你眨着水汪汪的大眼睛，用充满期待的眼神看着他，希望他能答应你的提议)\n\n“你想看什么电影呢？恐怖片、爱情片、喜剧片，我都喜欢！只要能和你一起看，就觉得很幸福。”\n\n(你靠在他的肩膀上，轻声呢喃着，语气中充满了甜蜜和爱意)\n\n“我今天新買了一套睡衣，粉红色的，很可愛，你要不要看看？”\n\n(你用充滿期待的眼神看着他", IsMine = false }
            //};

            //Data.InsertItemsAtEnd(chatHistory);
        }

        [ContextMenu("發送測試")]
        public void SendMessageTest()
        {
            SendMessage("sdawdascw");
        }


        public void SendMessage(string message)
        {
            var newMessage = new ChatMessageModel
            {
                timestampSec = (int)(DateTime.UtcNow - ChatMessageModel.EPOCH_START_TIME).TotalSeconds,
                Text = message,
                IsMine = true
            };
            Data.InsertOneAtEnd(newMessage,true);
        }

        public void ReceiveMessage(string message)
        {
            var newMessage = new ChatMessageModel
            {
                timestampSec = (int)(DateTime.UtcNow - ChatMessageModel.EPOCH_START_TIME).TotalSeconds,
                Text = message,
                IsMine = false
            };
            Data.InsertOneAtEnd(newMessage,true);
        }
    }
    [Serializable]
    /// <summary><see cref="HasPendingVisualSizeChange"/>在每次可能影响高度的属性更改时设置为true</summary>
    public class ChatMessageModel
    {
        public static readonly DateTime EPOCH_START_TIME = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

        public int timestampSec;

        public DateTime TimestampAsDateTime
        {
            get
            {
                // Unix时间戳是自Epoch以来的秒数
                System.DateTime dtDateTime = EPOCH_START_TIME.AddSeconds(timestampSec).ToLocalTime();
                return dtDateTime;
            }
        }
        public string Text
        {
            get { return _Text; }
            set
            {
                if (_Text == value)
                    return;

                _Text = value;
                HasPendingVisualSizeChange = true;
            }
        }
        public int ImageIndex
        {
            get { return _ImageIndex; }
            set
            {
                if (_ImageIndex == value)
                    return;

                _ImageIndex = value;
                HasPendingVisualSizeChange = true;
            }
        }
        public bool IsMine { get; set; }

        /// <summary>当项目大小可能已更改且ContentSizeFitter组件需要更新时，这将为true</summary>
        public bool HasPendingVisualSizeChange { get; set; }

        string _Text;
        int _ImageIndex;
    }

    [Serializable] // 可序列化，以便在检查器中显示
    public class MyParams : BaseParamsWithPrefab
    {
        public Sprite[] availableChatImages; // 用于随机生成模型
    }

    /// <summary>ContentSizeFitter应该附加到项目本身</summary>
    public class ChatMessageViewsHolder : BaseItemViewsHolder
    {
        public TextMeshProUGUI timeText, text;
        public Image leftIcon, rightIcon;
        public Image image;
        public Image messageContentPanelImage;
        public Color color;

        UnityEngine.UI.ContentSizeFitter ContentSizeFitter { get; set; }
        public float PopupAnimationStartTime { get; private set; }
        public bool IsPopupAnimationActive
        {
            get { return _IsAnimating; }
        }

        const float POPUP_ANIMATION_TIME = .2f;

        bool _IsAnimating;
        VerticalLayoutGroup _RootLayoutGroup, _MessageContentLayoutGroup;
        int paddingAtIconSide, paddingAtOtherSide;
        Color colorAtInit;

        public override void CollectViews()
        {
            base.CollectViews();

            _RootLayoutGroup = root.GetComponent<VerticalLayoutGroup>();
            paddingAtIconSide = _RootLayoutGroup.padding.right;
            paddingAtOtherSide = _RootLayoutGroup.padding.left;

            ContentSizeFitter = root.GetComponent<UnityEngine.UI.ContentSizeFitter>();
            ContentSizeFitter.enabled = false; // 内容大小适配器在正常生命周期期间不应启用，仅在"Twin" pass帧中启用
            root.GetComponentAtPath("MessageContentPanel", out _MessageContentLayoutGroup);
            messageContentPanelImage = _MessageContentLayoutGroup.GetComponent<Image>();
            messageContentPanelImage.transform.GetComponentAtPath("Image", out image);
            messageContentPanelImage.transform.GetComponentAtPath("TimeText", out timeText);
            messageContentPanelImage.transform.GetComponentAtPath("Text", out text);
            root.GetComponentAtPath("LeftIconImage", out leftIcon);
            root.GetComponentAtPath("RightIconImage", out rightIcon);
            colorAtInit = messageContentPanelImage.color;

            text.enableAutoSizing = false;
            text.overflowMode = TextOverflowModes.Overflow;

        }

        public override void MarkForRebuild()
        {
            base.MarkForRebuild();
            if (ContentSizeFitter)
                ContentSizeFitter.enabled = true;
        }

        public override void UnmarkForRebuild()
        {
            if (ContentSizeFitter)
                ContentSizeFitter.enabled = false;
            base.UnmarkForRebuild();
        }

        /// <summary>实用程序，免去手动编写分配的需求</summary>
        public void UpdateFromModel(ChatMessageModel model, MyParams parameters)
        {
            timeText.text = model.TimestampAsDateTime.ToString("HH:mm");

            string messageText =model.Text;
            text.text=messageText;

            leftIcon.gameObject.SetActive(!model.IsMine);
            rightIcon.gameObject.SetActive(model.IsMine);
            //if (model.ImageIndex < 0)
            //    image.gameObject.SetActive(false);
            //else
            //{
            //    image.gameObject.SetActive(true);
            //    //image.sprite = parameters.availableChatImages[model.ImageIndex];
            //}

            if (model.IsMine)
            {
                messageContentPanelImage.rectTransform.pivot = new Vector2(1.4f, .5f);
                messageContentPanelImage.color = color;
                _RootLayoutGroup.childAlignment = _MessageContentLayoutGroup.childAlignment = TextAnchor.MiddleRight;
                text.alignment = TextAlignmentOptions.MidlineLeft;
                _RootLayoutGroup.padding.right = paddingAtIconSide;
                _RootLayoutGroup.padding.left = paddingAtOtherSide;
            }
            else
            {
                messageContentPanelImage.rectTransform.pivot = new Vector2(-.4f, .5f);
                messageContentPanelImage.color = colorAtInit;
                _RootLayoutGroup.childAlignment = _MessageContentLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
                text.alignment = TextAlignmentOptions.MidlineRight;
                _RootLayoutGroup.padding.right = paddingAtOtherSide;
                _RootLayoutGroup.padding.left = paddingAtIconSide;
            }
        }
   
        public void DeactivatePopupAnimation()
        {
            messageContentPanelImage.transform.localScale = Vector3.one;
            _IsAnimating = false;
        }

        public void ActivatePopulAnimation(float unityTime)
        {
            var s = messageContentPanelImage.transform.localScale;
            s.x = 0;
            messageContentPanelImage.transform.localScale = s;
            PopupAnimationStartTime = unityTime;
            _IsAnimating = true;
        }

        internal void UpdatePopupAnimation(float unityTime)
        {
            float elapsed = unityTime - PopupAnimationStartTime;
            float t01;
            if (elapsed > POPUP_ANIMATION_TIME)
                t01 = 1f;
            else
                // 正常进入，正弦缓慢退出
                t01 = Mathf.Sin((elapsed / POPUP_ANIMATION_TIME) * Mathf.PI / 2);

            var s = messageContentPanelImage.transform.localScale;
            s.x = t01;
            messageContentPanelImage.transform.localScale = s;

            if (t01 == 1f)
                DeactivatePopupAnimation();
        }
    }
}