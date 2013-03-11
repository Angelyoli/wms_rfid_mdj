; �ű��� Inno Setup �ű��� ���ɡ�
; �����ĵ���ȡ���� INNO SETUP �ű��ļ���ϸ���ϣ�

#define MyAppName "���ӱ�ǩ����"
#define MyAppVerName "���ӱ�ǩ���� 1.5"
#define MyAppPublisher "�캣ŷ���Ƽ���Ϣ�����ţ����޹�˾"
#define MyAppURL "http://www.skyseaok.com/"
#define MyAppExeName "LabelServer.exe"

[Setup]
; ע��: AppId ��ֵ��Ψһʶ���������ı�־��
; ��Ҫ������������ʹ����ͬ�� AppId ֵ��
; (�ڱ������е���˵������� -> ���� GUID�����Բ���һ���µ� GUID)
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
SetupIconFile=D:\����Ŀ_�ֳ���\�ϳ���Ŀ\���ӱ�ǩϵͳ\LabelServer\LabelServer\2.ico
Password=thok
Compression=lzma
SolidCompression=yes

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "D:\����Ŀ_�ֳ���\�ϳ���Ŀ\���ӱ�ǩϵͳ\LabelServer\LabelServer\bin\Debug\LabelServer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\����Ŀ_�ֳ���\�ϳ���Ŀ\���ӱ�ǩϵͳ\LabelServer\LabelServer\bin\Debug\LabelServer.vshost.exe.manifest"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\����Ŀ_�ֳ���\�ϳ���Ŀ\���ӱ�ǩϵͳ\LabelServer\LabelServer\bin\Debug\OK.ELabel.DLL"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\����Ŀ_�ֳ���\�ϳ���Ŀ\���ӱ�ǩϵͳ\LabelServer\LabelServer\bin\Debug\UpgradeLog.XML"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\����Ŀ_�ֳ���\�ϳ���Ŀ\���ӱ�ǩϵͳ\LabelServer\LabelServer\bin\Debug\LabelServer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\����Ŀ_�ֳ���\�ϳ���Ŀ\���ӱ�ǩϵͳ\LabelServer\LabelServer\bin\Debug\LabelServer.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\����Ŀ_�ֳ���\�ϳ���Ŀ\���ӱ�ǩϵͳ\LabelServer\LabelServer\bin\Debug\LabelServer.sln"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\����Ŀ_�ֳ���\�ϳ���Ŀ\���ӱ�ǩϵͳ\LabelServer\LabelServer\bin\Debug\LabelServer.suo"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\����Ŀ_�ֳ���\�ϳ���Ŀ\���ӱ�ǩϵͳ\LabelServer\LabelServer\bin\Debug\LabelServer.vshost.exe"; DestDir: "{app}"; Flags: ignoreversion
; ע��: ��Ҫ���κι����ϵͳ�ļ�ʹ�� "Flags: ignoreversion" 

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent
