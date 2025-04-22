using UnityEngine;
using System.Collections;
using System.IO;
using SimpleFileBrowser;

public class FileBrowserTest : MonoBehaviour
{
	// 警告：由FileBrowser对话框返回的路径不包含尾随'\'字符
	// 警告：FileBrowser一次只能显示1个对话框

	void Start()
	{
        // 设置过滤器（可选）
        // 如果所有对话框都将使用相同的过滤器，则仅需设置一次即可
        SimpleFileBrowser.FileBrowser.SetFilters(true, new SimpleFileBrowser.FileBrowser.Filter("Images", ".jpg", ".png"), new SimpleFileBrowser.FileBrowser.Filter("Text Files", ".txt", ".pdf"));

        // 设置在对话框显示时选择的默认过滤器（可选）
        // 如果默认过滤器设置成功，则返回true
        // 在这种情况下，将图像过滤器设置为默认过滤器
        SimpleFileBrowser.FileBrowser.SetDefaultFilter(".jpg");

		// 设置排除的文件扩展名（可选）（默认情况下，.lnk和.tmp扩展名被排除）
		// 请注意，当您使用此函数时，.lnk和.tmp扩展名将不再被排除，
		// 除非您将它们明确添加为函数参数
		SimpleFileBrowser.FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

		// 向浏览器添加新的快捷方式（可选）（如果成功添加了快捷方式，则返回true）
		// 只需要添加一次快捷方式即可
		// 名称：Users
		// 路径：C:\Users
		// 图标：默认（文件夹图标）
		SimpleFileBrowser.FileBrowser.AddQuickLink("Users", "C:\\Users", null);

		// 显示一个保存文件对话框
		// onSuccess事件：未注册（这意味着此对话框非常无用）
		// onCancel事件：未注册
		// Save file/folder：file，Allow multiple selection：false
		// 初始路径："C:\"，初始文件名："Screenshot.png"
		// 标题："Save As"，提交按钮文本："Save"
		// FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\", "Screenshot.png", "Save As", "Save" );

		// 显示选择文件夹对话框
		// onSuccess 事件：打印所选文件夹的路径
		// onCancel 事件：打印"Canceled"
		// Load file/folder：folder，Allow multiple selection：false
		// 初始路径：默认（Documents），初始文件名：empty
		// 标题："Select Folder"，提交按钮文本："Select"
		// FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "Selected: " + paths[0] ); },
		//						   () => { Debug.Log( "Canceled" ); },
		//						   FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select" );

		// 协程示例
		// StartCoroutine(ShowLoadDialogCoroutine());
	}

	IEnumerator ShowLoadDialogCoroutine()
	{
		// 显示加载文件对话框并等待用户响应
		// Load file/folder：both，Allow multiple selection：true
		// 初始路径：默认（Documents），初始文件名：empty
		// 标题："Load File"，提交按钮文本："Load"
		yield return SimpleFileBrowser.FileBrowser.WaitForLoadDialog(SimpleFileBrowser.FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

		// 对话框已关闭
		// 打印用户是选择了某些文件/文件夹还是取消了操作（SimpleFileBrowser.FileBrowser.Success）
		Debug.Log(SimpleFileBrowser.FileBrowser.Success);

		if (SimpleFileBrowser.FileBrowser.Success)
		{
			// 打印所选文件的路径（SimpleFileBrowser.FileBrowser.Result）（如果SimpleFileBrowser.FileBrowser.Success为false，则为null）
			for (int i = 0; i < SimpleFileBrowser.FileBrowser.Result.Length; i++)
				Debug.Log(SimpleFileBrowser.FileBrowser.Result[i]);

            //// 通过FileBrowserHelpers读取第一个文件的字节
            //// 与File.ReadAllBytes相反，此功能也适用于Android 10+
            //byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

            //// 或者，将第一个文件复制到persistentDataPath
            //string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            //FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
        }
	}
}
