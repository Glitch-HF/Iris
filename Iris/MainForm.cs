using System;
using System.Linq;
using System.Windows.Forms;

using Iris.Requests;

namespace Iris
{
    public partial class MainForm : Form
    {

        #region Fields

        private Random rand = new Random();

        #endregion

        #region Initialization

        /// <summary>
        /// The main initialization function, called when the application is first executed.
        /// Form building events happen here.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the MainForm loads, called before the form is visible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // display the latest version #
            string fullVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                major = fullVersion.Split('.')[0],
                minor = fullVersion.Split('.')[1],
                build = fullVersion.Split('.')[2],
                revision = fullVersion.Split('.')[3];

            Text = string.Format("{0} - Version {1}.{2}.{3} (revision {4})", this.Text, major, minor, build, revision);
        }

        #endregion

        #region MenuStrip

        #region File

        private void reportAProblemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Edit

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Tools

        #endregion

        #region Help

        #endregion

        #endregion

        private void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLog("Begin creating account...");

                string firstName = txtFirstName.Text, username = txtUsername.Text, password = txtPassword.Text, email = txtEmail.Text;

                // Get First Name
                if (string.IsNullOrWhiteSpace(firstName))
                {
                    firstName = RandomString(1, false, true, false) + RandomString(rand.Next(3, 8), false, false, false);
                    WriteLog("Auto-gen FirstName: {0}", firstName);
                }
                else WriteLog("FirstName: {0}", firstName);

                // Get Username
                if (string.IsNullOrWhiteSpace(username))
                {
                    username = RandomString(rand.Next(6, 28), false, false);
                    WriteLog("Auto-gen Username: {0}", username);
                }
                else WriteLog("Username: {0}", username);

                // Get Password
                if (string.IsNullOrWhiteSpace(password))
                {
                    password = RandomString(rand.Next(10, 32), true);
                    WriteLog("Auto-gen Password: {0}", password);
                }
                else WriteLog("Password: {0}", password);

                // Get Email
                if (string.IsNullOrWhiteSpace(email))
                {
                    email = string.Format("{0}@{1}.{2}", RandomString(rand.Next(6, 16)), RandomString(rand.Next(6, 16), false, false, false), RandomString(rand.Next(3, 4), false, false, false));
                    WriteLog("Auto-gen Email: {0}", email);
                }
                else WriteLog("Email: {0}", email);

                RequestCreateAccount reqCreateAccount = new RequestCreateAccount(firstName, username, password, email);
                if(!string.IsNullOrWhiteSpace(txtProxyIP.Text))
                {
                    int proxyPort = 80;
                    string proxyIP = txtProxyIP.Text.Trim(), proxyUsername = null, proxyPassword = null, proxyDomain = null;
                    if(proxyIP.Contains(':'))
                    {
                        if(int.TryParse(proxyIP.Split(':')[1], out proxyPort))
                            proxyIP = proxyIP.Split(':')[0];

                        WriteLog("Proxy IP: {0}", proxyIP);
                        WriteLog("Proxy Port: {0}", proxyPort);
                    }
                    if(!string.IsNullOrWhiteSpace(txtProxyUsername.Text.Trim()))
                    {
                        proxyUsername = txtProxyUsername.Text.Trim();
                        proxyPassword = txtProxyPassword.Text.Trim();
                        if (string.IsNullOrWhiteSpace(proxyPassword))
                        {
                            WriteLog("Password can not be empty when specifying a user.");
                            WriteLog("Process complete.");
                            return;
                        }

                        WriteLog("Proxy Username: {0}", proxyUsername);
                        WriteLog("Proxy Password: {0}", proxyPassword);
                    }
                    if (!string.IsNullOrWhiteSpace(txtProxyDomain.Text.Trim()))
                        proxyDomain = txtProxyDomain.Text.Trim();

                    reqCreateAccount.Proxy(proxyIP, proxyUsername, proxyPassword, proxyDomain, proxyPort);
                }
                
#if DEBUG
                Console.WriteLine("{0}", reqCreateAccount.Response);
                #endif

                if (reqCreateAccount.Success)
                {
                    WriteLog("Account creation succes!");
                }
                else
                {
                    foreach(var error in reqCreateAccount.Errors)
                    {
                        WriteLog("Error: {0}", error);
                    }
                }

                WriteLog("Process complete.");
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        public void WriteLog(string message)
        {
            WriteLog("{0}", message);
        }

        public void WriteLog(string format, params object[] args)
        {
            var message = string.Format(format, args);
            txtConsole.AppendText(message + Environment.NewLine);
        }

        const string alphaChars = "abcdefghijklmnopqrstuvwxyz";
        const string numericChars = "0123456789";
        const string complexChars = "!@#$%^&*()-_=+,<.>/?\\|'\";:";
        private string RandomString(int len, bool complex = false, bool upperCase = true, bool numeric = true)
        {
            string charList = alphaChars;
            if (upperCase) charList = string.Concat(charList, charList.ToUpper());
            if (numeric) charList = string.Concat(charList, numericChars);
            if (complex) charList = string.Concat(charList, complexChars);
            return new string(Enumerable.Repeat(charList, len).Select(s => s[rand.Next(s.Length)]).ToArray());
        }
    }
}
