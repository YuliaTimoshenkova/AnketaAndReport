using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using DevExpress.XtraReports.UI;

namespace a1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int a = trackBar1.Value;
            int b = a % 10;
            if (a > 10 && a < 15) { label2.Text = a + "   лет "; }
            if (b > 1 && b < 5 && a != 10 && a != 11 && a != 12 && a != 13 && a != 14)
            { label2.Text = a + "   года "; }
            else
            {
                if (b == 1 && a != 11) { label2.Text = a + "   год "; }
                else { label2.Text = a + "   лет "; };
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
            var hd = new HobiData
            {
                music = checkBox1.Checked,
                sport = checkBox2.Checked,
                age = trackBar1.Value,
                other = checkBox3.Checked,
                name = textBox1.Text,
                otherP = textBox2.Text,
                voz = label2.Text,
            };
            listBox1.Items.Add(hd);
        }
        public class AnketaData
        {
            public SexType sex { get; set; }
            public List<HobiData> Hobi { get; set; }
            [XmlIgnore]
            public string Caption
            {
                get
                {
                    if (sex == SexType.Female)
                        return "Пол: женский";
                    return "Пол: мужской";
                }
            }
        }

        public class HobiData
        {
            public bool music { get; set; }
            public bool sport { get; set; }
            public bool other { get; set; }
            public int age { get; set; }
            public string name { get; set; }
            public string otherP { get; set; }
            public string voz { get; set; }
            [XmlIgnore]
            public string Description { get { return this.ToString(); } }
            public override string ToString()
            {
                var s = name + " , возраст:   " + voz + " увлекается:  ";
                if (music)
                    s += " музыкой, ";
                if (sport)
                    s += " спортом, ";
                if (other)
                    s += otherP;
                return s;
            }
        }
        public enum SexType
        {
            Female,
            Male
        }
        private AnketaData CreateAnketaData()
        {
            AnketaData ad = new AnketaData();
            if (radioButton1.Checked)
                ad.sex = SexType.Female;
            else
                ad.sex = SexType.Male;
            ad.Hobi = new List<HobiData>();
            foreach (HobiData hd in listBox1.Items)
            { ad.Hobi.Add(hd); }
            return ad;
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog() { Filter = "файл анкеты|*.anketaml" };
            var result = sfd.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                var fileName = sfd.FileName;
                var ad = CreateAnketaData();

                XmlSerializer xs = new XmlSerializer(typeof(AnketaData));
                var fileStream = File.Create(fileName);
                xs.Serialize(fileStream, ad);
                fileStream.Close();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var ar = new AnketaReport();
            AnketaData ad = CreateAnketaData();
            ar.DataSource = new BindingSource() { DataSource = ad };
            ar.ShowPreview();
        }


    }
}