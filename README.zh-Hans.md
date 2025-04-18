[English](README.md) | [简体中文](README.zh-Hans.md)

<img src="branding/logo.png" />

# 抖音直播录制

[![GitHub license](https://img.shields.io/github/license/emako/TiktokLiveRec)](https://github.com/emako/TiktokLiveRec/blob/master/LICENSE) [![Actions](https://github.com/emako/TiktokLiveRec/actions/workflows/build.yml/badge.svg)](https://github.com/emako/TiktokLiveRec/actions/workflows/library.nuget.yml) [![Platform](https://img.shields.io/badge/platform-Windows-blue?logo=windowsxp&color=1E9BFA)](https://dotnet.microsoft.com/en-us/download/dotnet/latest/runtime) [![GitHub downloads](https://img.shields.io/github/downloads/emako/TiktokLiveRec/total)](https://github.com/emako/TiktokLiveRec/releases)
[![GitHub downloads](https://img.shields.io/github/downloads/emako/TiktokLiveRec/latest/total)](https://github.com/emako/TiktokLiveRec/releases)

具有用户界面、无人值守操作和直播流录制功能。

实现基于 FFmpeg 以及 FFplay。

## 截图

<img src="assets/image-20241113165448238.png" alt="image-20241113165448238" style="transform:scale(0.5);" />

## 依赖运行时

For Windows: [.NET Desktop Runtime 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

For Others: [.NET Runtime 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

## 直播录制

支持以下直播平台

| 平台              | 状态 |
| ----------------- | ---- |
| Douyin (中国抖音) | 支持 |
| Tiktok (海外抖音) | 支持 |

怎么添加直播间：

```bash
# 国内抖音直播间链接类似如下：
https://live.douyin.com/XXX
https://www.douyin.com/root/live/XXX

# 海外抖音直播间链接类似如下：
https://www.tiktok.com/@XXX/live
```

## 支持系统

为了加快初版开发实现，首版基于 WPF 开发了 Windows 版本。

其他系统的实现会基于我个人需求或其他用户的反响。

另外 macOS 估计会是下一个支持的系统。

| 操作系统 | 开发框架 | 状态   |
| -------- | -------- | ------ |
| Windows  | WPF      | 支持   |
| macOS    | Avalonia | 开发中 |
| Ubuntu   | Avalonia | 待开发 |
| Android  | Avalonia | 待开发 |
| iOS      | Avalonia | 待开发 |
| tvOS     | 待定     | 待开发 |

## 自有Cookie

来看看 [GETCOOKIE_DOUYIN.md](doc/GETCOOKIE_DOUYIN.md) 或 [GETCOOKIE_TIKTOK.md](doc/GETCOOKIE_TIKTOK.md)。

## 隐私政策

[查看隐私政策](PrivacyPolicy.zh-Hans.md)。

## 许可证

本项目基于 [MIT 许可证](LICENSE)。

## 鸣谢

为了节约后续维护成本，直接参考了部分来自 [DouyinLiveRecorder](https://github.com/ihmily/DouyinLiveRecorder) 的字符串数据比如正则表达式。