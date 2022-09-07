<center>Language switch to: <a href="README.md">English</a></center>

# 向向的数独解决方案

## 正文

### 简介

一个使用暴力破解和普通逻辑算法解题的数独分析解题 SDK。目前该解决方案支持对数独的题目生成、使用逻辑技巧或无逻辑技巧解题和一些常见数独特性的验证（例如，验证是否一个指定的盘面是一个最小题目。所谓的最小题目指的是，盘面任意一个数字消失后，都会使得题目多解的题）。

比如说，你可以使用如下的代码来解一道题：

```csharp
using System;
using Sudoku.Collections;
using Sudoku.Solving.Manual;

// 读取一个字符串形式的数独盘面的代码信息，并解析为 'Grid' 类型的对象。
var grid = Grid.Parse("........6.....158...8.4.21.5..8..39.6.1.7.8.5.89..5..1.24.5.9...659.....9........");

// 声明实例化一个 'ManualSolver' 类型的实例，用于稍后的解题。
var solver = new ManualSolver();

// 以同步的形式解题。
var analysisResult = solver.Solve(grid);

// 你也可以转换数据类型为 ManualSolverResult 类型，以查看内部更多的数据信息。
//var analysisResultConverted = (ManualSolverResult)analysisResult;

// 输出分析结果。
// 你也可以使用 ToString 代替 ToDisplayString，它们底层是一样的。
Console.WriteLine(analysisResult.ToDisplayString());
//Console.WriteLine(analysisResult.ToString()); // 一样的。
//Console.WriteLine(analysisResultConverted.ToDisplayString()); // 一样的。
//Console.WriteLine(analysisResultConverted.ToString()); // 一样的。
```

以后，我想把这个解决方案用于**几乎所有平台**上。我可能会完成 Win10 App 项目、安卓项目、常用网络平台上的机器人（比如可能 QQ 啊，哔哩哔哩之类的）。

### UI 界面

![](docs/pic/win-ui.png)

程序还在完成之中！

### 关于该仓库的克隆仓库（Fork）以及代码更新推并请求（Pull Requests）

当然，你可以复制这个仓库到你的账号下，然后做你想做的任何事情。你可以在基于 [MIT](https://github.com/SunnieShine/Sudoku/blob/main/LICENSE) 开源协议的情况下做你任何想做的事情。不过，由于 Gitee 是从 GitHub 拷贝过来的，所以 Gitee 项目暂时不支持任何的代码推并请求，敬请谅解；不过这两个仓库都可以创建 issue。详情请参考下面的“基本信息”一栏的内容。

另外，这个仓库可能会更新得**非常频繁**（大概一天至少一次代码提交），而仓库备份到 Gitee 则大约是一天到两天一次。

实际上，每天至少一次更新的内容多数都是在重构代码，API 更进其实确实比较少。不过，数独游戏这种东西要想模拟人工解题算法的话，写代码的话就不容易看懂。我非常注重代码的整洁、代码的可读性，所以我要权衡算法的性能和可读性，找到一个平衡点。总之，敬请期待吧。

### 基本信息

请查看下面的表格获取更多信息。

| 项目地址 |                                                             | 备注                                                 |
| -------- | ----------------------------------------------------------- | ---------------------------------------------------- |
| GitHub   | [SunnieShine/Sudoku](https://github.com/SunnieShine/Sudoku) |                                                      |
| Gitee    | [SunnieShine/Sudoku](https://gitee.com/SunnieShine/Sudoku)  | 这个仓库从 GitHub 拷贝和同步过来的，是一个备份仓库。 |

| 百科页面 |                                                |
| -------- | ---------------------------------------------- |
| 中文介绍 | [GitHub](https://sunnieshine.github.io/Sudoku) |

> 我很遗憾我并未创建英文版的 Wiki 内容，因为工程量太大了。我学了很多年的英语，但是对于一些描述（尤其是细节的表达）要翻译成英语仍然有点困难。

| 编码信息       |                                       |
| -------------- | ------------------------------------- |
| 编程语言和版本 | C# 10                                  |
| 框架           | .NET 6                                |
| 缩进           | Tab（制表符）                          |
| 集成开发环境   | Visual Studio 2022（17.2 预览版 2.1）   |
| 自然语言支持   | 英语、简体中文                           |
| 用户接口       | `Sudoku.UI`                           |

> 请注意，编程语言的版本我一直用的是预览版，也就意味着即使有些语言特性不属于我上方给出的版本支持的特性，但是项目也在使用它们。
>
> 当然，你也可以使用 JetBrains 的 Rider 作为你的 IDE 来开发。随便你用什么都行，甚至是记事本（大笑）。虽说 C# 拥有一些只能在 VS 上使用的语法（`__makeref` 之类的关键字），不过这个项目里没有使用这些内容，因此你可以大大方方地使用别的 IDE 开发。
>
> 另外，框架和 IDE 使用版本可能在以后会继续更新。换句话说，它们并非一直都不变。这些信息**仅供参考**。
>
> MVVM 框架对我来说有些复杂，我还在学习期间，所以如果上手使用 MVVM 会影响我设计程序的进度，这有点太慢了，所以我暂时不打算考虑用这个框架进行架构的定义和设计。

### 完成列表

* [x] API
* [ ] UI 项目
  * [ ] 桌面项目
    * [x] ~~WPF 项目（删掉了，因为过时了）~~
    * [ ] ~~UWP 项目（不会考虑）~~
    * [x] ~~Winform 项目（删掉了，因为过时了）~~
    * [ ] **Windows UI（我正在做这个）**
  * [ ] MAUI 项目
* [ ] 一些常见平台的机器人
  * [ ] 哔哩哔哩（依赖官方 API）
  * [x] ~~QQ（删掉了，因为过时了）~~
  * [ ] 微信
  * [ ] 其它内容……
* [ ] Wiki 文档
  * [ ] 基本文档
  * [ ] 数独教程

### 项目开源许可证

[麻省理工开源许可证](https://github.com/SunnieShine/Sudoku/blob/main/LICENSE)

### 数独技巧参考

我列举一些我这个解决方案里用到和参考的数独技巧网站。这些网站内容都是我自己写和出品的，所以如果你想要了解数独技巧的具体细节，你可以参考这些链接来了解它们。是中文链接。

* [标准数独技巧教程（视频）_bilibili](https://www.bilibili.com/video/BV1Mx411z7uq)
* [标准数独技巧教程（专栏）_bilibili](https://www.bilibili.com/read/readlist/rl291187)

### 作者

小向，来自成都的一名四川~~普通大学~~师范大学的本科大学生。

