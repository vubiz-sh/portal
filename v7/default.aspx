﻿<%@ Page
  Language="C#"
  MasterPageFile="~/v7/site.master"
  AutoEventWireup="true"
  ValidateRequest="false"
  CodeBehind="default.aspx.cs"
  Inherits="portal.Default"
  UICulture="auto"
  Culture="auto" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
  <link href="/portal/styles/css/tiles.min.css" rel="stylesheet" />
  <link href="/portal/styles/css/default.min.css" rel="stylesheet" />
  <link href="/portal/styles/css/notice.css" rel="stylesheet" />

  <script>

    function __doPostBack(tileTargetType, tileTarget, tileName) {
      // this captures the tile click 
      // it stores the tileTargetType, tileTarget and tile Name into the form's hidden fields
      // we sumbit this page/form then act accordingly using Page_Load
      // note frmMaster is defined in site.master
      $("#tileTargetType").val(tileTargetType);
      $("#tileTarget").val(tileTarget);
      $("#tileName").val(tileName);
      $("#frmMaster").submit();
    }

    $(function () {
      // set fields to password
      $(".txtMembId, .txtMembPwd").attr("type", "password");
      // essentially toggle by mouseUp and mouseDown
      $(".eyeMembId, .eyeMembPwd")
        .mouseup(function () {
          $(this).prev().attr("type", "password");
        }).mousedown(function () {
          $(this).prev().attr("type", "text");
        });
    });

  </script>

</asp:Content>

