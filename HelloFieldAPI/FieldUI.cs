#region Copyright
//
// Copyright (C) 2015-2017 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
// Written by M.Harada
// 
#endregion // Copyright

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Added for RestSharp. 
using RestSharp;
using RestSharp.Deserializers;

namespace HelloFieldAPI
{
    public partial class FieldUI : Form
    {
        // Save ticket. 
        private static string m_ticket = "";

        public FieldUI()
        {
            InitializeComponent();
        }

        private void FieldUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_ticket != "")
            {
                bool result = Field.Logout(m_ticket);
            }
        }

        //=========================================================
        //  Login/Logout 
        //=========================================================
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            // Get the user name and password from the user. 
            string userName = textBoxUserName.Text;
            string password = textBoxPassword.Text;

            InitRequestResponse(); 
            this.Update();

            // Here is the main part that we call Field API to login 
            m_ticket = Field.Login(userName, password);

            // If success, change the button to logout.
            if (m_ticket != null && m_ticket.Length > 0)
            {
                buttonLogin.Enabled = false;
                buttonLogout.Enabled = true; 
            }

            // For our learning, 
            // show the request and response in the form. 
            ShowRequestResponse(); 
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            InitRequestResponse();
            this.Update();

            // Here is the main call to Field API. 
            bool result = Field.Logout(m_ticket);

            m_ticket = ""; 
            buttonLogin.Enabled = true;
            buttonLogout.Enabled = false;

            // For our learning, 
            // show the request and response in the form. 
            ShowRequestResponse();
        }

        //==========================================================
        // Helper functions 
        //==========================================================
        private void InitRequestResponse()
        {
            // initialize the request and response text in the form.
            textBoxRequest.Text = "Request comes here";
            labelStatus.Text = ""; 
            textBoxResponse.Text = "Response comes here. This may take seconds. Please wait...";
        }
        private void ShowRequestResponse()
        {
            // show the request and response in the form. 
            IRestResponse response = Field.m_lastResponse;
            textBoxRequest.Text = response.ResponseUri.AbsoluteUri;
            labelStatus.Text = "Status: " + response.StatusCode.ToString(); 
            textBoxResponse.Text = response.Content;
        }

    }
}
