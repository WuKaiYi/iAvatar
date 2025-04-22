# 8. API参考

本文档提供LearningVerse平台的API参考，包括核心API、扩展API和使用示例。

## 核心API
核心API提供系统基础功能的接口，包括用户管理、形象控制、空间管理等功能。

### 用户API
用户管理相关的API接口。

```csharp
// 用户登录
async Task<UserInfo> Login(string username, string password);

// 用户注册
async Task<bool> Register(string username, string password, UserProfile profile);

// 获取用户信息
async Task<UserInfo> GetUserInfo(string userId);

// 更新用户信息
async Task<bool> UpdateUserInfo(string userId, UserProfile profile);
```

### 形象API
虚拟形象相关的API接口。

```csharp
// 获取用户形象
async Task<AvatarData> GetUserAvatar(string userId);

// 更新用户形象
async Task<bool> UpdateUserAvatar(string userId, AvatarData avatarData);

// 导入形象模型
async Task<bool> ImportAvatarModel(string userId, string modelUrl);

// 导出形象模型
async Task<string> ExportAvatarModel(string userId, ExportFormat format);
```

### 动作API
动作捕捉相关的API接口。

```csharp
// 初始化动作捕捉
void InitializeMotionCapture(MotionCaptureSettings settings);

// 开始动作捕捉
void StartMotionCapture();

// 停止动作捕捉
void StopMotionCapture();

// 获取当前姿态数据
PoseData GetCurrentPose();
```

## 扩展API
扩展API提供系统高级功能的接口，包括AI对话、网络通信等功能。

### AI对话API
AI对话相关的API接口。

```csharp
// 发送对话消息
async Task<AIResponse> SendMessage(string userId, string message);

// 获取对话历史
async Task<List<Message>> GetChatHistory(string userId, int count);

// 清除对话历史
async Task<bool> ClearChatHistory(string userId);

// 设置AI对话参数
async Task<bool> SetAIParameters(string userId, AIParameters parameters);
```

### 网络通信API
网络通信相关的API接口。

```csharp
// 创建房间
async Task<string> CreateRoom(RoomSettings settings);

// 加入房间
async Task<bool> JoinRoom(string roomId, string userId);

// 离开房间
async Task<bool> LeaveRoom(string roomId, string userId);

// 发送消息
async Task<bool> SendNetworkMessage(string roomId, string userId, NetworkMessage message);
```

## 使用示例
以下示例展示如何使用LearningVerse API进行开发。

### 基础使用示例
```csharp
// 登录并获取用户信息
async Task LoginAndGetInfo(string username, string password)
{
    UserInfo user = await Login(username, password);
    if (user != null)
    {
        Debug.Log($"成功登录，用户ID：{user.UserId}");
        
        // 获取用户形象
        AvatarData avatar = await GetUserAvatar(user.UserId);
        
        // 初始化动作捕捉
        InitializeMotionCapture(new MotionCaptureSettings 
        { 
            UseCamera = true,
            SmoothFactor = 0.5f
        });
    }
}
```

### 高级使用示例
```csharp
// 创建房间并邀请用户
async Task CreateRoomAndInvite(string userId, List<string> invitees)
{
    // 创建房间
    string roomId = await CreateRoom(new RoomSettings
    {
        Name = "测试房间",
        MaxUsers = 10,
        IsPrivate = true
    });
    
    // 邀请用户
    foreach (string invitee in invitees)
    {
        await InviteToRoom(roomId, userId, invitee);
    }
    
    // 发送欢迎消息
    await SendNetworkMessage(roomId, userId, new NetworkMessage
    {
        Type = MessageType.Chat,
        Content = "欢迎加入房间！"
    });
}
```

## 自定义工具API
自定义工具API提供工具开发、管理和使用相关的接口。

### 工具开发API
工具开发相关的API接口。

```csharp
// 注册工具
bool RegisterTool(ToolInfo toolInfo, Type toolType);

// 工具生命周期接口
interface ICustomTool
{
    // 初始化工具
    void Initialize(ToolContext context);
    
    // 工具执行
    void Execute(ToolParameters parameters);
    
    // 工具清理
    void Cleanup();
    
    // 工具配置界面
    void OnGUI();
    
    // 工具事件处理
    void OnEvent(ToolEvent toolEvent);
}

// 工具配置接口
interface IToolSettings
{
    // 获取默认设置
    ToolSettings GetDefaultSettings();
    
    // 保存设置
    bool SaveSettings(ToolSettings settings);
    
    // 加载设置
    ToolSettings LoadSettings();
    
    // 重置设置
    void ResetSettings();
}
```

