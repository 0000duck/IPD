# IPD

## ABOUT

Inexbot PC Debugger 纳博特 pc 版调试软件。

.NET 6 / C# / WPF / Ribbon

项目介绍:[Blaze-PC调试软件(示例)](https://blaze.inexbot.com/docs/dotnet/ipd)

[Demo 下载](https://inexbot-use.oss-cn-shanghai.aliyuncs.com/ipd/net5.0-windows.zip)

<img src="https://blaze.inexbot.com/img/dotnet/ipd-2.png" />

## 项目说明

本项目使用部分 MVVM 模式，获取数据并更新界面采用 MVVM 模式 Binding 到控件，提交数据采用控件后台代码直接获取控件内容并发送的方式。

### 代码使用

1. clone 或下载项目；
2. 使用 Visual Studio 2022 或更新的版本打开项目 IPD.csproj
3. 生成并运行程序。

### 目录结构

| 目录/文件          | 作用                                       |
| ------------------ | ------------------------------------------ |
| IPD.csproj         | 项目文件                                   |
| App.xaml           | 程序主 xaml 文件                           |
| App.xaml           | 程序主 xaml 文件的后台代码，主要做资源引入 |
| MainWindow.xaml    | 主窗口 xaml 文件                           |
| MainWindow.xaml.cs | 主窗口后台代码                             |
| Resource.resx      | 资源文件                                   |
| Common             | Model、Command 等类型的基类                |
| Component          | 自定义控件后台代码                         |
| Converter          | 类型转换器                                 |
| Data               | 存储常用数据的 xml 文件                    |
| Dialogs            | 关于、点动、示波器等对话框                 |
| Model              | ViewModel，用来接收和保存控制器数据        |
| Resource           | 资源文件                                   |
| Tcp                | 网络通讯                                   |
| Themes             | 自定义控件的 xaml                          |
| Util               | 一些常用函数                               |
| View               | 各个界面                                   |

### 简要开发说明

- Ribbon

本项目采用了 FluentRibbon 库，像 Word 等软件一样上方有菜单导航栏，它在 View 下的 RibbonBar 目录下。

- 接收数据

接收数据需先在 Model 中的 HandleReceiveMessage.cs 文件中增加相应的命令字，并自行建立 ViewModel，在控件中绑定。

- 其它

请关注<strong>纳博特科技 Inexbot</strong>微信公众号，我们会在里面分享更多开发经验。

