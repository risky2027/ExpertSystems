using MoreLinq;
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

        List<JournalPositionInAlgorithm> listWorst = new List<JournalPositionInAlgorithm>(); //элементы в списке хуже нормальных
        List<JournalPositionInAlgorithm> listNorm = new List<JournalPositionInAlgorithm>(); //элементы в списке норм
        List<JournalPositionInAlgorithm> listBest = new List<JournalPositionInAlgorithm>(); //элементы в списке лучше нормальных

        public Form1()
        {
            InitializeComponent();

            db = new JournalContext();
            db.Journals.Load();

            dataGridView1.DataSource = db.Journals.Local.ToBindingList();

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Название журнала";
            dataGridView1.Columns[2].HeaderText = "Суммарное число цитирований журнала в РИНЦ";
            dataGridView1.Columns[3].HeaderText = "Общее число статей из журнала";
            dataGridView1.Columns[4].HeaderText = "Среднее число статей в выпуске";
            dataGridView1.Columns[5].HeaderText = "Средняя оценка по результатам общественной экспертизы";
            dataGridView1.Columns[6].HeaderText = "Показатель журнала в рейтинге SCIENCE INDEX";
            dataGridView1.Columns[7].HeaderText = "Место журнала в рейтинге SCIENCE INDEX";
            dataGridView1.Columns[8].HeaderText = "Место журнала в рейтинге по алгоритму";
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

            journal.NameOfJournal = addJFrom.textBox1.Text;
            if (Int32.TryParse(addJFrom.textBox2.Text, out num))
                journal.SumNumbersOfCitations = num;
            if (Int32.TryParse(addJFrom.textBox3.Text, out num))
                journal.SumNumbersOfArticles = num;
            if (Double.TryParse(addJFrom.textBox4.Text, out k))
                journal.AverageNumbersOfArticles = k;
            if (Double.TryParse(addJFrom.textBox5.Text, out k))
                journal.AverageMarkOfPublicExpertise = k;
            if (Double.TryParse(addJFrom.textBox6.Text, out k))
                journal.IndicatorInRating = k;
            if (Double.TryParse(addJFrom.textBox7.Text, out k))
                journal.PositionInScienceIndex = k;

            journal.PositionInAlgorithm = Math.Round(Int32.Parse(addJFrom.textBox2.Text) / Double.Parse(addJFrom.textBox4.Text) /
                Double.Parse(addJFrom.textBox3.Text) + 10 * Double.Parse(addJFrom.textBox5.Text) + Double.Parse(addJFrom.textBox6.Text), 2);

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

                addJFrom.textBox1.Text = journal.NameOfJournal.ToString();
                addJFrom.textBox2.Text = journal.SumNumbersOfCitations.ToString();
                addJFrom.textBox3.Text = journal.SumNumbersOfArticles.ToString();
                addJFrom.textBox4.Text = journal.AverageNumbersOfArticles.ToString();
                addJFrom.textBox5.Text = journal.AverageMarkOfPublicExpertise.ToString();
                addJFrom.textBox6.Text = journal.IndicatorInRating.ToString();
                addJFrom.textBox7.Text = journal.PositionInScienceIndex.ToString();

                DialogResult result = addJFrom.ShowDialog(this);

                if (result == DialogResult.Cancel)
                    return;

                int num = 0;
                double k = 0;

                journal.NameOfJournal = addJFrom.textBox1.Text;
                if (Int32.TryParse(addJFrom.textBox2.Text, out num))
                    journal.SumNumbersOfCitations = num;
                if (Int32.TryParse(addJFrom.textBox3.Text, out num))
                    journal.SumNumbersOfArticles = num;
                if (Double.TryParse(addJFrom.textBox4.Text, out k))
                    journal.AverageNumbersOfArticles = k;
                if (Double.TryParse(addJFrom.textBox5.Text, out k))
                    journal.AverageMarkOfPublicExpertise = k;
                if (Double.TryParse(addJFrom.textBox6.Text, out k))
                    journal.IndicatorInRating = k;
                if (Double.TryParse(addJFrom.textBox7.Text, out k))
                    journal.PositionInScienceIndex = k;

                journal.PositionInAlgorithm = Math.Round(Int32.Parse(addJFrom.textBox2.Text) / Double.Parse(addJFrom.textBox4.Text) /
                Double.Parse(addJFrom.textBox3.Text) + 10 * Double.Parse(addJFrom.textBox5.Text) + Double.Parse(addJFrom.textBox6.Text), 2);

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

        private void RateJournals()
        {
            var journals = db.Journals.ToList();

            var listA = new List<JournalPositionInAlgorithm>();
            for (int i = 0; i < journals.Count; i++)
            {
                listA.Add(new JournalPositionInAlgorithm
                {
                    NameOfJournal = journals[i].NameOfJournal,
                    PositionInAlgorithm = journals[i].PositionInAlgorithm
                });
            }

            var listB = listA.DistinctBy(x => x.PositionInAlgorithm);

            int[] arrD = new int[listB.Count()];

            for (int i = 0; i < listA.Count(); i++)
            {
                for (int j = 0; j < listB.Count(); j++)
                {
                    if (listA[i].PositionInAlgorithm == listB.ElementAt(j).PositionInAlgorithm)
                    {
                        arrD[j]++;
                    }
                }
            }

            int maxNumber = arrD.Max();
            int count = 0;

            for (int i = 0; i < arrD.Length; i++)
            {
                count += arrD[i];

                if (arrD[i] == maxNumber)
                {
                    for (int j = 0; j < listA.Count; j++)
                    {
                        if (listA[j].PositionInAlgorithm == listA[count].PositionInAlgorithm)
                        {
                            listNorm.Add(new JournalPositionInAlgorithm
                            {
                                NameOfJournal = listA[j].NameOfJournal,
                                PositionInAlgorithm = listA[j].PositionInAlgorithm
                            });
                        }
                    }
                    for (int r = 0; r < listA.Count; r++)
                    {
                        if (!listNorm.Contains(listA[r]))
                        {
                            if (listA[r].PositionInAlgorithm < listNorm[0].PositionInAlgorithm)
                            {
                                listWorst.Add(listA[r]);
                            }
                            else if (listA[r].PositionInAlgorithm > listNorm[0].PositionInAlgorithm)
                            {
                                listBest.Add(listA[r]);
                            }
                        }
                    }
                }
            }
        }


        private void Button4_Click(object sender, EventArgs e)
        {
            listBest.Clear();
            RateJournals();

            dataGridView1.DataSource = listBest.OrderBy(x => x.PositionInAlgorithm, OrderByDirection.Ascending).ToList(); ;

            dataGridView1.Columns[0].HeaderText = "Название журнала";
            dataGridView1.Columns[1].HeaderText = "Место журнала в рейтинге по алгоритму";

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            listNorm.Clear();
            RateJournals();

            dataGridView1.DataSource = listNorm;

            dataGridView1.Columns[0].HeaderText = "Название журнала";
            dataGridView1.Columns[1].HeaderText = "Место журнала в рейтинге по алгоритму";

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;

        }

        private void Button6_Click(object sender, EventArgs e)
        {
            listWorst.Clear();
            RateJournals();

            dataGridView1.DataSource = listWorst.OrderBy(x => x.PositionInAlgorithm, OrderByDirection.Ascending).ToList();

            dataGridView1.Columns[0].HeaderText = "Название журнала";
            dataGridView1.Columns[1].HeaderText = "Место журнала в рейтинге по алгоритму";

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            db = new JournalContext();
            db.Journals.Load();

            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = db.Journals.Local.ToBindingList();

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Название журнала";
            dataGridView1.Columns[2].HeaderText = "Суммарное число цитирований журнала в РИНЦ";
            dataGridView1.Columns[3].HeaderText = "Общее число статей из журнала";
            dataGridView1.Columns[4].HeaderText = "Среднее число статей в выпуске";
            dataGridView1.Columns[5].HeaderText = "Средняя оценка по результатам общественной экспертизы";
            dataGridView1.Columns[6].HeaderText = "Показатель журнала в рейтинге SCIENCE INDEX";
            dataGridView1.Columns[7].HeaderText = "Место журнала в рейтинге SCIENCE INDEX";
            dataGridView1.Columns[8].HeaderText = "Место журнала в рейтинге по алгоритму";

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }
    }
}
