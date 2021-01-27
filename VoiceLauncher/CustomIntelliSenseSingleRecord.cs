
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VoiceLauncher.Models;

namespace VoiceLauncher
{
    public partial class CustomIntelliSenseSingleRecord : Form
    {
        private VoiceLauncherContext db = new VoiceLauncherContext();
        public int CurrentId { get; set; }
        public string DefaultValueToSend { get; set; }
        public int? LanguageId { get; internal set; }
        public int? CategoryId { get; internal set; }

        public CustomIntelliSenseSingleRecord()
        {
            InitializeComponent();
        }

        private void CustomIntelliSenseSingleRecord_Load(object sender, EventArgs e)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.CustomIntelliSenses.Where(v => v.ID == CurrentId).Load();
            customIntelliSenseBindingSource.DataSource = db.CustomIntelliSenses.Local.ToBindingList();
            if (CurrentId == 0 && LanguageId != null && CategoryId != null)
            {
                CustomIntelliSense customIntelliSense = new CustomIntelliSense
                {
                    SendKeys_Value = DefaultValueToSend,
                    Command_Type = "SendKeys",
                    DeliveryType = "Copy and Paste",
                    LanguageID = (int)LanguageId,
                    CategoryID = (int)CategoryId
                };
                db.CustomIntelliSenses.Local.Add(customIntelliSense);
            }
            else
            {
                CustomIntelliSense customIntelliSense = new CustomIntelliSense
                {
                    SendKeys_Value = DefaultValueToSend,
                    Command_Type = "SendKeys",
                    DeliveryType = "Copy and Paste"
                };
                db.CustomIntelliSenses.Local.Add(customIntelliSense);
            }
            db.Languages.OrderBy(o => o.LanguageName).Load();
            customIntelliSenseBindingNavigator.BackColor = Color.FromArgb(100, 100, 100);
            customIntelliSenseBindingNavigator.ForeColor = Color.White;
            Text = $"Custom IntelliSense Single Record ID {CurrentId}";

            BindingSource bindingSourceLanguage = new BindingSource();
            bindingSourceLanguage.DataSource = db.Languages.Where(v => v.Active == true).OrderBy(v => v.LanguageName).ToList();
            languageIDComboBox.DataBindings.Clear();
            languageIDComboBox.DataSource = db.Languages.Where(v => v.Active == true).OrderBy(v => v.LanguageName).ToList();
            languageIDComboBox.DisplayMember = "LanguageName";  // the Name property in Language class
            languageIDComboBox.ValueMember = "ID";  // ditto for the Value property        }
            languageIDComboBox.DataBindings.Add(new Binding("SelectedValue", customIntelliSenseBindingSource, "LanguageID", true));

            db.Categories.Where(v => v.CategoryType == "IntelliSense Command").OrderBy(o => o.CategoryName).Load();
            BindingSource bindingSourceCategory = new BindingSource();
            bindingSourceCategory.DataSource = db.Categories.Where(v => v.CategoryType == "IntelliSense Command").OrderBy(v => v.CategoryName).ToList();
            categoryIDComboBox.DataBindings.Clear();
            categoryIDComboBox.DataSource = db.Categories.Where(v => v.CategoryType == "IntelliSense Command").OrderBy(v => v.CategoryName).ToList(); ;
            categoryIDComboBox.DisplayMember = "CategoryName";
            categoryIDComboBox.ValueMember = "ID";
            categoryIDComboBox.DataBindings.Add(new Binding("SelectedValue", customIntelliSenseBindingSource, "CategoryID", true));


            deliveryTypeComboBox.Items.Add("Copy and Paste");
            deliveryTypeComboBox.Items.Add("Send Keys");
            deliveryTypeComboBox.Items.Add("Executed as Script");
            deliveryTypeComboBox.Items.Add("Clipboard Only");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            db.Dispose();
            base.OnClosing(e);
        }

        private void customIntelliSenseBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            var local= db.CustomIntelliSenses.Local.ToBindingList();
            if (local.Count > 1)
            {
                var emptyOne = local.FirstOrDefault(v => v.Display_Value == null);
                if (emptyOne!= null )
                {
                    local.Remove(emptyOne);
                }
            }
            customIntelliSenseBindingSource.EndEdit();
            try
            {
                db.SaveChanges();
            }
            catch (Exception exception)
            {

                var message = ExceptionHandling.GetShortErrorMessage(exception);
                MessageBox.Show($"{message}", "Exception Occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Text = $"Saved Successfully at {DateTime.Now.ToShortTimeString()}";
        }

        private void CustomIntelliSenseSingleRecord_Shown(object sender, EventArgs e)
        {
            display_ValueTextBox.Focus();
        }
    }
}
