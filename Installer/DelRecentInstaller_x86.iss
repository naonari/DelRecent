; -- DelRecentInstaller.iss --
[Setup]
AppName=DelRecent
AppId=Growup_Consultant/Software/DelRecent
AppPublisher=Growup Consultant
AppCopyright=Growup Consultant
AppVerName=DelRecent 1.0.0.0
AppVersion=1.0.0.0
ArchitecturesAllowed=x86 x64
DefaultDirName={pf}\DelRecent
DefaultGroupName=DelRecent
UninstallDisplayIcon={app}\DelRecent.exe
ShowLanguageDialog=no
VersionInfoVersion=1.0.0.0
VersionInfoDescription=DelRecentセットアッププログラム

OutputBaseFilename=DelRecentInstaller_x86
OutputDir="."
SetupIconFile="..\DelRecent\DelRecent.ico"

[Files]
Source: "..\DelRecent\bin\Release\DelRecent.exe"; DestDir: "{app}"; Flags: ignoreversion

[Languages]
Name: japanese; MessagesFile: compiler:Languages\Japanese.isl