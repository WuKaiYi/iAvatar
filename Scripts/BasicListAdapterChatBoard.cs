/*
 * * * * This bare-bones script was auto-generated * * * *
 * The code commented with "/ * * /" demonstrates how data is retrieved and passed to the adapter, plus other common commands. You can remove/replace it once you've got the idea
 * Complete it according to your specific use-case
 * Consult the Example scripts if you get stuck, as they provide solutions to most common scenarios
 * 
 * Main terms to understand:
 *		Model = class that contains the data associated with an item (title, content, icon etc.)
 *		Views Holder = class that contains references to your views (Text, Image, MonoBehavior, etc.)
 * 
 * Default expected UI hiererchy:
 *	  ...
 *		-Canvas
 *		  ...
 *			-MyScrollViewAdapter
 *				-Viewport
 *					-Content
 *				-Scrollbar (Optional)
 *				-ItemPrefab (Optional)
 * 
 * Note: If using Visual Studio and opening generated scripts for the first time, sometimes Intellisense (autocompletion)
 * won't work. This is a well-known bug and the solution is here: https://developercommunity.visualstudio.com/content/problem/130597/unity-intellisense-not-working-after-creating-new-1.html (or google "unity intellisense not working new script")
 * 
 * 
 * Please read the manual under "/Docs", as it contains everything you need to know in order to get started, including FAQ
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using frame8.Logic.Misc.Other.Extensions;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using Defective.JSON;
using static IMBX.ImageLoader;
using BestHTTP;
//using UnityEngine.Events;
//// using Unity.Barracuda;

//// using Unity.Barracuda;
//using Unity.Services.Vivox;

// You should modify the namespace to your own or - if you're sure there won't ever be conflicts - remove it altogether
namespace ChatBoard
{
    // There are 2 important callbacks you need to implement, apart from Start(): CreateViewsHolder() and UpdateViewsHolder()
    // See explanations below
    public class BasicListAdapterChatBoard : OSA<MyParams, MyItemViewsHolder>
    {
        // Helper that stores data and notifies the adapter when items count changes
        // Can be iterated and can also have its elements accessed by the [] operator
        /// <summary>Fired when the number of items changes or refreshes</summary>
        public UnityEngine.Events.UnityEvent OnItemsUpdated;

        public SimpleDataHelper<MyListItemModel> Data { get; private set; }

        public Text content, username, Chatname, created_at, likes;
        public RawImage avatar_pic;


        #region OSA implementation
        protected override void Start()
        {
            Data = new SimpleDataHelper<MyListItemModel>(this);
            this.transform .parent .GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                Outline outline = this.transform.parent.GetComponentInChildren<Outline>();
                outline.enabled = !outline.enabled;

                GameObject.FindAnyObjectByType<CommentReply>().SetComment(ID, username.text, outline.enabled, outline);
                waitInput = outline.enabled;
            });

            // Calling this initializes internal data and prepares the adapter to handle item count changes
            base.Start();
            content = transform.parent.Find("Area/content").GetComponent<Text>();
            username = transform.parent.Find("Area/username").GetComponent<Text>();
            Chatname = transform.parent.Find("Area/name").GetComponent<Text>();
            created_at = transform.parent.Find("Area/created_at").GetComponent<Text>();
            likes = transform.parent.Find("Area/btn - Like/likes").GetComponent<Text>();
            avatar_pic = transform.parent.Find("Area/avatar_pic/cont - avatar/raw - avatar-image").GetComponent<RawImage>();

            Parameters.ItemPrefab.gameObject.SetActive(false);
            StartRefreshData();
            // Retrieve the models from your data source and set the items count
            /*
			RetrieveDataAndUpdate(500);
			*/
        }
        private void StartRefreshData()
        {
            StartCoroutine(RefreshData());
        }

        public int ID;
        public bool waitInput;
        IEnumerator RefreshData()
        {
            while (true)
            {
                if (waitInput == false)
                {
                    var request = new HTTPRequest(new Uri(ServerConfig.host + "/comment/comments/" + ID), HTTPMethods.Get, callback: OnRequestFinished);

                    request.Send();

                    yield return new WaitForSeconds(5);
                }
                else
                {
                    yield return null;
                }
            }
        }
        void OnRequestFinished(HTTPRequest req, HTTPResponse resp)
        {

            StartCoroutine(FetchMoreItemsFromDataSourceAndUpdate(resp.DataAsText));
        }
        //protected override void RebuildLayoutDueToScrollViewSizeChange()
        //{
        //    // Invalidate the last sizes so that they'll be re-calculated
        //    SetAllModelsHavePendingSizeChange();

        //    base.RebuildLayoutDueToScrollViewSizeChange();
        //}
        //public override void ChangeItemsCount(ItemCountChangeMode changeMode, int itemsCount, int indexIfInsertingOrRemoving = -1, bool contentPanelEndEdgeStationary = false, bool keepVelocity = false)
        //{
        //    base.ChangeItemsCount(changeMode, itemsCount, indexIfInsertingOrRemoving, contentPanelEndEdgeStationary, keepVelocity);

        //}
        /// <summary>
        /// <para>This is overidden only so that the items' title will be updated to reflect its new index in case of Insert/Remove, because the index is not stored in the model</para>
        /// <para>If you don't store/care about the index of each item, you can omit this</para>
        /// <para>For more info, see <see cref="OSA{TParams, TItemViewsHolder}.OnItemIndexChangedDueInsertOrRemove(TItemViewsHolder, int, bool, int)"/> </para>
        /// </summary>
        //protected override void OnItemIndexChangedDueInsertOrRemove(MyItemViewsHolder shiftedViewsHolder, int oldIndex, bool wasInsert, int removeOrInsertIndex)
        //{
        //    base.OnItemIndexChangedDueInsertOrRemove(shiftedViewsHolder, oldIndex, wasInsert, removeOrInsertIndex);

        //}
        #endregion




        void SetAllModelsHavePendingSizeChange()
        {
            foreach (var model in Data)
                model.HasPendingSizeChange = true;
        }
        protected override MyItemViewsHolder CreateViewsHolder(int itemIndex)
        {
            var instance = new MyItemViewsHolder();
            instance.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
            return instance;
        }
        void Selected()
        {
            Outline outline = this.GetComponentInChildren<Outline>();
            outline.enabled = !outline.enabled;
          
        }



        // This is called anytime a previously invisible item become visible, or after it's created, 
        // or when anything that requires a refresh happens
        // Here you bind the data from the model to the item's views
        // *For the method's full description check the base implementation
        protected override void UpdateViewsHolder(MyItemViewsHolder newOrRecycled)
        {


            MyListItemModel model = Data[newOrRecycled.ItemIndex];
            newOrRecycled.root.GetComponent<Button>().onClick.RemoveAllListeners();
            newOrRecycled.root.GetComponent<Button>().onClick.AddListener(()=> {
                Outline outline = newOrRecycled.root.GetComponentInChildren<Outline>();
                outline.enabled = !outline.enabled;

                GameObject.FindAnyObjectByType<CommentReply>().SetComment(model.id,model.username, outline.enabled, outline);
                waitInput = outline.enabled;
            } );
            //newOrRecycled.name.text = model.name;
            newOrRecycled.content.text = model.content;

            newOrRecycled.username.text = model.username;
            newOrRecycled.created_at.text = model.created_at;
            newOrRecycled.likes.text = model.likes;
            IMBX.ImageLoader imageLoader = IMBX.ImageLoader.Create();

            imageLoader.Load(0, model.avatar_pic, model.username, "user_avatar", CacheMode.UseCached, (texture, index) =>
            {
                if (texture != null)
                {
                    newOrRecycled.avatar_pic.texture = texture;
                }
            }, 0, 10);

            Transform child = newOrRecycled.root.Find("child");
            for (int i = 1; i < child.childCount; i++)
            {
                Destroy(child.GetChild(i).gameObject);
            }
            if (model.json.count != 0)
            {
                for (int i = 0; i < model.json.count; i++)
                {
                    GameObject game = Instantiate(child.GetChild(0).gameObject, child);
                    game.name = "" + model.json[i]["id"].intValue;
                    game.SetActive(true);
                    game.transform.GetComponentAtPath<Text>("content").text = ServerConfig.ParseUnicode(model.json[i]["content"].stringValue);
                    game.transform.GetComponentAtPath<Text>("created_at").text = model.json[i]["created_at"].stringValue;
                    game.transform.GetComponentAtPath<Text>("username").text = ServerConfig.ParseUnicode(model.json[i]["user"]["username"].stringValue);
                    game.transform.GetComponentAtPath<Text>("btn - Like/likes").text = "" + model.json[i]["likes"].intValue;

                    //game.GetComponent<Button>().onClick.RemoveAllListeners();
                    //game.GetComponent<Button>().onClick.AddListener(() => {
                    //    Outline outline = game.GetComponentInChildren<Outline>();
                    //    outline.enabled = !outline.enabled;

    
                    //    GameObject.FindAnyObjectByType<CommentReply>().SetComment(int.Parse(game.name), game.transform.GetComponentAtPath<Text>("username").text, outline.enabled, outline);

                    //    waitInput = outline.enabled;
                    //});

                    imageLoader.Load(0, model.json[i]["user"]["avatar_pic"].stringValue, model.json[i]["user"]["username"].stringValue, "user_avatar", CacheMode.UseCached, (texture, index) =>
                    {
                        if (texture != null)
                        {
                            game.transform.GetComponentAtPath<RawImage>("avatar_pic/cont - avatar/raw - avatar-image").texture = texture;
                        }
                    }, 0, 10);
                }


                newOrRecycled.MarkForRebuild();
                ScheduleComputeVisibilityTwinPass();
            }

        }



        // These are common data manipulation methods
        // The list containing the models is managed by you. The adapter only manages the items' sizes and the count
        // The adapter needs to be notified of any change that occurs in the data list. Methods for each
        // case are provided: Refresh, ResetItems, InsertItems, RemoveItems
        #region data manipulation
        public void AddItemsAt(int index, IList<MyListItemModel> items)
        {
            // Commented: the below 2 lines exemplify how you can use a plain list to manage the data, instead of a DataHelper, in case you need full control
            //YourList.InsertRange(index, items);
            //InsertItems(index, items.Length);

            Data.InsertItems(index, items);
        }

        public void RemoveItemsFrom(int index, int count)
        {
            // Commented: the below 2 lines exemplify how you can use a plain list to manage the data, instead of a DataHelper, in case you need full control
            //YourList.RemoveRange(index, count);
            //RemoveItems(index, count);

            Data.RemoveItems(index, count);
        }

        public void SetItems(IList<MyListItemModel> items)
        {
            // Commented: the below 3 lines exemplify how you can use a plain list to manage the data, instead of a DataHelper, in case you need full control
            /*// 注释：下面的3行代码示例展示了如何使用普通列表来管理数据，而不是使用DataHelper，以便在需要完全控制时使用。*/

            //YourList.Clear();
            //YourList.AddRange(items);
            //ResetItems(YourList.Count);

            Data.ResetItems(items);
        }
        #endregion


        // Here, we're requesting <count> items from the data source
        //void RetrieveDataAndUpdate(int count)
        //{
        //    StartCoroutine(FetchMoreItemsFromDataSourceAndUpdate(count));
        //}

        // Retrieving <count> models from the data source and calling OnDataRetrieved after.
        // In a real case scenario, you'd query your server, your database or whatever is your data source and call OnDataRetrieved after
        IEnumerator FetchMoreItemsFromDataSourceAndUpdate(string data)
        {
            Debug.Log(data);
            JSONObject json = new JSONObject(data);
            content.text = ServerConfig.ParseUnicode(json["content"].stringValue);
            username.text = ServerConfig.ParseUnicode(json["user"]["username"].stringValue);
            Chatname.text = ServerConfig.ParseUnicode(json["name"].stringValue);
            created_at.text = ServerConfig.ParseUnicode(json["created_at"].stringValue);
            likes.text = "" + json["likes"].intValue;
            IMBX.ImageLoader imageLoader = IMBX.ImageLoader.Create();

            imageLoader.Load(0, json["user"]["avatar_pic"].stringValue, json["user"]["username"].stringValue, "user_avatar", CacheMode.UseCached, (texture, index) =>
            {
                if (texture != null)
                {
                    avatar_pic.texture = texture;
                }
            }, 0, 10);
            // Simulating data retrieving delay
            yield return new WaitForSeconds(.5f);

            var newItems = new MyListItemModel[json["child_comments"].count];

            // Retrieve your data here

            for (int i = 0; i < json["child_comments"].count; ++i)
            {

                var model = new MyListItemModel()
                {

                    content = json["child_comments"][i]["content"].stringValue,
                    json = json["child_comments"][i]["child_comments"],
                    name = json["child_comments"][i]["name"].stringValue,
                    username = json["child_comments"][i]["user"]["username"].stringValue,
                    created_at = json["child_comments"][i]["created_at"].stringValue,
                    avatar_pic = json["child_comments"][i]["user"]["avatar_pic"].stringValue,
                    id= json["child_comments"][i]["id"].intValue

                };




                newItems[i] = model;
            }


            OnDataRetrieved(newItems);
        }

        void OnDataRetrieved(MyListItemModel[] newItems)
        {

            Data.ResetItems(newItems);

        }
    }
    [Serializable] // serializable, so it can be shown in inspector
    public class MyParams : BaseParamsWithPrefab
    {
        public Texture2D[] availableIcons; // used to randomly generate models


        [NonSerialized]
        public bool freezeContentEndEdgeOnCountChange;
    }
    // Class containing the data associated with an item
    public class MyListItemModel
    {
        public int id;
        public string content, username, name, created_at, likes, avatar_pic;
        public JSONObject json;
        internal bool HasPendingSizeChange;
    }



    /*这个类保存了一个项目的视图的引用。
    你的视图持有者应该扩展BaseItemViewsHolder用于ListViews和CellViewsHolder用于GridViews。*/
    // This class keeps references to an item's views.
    // Your views holder should extend BaseItemViewsHolder for ListViews and CellViewsHolder for GridViews
    public class MyItemViewsHolder : BaseItemViewsHolder
    {

        public Text content, username, name, created_at, likes;
        public RawImage avatar_pic;
        private ContentSizeFitter CSF;



        // Retrieving the views from the item's root GameObject
        public override void CollectViews()
        {
            base.CollectViews();

            CSF = root.GetComponent<UnityEngine.UI.ContentSizeFitter>();
            // The content size fitter should not be enabled during normal lifecycle, only in the "Twin" pass frame

            /*内容大小适配器在正常生命周期中不应启用，只在“双”传递帧中启用。*/
            CSF.enabled = false;

            root.GetComponentAtPath("content", out content);
            root.GetComponentAtPath("created_at", out created_at);
            root.GetComponentAtPath("username", out username);

            root.GetComponentAtPath("btn - Like/likes", out likes);
            root.GetComponentAtPath("avatar_pic/cont - avatar/raw - avatar-image", out avatar_pic);
        }

        public override void MarkForRebuild()
        {
            base.MarkForRebuild();

            //LayoutRebuilder.MarkLayoutForRebuild(root);
            //LayoutRebuilder.MarkLayoutForRebuild(root.Find("child").GetComponent<RectTransform>());

            if (CSF)
                CSF.enabled = true;
        }

        public override void UnmarkForRebuild()
        {
            if (CSF)
                CSF.enabled = false;
            base.UnmarkForRebuild();
        }




    }
}
