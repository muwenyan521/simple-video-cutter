using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Versioning;

namespace SimpleVideoCutter
{
    [SupportedOSPlatform("windows")]
    public static class ThemeManager
    {
        public static class Colors
        {
            // 浅色主题颜色
            public static dynamic Light => new 
            {
                Background = Color.FromArgb(240, 240, 240),
                Foreground = Color.FromArgb(30, 30, 30),
                ControlBackground = Color.White,
                ControlBorder = Color.FromArgb(200, 200, 200),
                ToolStripBackground = Color.FromArgb(245, 245, 245),
                MenuBackground = Color.White,
                MenuForeground = Color.FromArgb(30, 30, 30),
                SelectionBackground = Color.FromArgb(0, 120, 215),
                SelectionForeground = Color.White,
                TimelineBackground = Color.FromArgb(250, 250, 250),
                TimelineGrid = Color.FromArgb(220, 220, 220)
            };

            // 深色主题颜色
            public static dynamic Dark => new 
            {
                Background = Color.FromArgb(32, 32, 32),
                Foreground = Color.FromArgb(240, 240, 240),
                ControlBackground = Color.FromArgb(45, 45, 45),
                ControlBorder = Color.FromArgb(60, 60, 60),
                ToolStripBackground = Color.FromArgb(40, 40, 40),
                MenuBackground = Color.FromArgb(45, 45, 45),
                MenuForeground = Color.FromArgb(240, 240, 240),
                SelectionBackground = Color.FromArgb(0, 120, 215),
                SelectionForeground = Color.White,
                TimelineBackground = Color.FromArgb(35, 35, 35),
                TimelineGrid = Color.FromArgb(70, 70, 70)
            };
        }

        public static void ApplyTheme(Form form, ThemeMode themeMode)
        {
            var isDark = ShouldUseDarkTheme(themeMode);
            var colors = isDark ? Colors.Dark : Colors.Light;

            // 应用窗体主题
            ApplyFormTheme(form, colors);

            // 递归应用所有控件的主题
            ApplyControlTheme(form, colors);
        }

        private static bool ShouldUseDarkTheme(ThemeMode themeMode)
        {
            switch (themeMode)
            {
                case ThemeMode.Light:
                    return false;
                case ThemeMode.Dark:
                    return true;
                case ThemeMode.System:
                    return IsSystemDarkMode();
                default:
                    return IsSystemDarkMode();
            }
        }

        private static bool IsSystemDarkMode()
        {
            try
            {
                // 使用Windows API检测系统主题
                using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    if (key != null)
                    {
                        var value = key.GetValue("AppsUseLightTheme");
                        if (value != null && value is int intValue)
                        {
                            return intValue == 0; // 0表示深色模式
                        }
                    }
                }
            }
            catch
            {
                // 如果无法检测，默认使用浅色模式
            }
            return false;
        }

        private static void ApplyFormTheme(Form form, dynamic colors)
        {
            form.BackColor = colors.Background;
            form.ForeColor = colors.Foreground;
        }

        private static void ApplyControlTheme(Control control, dynamic colors)
        {
            // 跳过VLC控件，因为它有自己的主题
            if (control.GetType().Name.Contains("VlcControl"))
                return;

            // 应用控件主题
            ApplySingleControlTheme(control, colors);

            // 递归处理子控件
            foreach (Control child in control.Controls)
            {
                ApplyControlTheme(child, colors);
            }
        }

        private static void ApplySingleControlTheme(Control control, dynamic colors)
        {
            // 根据控件类型应用不同的主题
            // 注意：顺序很重要，派生类必须在基类之前匹配
            switch (control)
            {
                case DataGridView dataGridView:
                    ApplyDataGridViewTheme(dataGridView, colors);
                    break;
                case ListView listView:
                    ApplyListViewTheme(listView, colors);
                    break;
                case ListBox listBox:
                    ApplyListBoxTheme(listBox, colors);
                    break;
                case MenuStrip menuStrip:
                    ApplyMenuStripTheme(menuStrip, colors);
                    break;
                case ToolStrip toolStrip:
                    ApplyToolStripTheme(toolStrip, colors);
                    break;
                case StatusStrip statusStrip:
                    ApplyStatusStripTheme(statusStrip, colors);
                    break;
                case GroupBox groupBox:
                    ApplyGroupBoxTheme(groupBox, colors);
                    break;
                case Panel panel:
                    ApplyPanelTheme(panel, colors);
                    break;
                case TextBox textBox:
                    ApplyTextBoxTheme(textBox, colors);
                    break;
                case ComboBox comboBox:
                    ApplyComboBoxTheme(comboBox, colors);
                    break;
                case CheckBox checkBox:
                    ApplyCheckBoxTheme(checkBox, colors);
                    break;
                case RadioButton radioButton:
                    ApplyRadioButtonTheme(radioButton, colors);
                    break;
                case Button button:
                    ApplyButtonTheme(button, colors);
                    break;
                case Label label:
                    ApplyLabelTheme(label, colors);
                    break;
                default:
                    // 默认主题应用
                    control.BackColor = colors.ControlBackground;
                    control.ForeColor = colors.Foreground;
                    break;
            }
        }