<asp:Content ID="defaultContent" ContentPlaceHolderID="MainContent" runat="server">

  <input type="hidden" id="tileTargetType" name="tileTargetType" />
  <input type="hidden" id="tileTarget" name="tileTarget" />
  <input type="hidden" id="tileName" name="tileName" />

  <asp:Table ID="tabSignIn" CssClass="tabSignIn" runat="server" Visible="false">

    <asp:TableRow>
      <asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
        <h1 style="font-size: 1.2em; color: white;">
          <asp:Literal ID="Literal1" runat="server" Text="<%$  Resources:portal, credentials%>" />
        </h1>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableHeaderCell ColumnSpan="3" HorizontalAlign="Right">
        <asp:RadioButtonList CssClass="tabSignInLang" AutoPostBack="true" ID="radLang" runat="server" RepeatDirection="Horizontal" TextAlign="Right" BorderStyle="None">
          <asp:ListItem Value="en-US" Text="English" Selected="True">English</asp:ListItem>
          <asp:ListItem Value="fr-CA" Text="Français">Français</asp:ListItem>
        </asp:RadioButtonList>
      </asp:TableHeaderCell>
    </asp:TableRow>
    <asp:TableRow ID="rowMembId" CssClass="formRow">
      <asp:TableHeaderCell CssClass="tabSignInLabel" Text="<%$  Resources:portal, membId%>">Username :</asp:TableHeaderCell><asp:TableCell HorizontalAlign="Left">
        <asp:TextBox ID="txtMembId" CssClass="txtMembId" Width="250px" TextMode="SingleLine" Text="" runat="server"></asp:TextBox>
        <asp:Image ID="eyeMembId" CssClass="eyeMembId" ImageUrl="~/styles/icons/eye.png" runat="server" ToolTip="Hide/Show UserName" />
      </asp:TableCell><asp:TableCell>
        <asp:LinkButton CssClass="newButton" OnClick="btnMembId_Click" ID="btnMembId" runat="server" Text="<%$  Resources:portal, next%>" />
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="rowMembPwd" Visible="false">

      <asp:TableHeaderCell CssClass="tabSignInLabel" Text="<%$  Resources:portal, membPwd%>">Password :</asp:TableHeaderCell><asp:TableCell HorizontalAlign="Left">
        <asp:TextBox ID="txtMembPwd" CssClass="txtMembPwd" Width="250px" TextMode="SingleLine" Text="" runat="server"></asp:TextBox>
        <asp:Image ID="eyeMembPwd" CssClass="eyeMembPwd" ImageUrl="~/styles/icons/eye.png" runat="server" ToolTip="Hide/Show Password" />
      </asp:TableCell><asp:TableCell>
        <asp:LinkButton CssClass="newButton" OnClick="btnMembPwd_Click" ID="btnMembPwd" runat="server" Text="<%$  Resources:portal, next%>" />
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="rowCustId" Visible="false">
      <asp:TableHeaderCell CssClass="tabSignInLabel" Text="<%$  Resources:portal, custId%>">Customer Id :</asp:TableHeaderCell><asp:TableCell HorizontalAlign="Left">
        <asp:TextBox ID="txtCustId" MaxLength="8" Width="250px" CssClass="input upper" Text="VUBZ5678" runat="server"></asp:TextBox>
      </asp:TableCell><asp:TableCell>
        <asp:LinkButton CssClass="newButton" OnClick="btnCustId_Click" ID="btnCustId" runat="server" Text="<%$  Resources:portal, next%>" />
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell ColumnSpan="3" Style="padding: 20px;"><hr /></asp:TableCell>
    </asp:TableRow>


    <asp:TableRow>
      <asp:TableCell ColumnSpan="3">
        <asp:LinkButton CssClass="newButton" ID="butReturn" OnClick="butReturn_Click" runat="server" Text="<%$  Resources:portal, return%>" Visible="false" />
        <asp:LinkButton CssClass="newButton" ID="butRestart" OnClick="butRestart_Click" runat="server" Text="<%$  Resources:portal, restart%>" />
        <asp:LinkButton CssClass="newButton" ID="butBrowser" OnClick="butBrowser_Click" xOnClientClick="browserCheckerUrl()" runat="server" Text="<%$  Resources:portal, browserTest%>" />
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell ColumnSpan="3">
        <br />
        <br />
        <asp:LinkButton CssClass="newButton" ID="btnRegister" OnClick="btnRegister_Click" runat="server" Text="<%$  Resources:portal, register%>" Visible="false" />
        <asp:LinkButton CssClass="newButton" ID="btnForgot" OnClick="btnForgot_Click" runat="server" Text="<%$  Resources:portal, forgotCredentials%>" />
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="forgotEmail" Visible="false">
      <asp:TableCell ColumnSpan="3">
        <h2>
          <asp:Literal ID="Literal3" runat="server" Text="<%$  Resources:portal, forgot_1%>" />
        </h2>
        <div style="margin: 30px 0 20px;">
          <asp:TextBox runat="server"
            ID="txtEmail"
            TextMode="Email"
            Height="28px"
            Width="250px">
          </asp:TextBox>&nbsp;
          <asp:LinkButton runat="server"
            CssClass="newButton"
            ID="btnEmail"
            OnClick="btnEmail_Click"
            Text="<%$  Resources:portal, retrieve%>" />
        </div>
        <asp:Label ID="labEmail" CssClass="labEmail" runat="server"></asp:Label>
      </asp:TableCell>
    </asp:TableRow>
  </asp:Table>
  <div>
    <asp:Label ID="labWelcome" CssClass="labWelcome" runat="server" Text=""></asp:Label><br />
    <asp:Label ID="labManage" CssClass="labManage" runat="server" Text=""></asp:Label><br />
    <asp:Label ID="labContent" CssClass="labContent" runat="server" Text=""></asp:Label>
  </div>
  <div class="divPage" style="margin-top: 00px; background-color: inherit;">

    <asp:ListView runat="server"
      ID="lvTiles"
      Visible="false"
      DataKeyNames="tileNo"
      OnItemDataBound="lvTiles_ItemDataBound"
      DataSourceID="SqlDataSource1">
      <ItemTemplate>
        <li class="tile"
          onclick="__doPostBack('<%#Eval("tileTargetType")%>', '<%#Eval("tileTarget")%>', '<%#Eval("tileName")%>')"
          style="background-color: <%#Eval("tileColor")%>">
          <div class="tileIcon">
            <img src="../styles/tiles/<%#Eval("tileIcon")%>" />
          </div>
          <asp:Label runat="server" CssClass="tileTitle" ID="tileLabel" Text='<%# Eval("tileName") %>' />
        </li>
      </ItemTemplate>
      <LayoutTemplate>
        <ul id="itemPlaceholderContainer" runat="server" class="tileSet">
          <li runat="server" id="itemPlaceholder" />
        </ul>
      </LayoutTemplate>
    </asp:ListView>

    <asp:Image CssClass="logo" ID="logo" runat="server" />

    <div class="divNotice">
      <asp:Panel CssClass="panNotice" ID="notice" runat="server" Visible="false">
        <h1>
          <asp:Literal runat="server" Text="<%$  Resources:portal, noticeTitle%>"></asp:Literal>
        </h1>

        <p style="text-align: left;">
          <asp:Literal runat="server" Text="<%$  Resources:portal, noticeSubTitle%>" />
          <br /><br />
          <asp:ListBox 
            ID="lbxPurchases" 
            CssClass="lbxPurchases" 
            Width="400px" 
            runat="server" />
        </p>

        <asp:LinkButton ID="lnkNoticeY" CssClass="newButton2" Width="300" Height="25" OnClick="lnkNoticeY_Click" runat="server">
          <asp:Literal runat="server" Text="<%$  Resources:portal, noticeY%>" />
        </asp:LinkButton><br /><br />

        <asp:LinkButton ID="lnkNoticeN" CssClass="newButton2" Width="300" Height="45" OnClick="lnkNoticeN_Click" runat="server">
          <asp:Literal runat="server" Text="<%$  Resources:portal, noticeN%>" />
        </asp:LinkButton>

        <p style="text-align: left; margin-top: 30px;">
          <asp:Literal runat="server" Text="<%$  Resources:portal, noticeNote%>" />
        </p>
      </asp:Panel>
    </div>
  </div>



  <asp:SqlDataSource ID="SqlDataSource1" runat="server"
    ConnectionString="<%$ ConnectionStrings:apps %>"
    SelectCommand="
      SELECT 
        tileNo,
        tileGroup,
        tileName,
        tileOrder,
        tileMembLevel,
        tileColor,
        tileIcon,
        tileTargetType,
        tileTarget,
        tileActive,
        tileJobs
      FROM 
        apps.dbo.tiles
      WHERE 
        tileActive = 1
        AND @membLevel != 0    
        AND tileMembLevel &lt;= @membLevel 
        AND tileGroup = @group
        AND 
        (
          tileJobs IS NULL 
          OR (@membLevel = 5 AND tileJobs IS NULL)
          OR (@membLevel = 4 AND CHARINDEX('1', @membJobs) &gt; 0 AND tileJobs = '1')
          OR (@membLevel = 4 AND CHARINDEX('2', @membJobs) &gt; 0 AND tileJobs = '2')
          OR (@membLevel = 4 AND CHARINDEX('3', @membJobs) &gt; 0 AND tileJobs = '3')
        )
      ORDER BY 
        tileOrder, tileName 
    ">
    <SelectParameters>
      <asp:SessionParameter DefaultValue="5" Name="membLevel" SessionField="membLevel" />
      <asp:SessionParameter DefaultValue="home" Name="group" SessionField="tileGroup" />
      <asp:SessionParameter DefaultValue="null" Name="membJobs" SessionField="membJobs" />
    </SelectParameters>
  </asp:SqlDataSource>

</asp:Content>
