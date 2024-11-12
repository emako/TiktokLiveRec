[English](README.md) | [简体中文](README.zh-Hans.md)

# Tiktok Live Rec

[![GitHub license](https://img.shields.io/github/license/emako/TiktokLiveRec)](https://github.com/emako/TiktokLiveRec/blob/master/LICENSE) [![Actions](https://github.com/emako/TiktokLiveRec/actions/workflows/build.yml/badge.svg)](https://github.com/emako/TiktokLiveRec/actions/workflows/library.nuget.yml) [![Platform](https://img.shields.io/badge/platform-Windows-blue?logo=windowsxp&color=1E9BFA)](https://dotnet.microsoft.com/en-us/download/dotnet/latest/runtime)

With a graphical UI, unattended operation, and live streaming recording capabilities.

Based on FFmpeg and FFplay.

## Screen Shot

<img src="assets/image-20241111161919039.png" alt="image-20241111161919039" style="zoom: 33%;" />

## Dependencies Runtime

For Windows: [.NET Desktop Runtime 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

For Others: [.NET Runtime 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Live Streaming

Support following live site.

| Site          | Status    |
| ------------- | --------- |
| Douyin (抖音) | Available |
| Tiktok        | TBD       |

## Support OS

For rapid development, first implement WPF-based windows support.

Implementing other OS's based on my personal needs or other user reactions.

BTW macOS may will be the next supported OS.

| OS      | Framework         | Status    |
| ------- | ----------------- | --------- |
| Windows | WPF               | Available |
| macOS   | Avalonia          | TBD       |
| Ubuntu  | Avalonia/WPF+Wice | TBD       |

## Privacy Policy

See the [Privacy Policy](PrivacyPolicy.md).

## License

This project is licensed under the [MIT License](LICENSE).

## Thanks

To save maintenance costs, refer to the specific string data form [DouyinLiveRecorder](https://github.com/ihmily/DouyinLiveRecorder), just like regex and so on.

