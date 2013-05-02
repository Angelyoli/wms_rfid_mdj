; 脚本用 Inno Setup 脚本向导 生成。
; 查阅文档获取创建 INNO SETUP 脚本文件详细资料！

#define MyAppName "电子标签驱动"
#define MyAppVerName "电子标签驱动 1.5"
#define MyAppPublisher "天海欧康科技信息（厦门）有限公司"
#define MyAppURL "http://www.skyseaok.com/"
#define MyAppExeName "LabelServer.exe"

[Setup]
; 注意: AppId 的值是唯一识别这个程序的标志。
; 不要在其他程序中使用相同的 AppId 值。
; (在编译器中点击菜单“工具 -> 产生 GUID”可以产生一个新的 GUID)
AppId={{4A56F41A-306D-4376-9822-46B8700155DE}
AppName={#MyAppName}
AppVerName={#MyAppVerName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputDir=D:\
OutputBaseFilename=setup
SetupIconFile=D:\［项目_现场］\南充项目\电子标签系统\LabelServer\LabelServer\2.ico
Password=thok
Compression=lzma
SolidCompression=yes

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "D:\［项目_现场］\南充项目\电子标签系统\LabelServer\LabelServer\bin\Debug\LabelServer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\［项目_现场］\南充项目\电子标签系统\LabelServer\LabelServer\bin\Debug\LabelServer.vshost.exe.manifest"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\［项目_现场］\南充项目\电子标签系统\LabelServer\LabelServer\bin\Debug\OK.ELabel.DLL"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\［项目_现场］\南充项目\电子标签系统\LabelServer\LabelServer\bin\Debug\UpgradeLog.XML"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\［项目_现场］\南充项目\电子标签系统\LabelServer\LabelServer\bin\Debug\LabelServer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\［项目_现场］\南充项目\电子标签系统\LabelServer\LabelServer\bin\Debug\LabelServer.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\［项目_现场］\南充项目\电子标签系统\LabelServer\LabelServer\bin\Debug\LabelServer.sln"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\［项目_现场］\南充项目\电子标签系统\LabelServer\LabelServer\bin\Debug\LabelServer.suo"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\［项目_现场］\南充项目\电子标签系统\LabelServer\LabelServer\bin\Debug\LabelServer.vshost.exe"; DestDir: "{app}"; Flags: ignoreversion
; 注意: 不要在任何共享的系统文件使用 "Flags: ignoreversion" 

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent
