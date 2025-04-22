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
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using BestHTTP;
using UnityStandardAssets.Effects;
using Defective.JSON;
using BestHTTP.JSON;
using System.IO;
//// using Unity.Barracuda;
using static IMBX.ImageLoader;
using DungeonArchitect.Samples.ShooterGame;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// You should modify the namespace to your own or - if you're sure there won't ever be conflicts - remove it altogether
namespace Room
{
    // There is 1 important callback you need to implement, apart from Start(): UpdateCellViewsHolder()
    // See explanations below
    public class BasicGridAdapterRoom : GridAdapter<GridParams, MyGridItemViewsHolder>
    {
        private JSONObject jd;

        // Helper that stores data and notifies the adapter when items count changes
        // Can be iterated and can also have its elements accessed by the [] operator
        public SimpleDataHelper<MyGridItemModel> Data { get; private set; }


        #region GridAdapter implementation
        protected override void Start()
        {
            Data = new SimpleDataHelper<MyGridItemModel>(this);

            // Calling this initializes internal data and prepares the adapter to handle item count changes
            base.Start();

          
        }

        protected override void OnEnable()
        {
            initializes();
        }
        public void initializes()
        {
            var request = new HTTPRequest(new Uri(ServerConfig.host + "/api/ES3_Room_Upload?user_id=" + ES3.Load("user_id").ToString()), HTTPMethods.Get, onFinished);
            Debug.Log(request.Uri);

            request.Send();

            void onFinished(HTTPRequest originalRequest, HTTPResponse response)
            {
                Debug.Log(response.DataAsText);
                jd = new JSONObject(response.DataAsText);
                RetrieveDataAndUpdate(jd);
            }
        }
        // This is called anytime a previously invisible item become visible, or after it's created, 
        // or when anything that requires a refresh happens
        // Here you bind the data from the model to the item's views
        // *For the method's full description check the base implementation
        protected override void UpdateCellViewsHolder(MyGridItemViewsHolder newOrRecycled)
        {
          
            MyGridItemModel model = Data[newOrRecycled.ItemIndex];
            int dotIndex = model.Room_name.IndexOf('.');
            string result = model.Room_name.Substring(0, dotIndex);
            newOrRecycled.titleText.text = result;

            newOrRecycled.views.GetComponent<Lean.Gui.LeanButton>().OnClick.RemoveAllListeners();
            newOrRecycled.views.GetComponent<Lean.Gui.LeanButton>().OnClick.AddListener(() =>
            {
                if (SceneManager.GetActiveScene().name == "select_scene")
                {
                    ES3FormUser.Save<string>("CustomRoom", model.Room_name);
                    bl_SceneLoaderManager.LoadScene("DungeonLayout");
                    return;
                }

                foreach (GameObject g in GameObject.FindGameObjectsWithTag("CustomRoom"))
                {
                    g.SetActive(false);
                    // Destroy(g);
                }


                var settings = new ES3Settings();

                ES3FormUser.Save<string>("CustomRoom", model.Room_name);

                settings.path = model.Room_name;
                Debug.Log(settings.FullPath + " ES3AutoSaveMgr.Current.settings.path ");
                if (ES3.KeyExists("CustomRoomList", settings))
                {
                    ES3.Load("CustomRoomList", settings);
                }


                settings = new ES3Settings();
                settings.path = ES3FormUser.Load<string>("CustomSpace");
                ES3.Save("CustomRoomMap", model.map_url, settings);

                CustomToolInitializer.Instance.Generate3DScene();
            });
         
            newOrRecycled.delete.onClick.RemoveAllListeners();
            newOrRecycled.delete.onClick.AddListener(() =>
            {
                var request = new HTTPRequest(new Uri(ServerConfig.host + "/api/ES3_File_Room/" + model.id), HTTPMethods.Delete, onFinished);
                request.Send();
                void onFinished(HTTPRequest originalRequest, HTTPResponse response)
                {
                    Data.RemoveOne(newOrRecycled.ItemIndex);
                }
            });

            Debug.Log("model.map_url  " + model.map_url);
            if (model.map_url != "")
            {
                IMBX.ImageLoader imageLoader = IMBX.ImageLoader.Create();

                imageLoader.Load(0, model.map_url, "", "", CacheMode.NoCache, (texture, index) =>
                {
                    if (texture != null)
                    {
                        newOrRecycled.RoomMiniMap.GetComponent<DImageDisplayHandler>().SetRawImage(newOrRecycled.RoomMiniMap, texture);

                        //UploadFile(FileBrowser.Result[i]);
                    }
                }, 0, 10);
            }
        }

        // This is the best place to clear an item's views in order to prepare it from being recycled, but this is not always needed, 
        // especially if the views' values are being overwritten anyway. Instead, this can be used to, for example, cancel an image 
        // download request, if it's still in progress when the item goes out of the viewport.
        // <newItemIndex> will be non-negative if this item will be recycled as opposed to just being disabled
        // *For the method's full description check the base implementation
        /*
		protected override void OnBeforeRecycleOrDisableCellViewsHolder(MyGridItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
		{
			base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
		}
		*/
        #endregion

