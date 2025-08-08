using SimpleVideoCutter.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Versioning;

namespace SimpleVideoCutter
{
    [SupportedOSPlatform("windows")]
    public partial class FormSettings : Form
    {
        private MainForm? mainForm;

        public FormSettings()
        {
            InitializeComponent();

            var culture = CultureInfo.GetCultureInfo(VideoCutterSettings.Instance.Language ?? Thread.CurrentThread.CurrentUICulture.Name);
            if (culture != null)
            {
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            this.toolTip1.SetToolTip(this.comboBoxDefaultDirectory, string.Format(
                GlobalStrings.FormSettings_DefaultDirecttoryTooltip,
                @"{UserVideos}\n{UserDocuments}\n{MyComputer}".Replace(
                    @"\n", Environment.NewLine)));

            this.toolTip1.SetToolTip(this.comboBoxOutputDirectory, string.Format(
                GlobalStrings.FormSettings_OutputDirectoryTooltip,
                @"{UserVideos}\n{UserDocuments}\n{MyComputer}".Replace(
                    @"\n", Environment.NewLine)));

            this.toolTip1.SetToolTip(this.textBoxOutputFilePattern, string.Format(
                GlobalStrings.FormSettings_OutputFileNamePatternTooltip,
                    "{FileName}",
                    "{FileNameWithoutExtension}",
                    "{FileExtension}",
                    "{FileDate}",
                    "{Timestamp}"));

            this.comboBoxPreviewSize.DataSource =
                ((PreviewSize[])Enum.GetValues(typeof(PreviewSize))).Select(ps => new ComboBoxItem<PreviewSize>()
                {
                    Value = ps,
                    Title = ps.ToString()
                }).ToList();

            // 初始化主题选择下拉框
            this.comboBoxThemeMode.DataSource =
                ((ThemeMode[])Enum.GetValues(typeof(ThemeMode))).Select(tm => new ComboBoxItem<ThemeMode>()
                {
                    Value = tm,
                    Title = GetThemeModeDisplayName(tm)
                }).ToList();

            // 添加主题变更事件
            this.comboBoxThemeMode.SelectedIndexChanged += ComboBoxThemeMode_SelectedIndexChanged;

        }

        private void ComboBoxThemeMode_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (comboBoxThemeMode.SelectedValue is ThemeMode selectedTheme)
            {
                // 实时预览主题
                ThemeManager.ApplyTheme(this, selectedTheme);
            }
        }

        private string GetThemeModeDisplayName(ThemeMode themeMode)
        {
            switch (themeMode)
            {
                case ThemeMode.Light:
                    return "浅色模式";
                case ThemeMode.Dark:
                    return "深色模式";
                case ThemeMode.System:
                    return "跟随系统";
                default:
                    return themeMode.ToString();
            }
        }

        public void ShowSettingsDialog(MainForm? mainForm = null)
        {
            this.mainForm = mainForm;
            VideoCutterSettings.Instance.LoadSettings();
            SettingsToGUI();
            
            // 保存当前主题设置
            var originalTheme = VideoCutterSettings.Instance.ThemeMode;
            
            this.ShowDialog();
            
            // 如果用户取消了设置，恢复原来的主题
            if (this.DialogResult != DialogResult.OK)
            {
                ThemeManager.ApplyTheme(this, originalTheme);
            }
            else
            {
                // 如果用户确认了设置，通知主窗体刷新主题
                if (OperatingSystem.IsWindows())
                {
                    mainForm?.RefreshTheme();
                }
            }
        }


        private void SettingsToGUI()
        {
            var settings = VideoCutterSettings.Instance;

            comboBoxDefaultDirectory.Text = settings.DefaultInitialDirectory;
            comboBoxOutputDirectory.Text = settings.OutputDirectory;
            textBoxOutputFilePattern.Text = settings.OutputFilePattern;
            textBoxFFmpegPath.Text = settings.FFmpegPath;
            textBoxVideoFileExtensions.Text = String.Join(" ,", settings.VideoFilesExtensions);
            comboBoxPreviewSize.SelectedValue = settings.PreviewSize;
            comboBoxThemeMode.SelectedValue = settings.ThemeMode;

            SetBackgroundOfFFmpegPath();
        }

        private void GUIToSettings()
        {
            var settings = VideoCutterSettings.Instance;

            settings.DefaultInitialDirectory = comboBoxDefaultDirectory.Text;
            settings.OutputDirectory = comboBoxOutputDirectory.Text;
            settings.OutputFilePattern = textBoxOutputFilePattern.Text;
            settings.FFmpegPath = textBoxFFmpegPath.Text;
            settings.PreviewSize = (PreviewSize)(Enum.Parse(typeof(PreviewSize), comboBoxPreviewSize.SelectedValue?.ToString() ?? "L"));
            settings.ThemeMode = (ThemeMode)(Enum.Parse(typeof(ThemeMode), comboBoxThemeMode.SelectedValue?.ToString() ?? "System"));
            // TODO: parse VideoFilesExtensions

            settings.StoreSettings();
        }


        private string? SelectFile(string fileName)
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.DefaultExt = "exe";
                dialog.CheckFileExists = true;
                dialog.Filter = $"{GlobalStrings.FormSettings_ExecutableFiles} (*.exe)|*.exe";

                var result = dialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    return dialog.FileName;
                }
            }
            return null;
        }

        private string? SelectFolder()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                var result = dialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
            }
            return null;
        }

        private void SetBackgroundOfFFmpegPath()
        {
            if (string.IsNullOrWhiteSpace(textBoxFFmpegPath.Text) || !File.Exists(textBoxFFmpegPath.Text))
            {
                textBoxFFmpegPath.BackColor = Color.Orange;
            }
            else
            {
                textBoxFFmpegPath.BackColor = SystemColors.Window;
            }
        }

        private void textBoxFFmpegPath_TextChanged(object sender, EventArgs e)
        {
            SetBackgroundOfFFmpegPath();
        }

        private void buttonFFmpegPath_Click(object sender, EventArgs e)
        {
            var ffmpegPath = SelectFile("ffmpeg.exe");
            if (ffmpegPath != null)
                textBoxFFmpegPath.Text = ffmpegPath;
        }

        private void buttonDefaultDirectory_Click(object sender, EventArgs e)
        {
            var defaultDirectoryPath = SelectFolder();
            if (defaultDirectoryPath != null)
                comboBoxDefaultDirectory.Text = defaultDirectoryPath;
        }

        private void buttonOutputDirectory_Click(object sender, EventArgs e)
        {
            var outputDirectoryPath = SelectFolder();
            if (outputDirectoryPath != null)
                comboBoxOutputDirectory.Text = outputDirectoryPath;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            GUIToSettings();
            Close();
        }


        internal class ComboBoxItem<T>
        {
            public string? Title { get; set; }
            public T? Value { get; set; }

            public override bool Equals(object? obj)
            {
                if (obj is ComboBoxItem<T>)
                {
                    var other = obj as ComboBoxItem<T>;
                    return String.Equals(Value, other);
                }
                else
                    return false;
            }

            public override int GetHashCode()
            {
                return Value == null ? 0 : Value.GetHashCode();
            }
        }
    }
}