### 工具管理API
工具管理相关的API接口。

```csharp
// 安装工具
async Task<bool> InstallTool(string toolPackageUrl);

// 卸载工具
bool UninstallTool(string toolId);

// 获取已安装工具列表
List<ToolInfo> GetInstalledTools();

// 检查工具更新
async Task<List<ToolUpdateInfo>> CheckForUpdates();

// 更新工具
async Task<bool> UpdateTool(string toolId);
```

### 工具使用API
工具使用相关的API接口。

```csharp
// 获取工具实例
ICustomTool GetToolInstance(string toolId);

// 执行工具
async Task<ToolResult> ExecuteTool(string toolId, ToolParameters parameters);

// 设置工具参数
bool SetToolParameters(string toolId, ToolParameters parameters);

// 获取工具参数
ToolParameters GetToolParameters(string toolId);

// 保存工具配置预设
bool SaveToolPreset(string toolId, string presetName, ToolParameters parameters);

// 加载工具配置预设
ToolParameters LoadToolPreset(string toolId, string presetName);
```

### 工具市场API
工具市场相关的API接口。

```csharp
// 搜索工具
async Task<List<ToolInfo>> SearchTools(string keyword, string category, float minRating);

// 获取工具详情
async Task<ToolDetails> GetToolDetails(string toolId);

// 评价工具
async Task<bool> RateTool(string toolId, int rating, string comment);

// 发布工具
async Task<string> PublishTool(ToolPackage toolPackage);
```

### 自定义工具示例
以下示例展示如何使用自定义工具API。

```csharp
// 创建自定义工具
public class MyCustomTool : MonoBehaviour, ICustomTool, IToolSettings
{
    // 工具信息
    private ToolInfo toolInfo = new ToolInfo
    {
        Id = "my_custom_tool",
        Name = "我的自定义工具",
        Description = "这是一个示例工具",
        Version = "1.0.0",
        Category = "Utility"
    };
    
    // 工具设置
    private ToolSettings settings = new ToolSettings();
    
    // 工具上下文
    private ToolContext context;
    
    // 初始化
    void Start()
    {
        // 注册工具
        ToolManager.RegisterTool(toolInfo, typeof(MyCustomTool));
    }
    
    // 实现ICustomTool接口
    public void Initialize(ToolContext context)
    {
        this.context = context;
        settings = LoadSettings();
        Debug.Log("工具已初始化");
    }
    
    public void Execute(ToolParameters parameters)
    {
        Debug.Log("工具正在执行");
        // 工具逻辑
    }
    
    public void Cleanup()
    {
        SaveSettings(settings);
        Debug.Log("工具已清理");
    }
    
    public void OnGUI()
    {
        // 绘制工具配置界面
    }
    
    public void OnEvent(ToolEvent toolEvent)
    {
        // 处理工具事件
    }
    
    // 实现IToolSettings接口
    public ToolSettings GetDefaultSettings()
    {
        return new ToolSettings();
    }
    
    public bool SaveSettings(ToolSettings settings)
    {
        PlayerPrefs.SetString(toolInfo.Id + "_settings", JsonUtility.ToJson(settings));
        return true;
    }
    
    public ToolSettings LoadSettings()
    {
        string json = PlayerPrefs.GetString(toolInfo.Id + "_settings", "");
        if (string.IsNullOrEmpty(json))
        {
            return GetDefaultSettings();
        }
        return JsonUtility.FromJson<ToolSettings>(json);
    }
    
    public void ResetSettings()
    {
        settings = GetDefaultSettings();
        SaveSettings(settings);
    }
}
```

## 空间管理API
空间管理API提供虚拟空间的创建、配置、管理和分享相关的接口。

### 空间创建API
空间创建相关的API接口。

```csharp
// 创建空间
async Task<string> CreateSpace(SpaceSettings settings);

// 复制空间
async Task<string> CloneSpace(string sourceSpaceId, SpaceSettings newSettings);

// 删除空间
async Task<bool> DeleteSpace(string spaceId);

// 获取空间模板
async Task<List<SpaceTemplate>> GetSpaceTemplates(string category);
```

### 空间配置API
空间配置相关的API接口。

```csharp
// 获取空间设置
async Task<SpaceSettings> GetSpaceSettings(string spaceId);

// 更新空间设置
async Task<bool> UpdateSpaceSettings(string spaceId, SpaceSettings settings);

// 设置空间环境
async Task<bool> SetSpaceEnvironment(string spaceId, EnvironmentSettings environment);

// 设置空间背景音乐
async Task<bool> SetSpaceBackgroundMusic(string spaceId, string musicUrl);
```

