using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using VoiceLauncher.Models;

namespace VoiceLauncher
{
    public partial class CustomIntelliSenseSingleRecord : Form
    {
        private VoiceLauncherContext db = new VoiceLauncherContext();
        public int CurrentId { get; set; }

        public CustomIntelliSenseSingleRecord()
        {
            InitializeComponent();
        }

        private void CustomIntelliSenseSingleRecord_Load(object sender, EventArgs e)
        {
            db.CustomIntelliSenses.Where(v => v.ID == CurrentId).Load();
            customIntelliSenseBindingSource.DataSource = db.CustomIntelliSenses.Local.ToBindingList();
            db.Languages.OrderBy(o => o.LanguageName).Load();
            BindingSource bindingSourceLanguage = new BindingSource();
            bindingSourceLanguage.DataSource = db.Languages.Local.ToBindingList();
            languageIDComboBox.DataSource = bindingSourceLanguage;
            Text = $"Custom IntelliSense Single Record ID {CurrentId}";
            languageIDComboBox.DisplayMember = "LanguageName";  // the Name property in Choice class
            languageIDComboBox.ValueMember = "ID";  // ditto for the Value property        }
            languageIDComboBox.DataBindings.Clear();
            languageIDComboBox.DataBindings.Add(new Binding("SelectedValue", customIntelliSenseBindingSource, "LanguageId", true));
            db.Categories.Where(v => v.CategoryType == "IntelliSense Command").OrderBy(o => o.CategoryName).Load();
            BindingSource bindingSourceCategory = new BindingSource();
            bindingSourceCategory.DataSource = db.Categories.Local.ToBindingList();
            categoryIDComboBox.DataSource = bindingSourceCategory;
            categoryIDComboBox.DisplayMember = "CategoryName";
            categoryIDComboBox.ValueMember = "ID";
            categoryIDComboBox.DataBindings.Clear();
            categoryIDComboBox.DataBindings.Add(new Binding("SelectedValue", customIntelliSenseBindingSource, "CategoryId", true));
            deliveryTypeComboBox.Items.Add("Copy and Paste");
            deliveryTypeComboBox.Items.Add("Send Keys");
            deliveryTypeComboBox.Items.Add("Executed as Script");
            deliveryTypeComboBox.Items.Add("Clipboard Only");
        }

        private void customIntelliSenseBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            customIntelliSenseBindingSource.EndEdit();
            db.SaveChanges();
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
