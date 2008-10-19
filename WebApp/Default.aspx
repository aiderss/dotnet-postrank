<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Default.aspx.cs" Inherits="WebApp._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AideRSS .NET Widget</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <p>
                RSS Feed URL:
                <asp:TextBox ID="urlTextBox" runat="server" OnTextChanged="urlTextBox_TextChanged"
                    Width="458px">http://igvita.com/</asp:TextBox></p>
            <hr />
        </div>
        <div>
            <p>
                <asp:Label ID="titleLabel" runat="server" Font-Italic="False" Font-Size="X-Large"
                    Width="595px" BackColor="#E0E0E0" Font-Bold="True" Font-Names="Calibri"></asp:Label>&nbsp;</p>
            <p>
                <asp:Label ID="descriptionLabel" runat="server" Font-Size="Large" Width="595px" BackColor="#E0E0E0"
                    Font-Italic="True" Font-Names="Calibri"></asp:Label>&nbsp;</p>
            <table>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="levelRadioButtonList" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="levelRadioButtonList_SelectedIndexChanged" Font-Names="Calibri">
                            <asp:ListItem Selected="True">Top</asp:ListItem>
                            <asp:ListItem>Best</asp:ListItem>
                            <asp:ListItem>Great</asp:ListItem>
                            <asp:ListItem>Good</asp:ListItem>
                            <asp:ListItem>All</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="periodRadioButtonList" runat="server" AutoPostBack="True" 
                            Font-Names="Calibri" OnSelectedIndexChanged="periodRadioButtonList_SelectedIndexChanged">
                            <asp:ListItem Selected="True">Year</asp:ListItem>
                            <asp:ListItem>Month</asp:ListItem>
                            <asp:ListItem>Week</asp:ListItem>
                            <asp:ListItem>Day</asp:ListItem>
                            <asp:ListItem>Auto</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:Image ID="sparkline" runat="server" BackColor="#E0E0E0" />
                    </td>
                </tr>
            </table>
            <br />
        </div>
        <asp:Table ID="postTable" runat="server" Height="65px" Width="595px" Font-Names="Calibri">
            <asp:TableRow runat="server">
                <asp:TableCell runat="server"></asp:TableCell>
                <asp:TableCell runat="server">text</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
            </asp:TableRow>
        </asp:Table>
        <asp:Button ID="backButton" runat="server" Enabled="False" OnClick="backButton_Click"
            Text="<" />
        <asp:Button ID="forwardButton" runat="server" Enabled="False" OnClick="forwardButton_Click"
            Text=">" />
        <asp:TextBox ID="startTextBox" runat="server" Visible="False">0</asp:TextBox>
    </form>
</body>
</html>
