using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace AssetStudioGUI
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            var arch = Environment.Is64BitProcess ? "x64" : "x32";
            var appAssembly = typeof(Program).Assembly.GetName();
            var productName = appAssembly.Name;
            var productVer = appAssembly.Version.ToString();
            Text += " " + productName;
            productTitleLabel.Text = productName;
            productVersionLabel.Text = $"v{productVer} [{arch}]";
            productNamelabel.Text = productName;
            modVersionLabel.Text = productVer;

            licenseRichTextBox.Text = GetLicenseText();
        }

        private string GetLicenseText()
        {
            string license = "MIT License";

            if (File.Exists("LICENSE"))
            {
                string text = File.ReadAllText("LICENSE");
                license = text.Replace("\r", "")
                    .Replace("\n\n", "\r")
                    .Replace("\nCopyright", "\tCopyright")
                    .Replace("\n", " ")
                    .Replace("\r", "\n\n")
                    .Replace("\tCopyright", "\nCopyright");
            }

            return license;
        }

        private void checkUpdatesLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var ps = new ProcessStartInfo("https://github.com/aelurum/AssetStudio/releases")
            {
                UseShellExecute = true
            };
            Process.Start(ps);
        }

        private void gitPerfareLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var ps = new ProcessStartInfo("https://github.com/Perfare")
            {
                UseShellExecute = true
            };
            Process.Start(ps);
        }

        private void gitAelurumLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var ps = new ProcessStartInfo("https://github.com/aelurum")
            {
                UseShellExecute = true
            };
            Process.Start(ps);
        }
    }
}
