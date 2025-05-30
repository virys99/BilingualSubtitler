﻿using Octokit;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BilingualSubtitler.MainForm;

namespace BilingualSubtitler
{

    public partial class SettingsForm : Form
    {
        private int m_inititalCheckUpdatesGroupBoxLeft;
        private int m_inititalDownloadsDirectoryGroupBox;

        private Dictionary<string, ProcessPriorityClass> m_processPriorityNamesAndValues;
        private Dictionary<ProcessPriorityClass, string> m_processPriorityValuesAndNames;

        private List<Button> m_buttons;
        private Color m_previousButtonColor;
        private bool m_flagKeyIsInvalid = false;

        private FontFamily[] m_installedFontFamilies;

        public bool SettingsWasSaved = false;

        public SettingsForm(MainForm mainForm)
        {
            InitializeComponent();

            m_inititalCheckUpdatesGroupBoxLeft = checkUpdatesGroupBox.Left;
            m_inititalDownloadsDirectoryGroupBox = downloadsDirectoryGroupBox.Left;

            try
            {
                SetFormAccordingToSettings();
            }
            catch (Exception e)
            {
                throw new BilingualSubtitlerPropertiesLoadingException(e);
            }

            // Графика
            //

            m_processPriorityNamesAndValues = new Dictionary<string, ProcessPriorityClass>
            {
                { "Низкий", ProcessPriorityClass.Idle},
                { "Ниже среднего", ProcessPriorityClass.BelowNormal},
                { "Обычный", ProcessPriorityClass.Normal},
                { "Выше среднего", ProcessPriorityClass.AboveNormal},
                { "Высокий", ProcessPriorityClass.High},
                { "Реального времени", ProcessPriorityClass.RealTime},
            };

            m_processPriorityValuesAndNames = new Dictionary<ProcessPriorityClass, string>(m_processPriorityNamesAndValues.Count);
            //
            foreach (var key in m_processPriorityNamesAndValues.Keys)
                m_processPriorityValuesAndNames.Add(m_processPriorityNamesAndValues[key], key);

            // Лэйбл текущего приоритета процесса
            using (var p = Process.GetCurrentProcess())
            {
                currentProcessPriorityTextBox.Text = m_processPriorityValuesAndNames[p.PriorityClass];
            }

            // Заполняем КомбоБокс
            int indexForCurrentProcessPriority = 0;
            foreach (var prioprityName in m_processPriorityNamesAndValues.Keys)
            {
                var index = targetProcessPriorityTextBox.Items.Add(prioprityName);

                if (prioprityName == currentProcessPriorityTextBox.Text)
                    indexForCurrentProcessPriority = index;
            }
            targetProcessPriorityTextBox.SelectedIndex = indexForCurrentProcessPriority;

            // Default
            this.DialogResult = DialogResult.Cancel;

            // Версии
            var currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            currentAppVersionLabel.Text = currentVersion.ToString();
            // Проверка обновлений
            var checkUpdatesBgW = new BackgroundWorker();
            checkUpdatesBgW.DoWork += mainForm.CheckUpdatesBgW_DoWork;
            checkUpdatesBgW.RunWorkerCompleted += CheckUpdatesBgW_RunWorkerCompleted;
            lastAppVersionLabel.Text = "Идет проверка...";
            checkUpdatesBgW.RunWorkerAsync();
        }

