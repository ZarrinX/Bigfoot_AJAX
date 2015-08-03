using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;
using dnslib;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using System.Net;


public partial class _Default : System.Web.UI.Page
{
    string HostName; //Classwide host name reference
    string sAuth1A;
    string sAuth2A;
    int counter = 3;

    protected void cmd_FindNS_Click(object sender, EventArgs e)
    {
        pnlLookup.Visible = true;

        //Remove text from nameserver fields and clears any temp files
        CleanUp();

        //Formats domain name
        txtDomain.Text = txtDomain.Text.Trim();
        HostName = txtDomain.Text;

        //Lets put some effort into making sure they actually put in a domain name
        if (HostName == "")
        {
            lblQuickAnswer.Text = "Please enter a domain.";
            lblQuickAnswer.ForeColor = Color.Red;
            lblQuickAnswer.Font.Size = 18;
            lblQuickAnswer.Font.Bold = true;
        }
        else if (HostName.Contains(".") == false)
        {
            lblQuickAnswer.Text = "Please enter a valid domain.";
            lblQuickAnswer.ForeColor = Color.Red;
            lblQuickAnswer.Font.Size = 18;
            lblQuickAnswer.Font.Bold = true;
        }
        else
        {

            //Tracks how long the query took
            DateTime start_time;
            DateTime stop_time;
            TimeSpan elapsed_time;

            //Start timer
            start_time = DateTime.Now;

            //Dim logfile As String
            System.IO.StreamWriter oWrite;

            //Diables the button so people dont get all click happy
            cmd_FindNS.Enabled = false;

                        //starts the lookup
            StartLookup(HostName);

            

            //Calculates query time
            stop_time = DateTime.Now;
            elapsed_time = stop_time.Subtract(start_time);
            lblQtime.Text = "Query run time: " + elapsed_time.TotalMilliseconds.ToString("0") + "ms";

            //Tracks domain Queries
            oWrite = File.AppendText(@"C:\Bigfoot\logfile.txt");
            oWrite.WriteLine(DateTime.Now.ToString() + " Domain: " + HostName + "  " + elapsed_time.TotalMilliseconds.ToString("0") + "ms"); //& " User: " & System.Security.Principal.WindowsIdentity.GetCurrent().Name & ", " & User.Identity.Name);
            oWrite.Close();

            //Lets let them use it again
            cmd_FindNS.Enabled = true;
        }
    }

