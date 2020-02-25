# Quick Editor ToolKit for Unity

## Overview

Unity编辑器工具库

### Runtime Environment (运行环境)

Version : Unity 2018.x ~ 2020.x

### Preview (预览)
![image]()

## How to use?

[Wiki文档](https://github.com/henry-yuxi/QuickEditor.ToolKit/wiki)

[更新日志](https://github.com/henry-yuxi/QuickEditor.ToolKit/wiki/版本更新)

## How to install?

Unity can not install dependent packages automatically at present, you have to install them manually:
https://github.com/henry-yuxi/QuickEditor.Common

### UPM Install via manifest.json

In Packages folder, you will see a file named manifest.json. 

using this package add lines into ./Packages/manifest.json like next sample:
```
{
  "dependencies": {
    "com.sourcemuch.quickeditor.toolkit": "https://github.com/henry-yuxi/QuickEditor.ToolKit.git#0.0.1",
  }
}
```

### Unity 2019.3 Git URL

In Unity 2019.3 or greater, Package Manager is include the new feature that able to install the package via Git.

Open the package manager window (menu: Window > Package Manager), select "Add package from git URL...", fill in this in the pop-up textbox: 
https://github.com/henry-yuxi/QuickEditor.ToolKit.git#0.0.1.


### Unity UPM Git Extension (For 2019.2 and older version)

If you doesn't have this package before, please redirect to this git https://github.com/mob-sakai/UpmGitExtension then follow the instruction in README.md to install the UPM Git Extension to your Unity.

If you already installed. Open the Package Manager UI, you will see the git icon around the bottom left connor, Open it then follow the instruction using this git URL to perform package install.

请确保使用的UPM包是最终版本。

### Package URL's
| Version  |     Link      |
|----------|---------------|
<!--
| 0.0.1 | https://github.com/henry-yuxi/QuickEditor.ToolKit.git#0.0.1 |
-->

## See Also
GitHub Page : https://github.com/henry-yuxi/QuickEditor.ToolKit/

Issue tracker : https://github.com/henry-yuxi/QuickEditor.ToolKit/issues

## Dpendent plugins (依赖的插件)
<!--
[Odin] (https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041)
-->

## Include Projects (包含项目):

[hananoki: EditorToolbar](https://github.com/hananoki/EditorToolbar)

[marijnz: unity-toolbar-extender](https://github.com/marijnz/unity-toolbar-extender)