        private void CheckUpdatesBgW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is Tuple<string, Octokit.Release>)
            {
                var latestVersionOnGitHub = ((Tuple<string, Octokit.Release>)e.Result).Item1;
                var latestReleaseOnGitHub = ((Tuple<string, Octokit.Release>)e.Result).Item2;

                lastAppVersionLabel.Text = latestVersionOnGitHub;
            }
            else if (e.Result is Exception)
            {
                var exception = (Exception)e.Result;
                lastAppVersionLabel.Text = "Не удалось получить информацию\nо новых версиях :( (Подробности — по клику)";
                lastAppVersionLabel.Tag = exception;
            }


        }

        public void SetFormAccordingToSettings()
        {
            // Системные шрифты
            using InstalledFontCollection fontsCollection = new InstalledFontCollection();
            m_installedFontFamilies = fontsCollection.Families;
            foreach (FontFamily font in m_installedFontFamilies)
            {
                subtitlesAppearanceSettingsControl.OriginalSubtitlesFontComboBox.Items.Add(font.Name);
                subtitlesAppearanceSettingsControl.FirstRussianSubtitlesFontComboBox.Items.Add(font.Name);
                subtitlesAppearanceSettingsControl.SecondRussianSubtitlesFontComboBox.Items.Add(font.Name);
                subtitlesAppearanceSettingsControl.ThirdRussianSubtitlesFontComboBox.Items.Add(font.Name);
            }

            //m_flagKeyIsInvalid = true;

            foreach (var hotkeyString in Properties.Settings.Default.Hotkeys)
            {
                AddKeyToHotkeysDataGridView(hotkeyString);
            }

            videoPlayerPauseButtonTextBox.Text =
                new Hotkey(Properties.Settings.Default.VideoPlayerPauseButtonString).KeyValue;
            videoPlayerPauseButtonTextBox.Tag = Properties.Settings.Default.VideoPlayerPauseButtonString;
            videoPlayerChangeToBilingualSubtitlesButtonTextBox.Text =
                new Hotkey(Properties.Settings.Default.VideoPlayerChangeToBilingualSubtitlesHotkeyString).KeyValue;
            videoPlayerChangeToBilingualSubtitlesButtonTextBox.Tag =
                Properties.Settings.Default.VideoPlayerChangeToBilingualSubtitlesHotkeyString;
            videoPlayerChangeToOriginalSubtitlesButtonTextBox.Text =
                new Hotkey(Properties.Settings.Default.VideoPlayerChangeToOriginalSubtitlesHotkeyString).KeyValue;
            videoPlayerChangeToOriginalSubtitlesButtonTextBox.Tag =
                Properties.Settings.Default.VideoPlayerChangeToOriginalSubtitlesHotkeyString;

            // Файлы субтитров
            originalSubtitlesPathEndingTextBox.Text = Properties.Settings.Default.OriginalSubtitlesFileNameEnding;
            bilingualSubtitlesPathEndingTextBox.Text = Properties.Settings.Default.BilingualSubtitlesFileNameEnding;
            //
            CreateOriginalSubtitlesFileCheckBox.Checked = Properties.Settings.Default.CreateOriginalSubtitlesFile;
            CreateBilingualSubtitlesFileCheckBox.Checked = Properties.Settings.Default.CreateBilingualSubtitlesFile;

            videoplayerProcessNameTextBox.Text = Properties.Settings.Default.VideoPlayerProcessName;

            VideoPlayerPathTextBox.Text = Properties.Settings.Default.VideoPlayerPath;

            richTextBoxForYandexApiKeyInSeparateForm.Text = Properties.Settings.Default.YandexTranslatorAPIKey;
            gotTheYandexTranslatorAPIKeyCheckBox.Checked = Properties.Settings.Default.YandexTranslatorAPIEnabled;

            checkUpdatesOnAppStartCheckBox.Checked = Properties.Settings.Default.CheckUpdates;

            fixDotOrCommaAsTheFisrtCharOfNewLIneCheckBox.Checked = Properties.Settings.Default.FixDotOrCommaAsTheFisrtCharOfNewLIne;
            ReadAndWriteTitlesOfOriginIntoFinalFilesCheckBox.Checked = Properties.Settings.Default.ReadAndWriteTitlesOfOriginIntoFinalFiles;

            if (Properties.Settings.Default.AdvancedMode)
                advancedModeRadioButton.Checked = true;
            else
                notAdvancedModeRadioButton.Checked = true;

            downloadsFolderPathRichTextBox.Text = Properties.Settings.Default.DownloadsFolder;
            defaultFolderPathRichTextBox.Text = Properties.Settings.Default.FolderToOpenFilesByDefaultFrom;

            notifyAboutSuccessfullySavedSubtitlesFileCheckBox.Checked = Properties.Settings.Default.NotifyAboutSuccessfullySavedSubtitlesFile;


            SetFormAccordingToSubtitlesAppearanceSettings();

            SetFormAccordingToAdvancedModeOrNot();
        }

        private void SetFormAccordingToSubtitlesAppearanceSettings()
        {
            subtitlesAppearanceSettingsControl.SetAccordingToPropertiesSettings();
        }

        private void SetFormAccordingToAdvancedModeOrNot()
        {
            var itIsAdvancedMode = advancedModeRadioButton.Checked;

            fixDotOrCommaAsTheFisrtCharOfNewLIneCheckBox.Visible =
                ReadAndWriteTitlesOfOriginIntoFinalFilesCheckBox.Visible =
                processPriorityGroupBox.Visible =
                yandexTranslatorGroupBox.Visible =
                defaultDirectoryGroupBox.Visible =
                notifyAboutSuccessfullySavedSubtitlesFileCheckBox.Visible =
                itIsAdvancedMode;

            if (itIsAdvancedMode)
            {
                checkUpdatesGroupBox.Left = m_inititalCheckUpdatesGroupBoxLeft;
                downloadsDirectoryGroupBox.Left = m_inititalDownloadsDirectoryGroupBox;
            }
            else
            {
                checkUpdatesGroupBox.Left = (this.Width - checkUpdatesGroupBox.Width) / 2;
                downloadsDirectoryGroupBox.Left = (this.Width - downloadsDirectoryGroupBox.Width) / 2;
            }
        }


        private void SettingsForm_Load(object sender, EventArgs e)
        {
            //m_buttons = new List<Button>();
            //m_buttons.Add(buttonOk);
            //m_buttons.Add(buttonCancel);

            //foreach (var btn in m_buttons)
            //{
            //    btn.FlatAppearance.BorderSize = 0;
            //    btn.FlatStyle = FlatStyle.Flat;
            //}

            //if (m_flagKeyIsInvalid)
            //    richTextBoxLabelYouNeedToGetAPIKey.Text = "Введенный ключ неверен. " +
            //                                              "Для использования данного приложения необходимо ввести валидный ключ к API сервиса "
            //                                              + '"' + "Яндекс.Переводчик" + '"' + ".";

            //richTextBoxForYandexApiKeyInSeparateForm.Text = Properties.Settings.Default.YandexTranslatorAPIKey;

            ////Синий фон у кнопок при наведении курсора
            //foreach (var btn in m_buttons)
            //{
            //    btn.MouseEnter += new EventHandler(btn_MouseEnter);
            //    btn.MouseLeave += new EventHandler(btn_MouseLeave);
            //}

            //richTextBoxForYandexApiKeyInSeparateForm.Select();

        }

        private void AddKeyToHotkeysDataGridView(string hotkeyString)
        {
            var hotkey = new Hotkey(hotkeyString);

            var rowIndex = hotkeysDataGridView.Rows.Add(hotkey.KeyValue);
            hotkeysDataGridView.Rows[rowIndex].Tag = hotkeyString;
        }

        private void AddKeyToHotkeysDataGridView(Hotkey hotkey)
        {
            var rowIndex = hotkeysDataGridView.Rows.Add(hotkey.KeyValue);
            hotkeysDataGridView.Rows[rowIndex].Tag = hotkey.ToString();
        }

        private void btn_MouseEnter(object sender, EventArgs e)
        {
            m_previousButtonColor = ((Button)sender).BackColor;
            ((Button)sender).BackColor = Color.Gold;

        }

        private void btn_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = m_previousButtonColor;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            bool processPriorityChangedSuccessfully;
            try
            {
                using (var p = Process.GetCurrentProcess())
                {
                    p.PriorityClass = m_processPriorityNamesAndValues[targetProcessPriorityTextBox.Text];

                }
                processPriorityChangedSuccessfully = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Смена приоритета процесса не удалась.\n\n\nОшибка:{ex}", "Смена приоритета процесса не удалась",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                processPriorityChangedSuccessfully = false;
            }
            //
            if (processPriorityChangedSuccessfully)
                Properties.Settings.Default.ProcessPriority = m_processPriorityNamesAndValues[targetProcessPriorityTextBox.Text];


            //Вычищаем символы переноса строки и пробелы из ключа
            int carrySymbols = richTextBoxForYandexApiKeyInSeparateForm.Text.Split('\n').Length - 1;
            int spaceSymbols = richTextBoxForYandexApiKeyInSeparateForm.Text.Split(' ').Length - 1;

            Properties.Settings.Default.Hotkeys = new StringCollection();
            foreach (DataGridViewRow hotkeyRow in hotkeysDataGridView.Rows)
            {
                if (hotkeyRow.Tag != null)
                    Properties.Settings.Default.Hotkeys.Add((string)hotkeyRow.Tag);
            }

            Properties.Settings.Default.VideoPlayerPauseButtonString = (string)videoPlayerPauseButtonTextBox.Tag;
            Properties.Settings.Default.VideoPlayerChangeToBilingualSubtitlesHotkeyString =
                (string)videoPlayerChangeToBilingualSubtitlesButtonTextBox.Tag;
            Properties.Settings.Default.VideoPlayerChangeToOriginalSubtitlesHotkeyString =
                (string)videoPlayerChangeToOriginalSubtitlesButtonTextBox.Tag;
            Properties.Settings.Default.CreateOriginalSubtitlesFile = CreateOriginalSubtitlesFileCheckBox.Checked;
            Properties.Settings.Default.CreateBilingualSubtitlesFile = CreateBilingualSubtitlesFileCheckBox.Checked;
            Properties.Settings.Default.OriginalSubtitlesFileNameEnding = originalSubtitlesPathEndingTextBox.Text;
            Properties.Settings.Default.BilingualSubtitlesFileNameEnding = bilingualSubtitlesPathEndingTextBox.Text;
            // TODO v11
            //Properties.Settings.Default.ChangeRussianSubtitlesStylesAccordingToOriginal = subtitlesAppearanceSettingsControl.ChangeRussianSubtitlesStylesAccordingToOriginalCheckBox.Checked;
            //Properties.SubtitlesAppearanceSettings.Default.SecondAndThirdRussianSubtitlesAtTopOfTheScreen = subtitlesAppearanceSettingsControl.SecondAndThirdRussianSubtitlesAtTheTopOfScreenCheckBox.Checked;
            //Properties.SubtitlesAppearanceSettings.Default.SecondAndThirdRussianSubtitlesAtTopOfTheScreenEnabled = subtitlesAppearanceSettingsControl.SecondAndThirdRussianSubtitlesAtTheTopOfScreenCheckBox.Enabled;

            Properties.Settings.Default.YandexTranslatorAPIKey =
                richTextBoxForYandexApiKeyInSeparateForm.Text.Substring(0,
                    richTextBoxForYandexApiKeyInSeparateForm.Text.Length - spaceSymbols - carrySymbols);

            Properties.Settings.Default.VideoPlayerProcessName = videoplayerProcessNameTextBox.Text;

            Properties.SubtitlesAppearanceSettings.Default.OriginalSubtitlesStyleString = $"{subtitlesAppearanceSettingsControl.OriginalSubtitlesFontComboBox.Text};" +
                                                                       $"{subtitlesAppearanceSettingsControl.OriginalSubtitlesMarginNumericUpDown.Text};" +
                                                                       $"{subtitlesAppearanceSettingsControl.OriginalSubtitlesSizeNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.OriginalSubtitlesOutlineNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.OriginalSubtitlesShadowNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.OriginalSubtitlesTransparencyPercentageNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.OriginalSubtitlesShadowTransparencyPercentageNumericUpDown.Value};" +
                                                                       $"{(subtitlesAppearanceSettingsControl.OriginalSubtitlesInOneLineCheckBox.Checked ? 1 : 0)}";

            Properties.SubtitlesAppearanceSettings.Default.FirstRussianSubtitlesStyleString = $"{subtitlesAppearanceSettingsControl.FirstRussianSubtitlesFontComboBox.Text};" +
                                                                       $"{subtitlesAppearanceSettingsControl.FirstRussianSubtitlesMarginNumericUpDown.Text};" +
                                                                       $"{subtitlesAppearanceSettingsControl.FirstRussianSubtitlesSizeNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.FirstRussianSubtitlesOutlineNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.FirstRussianSubtitlesShadowNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.FirstRussianSubtitlesTransparencyPercentageNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.FirstRussianSubtitlesShadowTransparencyPercentageNumericUpDown.Value};" +
                                                                       $"{(subtitlesAppearanceSettingsControl.FirstRussianSubtitlesInOneLineCheckBox.Checked ? 1 : 0)}";

            Properties.SubtitlesAppearanceSettings.Default.SecondRussianSubtitlesStyleString = $"{subtitlesAppearanceSettingsControl.SecondRussianSubtitlesFontComboBox.Text};" +
                                                                       $"{subtitlesAppearanceSettingsControl.SecondRussianSubtitlesMarginNumericUpDown.Text};" +
                                                                       $"{subtitlesAppearanceSettingsControl.SecondRussianSubtitlesSizeNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.SecondRussianSubtitlesOutlineNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.SecondRussianSubtitlesShadowNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.SecondRussianSubtitlesTransparencyPercentageNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.SecondRussianSubtitlesShadowTransparencyPercentageNumericUpDown.Value};" +
                                                                       $"{(subtitlesAppearanceSettingsControl.SecondRussianSubtitlesInOneLineCheckBox.Checked ? 1 : 0)}";

            Properties.SubtitlesAppearanceSettings.Default.ThirdRussianSubtitlesStyleString = $"{subtitlesAppearanceSettingsControl.ThirdRussianSubtitlesFontComboBox.Text};" +
                                                                       $"{subtitlesAppearanceSettingsControl.ThirdRussianSubtitlesMarginNumericUpDown.Text};" +
                                                                       $"{subtitlesAppearanceSettingsControl.ThirdRussianSubtitlesSizeNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.ThirdRussianSubtitlesOutlineNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.ThirdRussianSubtitlesShadowNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.ThirdRussianSubtitlesTransparencyPercentageNumericUpDown.Value};" +
                                                                       $"{subtitlesAppearanceSettingsControl.ThirdRussianSubtitlesShadowTransparencyPercentageNumericUpDown.Value};" +
                                                                       $"{(subtitlesAppearanceSettingsControl.ThirdRussianSubtitlesInOneLineCheckBox.Checked ? 1 : 0)}";

            Properties.Settings.Default.YandexTranslatorAPIEnabled = gotTheYandexTranslatorAPIKeyCheckBox.Checked;

            Properties.SubtitlesAppearanceSettings.Default.MarginCheckBoxChecked = subtitlesAppearanceSettingsControl.marginCheckBox.Checked;
            Properties.SubtitlesAppearanceSettings.Default.SizeCheckBoxChecked = subtitlesAppearanceSettingsControl.sizeCheckBox.Checked;
            Properties.SubtitlesAppearanceSettings.Default.OutlineCheckBoxChecked = subtitlesAppearanceSettingsControl.outlineCheckBox.Checked;
            Properties.SubtitlesAppearanceSettings.Default.ShadowCheckBoxChecked = subtitlesAppearanceSettingsControl.shadowCheckBox.Checked;
            Properties.SubtitlesAppearanceSettings.Default.TransparencyCheckBoxChecked = subtitlesAppearanceSettingsControl.transparencyCheckBox.Checked;
            Properties.SubtitlesAppearanceSettings.Default.ShadowTransparencyCheckBoxChecked = subtitlesAppearanceSettingsControl.shadowTransparencyCheckBox.Checked;
            //
            Properties.SubtitlesAppearanceSettings.Default.SetTheSameValuesForAllSubtitles = subtitlesAppearanceSettingsControl.SetTheSameValuesForAllSubtitlesCheckBox.Checked;
            Properties.SubtitlesAppearanceSettings.Default.СhangeOnTheSameDeltaValuesForAllSubtitles = subtitlesAppearanceSettingsControl.ChangeOnTheSameDeltaValuesForAllSubtitlesCheckBox.Checked;
            Properties.SubtitlesAppearanceSettings.Default.ChangeMarginsToPairSubtitles = subtitlesAppearanceSettingsControl.ChangeMarginsToPairSubtitlesCheckBox.Checked;

            Properties.Settings.Default.FixDotOrCommaAsTheFisrtCharOfNewLIne = fixDotOrCommaAsTheFisrtCharOfNewLIneCheckBox.Checked;
            Properties.Settings.Default.ReadAndWriteTitlesOfOriginIntoFinalFiles = ReadAndWriteTitlesOfOriginIntoFinalFilesCheckBox.Checked;

            Properties.Settings.Default.CheckUpdates = checkUpdatesOnAppStartCheckBox.Checked;

            Properties.Settings.Default.AdvancedMode = advancedModeRadioButton.Checked;

            Properties.Settings.Default.DownloadsFolder = downloadsFolderPathRichTextBox.Text;
            Properties.Settings.Default.FolderToOpenFilesByDefaultFrom = defaultFolderPathRichTextBox.Text;

            Properties.Settings.Default.NotifyAboutSuccessfullySavedSubtitlesFile = notifyAboutSuccessfullySavedSubtitlesFileCheckBox.Checked;


            Properties.Settings.Default.Save();
            Properties.SubtitlesAppearanceSettings.Default.Save();
            this.DialogResult = DialogResult.OK;
            this.SettingsWasSaved = true;
            this.Close();

        }

        private void linkLabelGetAPIKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //this.linkLabelGetAPIKey.LinkVisited = true;
            //System.Diagnostics.Process.Start("https://tech.yandex.ru/keys/get/?service=trnsl");
        }



        private void YandexTranslatorAPIKeyForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void linkLabelYandexAPIKeysList_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabelYandexAPIKeysList.LinkVisited = true;
            MainForm.OpenUrl("https://tech.yandex.ru/keys/");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var hotkeySettingForm = new HotkeySettingForm(true, true);
            var dialogResult = hotkeySettingForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                // TODO Если кнопка уже установлена

                AddKeyToHotkeysDataGridView(hotkeySettingForm.SettedHotkey);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены? Все настройки будут сброшены к настройкам по умолчанию!", "",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                Properties.Settings.Default.Reset();
                //
                var currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                Properties.Settings.Default.LatestInstalledVersion = currentVersion.ToString();
                //
                Properties.Settings.Default.Save();

                Properties.SubtitlesAppearanceSettings.Default.Reset();
                Properties.SubtitlesAppearanceSettings.Default.Save();

                SettingsWasSaved = true;

                Close();
            }
        }

        private void videoplayerPauseHotkeySetButton_Click(object sender, EventArgs e)
        {
            var hotkeySettingForm = new HotkeySettingForm(true);
            var dialogResult = hotkeySettingForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                videoPlayerPauseButtonTextBox.Text = hotkeySettingForm.SettedHotkey.KeyValue;
                videoPlayerPauseButtonTextBox.Tag = hotkeySettingForm.SettedHotkey.ToString();
            }

            hotkeySettingForm.Dispose();
        }

        private void videoplayerNextSubtitlesHotkeySetButton_Click(object sender, EventArgs e)
        {
            var hotkeySettingForm = new HotkeySettingForm();
            var dialogResult = hotkeySettingForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                videoPlayerChangeToBilingualSubtitlesButtonTextBox.Text = hotkeySettingForm.SettedHotkey.KeyValue;
                videoPlayerChangeToBilingualSubtitlesButtonTextBox.Tag = hotkeySettingForm.SettedHotkey.ToString();
            }

            hotkeySettingForm.Dispose();
        }

        private void videoPlayerChangeToOriginalSubtitlesHotkeySetButton_Click(object sender, EventArgs e)
        {
            var hotkeySettingForm = new HotkeySettingForm();
            var dialogResult = hotkeySettingForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                videoPlayerChangeToOriginalSubtitlesButtonTextBox.Text = hotkeySettingForm.SettedHotkey.KeyValue;
                videoPlayerChangeToOriginalSubtitlesButtonTextBox.Tag = hotkeySettingForm.SettedHotkey.ToString();
            }

            hotkeySettingForm.Dispose();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            ClearSelectedRows(hotkeysDataGridView);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Clear(hotkeysDataGridView);
        }

        public void Clear(DataGridView dataGridView)
        {
            while (dataGridView.Rows.Count > 1)
                for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
                    dataGridView.Rows.Remove(dataGridView.Rows[i]);
        }


        public void ClearSelectedRows(DataGridView dataGridView)
        {
            //for (int i = dataGridView.Rows.Count - 1; i > 0; i--)
            //{
            //    var row = dataGridView.Rows[i];
            //    if (row.Selected)
            //        dataGridView.Rows.Remove(row);
            //}

            for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
            {
                var row = dataGridView.Rows[i];
                if (row.Selected)
                {
                    dataGridView.Rows.Remove(row);
                    //i--;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string formats = "Исполняемые файлы |*.exe";

            openFileDialog.Filter = formats;
            var result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                var videoPlayerExecutable = new FileInfo(openFileDialog.FileName);
                videoplayerProcessNameTextBox.Text = videoPlayerExecutable.Name.Substring(0,
                    videoPlayerExecutable.Name.Length - videoPlayerExecutable.Extension.Length);
            }

            openFileDialog.Dispose();
        }

        private void originalSubtitlesFontComboBox_TextChanged(object sender, EventArgs e)
        {
            //if (changeRussianSubtitlesStylesAccordingToOriginalCheckBox.Checked)
            //{
            //    firstRussianSubtitlesFontComboBox.Text = secondRussianSubtitlesFontComboBox.Text =
            //        thirdRussianSubtitlesFontComboBox.Text =
            //            originalSubtitlesFontComboBox.Text;
            //}
        }

        private void button9_Click(object sender, EventArgs e)
        {
            using var hotkeysToAuthorsExtendedDefaultsForm = new HotkeysToAuthorsExtendedDefaultsForm();
            var result = hotkeysToAuthorsExtendedDefaultsForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                Properties.Settings.Default.Hotkeys = new StringCollection
                {
                    "Left@37@",
                    "Up@38@",
                    "Down@40@",
                    "Right@39@",
                    "NumPad0@96@",
                    "Decimal@110@",
                    "Return@13@",
                    "Add@107@",
                    "Subtract@109@",
                    "Multiply@106@",
                    "Divide@111@",
                    "F3@114@",
                    "Space@32@"
                };


                Clear(hotkeysDataGridView);
                foreach (var hotkeyString in Properties.Settings.Default.Hotkeys)
                {
                    AddKeyToHotkeysDataGridView(hotkeyString);
                }
            }
        }


        private void currentProcessPriorityTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void lastAppVersionLabel_Click(object sender, EventArgs e)
        {
            var exception = (Exception)((System.Windows.Forms.Label)sender).Tag;

            MessageBox.Show($"Не удалось получить информацию о новых версиях.\nОшибка:{exception}", "Не удалось получить информацию о новых версиях",
               MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
        }

        private void ResetSubtitlesAppearanceToDefaultButton_Click(object sender, EventArgs e)
        {

            var result = MessageBox.Show("Сбросить настройки вида субтитров к значениям по умолчанию?", string.Empty, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                Properties.SubtitlesAppearanceSettings.Default.Reset();
                Properties.SubtitlesAppearanceSettings.Default.Save();

                SetFormAccordingToSubtitlesAppearanceSettings();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl("https://github.com/0xotHik/BilingualSubtitler/releases");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl("https://t.me/bilingualsubtitler");
        }

        private void notAdvancedModeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            advancedModeRadioButton.Checked = !((RadioButton)sender).Checked;

            SetFormAccordingToAdvancedModeOrNot();
        }

        private void advancedModeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            notAdvancedModeRadioButton.Checked = !((RadioButton)sender).Checked;

            SetFormAccordingToAdvancedModeOrNot();
        }

        private void downloadsFolderPathSetButton_Click(object sender, EventArgs e)
        {
            var openFolderDialog = new FolderBrowserDialog();
            openFolderDialog.SelectedPath = downloadsFolderPathRichTextBox.Text;
            var dialogResult = openFolderDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
                downloadsFolderPathRichTextBox.Text = openFolderDialog.SelectedPath;
        }

        private void defaultFolderPathSetButton_Click(object sender, EventArgs e)
        {
            var openFolderDialog = new FolderBrowserDialog();
            openFolderDialog.SelectedPath = defaultFolderPathRichTextBox.Text;
            var dialogResult = openFolderDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
                defaultFolderPathRichTextBox.Text = openFolderDialog.SelectedPath;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            using var formAbout = new FormAbout();
            formAbout.ShowDialog();
        }

        private void advancedModeRadioButton_Click(object sender, EventArgs e)
        {
            bool consolasIsIntalled = false;
            foreach (FontFamily font in m_installedFontFamilies)
            {
                if (font.Name == "Consolas")
                    consolasIsIntalled = true;
            }

            if (consolasIsIntalled)
            {
                var appModeWasChangedToExtendedForm = new AppModeWasChangedToExtendedForm();
                appModeWasChangedToExtendedForm.ShowDialog();

                var settedRussianSubtitlesStreamToSetConsolasTo = appModeWasChangedToExtendedForm.SettedRussianSubtitlesStreamToSetConsolasTo;
                if (settedRussianSubtitlesStreamToSetConsolasTo != null)
                {
                    if (settedRussianSubtitlesStreamToSetConsolasTo.Value == 1)
                    {
                        subtitlesAppearanceSettingsControl.FirstRussianSubtitlesFontComboBox.TextChanged -= firstRussianSubtitlesFontComboBox_TextChanged;

                        subtitlesAppearanceSettingsControl.FirstRussianSubtitlesFontComboBox.Text = "Consolas";
                        subtitlesAppearanceSettingsControl.FirstRussianSubtitlesSizeNumericUpDown.Value = subtitlesAppearanceSettingsControl.FirstRussianSubtitlesSizeNumericUpDown.Value - 2;

                        subtitlesAppearanceSettingsControl.FirstRussianSubtitlesFontComboBox.TextChanged += firstRussianSubtitlesFontComboBox_TextChanged;
                    }
                    else if (settedRussianSubtitlesStreamToSetConsolasTo.Value == 2)
                    {
                        subtitlesAppearanceSettingsControl.SecondRussianSubtitlesFontComboBox.Text = "Consolas";
                        subtitlesAppearanceSettingsControl.SecondRussianSubtitlesSizeNumericUpDown.Value = subtitlesAppearanceSettingsControl.SecondRussianSubtitlesSizeNumericUpDown.Value - 2;
                    }
                    else if (settedRussianSubtitlesStreamToSetConsolasTo.Value == 3)
                    {
                        subtitlesAppearanceSettingsControl.ThirdRussianSubtitlesFontComboBox.Text = "Consolas";
                        subtitlesAppearanceSettingsControl.ThirdRussianSubtitlesSizeNumericUpDown.Value = subtitlesAppearanceSettingsControl.ThirdRussianSubtitlesSizeNumericUpDown.Value - 2;
                    }
                }
            }
        }


        private void firstRussianSubtitlesFontComboBox_TextChanged(object sender, EventArgs e) { }

        private void changeRussianSubtitlesStylesAccordingToOriginalCheckBox_CheckedChanged(object sender, EventArgs e) { }

        private void subtitlesAppearanceSettingsControl_Load(object sender, EventArgs e)
        {
            subtitlesAppearanceSettingsControl.ResetSubtitlesAppearanceToDefaultButton.Click += ResetSubtitlesAppearanceToDefaultButton_Click;
        }

        private void videoplayerProcessNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            {

                OpenFileDialog openFileDialog = new OpenFileDialog();
                string formats = "Исполняемые файлы |*.exe";

                openFileDialog.Filter = formats;
                var result = openFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {

                    Properties.Settings.Default.VideoPlayerPath = openFileDialog.FileName;
                    Properties.Settings.Default.Save();
                    VideoPlayerPathTextBox.Text = Properties.Settings.Default.VideoPlayerPath;
                }

                openFileDialog.Dispose();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void VideoPlayerPathTextBox_TextChanged(object sender, EventArgs e)
        {

        }


        //public partial class SettingsForm : Form
        //{
        //    [DllImport("user32.dll")]
        //    static extern bool SetKeyboardState(byte[] lpKeyState);
        //    [DllImport("user32.dll")]
        //    static extern bool GetKeyboardState(byte[] lpKeyState);

        //    public SettingsForm()
        //    {
        //        InitializeComponent();
        //    }

        //    private void KeySettingForm_KeyPress(object sender, KeyPressEventArgs e)
        //    {
        //        var shift = new byte[256];
        //        GetKeyboardState(shift);

        //        File.WriteAllBytes("C:\\Users\\jenek\\source\\repos\\0xotHik\\" +
        //                           "BilingualSubtitler\\BilingualSubtitler\\bin\\Debug\\shift.dat", shift);

        //    }

        //    private void KeySettingForm_KeyDown(object sender, KeyEventArgs e)
        //    {
        //        var shift = new byte[256];
        //        GetKeyboardState(shift);

        //        File.WriteAllBytes("C:\\Users\\jenek\\source\\repos\\0xotHik\\" +
        //                           "BilingualSubtitler\\BilingualSubtitler\\bin\\Debug\\shiftDown.dat", shift);
        //    }
        //}
    }
}