        // These are common data manipulation methods
        // The list containing the models is managed by you. The adapter only manages the items' sizes and the count
        // The adapter needs to be notified of any change that occurs in the data list. 
        // For GridAdapters, only Refresh and ResetItems work for now
        #region data manipulation
        public void AddItemsAt(int index, IList<MyGridItemModel> items)
        {
            //Commented: this only works with Lists. ATM, Insert for Grids only works by manually changing the list and calling NotifyListChangedExternally() after
            //Data.InsertItems(index, items);
            Data.List.InsertRange(index, items);
            Data.NotifyListChangedExternally();
        }

        public void RemoveItemsFrom(int index, int count)
        {
            //Commented: this only works with Lists. ATM, Remove for Grids only works by manually changing the list and calling NotifyListChangedExternally() after
            //Data.RemoveRange(index, count);
            Data.List.RemoveRange(index, count);
            Data.NotifyListChangedExternally();
        }

        public void SetItems(IList<MyGridItemModel> items)
        {
            Data.ResetItems(items);
        }
        #endregion


        // Here, we're requesting <count> items from the data source
        void RetrieveDataAndUpdate(JSONObject jSON)
        {
            StartCoroutine(FetchMoreItemsFromDataSourceAndUpdate(jSON));
        }

        // Retrieving <count> models from the data source and calling OnDataRetrieved after.
        // In a real case scenario, you'd query your server, your database or whatever is your data source and call OnDataRetrieved after
        IEnumerator FetchMoreItemsFromDataSourceAndUpdate(JSONObject jSON)
        {

            // Simulating data retrieving delay
            yield return new WaitForSeconds(.5f);

            var newItems = new MyGridItemModel[jSON.count];
            for (int i = 0; i < jSON.count; i++)
            {
                var request = new HTTPRequest(new Uri(jSON[i]["file_url"].stringValue), HTTPMethods.Get, onFinished);
                request.Send();
                void onFinished(HTTPRequest originalRequest, HTTPResponse response)
                {
                    Debug.Log(response.DataAsText);
                    File.WriteAllBytes(Application.persistentDataPath + "/" + Path.GetFileName(originalRequest.Uri.ToString()), response.Data);
                }

                var model = new MyGridItemModel()
                {

                    Room_name = jSON[i]["Room_name"].stringValue,
                    file_url = jSON[i]["file_url"].stringValue,
                    id = jSON[i]["id"].intValue,
                    map_url = jSON[i]["map_url"].stringValue

                };
                newItems[i] = model;

                //Debug.Log(m_trigger);
            }

            OnDataRetrieved(newItems);

        }

        void OnDataRetrieved(MyGridItemModel[] newItems)
        {
            //Commented: this only works with Lists. ATM, Insert for Grids only works by manually changing the list and calling NotifyListChangedExternally() after
            // Data.InsertItemsAtEnd(newItems);

           Data.ResetItems(newItems);   
            Data.NotifyListChangedExternally();
        }
    }


    // Class containing the data associated with an item
    public class MyGridItemModel
    {
        public string Room_name;
        public string file_url;
        public int id;
        public string map_url;
        /*
        public string title;
        public Color color;
        */
    }


    // This class keeps references to an item's views.
    // Your views holder should extend BaseItemViewsHolder for ListViews and CellViewsHolder for GridViews
    // The cell views holder should have a single child (usually named "Views"), which contains the actual 
    // UI elements. A cell's root is never disabled - when a cell is removed, only its "views" GameObject will be disabled
    public class MyGridItemViewsHolder : CellViewsHolder
    {
        public RawImage RoomMiniMap;
        public Image BackgroundImage, loadingP, rogress, overlayImage;
        public Text titleText, eta, name, count, describe;
        public Toggle toggle, like;
        public Button scence, delete;


        public override void CollectViews()
        {
            base.CollectViews();

            views.GetComponentAtPath("BackgroundImage", out BackgroundImage);
            views.GetComponentAtPath("BackgroundImage/mask/pic_file", out RoomMiniMap);
            views.GetComponentAtPath("delete", out delete);
            views.GetComponentAtPath("TitleText", out titleText);

        }

        /*
        public Text titleText;
        public Image backgroundImage;
        */


        // 从项的根GameObject中检索视图
        //      public override void CollectViews()
        //{
        //	base.CollectViews();

        //	// GetComponentAtPath 是 frame8.Logic.Misc.Other.Extensions 中的一个方便的扩展方法
        //	// 它通过类型推断变量的组件，因此您不需要自己指定它
        //	/*
        //	views.GetComponentAtPath("TitleText", out titleText);
        //	views.GetComponentAtPath("BackgroundImage", out backgroundImage);
        //	*/
        //}

        // 这通常是项的根的唯一子项，称为 "Views"。
        // 这是默认实现将要查找的内容，但为了灵活性，
        // 提供了此回调，以防它的名称不同或有多个子项
        // *有关更多信息，请参见 GridExample.cs
        /*
        protected override RectTransform GetViews()
        { return root.Find("Views").transform as RectTransform; }
        */

        // 如果有子布局组，请重写此方法。当调用此回调时，它们需要标记重建
        /*
        public override void MarkForRebuild()
        {
            base.MarkForRebuild();

            LayoutRebuilder.MarkLayoutForRebuild(yourChildLayout1);
            LayoutRebuilder.MarkLayoutForRebuild(yourChildLayout2);
            AChildSizeFitter.enabled = true;
        }
        */

        // 如果已经重写了 MarkForRebuild()，请重写此方法
        /*
        public override void UnmarkForRebuild()
        {
            AChildSizeFitter.enabled = false;

            base.UnmarkForRebuild();
        }
        */
    }
}
