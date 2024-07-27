# 路径修改器

一键修改桌面、文档等的系统路径。

# 定制
修改软件同目录下的`configuration.json`文件

|key|type|explain|
|:---:|:---:|:---|
|enable_gross_move|bool|是否将所有（以下6个）系统路径移动到某个特定目录(gross_move_floder)
|gross_move_floder|string|若上面项为真，则将系统目录放在这个目录下的Desktop、Music等子目录下
|auto_confirm|bool|若为真，且配置文件正确，则不进行确认操作
|desktop_path|string|桌面路径|
|video_path|string|视频路径|
|download_path|string|下载路径|
|document_path|string|文档路径|
|picture_path|string|图片路径|
|music_path|string|音乐路径|


# 亮点

软件小巧（通过.Net AOT实现）
