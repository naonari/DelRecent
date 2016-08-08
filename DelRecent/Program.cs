using System;
using System.IO;
using Microsoft.Win32;

namespace DelRecent
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 最近使用したファイルを削除します。
            DeleteRecentFiles();

            // クイックアクセスに最近使用したファイルを表示させないように設定します。
            SetHiedRecentKey();

            // エクスプローラーで開くを「PC」に設定します。
            SetExplorerAdvancedLaunchTo();
        }

        /// <summary>
        /// 最近使用したファイルを削除します。
        /// </summary>
        private static void DeleteRecentFiles()
        {
            // 最近使用したフォルダのパスを取得します。
            var recentDir = Environment.GetFolderPath(Environment.SpecialFolder.Recent);

            // フォルダ内のファイルを走査します。
            Array.ForEach(Directory.GetFiles(recentDir), p =>
            {
                // ファイル情報クラスをインスタンス化します。
                var fi = new FileInfo(p);

                try
                {
                    // 読み取り専用属性がある場合は、読み取り専用属性を解除します。
                    if ((fi.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        fi.Attributes = FileAttributes.Normal;
                    }
                    // ファイルを削除します。
                    fi.Delete();
                }
                catch
                {
                    //例外処理は行いません。
                }
            });
        }

        // OSのバージョン情報が格納されたレジストリーキー。
        private static readonly string CURRENT_VERSION_KEY = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion";

        // OSのバージョン情報が格納されたレジストリ名称。
        private static readonly string CURRENT_VERSION_NAME = "CurrentMajorVersionNumber";

        // クイックアクセスに最近使用したファイルを表示させる値のレジストリーキー。
        private static readonly string SHOW_RECENT_KEY = "ShowRecent";

        // クイックアクセスに最近使用したファイルを表示させる値のレジストリーのサブキー。
        private static readonly string SHOW_RECENT_SUB_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer";

        // 無効値。
        private static readonly int DISABLE_VALUE = 0;

        /// <summary>
        /// クイックアクセスに最近使用したファイルを表示させないように設定します。
        /// </summary>
        private static void SetHiedRecentKey()
        {
            // OSのバージョンを取得します。
            var osMajorVersionObj = Registry.GetValue(CURRENT_VERSION_KEY, CURRENT_VERSION_NAME, "0");

            // windows10以下のOSの場合は処理を終了します。
            if (!(osMajorVersionObj is int) || (int)osMajorVersionObj < 10) return;

            try
            {
                // レジストリを取得します。
                using (var regKey = Registry.CurrentUser.CreateSubKey(SHOW_RECENT_SUB_KEY))
                {
                    // レジストリーの値を判定します。
                    if (!DISABLE_VALUE.Equals(regKey.GetValue(SHOW_RECENT_KEY)))
                    {
                        // レジストリーに無効値を設定します。
                        regKey.SetValue(SHOW_RECENT_KEY, DISABLE_VALUE, RegistryValueKind.DWord);
                    }

                    // レジストリーを閉じます。
                    regKey.Close();
                }
            }
            catch
            {
                return;
            }
        }

        // エクスプローラーで開くの設定値のレジストリーキー。
        private static readonly string ADVANCED_LAUNCHTO_KEY = "LaunchTo";

        // エクスプローラーで開くの設定値のレジストリーのサブキー。
        private static readonly string ADVANCED_LAUNCHTO_SUB_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";

        // PC設定値。
        private static readonly string PC_VALUE = "1";

        /// <summary>
        /// エクスプローラーで開くを「PC」に設定します。
        /// </summary>
        private static void SetExplorerAdvancedLaunchTo()
        {
            // OSのバージョンを取得します。
            var osMajorVersionObj = Registry.GetValue(CURRENT_VERSION_KEY, CURRENT_VERSION_NAME, "0");

            // windows10以下のOSの場合は処理を終了します。
            if (!(osMajorVersionObj is int) || (int)osMajorVersionObj < 10) return;

            try
            {
                // レジストリを取得します。
                using (var regKey = Registry.CurrentUser.CreateSubKey(ADVANCED_LAUNCHTO_SUB_KEY))
                {
                    // レジストリーの値を判定します。
                    if (!PC_VALUE.Equals(regKey.GetValue(ADVANCED_LAUNCHTO_KEY)))
                    {
                        // レジストリーに無効値を設定します。
                        regKey.SetValue(ADVANCED_LAUNCHTO_KEY, PC_VALUE, RegistryValueKind.DWord);
                    }

                    // レジストリーを閉じます。
                    regKey.Close();
                }
            }
            catch
            {
                return;
            }
        }
    }
}
