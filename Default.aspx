<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableSessionState="True" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bigfoot Revision E-2</title>
</head>
<body style="position: absolute">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="AJAXStatus" runat="server" />
           
                <strong><span style="margin-top: 0px; font-size: 10pt; padding-top: 0px"></span>
                </strong>
                <asp:UpdatePanel ID="pnlSubmit" runat="server">
                    <ContentTemplate>
                        <a href="http://www.atstools.info"><span style="font-size: 10pt"><strong>atstools.info home</strong></span></a><span style="font-size: 10pt"><strong> | </strong></span>
                        <a href="http://atstools.info/ninja"><span style="font-size: 10pt"><strong>ninja- ip unblocks</strong></span></a><span style="font-size: 10pt"><strong>
                    | biscuit- bounceback parser | </strong></span><a href="http://www.DNSstuff.com"><span style="font-size: 10pt">
                                    <strong>dnsstuff.com</strong></span></a><span style="font-size: 10pt"><strong> | </strong>
                                    </span><a href="http://www.Kloth.net"><span style="font-size: 10pt"><strong>kloth.net</strong></span></a><span
                                        style="font-size: 10pt"><strong>&nbsp;<br />
                                        </strong></span>
                    <strong>
                        <br />
                        Nameserver dig tool<br />
                    </strong><span style="color: dimgray">This tool will determine what nameserver a domain
                    should be using. Please contact Zachary Rice (</span><a href="mailto:zrice@secureserver.net"><span
                        style="color: blue">zrice@secureserver.net</span></a><span style="color: dimgray">)
                            with any questions.<br />
                        </span><strong>Enter Domain: </strong>
                <asp:TextBox ID="txtDomain" runat="server" ToolTip="Enter a domain name" Width="264px"></asp:TextBox><strong>
                    <asp:Button ID="cmd_FindNS" runat="server" OnClick="cmd_FindNS_Click" Text="Lookup"
                        ToolTip="Begin Nameserver Lookup" Width="80px" /></strong>&nbsp; <a href="http://bigfoot.atstools.info/faq.htm">
                            <strong>FAQ</strong></a>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <hr />
                <asp:UpdateProgress ID="UpdateNS" runat="server" AssociatedUpdatePanelID="pnlSubmit" DisplayAfter="0">
                    <ProgressTemplate>
                        <strong style="position: relative">
                            </strong>
                                    <img src="ajax-loader.gif" style="left: 0px; vertical-align: middle" />Running Query...
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <br />
                <div>
                <asp:UpdatePanel ID="ServerUpdate" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                            <asp:Panel ID="pnlLookup" runat="server" Height="50px" Width="100%" RenderMode="Block" Visible="False">
                                <strong>
                                Answer:<br />
                                </strong>
                <asp:Label ID="lblQuickAnswer" runat="server" Height="16px" Width="792px"></asp:Label><br />
                <br />
                <asp:Label ID="lblAuthNS1" runat="server" Font-Bold="True" ForeColor="Green" Width="248px"></asp:Label><br />
                <asp:Label ID="lblAuthNS2" runat="server" Font-Bold="True" ForeColor="Green" Width="248px"></asp:Label><br />
                <br />
                <asp:Label ID="lblAddInfo" runat="server" Font-Bold="True" Font-Size="Medium" Width="792px"></asp:Label><br />
                <br />
                <asp:Label ID="lblParkNote" runat="server" Font-Bold="True" ForeColor="#000000" Width="790px"></asp:Label><br />
                <br />
                        <span>&nbsp;</span><strong>Details:<br />
                </strong>
                <table border="1" style="font-weight: bold; width: 464px; font-family: 'Courier New';
                    height: 48px; color: #000000;">
                    <tr>
                        <td style="width: 245px; background-color: silver">
                            Hosting Servers</td>
                        <td style="width: 401px; background-color: silver">
                            Query Results</td>
                    </tr>
                    <tr>
                        <td style="width: 245px; background-color: white">
                            wsc1.jomax.net</td>
                        <td style="width: 401px; background-color: white">
                            <asp:Label ID="lblWSC1result" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 245px; background-color: white">
                            wsc2.jomax.net</td>
                        <td style="width: 401px; background-color: white">
                            <asp:Label ID="lblWSC2result" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 245px; background-color: gainsboro">
                            ns1.secureserver.net</td>
                        <td style="width: 401px; background-color: gainsboro">
                            <asp:Label ID="lblNS1result" runat="server" Width="200px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 245px; height: 22px; background-color: gainsboro">
                            <span>ns2.secureserver.net</span></td>
                        <td style="width: 401px; height: 22px; background-color: gainsboro">
                            <asp:Label ID="lblNS2result" runat="server" Width="200px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 245px">
                            ns3.secureserver.net</td>
                        <td style="width: 401px">
                            <asp:Label ID="lblNS3result" runat="server" Width="200px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 245px; height: 22px">
                            ns4.secureserver.net</td>
                        <td style="width: 401px; height: 22px">
                            <asp:Label ID="lblNS4result" runat="server" Width="200px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 245px; height: 22px; background-color: gainsboro">
                            ns5.secureserver.net</td>
                        <td style="width: 401px; height: 22px; background-color: gainsboro">
                            <asp:Label ID="lblNS5result" runat="server" Width="200px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 245px; height: 22px; background-color: gainsboro">
                            ns6.secureserver.net</td>
                        <td style="width: 401px; height: 22px; background-color: gainsboro">
                            <asp:Label ID="lblNS6result" runat="server" Width="200px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; width: 245px; font-family: 'Courier New'; height: 22px;
                            background-color: silver">
                            Off-Site DNS Servers</td>
                        <td style="font-weight: bold; width: 401px; font-family: 'Courier New'; height: 22px;
                            background-color: silver">
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; width: 245px; font-family: 'Courier New'; height: 22px">
                            mns1.secureserver.net</td>
                        <td style="width: 401px; font-family: 'Courier New'; height: 22px">
                            <asp:Label ID="lblOffSite1" runat="server" Width="200px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; width: 245px; font-family: 'Courier New'; height: 22px">
                            mns2.secureserver.net</td>
                        <td style="width: 401px; font-family: 'Courier New'; height: 22px">
                            <asp:Label ID="lblOffSite2" runat="server" Width="200px"></asp:Label></td>
                    </tr>
                </table>
                <asp:Label ID="lblQtime" runat="server" Width="448px"></asp:Label><br />
                <br />
                <asp:Label ID="lblRawNS" runat="server" Width="449px" UpdateMode="Conditional"></asp:Label><br />
                                <br />
                                
                                
                                <Center>
                                    &nbsp;
                                Powered By:<br />
                    <a href="faq.htm">
                        <img src="about.png" style="border-top-style: none; border-right-style: none; border-left-style: none;
                            border-bottom-style: none; background-color: transparent; text-align: center;" /></a><br />
                    Comments or suggestions? <a href="mailto:zrice@secureserver.net">zrice@secureserver.net</a>&nbsp; 
                            </asp:Panel>
                            <br />
                            <center><a href="mailto:zrice@secureserver.net"></a>&nbsp;</center></Center></div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmd_FindNS" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <center>
                                <a href="mailto:zrice@secureserver.net"></a>&nbsp;</center>
        </div>
    </form>
</body>
</html>
