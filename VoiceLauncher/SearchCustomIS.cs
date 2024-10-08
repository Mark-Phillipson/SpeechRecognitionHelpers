﻿
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using VoiceLauncher.Models;

using WindowsInput;
using WindowsInput.Native;

namespace VoiceLauncher
{
  public partial class SearchCustomIS : Form
  {
    VoiceLauncherContext db = new VoiceLauncherContext();
    public string SearchTerm { get; set; }
    public string LanguageName { get; set; }
    public string CategoryName { get; set; }
    private bool _useInputSimulator { get; set; }
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
      else if (!string.IsNullOrWhiteSpace(LanguageName) && !string.IsNullOrWhiteSpace(CategoryName))
      {
        var language = db.Languages.FirstOrDefault(l => l.LanguageName.ToLower() == LanguageName);
        var category = db.Categories.FirstOrDefault(c => c.CategoryName.ToLower() == CategoryName);
        if (language != null && category != null)
        {
          customIntelliSenseBindingSource1.Filter = $"[LanguageID] = {language.ID} And [CategoryID] = {category.ID}";
          customIntelliSenseBindingSource1.Sort = "[Display_Value]";
        }
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
      PerformAction(e.KeyCode);
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
        textBoxRemarks.Text = currentItem.Row["Remarks"].ToString();
        var languageId = Int32.Parse(currentItem.Row["LanguageID"].ToString());
        var categoryId = Int32.Parse(currentItem.Row["CategoryID"].ToString());
        var language = db.Languages.Where(c => c.ID == languageId).FirstOrDefault();
        var category = db.Categories.Where(v => v.ID == categoryId).FirstOrDefault();
        if (language != null && category != null)
        {
          Text = $"Language: {language.LanguageName} Category: {category.CategoryName} Press Delete to Edit";
          labelLanguageAndCategory.Text = $"{language.LanguageName} / {category.CategoryName}";
        }
      }
    }

    private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        PerformAction(Keys.Enter);
      }
      else if (e.KeyCode == Keys.Down)
      {
        customIntelliSenseListBox.Focus();
      }
    }

    private void buttonInsert_Click(object sender, EventArgs e)
    {
      PerformAction(Keys.Enter);
    }
    private void PerformAction(Keys keyCode)
    {
      string value = "";
      if (keyCode == Keys.Enter)
      {
        var currentItem = (DataRowView)customIntelliSenseListBox.SelectedItem;
        string delivery = currentItem.Row["DeliveryType"].ToString();
        value = currentItem.Row["SendKeys_Value"].ToString();
        var id = Int32.Parse(currentItem.Row["ID"].ToString());
        InputSimulator inputSimulator = new InputSimulator();
        try
        {
          if (_useInputSimulator)
          {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.MENU, VirtualKeyCode.TAB);
          }
          else
          {
            SendKeys.SendWait("%{Tab}");
          }
          if (delivery == "Copy and Paste")
          {
            Clipboard.SetText(value);
            Thread.Sleep(100);
            if (_useInputSimulator)
            {
              inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
            }
            else
            {
              SendKeys.SendWait("^v");
            }
            Text = $"{value} has been pasted";
          }
          else
          {
            int msec = (int)(400);
            Thread.Sleep(msec);
            if (_useInputSimulator)
            {
              inputSimulator.Keyboard.TextEntry(value.Replace("{Space}", " "));
            }
            else
            {
              SendKeys.SendWait(value.Replace("{Space}", " "));
            }
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
              if (_useInputSimulator)
              {
                inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V); 
              }
              else
              {
                SendKeys.Send("^V"); 
              }
            }
            else
            {
              if (_useInputSimulator)
              {
                inputSimulator.Keyboard.TextEntry(value.Replace("{Space}", " ")); 
              }
              else
              {
                SendKeys.SendWait(item.SendKeysValue.Replace("{Space}", " ")); 
              }
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
      else if (keyCode == Keys.Delete)
      {
        CustomIntelliSenseSingleRecord customIntelliSenseSingleRecord = new CustomIntelliSenseSingleRecord();
        var currentItem = (DataRowView)customIntelliSenseListBox.SelectedItem;
        var id = Int32.Parse(currentItem.Row["ID"].ToString());
        customIntelliSenseSingleRecord.CurrentId = id;
        customIntelliSenseSingleRecord.ShowDialog();
      }

    }
    private void groupBoxResults_Enter(object sender, EventArgs e)
    {

    }

    private void buttonCopyText_Click_1(object sender, EventArgs e)
    {
      Clipboard.SetText(textBoxResult.Text);
      buttonCopyText.Text = "Copied!";
    }

    private void buttonInsert_Click_1(object sender, EventArgs e)
    {
      PerformAction(Keys.Enter);
    }


		private void checkBoxUseInputSimulator_CheckedChanged_1(object sender, EventArgs e)
		{
			_useInputSimulator = checkBoxUseInputSimulator.Checked;

		}

		private void buttonOpenWebsite_Click_1(object sender, EventArgs e)
		{
			var currentItem = (DataRowView)customIntelliSenseListBox.SelectedItem;
			var id = Int32.Parse(currentItem.Row["ID"].ToString());
			System.Diagnostics.Process.Start($"http://localhost:5000/customintellisense/{id}");

		}
	}
}