### 空间管理API
空间管理相关的API接口。

```csharp
// 获取用户空间列表
async Task<List<SpaceInfo>> GetUserSpaces(string userId);

// 获取空间详情
async Task<SpaceDetails> GetSpaceDetails(string spaceId);

// 添加空间物体
async Task<string> AddSpaceObject(string spaceId, SpaceObject obj);

// 移除空间物体
async Task<bool> RemoveSpaceObject(string spaceId, string objectId);

// 移动空间物体
async Task<bool> MoveSpaceObject(string spaceId, string objectId, Vector3 position, Quaternion rotation);

// 获取空间使用统计
async Task<SpaceUsageStats> GetSpaceUsageStats(string spaceId, DateTime startDate, DateTime endDate);
```

### 空间访问API
空间访问相关的API接口。

```csharp
// 进入空间
async Task<bool> EnterSpace(string spaceId, string userId);

// 离开空间
async Task<bool> LeaveSpace(string spaceId, string userId);

// 获取空间当前用户
async Task<List<UserInfo>> GetSpaceUsers(string spaceId);

// 检查用户是否有权访问空间
async Task<bool> CanAccessSpace(string spaceId, string userId);
```

### 空间分享API
空间分享相关的API接口。

```csharp
// 分享空间
async Task<string> ShareSpace(string spaceId, ShareSettings settings);

// 获取分享链接
async Task<string> GetShareLink(string spaceId);

// 邀请用户到空间
async Task<bool> InviteToSpace(string spaceId, string inviterId, string inviteeId);

// 设置空间权限
async Task<bool> SetSpacePermissions(string spaceId, string userId, SpacePermissions permissions);
```

### 空间协作API
空间协作相关的API接口。

```csharp
// 锁定空间物体（编辑锁定）
async Task<bool> LockSpaceObject(string spaceId, string objectId, string userId);

// 解锁空间物体
async Task<bool> UnlockSpaceObject(string spaceId, string objectId, string userId);

// 获取版本历史
async Task<List<SpaceVersion>> GetSpaceVersionHistory(string spaceId);

// 恢复到指定版本
async Task<bool> RestoreSpaceVersion(string spaceId, string versionId);
```

### 空间管理示例
以下示例展示如何使用空间管理API。

```csharp
// 创建和配置自定义空间
async Task CreateAndConfigureSpace(string userId)
{
    // 创建空间
    string spaceId = await CreateSpace(new SpaceSettings
    {
        Name = "我的会议室",
        Description = "用于团队会议的虚拟空间",
        IsPrivate = true,
        MaxUsers = 20,
        TemplateId = "meeting_room_01"
    });
    
    // 配置空间环境
    await SetSpaceEnvironment(spaceId, new EnvironmentSettings
    {
        SkyboxType = SkyboxType.Daytime,
        LightingIntensity = 0.8f,
        AmbientSounds = "ambient_office_01"
    });
    
    // 添加空间物体
    string tableId = await AddSpaceObject(spaceId, new SpaceObject
    {
        Type = SpaceObjectType.Furniture,
        PrefabId = "conference_table_01",
        Position = new Vector3(0, 0, 0),
        Rotation = Quaternion.identity,
        Scale = new Vector3(1, 1, 1)
    });
    
    // 设置空间权限
    await SetSpacePermissions(spaceId, "user_group_01", new SpacePermissions
    {
        CanView = true,
        CanEdit = false,
        CanInvite = false,
        CanManage = false
    });
    
    // 生成分享链接
    string shareLink = await GetShareLink(spaceId);
    Debug.Log($"空间创建成功，分享链接：{shareLink}");
}

// 进入空间并与物体交互
async Task EnterAndInteractWithSpace(string userId, string spaceId)
{
    // 进入空间
    bool success = await EnterSpace(spaceId, userId);
    if (success)
    {
        Debug.Log("成功进入空间");
        
        // 获取空间物体列表
        List<SpaceObject> objects = await GetSpaceObjects(spaceId);
        
        // 与物体交互
        foreach (SpaceObject obj in objects)
        {
            if (obj.Type == SpaceObjectType.Interactive)
            {
                await InteractWithObject(spaceId, obj.Id, userId);
            }
        }
        
        // 离开空间
        await LeaveSpace(spaceId, userId);
    }
}
``` 