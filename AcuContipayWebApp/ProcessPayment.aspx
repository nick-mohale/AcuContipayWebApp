<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcessPayment.aspx.cs" Inherits="AcuContipayWebApp.ProcessPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section id="main-content">
        <section id="wrapper">
            <div class="row">
                <div class="col-lg-12">
                    <section class="panel">
                        <header class="panel-heading text-center">
                            <h1 style="font-size: 24px;">Contipay Payment</h1>
                        </header>
                    </section>
                </div>
            </div>


            <div class="row">
                <div class="col-md-9 col-md-offset-1">
                    <div class="form-group">
                        <div class="table-responsive">

                            <asp:GridView ID="gvOpenOrders" runat="server" Width="100%" AutoGenerateColumns="False"
                                AutoGenerateSelectButton="true" OnSelectedIndexChanged="gvSelIdxChanged"
                                CssClass="table table-bordered table-condensed table-responsive table-hover"
                                BorderWidth="1px" BorderColor="#CCCCCC"
                                CellPadding="4" DataKeyNames="orderNbr">


                                <Columns>
                                    <asp:BoundField DataField="orderType" HeaderText="Order Type" />
                                    <asp:BoundField DataField="orderNbr" HeaderText="SO#" />
                                    <asp:BoundField DataField="approved" HeaderText="Approved" />
                                    <asp:BoundField DataField="status" HeaderText="Status" />
                                      <asp:BoundField DataField="contipayStatus" HeaderText="Contipay Status" />
                                    <asp:BoundField DataField="orderDate" HeaderText="Date" />
                                    <asp:BoundField DataField="CreatedByID_Creator_Username" HeaderText="Created By" />
                                    <asp:BoundField DataField="OrderQty" HeaderText="Order Qty" />
                                    <asp:BoundField DataField="CuryID" HeaderText="Currency" />
                                    <asp:BoundField DataField="CuryOrderTotal" HeaderText="Order Total" />
                                    <asp:BoundField DataField="CuryUnpaidBalance" HeaderText="Unpaid Bal" />
                                </Columns>

                                <FooterStyle BackColor="#F5F5F5" ForeColor="#333333" />
                                <HeaderStyle BackColor="#F5F5F5" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#F5F5F5" ForeColor="#333333" HorizontalAlign="Left" />
                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                <AlternatingRowStyle BackColor="#F2F2F2" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E0E0E0" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F5F5" />
                                <SortedAscendingHeaderStyle BackColor="#F5F5F5" />
                                <SortedDescendingCellStyle BackColor="#F5F5F5" />
                                <SortedDescendingHeaderStyle BackColor="#F5F5F5" />
                            </asp:GridView>

                        </div>
                    </div>
                </div>
            </div>




            <div class="row">
                <div class="col-md-4 col-md-offset-1">
                    <div class="form-group">
                        <asp:Label Text="Sales Order Nbr.:" runat="server" />
                        <asp:TextBox ID="txtOrderNbr" runat="server" Enabled="false" CssClass="form-control input-group-sm" />
                    </div>
                </div>


                <div class="col-md-4 col-md-offset-1">
                    <label for="ddlContipayProvider">Provider:</label>
                    <asp:DropDownList ID="ddlContipayProvider" runat="server" Enabled="true" CssClass="form-control input-sm">
                        <asp:ListItem Text="" />
                        <asp:ListItem Text="Visa" />
                        <asp:ListItem Text="Mastercard" />
                        <asp:ListItem Text="American Express" />
                    </asp:DropDownList>
                </div>


            </div>


            <div class="row">

                <div class="col-md-4 col-md-offset-1">
                    <div class="form-group">
                        <asp:Label Text="Sales Order Date:" runat="server" />
                        <asp:TextBox ID="txtOrderDate" runat="server" Enabled="false" TextMode="Date" CssClass="form-control input-group-sm" />
                    </div>
                </div>

                <div class="col-md-4 col-md-offset-1">
                    <div class="form-group">
                        <asp:Label Text="Account Nbr.:" runat="server" />
                        <asp:TextBox runat="server" Enabled="true" CssClass="form-control input-group-sm" />
                    </div>
                </div>



                <div class="row">
                    <div class="col-md-4 col-md-offset-1">
                        <div class="form-group">
                            <asp:Label Text="Transaction Date:" runat="server" />
                            <asp:TextBox runat="server" Enabled="true" TextMode="Date" CssClass="form-control input-group-sm" />
                        </div>
                    </div>

                    <div class="col-md-4 col-md-offset-1">
                        <div class="form-group">
                            <asp:Label Text="Account Name:" runat="server" />
                            <asp:TextBox runat="server" Enabled="true" CssClass="form-control input-group-sm" />
                        </div>
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
                        <asp:Label Text="Code/CVV:" runat="server" />
                        <asp:TextBox runat="server" Enabled="true" CssClass="form-control input-group-sm" />
                    </div>
                </div>




                <div class="row">
                    <div class="col-md-4 col-md-offset-1">
                        <div class="form-group">
                            <asp:Label Text="Sales Order Curr:" runat="server" />
                            <asp:TextBox ID="txtCuryID" runat="server" Enabled="false" CssClass="form-control input-group-sm" />
                        </div>
                    </div>

                    <div class="col-md-4 col-md-offset-1">
                        <div class="form-group">
                            <asp:Label Text="Expiry:" runat="server" />
                            <asp:TextBox runat="server" Enabled="true" CssClass="form-control input-group-sm" />
                        </div>
                    </div>
                </div>

            </div>
            <div class="row">
                <div class="col-md-4 col-md-offset-1">
                    <div class="form-group">
                        <asp:Label Text="Sales Order Amt Due:" runat="server" />
                        <asp:TextBox ID="txtCuryOrderTotal" runat="server" Enabled="false" CssClass="form-control input-group-sm" />
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
                        <asp:Label Text="Reference Nbr.:" runat="server" />
                        <asp:TextBox runat="server" Enabled="false" CssClass="form-control input-group-sm" />
                    </div>
                </div>

                <div class="col-md-4 col-md-offset-1">
                    <div class="form-group">
                        <asp:Label Text="Contipay Status:" runat="server" />
                        <asp:TextBox ID="txtContiPayStatus" runat="server" Enabled="false" CssClass="form-control input-group-sm" />
                    </div>
                </div>



            </div>

            <div class="row">

                <div class="col-md-4 col-md-offset-1">
                    <div class="form-group">
                        <asp:Label Text="Unpaid Balance.:" runat="server" />
                        <asp:TextBox ID="txtUpaidBalance" runat="server" Enabled="false" CssClass="form-control input-group-sm" />
                    </div>
                </div>

                <div class="col-md-4 col-md-offset-1">
                    <asp:Button ID="btnClear" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnClear_Click" />
                    <asp:Button ID="btnVerify" runat="server" Text="Verify" CssClass="btn btn-success" OnClick="btnVerify_Click" />
                    <asp:Button ID="btnPay" runat="server" Text="     Pay    " CssClass="btn btn-primary" OnClick="btnPay_Click" />
                </div>


            </div>




  
        </section>
    </section>
</asp:Content>
