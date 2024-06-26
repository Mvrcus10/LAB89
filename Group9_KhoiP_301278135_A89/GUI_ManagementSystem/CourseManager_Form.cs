﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManagementSystem;

namespace GUI_ManagementSystem
{
    public partial class CourseManager_Form : Form
    {
        CourseManager courseManager = new CourseManager();
        List<Course> courses=CourseManager.Courses;
        List<TaskManagementSystem.Task> tasks=new List<TaskManagementSystem.Task>();

        public CourseManager_Form()
        {
            InitializeComponent();
            btn_GoMain.Click += Btn_GoMain_Click;
            btn_AddCourse.Click += Btn_AddCourse_Click;
        }

        private void Btn_GoMain_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_AddCourse_Click(object sender, EventArgs e)
        {
            AddEditCourse_Form addEditCourse = new AddEditCourse_Form(default);
            this.Hide();            
            addEditCourse.FormClosed += ShowThis;
            if (addEditCourse.ShowDialog() != DialogResult.OK) return;
            updateCourseListBox();
        }

        private void updateCourseListBox()
        {
            lsbx_Courses.Items.Clear();
            foreach (Course course in this.courses)
            {
                lsbx_Courses.Items.Add(course);
            }
            btn_EditCourse.Visible = lsbx_Courses.SelectedIndex != -1;
            btn_DeleteCourse.Visible = lsbx_Courses.SelectedIndex != -1;
        }

        private void Btn_EditCourse_Click(object sender, EventArgs e)
        {
            Course editedCourse = lsbx_Courses.SelectedItem as Course;
            AddEditCourse_Form addEditCourse = new AddEditCourse_Form(editedCourse);
            this.Hide();
            addEditCourse.FormClosed += ShowThis;
            if (addEditCourse.ShowDialog() != DialogResult.OK) return;            
            updateCourseListBox();
        }
        private void Btn_DeleteCourse_Click(object sender, EventArgs e)
        {
            DeleteConfirmation_Form deleteConfirmation = new DeleteConfirmation_Form();
            this.Hide();
            deleteConfirmation.FormClosed += ShowThis;
            if (deleteConfirmation.ShowDialog() == DialogResult.OK)
            {
                courses.RemoveAt(lsbx_Courses.SelectedIndex);
                lsbx_Courses.Items.Remove(lsbx_Courses.SelectedItem);
                updateCourseListBox();
            }
            else return;
        }
        private void ShowThis(object sender, FormClosedEventArgs e)
        {
            this.Show();
            lbl_CourseDetails.Text = string.Empty;
        }

        private void CourseManager_Form_Load(object sender, EventArgs e)
        {
            updateCourseListBox();
        }

        private void lsbx_Courses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsbx_Courses.SelectedIndex == -1) return;
            Course course = (Course)lsbx_Courses.SelectedItem;
            lbl_CourseDetails.Text = course.Formatted_String();
            btn_EditCourse.Visible = lsbx_Courses.SelectedIndex != -1;
            btn_DeleteCourse.Visible = lsbx_Courses.SelectedIndex != -1;
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            try
            {
                string path = "CourseManager.json";
                courseManager.Save(path);
                MessageBox.Show("Successfully saved", "All courses", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch { throw; }
        }

        private void btn_Import_Click(object sender, EventArgs e)
        {
            try
            {
                string path = "CourseManager.json";
                courseManager.Load(path);
                MessageBox.Show("Successfully loaded", "All courses", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch { throw; }
            finally
            {
                //updateCourseListBox();
                //Refresh();
                CourseManager_Form courseManager_Form = new CourseManager_Form();
                courseManager_Form.ShowDialog();
                Close();
            }
        }
    }
}