    //Logic statements that try and find the correct nameserver
    void StartLookup(string HostName)
    {
        try
        {
            //Logs NS1/NS2 as authoritative nameservers when value =1;
            bool Parked = false;
            int ns1and2 = 0;
            int ns3and4 = 0;
            int ns5and6 = 0;
            int Offsite = 0;
            int Jomax = 0;
            int ns1JomaxAuth = 0;
            
            lblAuthNS1.Enabled = true;
            lblAuthNS2.Enabled = true;

            //Auth check for wsc1.jomax.net
            if (zDNS.SOAlookup(HostName, "WSC1.JOMAX.NET").Contains("WSC1.JOMAX.NET"))
            {
                lblWSC1result.Text = "AUTHORITATIVE";
                lblWSC1result.Font.Bold = true;
                lblWSC2result.Text = "AUTHORITATIVE";
                lblWSC2result.Font.Bold = true;
                Jomax = 1;
                ns1JomaxAuth = ns1JomaxAuth + 1;
            }
            else
            {
                lblWSC1result.Text = "NO ANSWER";
                lblWSC1result.Font.Bold = false;
                lblWSC2result.Text = "NO ANSWER";
                lblWSC2result.Font.Bold = false;
            }

            //Auth check for ns1.secureserver.net
            if (zDNS.SOAlookup(HostName, "NS1.SECURESERVER.NET").Contains("NS1.SECURESERVER.NET"))
            {
                lblNS1result.Text = "AUTHORITATIVE";
                lblNS1result.Font.Bold = true;
                lblNS2result.Text = "AUTHORITATIVE";
                lblNS2result.Font.Bold = true;
                ns1and2 = 1;
                ns1JomaxAuth = ns1JomaxAuth + 1;
                lblWSC1result.Text = "NON AUTHORITATIVE";
                lblWSC1result.Font.Bold = false;
                lblWSC2result.Text = "NON AUTHORITATIVE";
                lblWSC2result.Font.Bold = false;
            }
            else
            {
                lblNS1result.Text = "NO ANSWER";
                lblNS1result.Font.Bold = false;
                lblNS2result.Text = "NO ANSWER";
                lblNS2result.Font.Bold = false;
            }

            //Auth check for ns3.secureserver.net
            if (zDNS.SOAlookup(HostName, "NS3.SECURESERVER.NET").Contains("NS3.SECURESERVER.NET"))
            {
                lblNS3result.Text = "AUTHORITATIVE";
                lblNS3result.Font.Bold = true;
                lblNS4result.Text = "AUTHORITATIVE";
                lblNS4result.Font.Bold = true;
                ns3and4 = ns3and4 + 1;
            }
            else if ((ns1JomaxAuth == 1) & (lblNS3result.Text == "AUTHORITATIVE"))
            {
                lblNS3result.Text = "NON AUTHORITATIVE";
                lblNS3result.Font.Bold = false;
                lblNS4result.Text = "NON AUTHORITATIVE";
                lblNS4result.Font.Bold = false;
            }
            else
            {
                lblNS3result.Text = "NO ANSWER";
                lblNS3result.Font.Bold = false;
                lblNS4result.Text = "NO ANSWER";
                lblNS4result.Font.Bold = false;
            }

            //Auth check for ns5.secureserver.net
            if (zDNS.SOAlookup(HostName, "NS5.SECURESERVER.NET").Contains("NS5.SECURESERVER.NET"))
            {
                lblNS5result.Text = "AUTHORITATIVE";
                lblNS5result.Font.Bold = true;
                lblNS6result.Text = "AUTHORITATIVE";
                lblNS6result.Font.Bold = true;
                ns5and6 = 1;
            }
            else
            {
                lblNS5result.Text = "NO ANSWER";
                lblNS5result.Font.Bold = false;
                lblNS6result.Text = "NO ANSWER";
                lblNS6result.Font.Bold = false;
            }

            //Auth check for mns1.secureserver.net
            if (zDNS.IsOffsite(HostName, "MNS1.SECURESERVER.NET").Contains("MNS1.SECURESERVER.NET"))
            {
                lblOffSite1.Text = "AUTHORITATIVE";
                lblOffSite1.Font.Bold = true;
                lblOffSite2.Text = "AUTHORITATIVE";
                lblOffSite2.Font.Bold = true;
                Offsite = 1;
            }
            else
            {
                lblOffSite1.Text = "NO ANSWER";
                lblOffSite1.Font.Bold = false;
                lblOffSite2.Text = "NO ANSWER";
                lblOffSite2.Font.Bold = false;
            }

            //Displays authoritative nameservers. Value should always equal 1 or 0.

            if ((ns3and4 == 1) & (ns5and6 == 1))
            {
                lblNS3result.Text = "NON AUTHORITATIVE";
                lblNS3result.Font.Bold = false;
                lblNS4result.Text = "NON AUTHORITATIVE";
                lblNS4result.Font.Bold = false;
            }

            if ((Jomax == 1) & (ns3and4 == 1))
            {
                lblNS3result.Text = "NON AUTHORITATIVE";
                lblNS3result.Font.Bold = false;
                lblNS4result.Text = "NON AUTHORITATIVE";
                lblNS4result.Font.Bold = false;
            }

            if ((ns1and2 == 1) & (ns3and4 == 1))
            {
                lblNS3result.Text = "NON AUTHORITATIVE";
                lblNS3result.Font.Bold = false;
                lblNS4result.Text = "NON AUTHORITATIVE";
                lblNS4result.Font.Bold = false;
            }

            if ((ns1and2 + ns3and4 + Jomax >= 1) & (Offsite == 1))
            {
                lblOffSite1.Text = "NON AUTHORITATIVE";
                lblOffSite1.Font.Bold = false;
                lblOffSite2.Text = "NON AUTHORITATIVE";
                lblOffSite2.Font.Bold = false;
            }

            if (Jomax == 1)
            { //Authority check for wsc1/wsc2;
                lblQuickAnswer.Text = ("The authoritative nameservers for " + HostName + " are:");
                lblAuthNS1.Text = "WSC1.JOMAX.NET";
                lblAuthNS2.Text = "WSC2.JOMAX.NET";
                lblWSC1result.Text = "AUTHORITATIVE";
                lblWSC1result.Font.Bold = true;
                lblWSC2result.Text = "AUTHORITATIVE";
                lblWSC2result.Font.Bold = true;
                lblNS1result.Text = "NON AUTHORITATIVE";
                lblNS1result.Font.Bold = false;
                lblNS2result.Text = "NON AUTHORITATIVE";
                lblNS2result.Font.Bold = false;
                lblAddInfo.Text = "Records were also found on NS1/NS2.SECURESERVER.NET. However, this account was setup prior to  December 15 , 2005. This domain should use the JOMAX.NET servers.";
            }
            else if (ns1and2 == 1)
            {
                lblQuickAnswer.Text = ("The authoritative nameservers for " + HostName + " are:");
                lblAuthNS1.Text = "NS1.SECURESERVER.NET";
                lblAuthNS2.Text = "NS2.SECURESERVER.NET";
                lblAddInfo.Text = "Records were also found on WSC1/WSC2.JOMAX.NET. However, this account was setup after to  November 14, 2006. This domain should use the SECURESERVER.NET servers.";
            }
            else if ((ns5and6 == 1) & (ns3and4 == 1))
            { //Authority check for NS5/NS6
                lblQuickAnswer.Text = ("The authoritative nameservers for " + HostName + " are:");
                lblAuthNS1.Text = "NS5.SECURESERVER.NET";
                lblAuthNS2.Text = "NS6.SECURESERVER.NET";
                lblAddInfo.Text = "This account was setup after January 31, 2007. The customer should be using these nameservers.";
            }
            else if (ns3and4 == 1)
            { //Authority check for NS3/NS4;
                lblQuickAnswer.Text = ("The authoritative nameservers for " + HostName + " are:");
                lblAuthNS1.Text = "NS3.SECURESERVER.NET";
                lblAuthNS2.Text = "NS4.SECURESERVER.NET";
                lblAddInfo.Text = "This account was setup between December 15 , 2005 and November 14, 2006. This customer should be using these nameservers.";
            }
            else if (ns5and6 == 1)
            { //Authority check for NS5/NS6;
                lblQuickAnswer.Text = ("The authoritative nameservers for " + HostName + " are:");
                lblAuthNS1.Text = "NS5.SECURESERVER.NET";
                lblAuthNS2.Text = "NS6.SECURESERVER.NET";
                lblAddInfo.Text = "This account was setup after January 31, 2007. The customer should be using these nameservers.";
            }
            else if (Offsite == 1)
            { //Authority check for off-site DNS;
                lblQuickAnswer.Text = ("The authoritative nameservers for " + HostName + " are:");
                lblAuthNS1.Text = "MNS1.SECURESERVER.NET";
                lblAuthNS2.Text = "MNS2.SECURESERVER.NET";
                lblAddInfo.Text = "These are the nameservers used for Off-Site DNS.";
            }

            else if (zDNS.IsParked(HostName, "ns39.domaincontrol.com").Contains(HostName))
            {
                lblQuickAnswer.Text = ("The DNS zone file for " + HostName + " resides on these parked nameservers:");
                lblAuthNS1.Text = "NS39.DOMAINCONTROL.COM";
                lblAuthNS2.Text = "NS40.DOMAINCONTROL.COM";
                lblParkNote.Text = "NOTE: If the doman is hosted here it can continue to use the parked servers, but it should be moved to the default hosting nameservers if there is a problem. This action will create a new zone file on the hosting server";
                lblAddInfo.Text = "If the customer has hosting, this domain can be moved to the default hosting nameservers.";
                Parked = true;
            }
            else if (zDNS.IsParked(HostName, "ns41.domaincontrol.com").Contains(HostName))
            {
                lblQuickAnswer.Text = ("The DNS zone file for " + HostName + " resides on these parked nameservers:");
                lblAuthNS1.Text = "NS41.DOMAINCONTROL.COM";
                lblAuthNS2.Text = "NS42.DOMAINCONTROL.COM";
                lblParkNote.Text = "NOTE: If the doman is hosted here it can continue to use the parked servers, but it should be moved to the default hosting nameservers if there is a problem. This action will create a new zone file on the hosting server";
                lblAddInfo.Text = "If the customer has hosting, this domain can be moved to the default hosting nameservers.";
                Parked = true;
            }
            else if (zDNS.IsParked(HostName, "ns43.domaincontrol.com").Contains(HostName))
            {
                lblQuickAnswer.Text = ("The DNS zone file for " + HostName + " resides on these parked nameservers:");
                lblAuthNS1.Text = "NS43.DOMAINCONTROL.COM";
                lblAuthNS2.Text = "NS44.DOMAINCONTROL.COM";
                lblParkNote.Text = "NOTE: If the doman is hosted here it can continue to use the parked servers, but it should be moved to the default hosting nameservers if there is a problem. This action will create a new zone file on the hosting server";
                lblAddInfo.Text = "If the customer has hosting, this domain can be moved to the default hosting nameservers.";
                Parked = true;
            }

            else if (ns1and2 + ns5and6 + ns3and4 + Offsite + Jomax >= 0)
            {
                                
                while (counter < 40)
                {
                    
                    //lblStatus.Text = ("Checking park" + counter + ".secureserver.net");
                    if (zDNS.IsParked(HostName, "park" + counter + ".secureserver.net").Contains(HostName))
                    {
                        lblQuickAnswer.Text = ("The DNS zone file for " + HostName + " resides on these parked nameservers:");
                        lblAuthNS1.Text = "PARK" + counter + ".SECURESERVER.NET";
                        lblAuthNS2.Text = "PARK" + (counter + 1) + ".SECURESERVER.NET";
                        lblParkNote.Text = "NOTE: If the doman is hosted here it can continue to use the parked servers, but it should be moved to the default hosting nameservers if there is a problem. This action will create a new zone file on the hosting server";
                        lblAddInfo.Text = "If the customer has hosting, this domain can be moved to the default hosting nameservers.";
                        counter = 40;
                        Parked = true;
                    }
                    else
                        counter = counter + 2;
                }
            }

            else //if no servers respond, say this
            {
            //Moved to line 344
            }

            if (Parked == false & (ns1and2 + ns5and6 + ns3and4 + Offsite + Jomax == 0))
            {
                lblQuickAnswer.Text = (HostName + " is not hosted here.");
                lblQuickAnswer.ForeColor = Color.Red;
                lblQuickAnswer.Font.Size = 18;
                lblQuickAnswer.Font.Bold = true;
                lblAuthNS1.Enabled = false;
                lblAuthNS2.Enabled = false;
            }

            else //if (lblAuthNS1.Text != "")
            {
                try
                    {
                //finds the IP address of the hostname (A record validation)
                foreach (IPAddress ipNS1 in Dns.GetHostEntry(lblAuthNS1.Text).AddressList)
                {
                    sAuth1A = ipNS1.ToString();
                }
                foreach (IPAddress ipNS2 in Dns.GetHostEntry(lblAuthNS2.Text).AddressList)
                {
                    sAuth2A = ipNS2.ToString();
                }

                lblAuthNS1.Text = lblAuthNS1.Text + "  (" + sAuth1A + ")";
                lblAuthNS2.Text = lblAuthNS2.Text + "  (" + sAuth2A + ")";
                    }
                catch (System.Net.Sockets.SocketException)
                {
                    lblAuthNS1.Text = lblAuthNS1.Text;
                    lblAuthNS2.Text = lblAuthNS2.Text;
                }

            }
            

        }
        catch (Exception ex)
        {
            System.IO.StreamWriter oWrite;
            oWrite = File.AppendText(@"C:\Bigfoot\errorlog.txt");
            oWrite.WriteLine(ex);
            oWrite.Close();
            lblQuickAnswer.Text = ("An error has been detected. The details of this error have been recorded.");
            lblQuickAnswer.ForeColor = Color.Red;
            lblQuickAnswer.Font.Size = 18;
            lblQuickAnswer.Font.Bold = true;
            lblAuthNS1.Enabled = false;
            lblAuthNS2.Enabled = false;
        }

    }

    //resets all the lables
    protected void CleanUp()
    {
        lblQuickAnswer.ForeColor = Color.Black;
        lblQuickAnswer.Font.Size = 12;
        lblQuickAnswer.Font.Bold = false;
        lblAddInfo.Text = "";
        lblWSC1result.Text = "";
        lblWSC1result.Font.Bold = false;
        lblWSC2result.Text = "";
        lblWSC2result.Font.Bold = false;
        lblNS1result.Text = "";
        lblNS1result.Font.Bold = false;
        lblNS2result.Text = "";
        lblNS2result.Font.Bold = false;
        lblNS3result.Text = "";
        lblNS3result.Font.Bold = false;
        lblNS4result.Text = "";
        lblNS4result.Font.Bold = false;
        lblNS5result.Text = "";
        lblNS5result.Font.Bold = false;
        lblNS6result.Text = "";
        lblNS6result.Font.Bold = false;
        lblOffSite1.Text = "";
        lblOffSite1.Font.Bold = false;
        lblOffSite2.Text = "";
        lblOffSite2.Font.Bold = false;
        lblAuthNS1.Text = "";
        lblAuthNS2.Text = "";
        lblParkNote.Text = "";
    }
    //this is the shape of things to come
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}