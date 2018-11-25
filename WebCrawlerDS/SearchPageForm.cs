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

namespace WebCrawlerDS
{
    public partial class SearchPageForm : Form
    {
        System.IO.StreamWriter file2;
        AutoCompleteStringCollection DataCollection;
        SearchInUrls searchObj;
        public SearchPageForm()
        {
            InitializeComponent();
            label1.Text = "";
            searchObj = new SearchInUrls();
            Thread oThread = new Thread(new ThreadStart(searchObj.setupData));
            oThread.Start();
            oThread.Join();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            button1.Click += btn1_Click;
            button1.Text = "Search";
            listView1.Scrollable = true;
            var item2 = new ListViewItem(new[] { "abc" });
            listView1.Width = 590;
            listView1.Height = 150;
            listView1.View = View.Details;
           // listView1.Columns.Add("S.No", -2, HorizontalAlignment.Left);
           
            listView1.Columns.Add("URLs matching key words", -2, HorizontalAlignment.Left);

            textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            DataCollection = new AutoCompleteStringCollection();
           addItems(DataCollection);
            textBox1.AutoCompleteCustomSource = DataCollection;
        }

        public void addItems(AutoCompleteStringCollection col)
        {
            for (int i = 0; i < searchObj.dic.Count; i++ )
                col.Add(searchObj.dic[i]);

        }
        private void btn1_Click(object sender, EventArgs e)
        {
            Control control = button1.Parent;
            // Set the text and backcolor of the parent control.
            control.Text = "My Groupbox";
            control.BackColor = Color.Azure;
            // Get the form that the Button control is contained within.
            Form myForm = button1.FindForm();
            // Set the text and color of the form containing the Button.
            myForm.Text = "The Form of My Control";
          //  myForm.BackColor = Color.Green;

        }

        private void SearchPageForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if(listView1.SelectedItems.Count == 0)
                return;
            var firstSelectedItem = listView1.SelectedItems[0];

          //  MessageBox.Show(firstSelectedItem.Text);
            System.Diagnostics.Process.Start(firstSelectedItem.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int c = 0;
            int s = DataCollection.Count;
            for(int i = 0;i < s;i++)
            {
                if (DataCollection[i].Equals(textBox1.Text))
                    c = 1;
            }

            if(c == 0)
            {
                file2 = new System.IO.StreamWriter("d:\\wordsDic.txt");
                file2.WriteLine(textBox1.Text);
                file2.Close();
                DataCollection.Add(textBox1.Text);
            }

            List<string> value = new List<string>() ; // Used to store the return value
            label1.Text = "Searching..";
            var thread = new Thread(
              () =>
              {
                  value = searchObj.getSearchResults(textBox1.Text); // Publish the return value
              });
            thread.Start();
            thread.Join(); 
            label1.Text = value.Count+" matched records";
           // listView1.DataSource = null;
            listView1.Items.Clear();
            for(int i=0; i<value.Count;i++)
            {
                ListViewItem item2 = new ListViewItem(new[] { value[i] });
                listView1.Items.Add(item2);
            }
        }





    }
}
