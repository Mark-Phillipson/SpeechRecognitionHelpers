﻿using DictationBoxMSP;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using VoiceLauncher.Models;

namespace VoiceLauncher
{
    public partial class SearchCustomIS : Form
    {
        VoiceLauncherContext db = new VoiceLauncherContext();
        public string SearchTerm { get; set; }
        public SearchCustomIS()
        {
            InitializeComponent();
        }

        private void customIntelliSenseBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.customIntelliSenseBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.voiceLauncherDataSet);

        }

        private void SearchCustomIS_Load(object sender, EventArgs e)
        {


            textBoxSearch.Text = SearchTerm;
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                customIntelliSenseBindingSource1.Filter = $"[Display_Value] like '%{SearchTerm}%'";
                customIntelliSenseBindingSource1.Sort = "[Display_Value]";
            }
            this.customIntelliSenseTableAdapter.Fill(this.voiceLauncherDataSet.CustomIntelliSense);
            //// TODO: This line of code loads data into the 'voiceLauncherDataSet.CustomIntelliSense' table. You can move, or remove it, as needed.
            //this.customIntelliSenseTableAdapter.Fill(this.voiceLauncherDataSet.CustomIntelliSense);
            UpdateResult();

        }

        private void customIntelliSenseBindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.customIntelliSenseBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.voiceLauncherDataSet);

        }


        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxSearch.Text))
            {
                SearchTerm = textBoxSearch.Text;
                customIntelliSenseBindingSource1.Filter = $"[Display_Value] like '%{SearchTerm}%'";
                Text = $"Custom IntelliSense Search Term: {SearchTerm}";
            }

            UpdateResult();
        }

        private void customIntelliSenseListBox_KeyDown(object sender, KeyEventArgs e)
        {
            string value = "";
            if (e.KeyCode == Keys.Enter)
            {
                var currentItem = (DataRowView)customIntelliSenseListBox.SelectedItem;
                string delivery = currentItem.Row["DeliveryType"].ToString();
                value = currentItem.Row["SendKeys_Value"].ToString();
                var id = Int32.Parse(currentItem.Row["ID"].ToString());
                try
                    {
                    SendKeys.SendWait("%{Tab}");
                    if (delivery == "Copy and Paste")
                    {
                        Clipboard.SetText(value);
                        SendKeys.SendWait("^v");
                        Text = $"{value} has been pasted";
                    }
                    else
                    {
                        int msec = (int)(1000);
                        Thread.Sleep(msec);
                        SendKeys.SendWait(value.Replace("{Space}"," "));
                    }

                    var additionalCommands = db.AdditionalCommands.Where(w => w.CustomIntelliSenseId == id).ToList();
                    foreach (AdditionalCommand item in additionalCommands)
                    {
                        if (item.WaitBefore > 0)
                        {
                            int msec = (int)(item.WaitBefore * 100);
                            Thread.Sleep(msec);
                        }
                        if (item.DeliveryType == "Copy and Paste")
                        {
                            Clipboard.SetText(item.SendKeysValue);
                            SendKeys.Send("^V");
                        }
                        else
                        {
                            SendKeys.SendWait(item.SendKeysValue.Replace("{Space}"," "));
                        }
                    }
                }
                catch (Exception exception)
                {
                    Clipboard.SetText(value);
                    var message = exception.Message;
                    if (exception.InnerException != null)
                    {
                        message = $"{message} Inner Exception {exception.InnerException.Message}";
                    }
                    DisplayMessage displayMessage = new DisplayMessage($"Error occurred with: {value}\r\r{message}");
                    Application.Run(displayMessage);
                }
                Application.Exit();
                
            }
            else if (e.KeyCode == Keys.Delete)
            {
                CustomIntelliSenseSingleRecord customIntelliSenseSingleRecord= new CustomIntelliSenseSingleRecord();
                var currentItem = (DataRowView)customIntelliSenseListBox.SelectedItem;
                var id = Int32.Parse(currentItem.Row["ID"].ToString());
                customIntelliSenseSingleRecord.CurrentId = id;
                customIntelliSenseSingleRecord.ShowDialog();
            }
        }

        private void customIntelliSenseListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void UpdateResult()
        {
            string value = "";
            var currentItem = (DataRowView)customIntelliSenseListBox.SelectedItem;
            if (currentItem != null)
            {
                value = currentItem.Row["SendKeys_Value"].ToString();
                textBoxResult.Text = value;
                var  languageId = Int32.Parse(currentItem.Row["LanguageID"].ToString());
                var  categoryId = Int32.Parse(currentItem.Row["CategoryID"].ToString());
                var language = db.Languages.Where(c => c.ID == languageId).FirstOrDefault();
                var category = db.Categories.Where(v => v.ID == categoryId).FirstOrDefault();
                if (language != null && category != null)
                {
                    Text = $"Language: {language.LanguageName} Category: {category.CategoryName} Press Delete to Edit";
                }
            }
        }

        private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                customIntelliSenseListBox_KeyDown(sender, e);
            }
            else if (e.KeyCode == Keys.Down)
            {
                customIntelliSenseListBox.Focus();
            }
        }
    }
}