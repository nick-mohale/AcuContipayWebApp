<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcessPayment.aspx.cs" Inherits="AcuContipayWebApp.ProcessPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <section id="main-content">
        <section id="wrapper">
            <div class="row">
                <div class="col-lg-12">
                    <section class="panel">
                        <header class="panel-heading">
                            <div class="col-md-4 col-md-offset-4">
                                <h1>Contipay Payment</h1>
                            </div>
                        </header>

                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="Sales Order Type:" runat="server" />
                                        <asp:DropDownList runat="server" Enabled="true" CssClass="form-control input-sm">
                                            <asp:ListItem Text="SO" />
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="Contipay Provider:" runat="server" />
                                        <asp:DropDownList runat="server" Enabled="true" CssClass="form-control input-sm">
                                            <asp:ListItem Text="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="Sales Order Nbr.:" runat="server" />
                                        <asp:TextBox runat="server" Enabled="false" CssClass="form-control input-group-sm" />
                                    </div>
                                </div>

                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="Account Nbr.:" runat="server" />
                                        <asp:TextBox runat="server" Enabled="true" CssClass="form-control input-group-sm" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">

                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="Sales Order Date:" runat="server" />
                                        <asp:TextBox runat="server" Enabled="false" TextMode="Date" CssClass="form-control input-group-sm" />
                                    </div>
                                </div>

                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="Account Name:" runat="server" />
                                        <asp:TextBox runat="server" Enabled="true" CssClass="form-control input-group-sm" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="Tran Date:" runat="server" />
                                        <asp:TextBox runat="server" Enabled="true" TextMode="Date" CssClass="form-control input-group-sm" />
                                    </div>
                                </div>

                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="Code/CVV:" runat="server" />
                                        <asp:TextBox runat="server" Enabled="true" CssClass="form-control input-group-sm" />
                                    </div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="Payment Method:" runat="server" />
                                        <asp:DropDownList runat="server" Enabled="false" CssClass="form-control input-sm">
                                            <asp:ListItem Text="ContiPay" />
                                        </asp:DropDownList>
                                    </div>

                                </div>

                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="Expiry:" runat="server" />
                                        <asp:TextBox runat="server" Enabled="true" CssClass="form-control input-group-sm" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="Sales Order Curr:" runat="server" />
                                        <asp:TextBox runat="server" Enabled="false" CssClass="form-control input-group-sm" />
                                    </div>
                                </div>
                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="SMS Nbr.:" runat="server" />
                                        <asp:TextBox runat="server" Enabled="true" CssClass="form-control input-group-sm" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="Sales Order Amt Due:" runat="server" />
                                        <asp:TextBox runat="server" Enabled="false" CssClass="form-control input-group-sm" />
                                    </div>
                                </div>

                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="Status:" runat="server" />
                                        <asp:TextBox runat="server" Enabled="true" CssClass="form-control input-group-sm" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4 col-md-offset-1">
                                    <div class="form-group">
                                        <asp:Label Text="Reference Nbr.:" runat="server" />
                                        <asp:TextBox runat="server" Enabled="false" CssClass="form-control input-group-sm" />
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-9 col-md-offset-1">
                                    <div class="form-group">
                                        <div class="table-responsive">
                                            <%-- <asp:GridView ID="gv" Width="100%" AutoGenerateSelectButton="true"
                                                OnSelecetedIndexChanged="gv_SelIdxChanged"
                                                CssClass="table table-bordered table-condensed table-responsive table-hover"
                                                runat="server">
                                                <AlternatingRowStyle BackColor="White" />
                                                <HeaderStyle BackColor="#6B696B" Font-Bold="true" Font-Size="Larger" ForeColor="White" />
                                                <RowStyle BackColor="#f5f5f5" />
                                                <SelectedRowStyle BackColor="#669999" Font-Bold="true" ForeColor="White" />

                                            </asp:GridView>--%>
                                            <asp:GridView ID="gvOpenOrders" Width="100%" runat="server" AutoGenerateColumns="False"
                                                AutoGenerateSelectButton="true" OnSelecetedIndexChanged="gvSelIdxChanged"
                                                CssClass="table table-bordered table-condensed table-responsive table-hover"
                                                BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                                                CellPadding="4" DataKeyNames="id">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Order Type">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("orderType") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("orderType") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SO#">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("orderNbr") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("orderNbr") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approved">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("approved") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("approved") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("status") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("status") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   <%-- <asp:TemplateField HeaderText="Contipay Status">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("cpStatus") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("cpStatus") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Date">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("orderDate") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("orderDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Created By">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("CreatedByID_Creator_Username") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("CreatedByID_Creator_Username") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Order Qty">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("OrderQty") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label8" runat="server" Text='<%# Bind("OrderQty") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Currency">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox9" runat="server" Text='<%# Bind("CuryID") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label9" runat="server" Text='<%# Bind("CuryID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Order Total">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox10" runat="server" Text='<%# Bind("CuryOrderTotal") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label10" runat="server" Text='<%# Bind("CuryOrderTotal") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unpaid Bal">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox11" runat="server" Text='<%# Bind("CuryUnpaidBalance") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label11" runat="server" Text='<%# Bind("CuryUnpaidBalance") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                                                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                                <RowStyle BackColor="White" ForeColor="#003399" />
                                                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                                <SortedAscendingCellStyle BackColor="#EDF6F6" />
                                                <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                                                <SortedDescendingCellStyle BackColor="#D6DFDF" />
                                                <SortedDescendingHeaderStyle BackColor="#002876" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </section>
    </section>
</asp:Content>
