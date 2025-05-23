[English](README.md) | [简体中文](README.zh-Hans.md)

<img src="branding/logo.png" />

# Tiktok Live Rec

[![GitHub license](https://img.shields.io/github/license/emako/TiktokLiveRec)](https://github.com/emako/TiktokLiveRec/blob/master/LICENSE) [![Actions](https://github.com/emako/TiktokLiveRec/actions/workflows/build.yml/badge.svg)](https://github.com/emako/TiktokLiveRec/actions/workflows/library.nuget.yml) [![Platform](https://img.shields.io/badge/platform-Windows-blue?logo=windowsxp&color=1E9BFA)](https://dotnet.microsoft.com/en-us/download/dotnet/latest/runtime) [![GitHub downloads](https://img.shields.io/github/downloads/emako/TiktokLiveRec/total)](https://github.com/emako/TiktokLiveRec/releases)
[![GitHub downloads](https://img.shields.io/github/downloads/emako/TiktokLiveRec/latest/total)](https://github.com/emako/TiktokLiveRec/releases)

With a graphical UI, unattended operation, and live streaming recording capabilities.

Based on FFmpeg and FFplay.

## Screen Shot

<img src="assets/image-20241113165355466.png" alt="image-20241113165355466" style="transform:scale(0.5);" />

## Dependencies Runtime

For Windows: [.NET Desktop Runtime 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

For Others: [.NET Runtime 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

## Live Streaming

Support following live site.

| Site          | Status    |
| ------------- | --------- |
| Douyin (抖音) | Available |
| Tiktok        | Available |

How to add live room:

```bash
# Douyin room URL like following:
https://live.douyin.com/XXX
https://www.douyin.com/root/live/XXX

# Tiktok room URL like following:
https://www.tiktok.com/@XXX/live
```

## Support OS

For rapid development, first implement WPF-based windows support.

Implementing other OS's based on my personal needs or other user reactions.

BTW macOS may will be the next supported OS.

| OS      | Framework | Status            |
| ------- | --------- | ----------------- |
| Windows | WPF       | Available         |
| macOS   | Avalonia  | Under Development |
| Ubuntu  | Avalonia  | TBD               |
| Android | Avalonia  | TBD               |
| iOS     | Avalonia  | TBD               |
| tvOS    | TBD       | TBD               |

## Your Cookie Can

Check it from [GETCOOKIE_DOUYIN.md](doc/GETCOOKIE_DOUYIN.md) or [GETCOOKIE_TIKTOK.md](doc/GETCOOKIE_TIKTOK.md).

## Privacy Policy

See the [Privacy Policy](PrivacyPolicy.md).

## License

This project is licensed under the [MIT License](LICENSE).

## Thanks

To save maintenance costs, refer to the specific string data form [DouyinLiveRecorder](https://github.com/ihmily/DouyinLiveRecorder), just like regex and so on.

