﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using FreeNet;
using System.IO;
namespace CodeFight
{
    public partial class Form2 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        String userName = "Default";
        String enemyName = "Default";
        String moon = "Default";
        String qNum = "0";
        int closed = 0;
        public Form2(String userName)
        {
            this.userName = userName;
            InitializeComponent();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (winImage.Visible != true && loseImage.Visible != true)
            {
                closed = 1;
                giveup.PerformClick();
            }
            Application.Exit();
        }
        public void getJson()
        {
            while (get.a.Length == 0)
            {
                if (closed == 1)
                {
                    return;
                }
                Delay(1);
            }
            string[] tmp = get.a.Split(' ');
            get.a = string.Empty;
            this.enemyName = tmp[0];
            this.moon = File.ReadAllText("./resource/" + tmp[1] + ".txt", Encoding.Default);
            this.qNum = tmp[1];
            timer1.Stop();
            label2.Hide();
            textBox1.AcceptsTab = true;
            languege.SelectedItem = 1;
            setting();
        }
        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }
        public void setting()
        {
            user2.Text = userName;
            user1.Text = enemyName;
            questionBox.Text = moon;
            panel1.Visible = true;
            while (get.a.Length == 0)
            {
                Delay(1);
            }
            if (get.a == "win")
            {
                winImage.Visible = true;
            }
            else
            {
                loseImage.Visible = true;
            }
        }

        private async void compileButton_ClickAsync(object sender, EventArgs e)
        {
            var nowLang = Convert.ToString(languege.SelectedIndex + 1);
            var source = textBox1.Text;
            System.Windows.Forms.Application.DoEvents();
            var values = new Dictionary<string, string> { { "source", source }, { "qnum", qNum }, { "languege", nowLang } };
            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://114.202.56.161:5000", content);

            var responseString = await response.Content.ReadAsStringAsync();

            if (responseString.IndexOf("sucksex!!") >= 0)
            {
                compilerResultBox.Text = "컴파일 성공!";
                Socket_.post("win "+userName);
            }
            else if(compilerResultBox.Text.IndexOf("Fail") >= 0)
            {
                compilerResultBox.Text = "컴파일 실패";
            }
            else
            {
                compilerResultBox.Text = responseString;
            }
        }

        private void giveup_Click(object sender, EventArgs e)
        {
            Socket_.post("lose " + userName);
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //getJson();
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            getJson();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(label2.Text == "...")
            {
                label2.Text = ".";
            }
            else
            {
                label2.Text += ".";
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F5)
            {
                compileButton.PerformClick();
            }
        }

        private void loseImage_Click(object sender, EventArgs e)
        {

        }
    }
    static public class get
    {
        public static string a = string.Empty;

    }
}