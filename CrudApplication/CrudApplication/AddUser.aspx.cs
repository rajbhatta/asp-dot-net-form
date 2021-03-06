﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrudApplication.Modals;
using CrudApplication.Services.DatabaseHandler;
using CrudApplication.Services.DatabaseUtil;
using CrudApplication.Services.DbEnvironment;

namespace CrudApplication
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            User user = PopulateUserFromForm();
            UserDataMySqlTransferService userDataMySqlTransferService = instantiateUserDataMySqlTransferService();
            string insertUserSqlQuery = userDataMySqlTransferService.convertSQLQueryForSave(user);
          
            MySqlDatabaseHanlder mysqlDatabaseHanlder = instantiateMySqlDatabaseHanlder();
            int userId=mysqlDatabaseHanlder.SaveUserToDatabase(insertUserSqlQuery);

            string insertUserSkillsQuery=userDataMySqlTransferService.sqlQueryforSaveUserProgrammingSkill(user, userId);
            mysqlDatabaseHanlder.RunQueryToDatabase(insertUserSkillsQuery);

            Response.Redirect("~/ManageUser.aspx");
        }

        private MySqlDatabaseHanlder instantiateMySqlDatabaseHanlder()
        {
            return new MySqlDatabaseHanlder(new MySqlDbPropertyService());
        }

        private UserDataMySqlTransferService instantiateUserDataMySqlTransferService()
        {
            return new UserDataMySqlTransferService();
        }

        private User PopulateUserFromForm()
        {
            User user = new User();
            user.name = txtBoxName.Text;
            user.email = txtBoxEmail.Text;
            user.address = txtBoxAddress.Text;
            user.address2 = txtBoxAddress2.Text;
            user.city = txtBoxCity.Text;
            user.province = ddlProvince.SelectedValue;
            user.postalCode = txtBoxPostalCode.Text;
            user.gender = GetGenderValue();
            user.programmingSkills=GetProgrammingSkills();
            user.availability = calAvailableDate.SelectedDate.ToShortDateString();
            user.username = txtBoxUsername.Text;
            user.password = HashPassword(txtBoxPassword.Text, new MD5CryptoServiceProvider());
            user.comment = txtBoxComment.Text;
            return user;
        }

        private string GetGenderValue()
        {
            string value = "";
            bool isChecked = rdBtnGenderMale.Checked;
            if (isChecked)
                value = rdBtnGenderMale.Text;
            else
                value = rdBtnGenderFemale.Text;
            return value;
        }

        private string GetProgrammingSkills()
        {
            string message = "";
            if (chkBoxMachineLearning.Checked)
            {
                message = chkBoxMachineLearning.Text + " ";
            }
            if (chkBoxPython.Checked)
            {
                message += chkBoxPython.Text + " ";
            }
            if (chkBoxJava.Checked)
            {
                message += chkBoxJava.Text;
            }
            if (chkBoxDatabase.Checked)
            {
                message = chkBoxDatabase.Text + " ";
            }
            if (chkBoxFrontEnd.Checked)
            {
                message += chkBoxFrontEnd.Text + " ";
            }
            if (chkBoxDotNet.Checked)
            {
                message += chkBoxDotNet.Text;
            }
            if (chkBoxBasicComputer.Checked)
            {
                message += chkBoxBasicComputer.Text;
            }
            return message;
        }
    

        private string HashPassword(string input, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }
    }
}