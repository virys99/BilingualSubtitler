﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BilingualSubtitler
{
    public partial class SubtitlesAppearanceSettings : UserControl
    {
        public SubtitlesAppearanceSettings()
        {
            InitializeComponent();
        }

        private void changeRussianSubtitlesStylesAccordingToOriginalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // TODO v11
            //SecondAndThirdRussianSubtitlesAtTheTopOfScreenCheckBox.Enabled =
            ////
            ////firstRussianSubtitlesFontComboBox.Enabled = 
            //FirstRussianSubtitlesMarginNumericUpDown.Enabled =
            //    FirstRussianSubtitlesShadowNumericUpDown.Enabled =
            //        FirstRussianSubtitlesOutlineNumericUpDown.Enabled = FirstRussianSubtitlesSizeNumericUpDown.Enabled =
            //                // firstRussianSubtitlesTransparencyPercentageNumericUpDown.Enabled =
            //                // Вторые
            //                SecondRussianSubtitlesFontComboBox.Enabled =
            //                    SecondRussianSubtitlesMarginNumericUpDown.Enabled =
            //                        SecondRussianSubtitlesShadowNumericUpDown.Enabled =
            //                            SecondRussianSubtitlesOutlineNumericUpDown.Enabled =
            //                                SecondRussianSubtitlesSizeNumericUpDown.Enabled =
            //                                    SecondRussianSubtitlesTransparencyPercentageNumericUpDown.Enabled =
            //                                    SecondRussianSubtitlesShadowTransparencyPercentageNumericUpDown.Enabled =
            //                                        // Третьи
            //                                        ThirdRussianSubtitlesFontComboBox.Enabled =
            //                                            ThirdRussianSubtitlesMarginNumericUpDown.Enabled =
            //                                                ThirdRussianSubtitlesShadowNumericUpDown.Enabled =
            //                                                    ThirdRussianSubtitlesOutlineNumericUpDown.Enabled =
            //                                                        ThirdRussianSubtitlesSizeNumericUpDown.Enabled =
            //                                                            ThirdRussianSubtitlesTransparencyPercentageNumericUpDown
            //                                                                    .Enabled =
            //                                                                    ThirdRussianSubtitlesShadowTransparencyPercentageNumericUpDown
            //                                                                    .Enabled =
            //                                                                //
            //                                                                !ChangeRussianSubtitlesStylesAccordingToOriginalCheckBox.Checked;

            //SecondAndThirdRussianSubtitlesAtTheTopOfScreenCheckBox.Enabled = ChangeRussianSubtitlesStylesAccordingToOriginalCheckBox.Checked;
        }

        private void firstRussianSubtitlesFontComboBox_TextChanged(object sender, EventArgs e)
        {
            // TODO v11
            //if (ChangeRussianSubtitlesStylesAccordingToOriginalCheckBox.Checked)
            //{
            //    SecondRussianSubtitlesFontComboBox.Text =
            //        ThirdRussianSubtitlesFontComboBox.Text =
            //            FirstRussianSubtitlesFontComboBox.Text;
            //}
        }

        private void ChangeMargin()
        { 
        //    if (ChangeRussianSubtitlesStylesAccordingToOriginalCheckBox.Checked)
        //    {
        //        if (SecondAndThirdRussianSubtitlesAtTheTopOfScreenCheckBox.Enabled && SecondAndThirdRussianSubtitlesAtTheTopOfScreenCheckBox.Checked)
        //        {
        //            var firstRussianSubtitlesMargin = OriginalSubtitlesMarginNumericUpDown.Value -
        //                                              (2 * OriginalSubtitlesSizeNumericUpDown.Value +
        //                                               OriginalSubtitlesSizeNumericUpDown.Value / 10);
        //            FirstRussianSubtitlesMarginNumericUpDown.Value =
        //                firstRussianSubtitlesMargin < 0 ? 0 : firstRussianSubtitlesMargin;

        //            ThirdRussianSubtitlesMarginNumericUpDown.Value = 290 - (2 * OriginalSubtitlesSizeNumericUpDown.Value +
        //                                               OriginalSubtitlesSizeNumericUpDown.Value / 10);
        //            SecondRussianSubtitlesMarginNumericUpDown.Value = ThirdRussianSubtitlesMarginNumericUpDown.Value -
        //                (2 * OriginalSubtitlesSizeNumericUpDown.Value +
        //                                               OriginalSubtitlesSizeNumericUpDown.Value / 10);
        //        }
        //        else
        //        {
        //            FirstRussianSubtitlesMarginNumericUpDown.Value =
        //                OriginalSubtitlesMarginNumericUpDown.Value +
        //                (2 * OriginalSubtitlesSizeNumericUpDown.Value +
        //                 OriginalSubtitlesSizeNumericUpDown.Value / 10);
        //            SecondRussianSubtitlesMarginNumericUpDown.Value = OriginalSubtitlesMarginNumericUpDown.Value +
        //                                                              (2 * OriginalSubtitlesSizeNumericUpDown.Value +
        //                                                               OriginalSubtitlesSizeNumericUpDown.Value / 10) * 2;

        //            var thirdRussianSubtitlesMargin = OriginalSubtitlesMarginNumericUpDown.Value -
        //                                              (2 * OriginalSubtitlesSizeNumericUpDown.Value +
        //                                               OriginalSubtitlesSizeNumericUpDown.Value / 10);
        //            ThirdRussianSubtitlesMarginNumericUpDown.Value =
        //                thirdRussianSubtitlesMargin < 0 ? 0 : thirdRussianSubtitlesMargin;
        //        }

        //    }
        }

        private void originalSubtitlesMarginNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            // Если включен "Перемещать попарно" — двигаем еще 1-е русские



            //if (changeMarginsSimultaneouslyCheckBox.Checked)
            //    ChangeMargin();
        }

        private void originalSubtitlesSizeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            // Если включен "Перемещать попарно" — двигаем еще 1-е русские



            //TODO v11

            //if (changeMarginsSimultaneouslyCheckBox.Checked) 
            //    ChangeMargin();
        }

        private void originalSubtitlesOutlineNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            //if (changeAdditionalSettingsSimultaneouslyCheckBox.Checked)
            //{
            //    FirstRussianSubtitlesOutlineNumericUpDown.Value =
            //        SecondRussianSubtitlesOutlineNumericUpDown.Value =
            //           ThirdRussianSubtitlesOutlineNumericUpDown.Value =
            //               OriginalSubtitlesOutlineNumericUpDown.Value;
            //}
        }

        private void originalSubtitlesShadowNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            //if (changeAdditionalSettingsSimultaneouslyCheckBox.Checked)
            //{
            //    FirstRussianSubtitlesShadowNumericUpDown.Value =
            //        SecondRussianSubtitlesShadowNumericUpDown.Value =
            //           ThirdRussianSubtitlesShadowNumericUpDown.Value =
            //               OriginalSubtitlesShadowNumericUpDown.Value;
            //}
        }

        private void originalSubtitlesTransparencyPercentageNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            //if (changeAdditionalSettingsSimultaneouslyCheckBox.Checked)
            //{
            //    FirstRussianSubtitlesTransparencyPercentageNumericUpDown.Value= 
            //    SecondRussianSubtitlesTransparencyPercentageNumericUpDown.Value =
            //    ThirdRussianSubtitlesTransparencyPercentageNumericUpDown.Value =
            //        OriginalSubtitlesTransparencyPercentageNumericUpDown.Value;
            //}
        }

        private void firstRussianSubtitlesTransparencyPercentageNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
        }

        private void FirstRussianSubtitlesShadowTransparencyPercentageNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
        }

        private void changeOnTheSameDeltaValuesForAllSubtitlesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
        //    var changeOnTheSameDeltaValuesForAllSubtitles = ((RadioButton)sender).Checked;

        //    setTheSameValuesForAllSubtitlesRadioButton.Checked = !changeOnTheSameDeltaValuesForAllSubtitles;
        //    marginCheckBox.Enabled = changeOnTheSameDeltaValuesForAllSubtitles;

        }

        private void setTheSameValuesForAllSubtitlesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //var setTheSameValuesForAllSubtitles = ((RadioButton)sender).Checked;

            //changeOnTheSameDeltaValuesForAllSubtitlesRadioButton.Checked = !setTheSameValuesForAllSubtitles;
            //marginCheckBox.Enabled = !setTheSameValuesForAllSubtitles;
        }

        private void changeOnTheSameDeltaValuesForAllSubtitlesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var changeOnTheSameDeltaValuesForAllSubtitles = ((CheckBox)sender).Checked;

            setTheSameValuesForAllSubtitlesCheckBox.Checked = !changeOnTheSameDeltaValuesForAllSubtitles;
            marginCheckBox.Enabled = changeOnTheSameDeltaValuesForAllSubtitles;
        }

        private void setTheSameValuesForAllSubtitlesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var setTheSameValuesForAllSubtitles = ((CheckBox)sender).Checked;

            changeOnTheSameDeltaValuesForAllSubtitlesCheckBox.Checked = !setTheSameValuesForAllSubtitles;
            marginCheckBox.Enabled = !setTheSameValuesForAllSubtitles;
        }
    }
}
