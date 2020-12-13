using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Dept.Model;

namespace Dept
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dept.DeptDBDataSet deptDataSet;

        Dept.DeptDBDataSetTableAdapters.subjectsTableAdapter deptDataSetsubjectsTableAdapter = new Dept.DeptDBDataSetTableAdapters.subjectsTableAdapter();
        System.Windows.Data.CollectionViewSource subjectsViewSource;

        Dept.DeptDBDataSetTableAdapters.teachersTableAdapter deptDataSetteachersTableAdapter = new Dept.DeptDBDataSetTableAdapters.teachersTableAdapter();
        System.Windows.Data.CollectionViewSource teachersViewSource;

        Dept.DeptDBDataSetTableAdapters.studentsTableAdapter deptDataSetstudentsTableAdapter = new Dept.DeptDBDataSetTableAdapters.studentsTableAdapter();
        System.Windows.Data.CollectionViewSource studentsViewSource;
        
        Dept.DeptDBDataSetTableAdapters.specialitiesTableAdapter deptDataSetspecialitiesTableAdapter = new Dept.DeptDBDataSetTableAdapters.specialitiesTableAdapter();
        System.Windows.Data.CollectionViewSource specialitiesViewSource;

        Dept.DeptDBDataSetTableAdapters.subjects_specialitiesTableAdapter deptDataSetsubject_specialitiesTableAdapter = new Dept.DeptDBDataSetTableAdapters.subjects_specialitiesTableAdapter();
        System.Windows.Data.CollectionViewSource subjects_specialitiesViewSource;

        Dept.DeptDBDataSetTableAdapters.groupsTableAdapter deptDataSetgroupsTableAdapter = new Dept.DeptDBDataSetTableAdapters.groupsTableAdapter();
        Dept.DeptDBDataSetTableAdapters.teachers_subjectsTableAdapter deptDataSetteachers_subjectsTableAdapter = new Dept.DeptDBDataSetTableAdapters.teachers_subjectsTableAdapter();



        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            this.Closing += MainWindow_Closing; 
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                var r = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход из программы", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes)
                {
                  //    this.Close();
                }

                else
                {
                    e.Cancel = true;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void MainWindow_StateChanged(object sender, EventArgs e)
        //{

        //}


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            deptDataSet = ((Dept.DeptDBDataSet)(this.FindResource("deptDataSet")));

            // дисциплины ---------------------------------------------------------------
            deptDataSetsubjectsTableAdapter.Fill(deptDataSet.subjects);

            deptDataSetgroupsTableAdapter.Fill(deptDataSet.groups);
            deptDataSetteachers_subjectsTableAdapter.Fill(deptDataSet.teachers_subjects);

            subjectsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("subjectsViewSource")));
            subjectsViewSource.View.MoveCurrentToFirst();

            // преподаватели --------------------------------------------------------------
            deptDataSetteachersTableAdapter.Fill(deptDataSet.teachers);
            teachersViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("teachersViewSource")));
            teachersViewSource.View.MoveCurrentToFirst();

            // студенты --------------------------------------------------------------------
            deptDataSetstudentsTableAdapter.Fill(deptDataSet.students);
            studentsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("studentsViewSource")));
            studentsViewSource.View.MoveCurrentToFirst();



            List<string> listSubj = new List<string>();

            foreach (DataRow row in deptDataSet.subjects.Rows)
            {
                listSubj.Add(row.ItemArray[1].ToString());

            }
            cbSubj.ItemsSource = listSubj;

            cbSubj.SelectedIndex = 0;

            cbSubj.Loaded += cbSubj_Selected;


            // Загрузить данные в таблицу specialities. Можно изменить этот код как требуется.
            deptDataSetspecialitiesTableAdapter.Fill(deptDataSet.specialities);
            specialitiesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("specialitiesViewSource")));
            specialitiesViewSource.View.MoveCurrentToFirst();

            deptDataSetsubject_specialitiesTableAdapter.Fill(deptDataSet.subjects_specialities);
            subjects_specialitiesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("subjects_specialitiesViewSource")));
            subjects_specialitiesViewSource.View.MoveCurrentToFirst();

            List<speciality> specialities = new List<speciality>();

            foreach (var s in deptDataSet.specialities)
            {
                List<string> grList = new List<string>();
                List<string> stList = new List<string>();
                List<string> subjList = new List<string>();
                List<string> teachList = new List<string>();

                foreach (var gr in deptDataSet.groups)
                {
                    if (gr.specialty_id == s.id)
                    {

                        grList.Add(gr.group_number);

                        foreach (var st in deptDataSet.students)
                        {
                            if (st.group_id == gr.id)
                            {
                                stList.Add(st.name);
                            }
                        }
                    }
                }

                List<int> subjIdList = new List<int>();

                foreach (var ss in deptDataSet.subjects_specialities)
                {

                    if (ss.specialities_id == s.id)
                    {
                        subjIdList.Add(ss.subjects_id);
                    }
                }


                foreach (var subj in deptDataSet.subjects)
                {

                    if (subjIdList.Exists(i => i == subj.id))

                        subjList.Add(subj.subject);
                }

                List<int> teachIdList = new List<int>();

                foreach (var t in deptDataSet.teachers_subjects)
                {
                    if (subjIdList.Exists(i => i == t.subject_id))
                        teachIdList.Add(t.teacher_id);
                }

                foreach (var t in deptDataSet.teachers)
                {
                    if (teachIdList.Exists(te => te == t.id))
                        teachList.Add(t.name);
                }

                specialities.Add(new speciality(s.speciality, s.id, grList.Count, stList, subjList, teachList));
            }


            specDg.ItemsSource = specialities;

            // Загрузить данные в таблицу staffSchedule. Можно изменить этот код как требуется.
            Dept.DeptDBDataSetTableAdapters.staffScheduleTableAdapter deptDataSetstaffScheduleTableAdapter = new Dept.DeptDBDataSetTableAdapters.staffScheduleTableAdapter();
            deptDataSetstaffScheduleTableAdapter.Fill(deptDataSet.staffSchedule);
            System.Windows.Data.CollectionViewSource staffScheduleViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("staffScheduleViewSource")));
            staffScheduleViewSource.View.MoveCurrentToFirst();
        }

        private void subjectsDataGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            

            //DataGrid dataGri = sender as DataGrid;

            //var dgr0 = dataGri.SelectedCells;

            //var txt1 = (dgr0[0].Column.GetCellContent(dgr0[0].Item) as TextBlock).Text;
            //var txt2 = (dgr0[1].Column.GetCellContent(dgr0[1].Item) as TextBlock).Text;
         }

        private void subjectsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            //    DataGrid dataGrid = sender as DataGrid;

            //   var dgs = dataGrid.SelectedCells;

            //foreach(DataRowView v in dataGrid.Items)
            //{
            //   deptDataSet.subjects.Rows.Add((DataRowView)v);

            //    //foreach (var u in v.Row.ItemArray)
            //    //    MessageBox.Show(u.ToString());
            //}
          //  DeptDataSet.subjectsDataTable subjectsRows = sender as DeptDataSet.subjectsDataTable;

            //try
            //{
           
            //   MessageBox.Show("in");

            //   }
            //catch(Exception exc)
            //{
            //    MessageBox.Show("fail");
            //    MessageBox.Show(exc.Message);
            //}

           
        }
       
        private void cbSubj_Selected(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            
            int i = comboBox.SelectedIndex + 1;
            string expression = "subjects_id = " + i.ToString();

            DataRow[] foundRows1 = deptDataSet.subjects_specialities.Select(expression);
            DataRow[] foundRows2 = new DataRow[] { };
            
         //   MessageBox.Show("deptDataSet.subjects_specialities - " + deptDataSet.subjects_specialities.Rows.Count.ToString());

            List<string> specNumList = new List<string>();


            foreach (var v in foundRows1)
            {
                
                string expression1 = "id = " + v.ItemArray[2];

                foundRows2 = deptDataSet.specialities.Select(expression1);
            }

            foreach (var v in foundRows2)
                specNumList.Add(v.ItemArray[1].ToString());

            cbSpec.ItemsSource = specNumList;
        }

        private void btnFindTeachers_Click(object sender, RoutedEventArgs e)
        {
            
           if(int.TryParse( loadLimitTb.Text, out int loadLvl))
                {
     
                    string expression = "academic_load < " + loadLvl;


                    DataRow[] foundRows = deptDataSet.teachers.Select(expression);

                    lowloadTeachersDg.ItemsSource = foundRows;
                }
            else
            {
                MessageBox.Show("Укажите целое число");
            }
        }

        private void saveSbjBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("изменённых строк в таблице дисциплин - " + deptDataSetsubjectsTableAdapter.Update(deptDataSet.subjects));
            }
            catch (Exception exp1)
            {
                MessageBox.Show(exp1.Message);
            }
            
        }

        private void saveTeachBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("изменённых строк в таблице учителей - " + deptDataSetteachersTableAdapter.Update(deptDataSet.teachers));
            }
            catch (Exception exp1)
            {
                MessageBox.Show(exp1.Message);
            }
        }

        private void saveStudBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("изменённых строк в таблице учеников - " + deptDataSetstudentsTableAdapter.Update(deptDataSet.students));
            }
            catch (Exception exp1)
            {
                MessageBox.Show(exp1.Message);
            }
        }
    }
}
