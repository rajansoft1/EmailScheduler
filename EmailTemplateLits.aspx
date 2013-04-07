<%@ Page Title="" Language="C#" MasterPageFile="~/scheduler.master" AutoEventWireup="true" CodeFile="EmailTemplateLits.aspx.cs" Inherits="EmailTemplateLits" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <div class="box span14">
                <div class="box-header">
                    <h2>
                        <i class="halflings-icon align-justify"></i><span class="break"></span>EmailTemplates</h2>
                    <div class="box-icon">
                        <a href="#" class="btn-setting"><i class="halflings-icon wrench"></i></a><a href="#"
                            class="btn-minimize"><i class="halflings-icon chevron-up"></i></a><a href="#" class="btn-close">
                                <i class="halflings-icon remove"></i></a>
                    </div>
                </div>
                 <div class="box-content">
                 <div class="controls">
                                  <asp:DropDownList ID="ddlSheet" CssClass="input-xlarge" runat="server" 
                                      AutoPostBack="True" onselectedindexchanged="ddlSheet_SelectedIndexChanged">
                                  </asp:DropDownList>
								
                                
							  </div>
                 </div>
                <div class="box-content">
               
                    <asp:GridView DataKeyNames="Id" CssClass="table" GridLines="None" 
                        ID="GridView1" runat="server" 
                        EnableModelValidation="True" onrowcommand="GridView1_RowCommand">
                        <Columns>
                            <asp:ButtonField CommandName="myDelete" Text="Delete">
                            <ControlStyle CssClass="btn btn-primary" />
                            </asp:ButtonField>
                            <asp:ButtonField CommandName="myEdit" Text="Edit">
                            <ControlStyle CssClass="btn btn-primary" />
                            </asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                   
                </div>
            </div>

</asp:Content>

