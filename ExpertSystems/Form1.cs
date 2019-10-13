using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpertSystems
{
    public partial class Form1 : Form
    {
        JournalContext db;
        public Form1()
        {
            InitializeComponent();

            db = new JournalContext();
            db.Journals.Load();

            dataGridView1.DataSource = db.Journals.Local.ToBindingList();

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Суммарное число цитирований журнала в РИНЦ";
            dataGridView1.Columns[2].HeaderText = "Среднее число статей в выпуске";
            dataGridView1.Columns[3].HeaderText = "Средняя оценка по результатам общественной экспертизы";
            dataGridView1.Columns[4].HeaderText = "Показатель журнала в рейтинге SCIENCE INDEX";
            dataGridView1.Columns[5].HeaderText = "Средний индекс Хирша авторов";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            AddJournalForm addJFrom = new AddJournalForm();
            DialogResult result = addJFrom.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            Journal journal = new Journal();

            int num = 0;
            double k = 0;
            if (Int32.TryParse(addJFrom.textBox1.Text, out num))
                journal.SumNumbersOfCitations = num;
            if (Double.TryParse(addJFrom.textBox2.Text, out k))
                journal.AverageNumbersOfArticles = k;
            if (Double.TryParse(addJFrom.textBox3.Text, out k))
                journal.AverageMarkOfPublicExpertise = k;
            if (Double.TryParse(addJFrom.textBox4.Text, out k))
                journal.IndicatorInRating = k;
            if (Double.TryParse(addJFrom.textBox5.Text, out k))
                journal.AverageIndexHirsh = k;

            db.Journals.Add(journal);
            db.SaveChanges();

            MessageBox.Show("Новый журнал добавлен");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                Journal journal = db.Journals.Find(id);

                AddJournalForm addJFrom = new AddJournalForm();

                addJFrom.textBox1.Text = journal.SumNumbersOfCitations.ToString();
                addJFrom.textBox2.Text = journal.AverageNumbersOfArticles.ToString();
                addJFrom.textBox3.Text = journal.AverageMarkOfPublicExpertise.ToString();
                addJFrom.textBox4.Text = journal.IndicatorInRating.ToString();
                addJFrom.textBox5.Text = journal.AverageIndexHirsh.ToString();

                DialogResult result = addJFrom.ShowDialog(this);

                if (result == DialogResult.Cancel)
                    return;

                int num = 0;
                double k = 0;
                if (Int32.TryParse(addJFrom.textBox1.Text, out num))
                    journal.SumNumbersOfCitations = num;
                if (Double.TryParse(addJFrom.textBox2.Text, out k))
                    journal.AverageNumbersOfArticles = k;
                if (Double.TryParse(addJFrom.textBox3.Text, out k))
                    journal.AverageMarkOfPublicExpertise = k;
                if (Double.TryParse(addJFrom.textBox4.Text, out k))
                    journal.IndicatorInRating = k;
                if (Double.TryParse(addJFrom.textBox5.Text, out k))
                    journal.AverageIndexHirsh = k;

                db.SaveChanges();
                dataGridView1.Refresh(); // обновляем грид
                MessageBox.Show("Журнал обновлен");
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                Journal journal = db.Journals.Find(id);
                db.Journals.Remove(journal);
                db.SaveChanges();

                MessageBox.Show("Журнал удален");
            }
        }
    }
}