        private static void ApplyToolStripTheme(ToolStrip toolStrip, dynamic colors)
        {
            toolStrip.BackColor = colors.ToolStripBackground;
            toolStrip.ForeColor = colors.Foreground;
            toolStrip.Renderer = new CustomToolStripRenderer(colors);
        }

        private static void ApplyMenuStripTheme(MenuStrip menuStrip, dynamic colors)
        {
            menuStrip.BackColor = colors.MenuBackground;
            menuStrip.ForeColor = colors.MenuForeground;
            menuStrip.Renderer = new CustomMenuStripRenderer(colors);
        }

        private static void ApplyStatusStripTheme(StatusStrip statusStrip, dynamic colors)
        {
            statusStrip.BackColor = colors.ToolStripBackground;
            statusStrip.ForeColor = colors.Foreground;
            statusStrip.Renderer = new CustomToolStripRenderer(colors);
        }

        private static void ApplyPanelTheme(Panel panel, dynamic colors)
        {
            panel.BackColor = colors.ControlBackground;
            panel.ForeColor = colors.Foreground;
        }

        private static void ApplyGroupBoxTheme(GroupBox groupBox, dynamic colors)
        {
            groupBox.BackColor = colors.ControlBackground;
            groupBox.ForeColor = colors.Foreground;
        }

        private static void ApplyTextBoxTheme(TextBox textBox, dynamic colors)
        {
            textBox.BackColor = colors.ControlBackground;
            textBox.ForeColor = colors.Foreground;
            textBox.BorderStyle = BorderStyle.FixedSingle;
        }

        private static void ApplyComboBoxTheme(ComboBox comboBox, dynamic colors)
        {
            comboBox.BackColor = colors.ControlBackground;
            comboBox.ForeColor = colors.Foreground;
        }

        private static void ApplyButtonTheme(Button button, dynamic colors)
        {
            button.BackColor = colors.ControlBackground;
            button.ForeColor = colors.Foreground;
            button.FlatStyle = FlatStyle.Flat;
        }

        private static void ApplyCheckBoxTheme(CheckBox checkBox, dynamic colors)
        {
            checkBox.BackColor = colors.ControlBackground;
            checkBox.ForeColor = colors.Foreground;
        }

        private static void ApplyRadioButtonTheme(RadioButton radioButton, dynamic colors)
        {
            radioButton.BackColor = colors.ControlBackground;
            radioButton.ForeColor = colors.Foreground;
        }

        private static void ApplyLabelTheme(Label label, dynamic colors)
        {
            label.BackColor = Color.Transparent;
            label.ForeColor = colors.Foreground;
        }

        private static void ApplyListBoxTheme(ListBox listBox, dynamic colors)
        {
            listBox.BackColor = colors.ControlBackground;
            listBox.ForeColor = colors.Foreground;
        }

        private static void ApplyListViewTheme(ListView listView, dynamic colors)
        {
            listView.BackColor = colors.ControlBackground;
            listView.ForeColor = colors.Foreground;
        }

        private static void ApplyDataGridViewTheme(DataGridView dataGridView, dynamic colors)
        {
            dataGridView.BackColor = colors.ControlBackground;
            dataGridView.ForeColor = colors.Foreground;
            dataGridView.GridColor = colors.ControlBorder;
            dataGridView.BackgroundColor = colors.ControlBackground;
        }
    }

    // 自定义ToolStrip渲染器
    [SupportedOSPlatform("windows")]
    public class CustomToolStripRenderer : ToolStripProfessionalRenderer
    {
        private readonly dynamic colors;

        public CustomToolStripRenderer(dynamic colors)
        {
            this.colors = colors;
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(colors.ToolStripBackground), e.AffectedBounds);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item is ToolStripButton button)
            {
                if (button.Pressed || button.Checked)
                {
                    e.Graphics.FillRectangle(new SolidBrush(colors.SelectionBackground), e.Item.Bounds);
                }
                else if (button.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(50, colors.SelectionBackground)), e.Item.Bounds);
                }
            }
        }
    }

    // 自定义MenuStrip渲染器
    [SupportedOSPlatform("windows")]
    public class CustomMenuStripRenderer : ToolStripProfessionalRenderer
    {
        private readonly dynamic colors;

        public CustomMenuStripRenderer(dynamic colors)
        {
            this.colors = colors;
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(colors.MenuBackground), e.AffectedBounds);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item is ToolStripMenuItem menuItem)
            {
                if (menuItem.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(colors.SelectionBackground), e.Item.Bounds);
                }
            }
        }
    }
}
