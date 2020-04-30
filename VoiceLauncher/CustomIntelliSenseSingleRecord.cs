using System;
using System.Data;
using System.Data.Entity;
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
            db.CustomIntelliSenses.Where(v => v.ID == CurrentId).Load();
            customIntelliSenseBindingSource.DataSource = db.CustomIntelliSenses.Local.ToBindingList();
            if (CurrentId == 0)
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
            db.Languages.OrderBy(o => o.LanguageName).Load();
            customIntelliSenseBindingNavigator.BackColor = Color.FromArgb(100, 100, 100);
            customIntelliSenseBindingNavigator.ForeColor = Color.White;
            BindingSource bindingSourceLanguage = new BindingSource();
            bindingSourceLanguage.DataSource = db.Languages.Local.ToBindingList();
            languageIDComboBox.DataSource = bindingSourceLanguage;
            Text = $"Custom IntelliSense Single Record ID {CurrentId}";
            languageIDComboBox.DisplayMember = "LanguageName";  // the Name property in Choice class
            languageIDComboBox.ValueMember = "ID";  // ditto for the Value property        }
            languageIDComboBox.DataBindings.Clear();
            languageIDComboBox.DataBindings.Add(new Binding("SelectedValue", customIntelliSenseBindingSource, "LanguageId"));
            db.Categories.Where(v => v.CategoryType == "IntelliSense Command").OrderBy(o => o.CategoryName).Load();
            BindingSource bindingSourceCategory = new BindingSource();
            bindingSourceCategory.DataSource = db.Categories.Local.ToBindingList();
            categoryIDComboBox.DataSource = bindingSourceCategory;
            categoryIDComboBox.DisplayMember = "CategoryName";
            categoryIDComboBox.ValueMember = "ID";
            categoryIDComboBox.DataBindings.Clear();
            categoryIDComboBox.DataBindings.Add(new Binding("SelectedValue", customIntelliSenseBindingSource, "CategoryId"));
            deliveryTypeComboBox.Items.Add("Copy and Paste");
            deliveryTypeComboBox.Items.Add("Send Keys");
            deliveryTypeComboBox.Items.Add("Executed as Script");
            deliveryTypeComboBox.Items.Add("Clipboard Only");
        }

        private void customIntelliSenseBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            customIntelliSenseBindingSource.EndEdit();
            try
            {
                db.SaveChanges();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            this.Text = $"Saved Successfully at {DateTime.Now.ToShortTimeString()}";

        }

        private void comboBoxLanguageID_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void sendKeys_ValueLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
