<%@ Page Title="" Language="C#" MasterPageFile="~/scheduler.master" AutoEventWireup="true" CodeFile="StartAfterMapping.aspx.cs" Inherits="StartAfterMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row-fluid sortable ui-sortable">
        <div class="box span6">
            <div class="box-header">
                <h2>
                    <i class="halflings-icon align-justify"></i><span class="break"></span>Start After Maping</h2>
                <div class="box-icon">
                    <a href="#" class="btn-setting"><i class="halflings-icon wrench"></i></a><a href="#"
                        class="btn-minimize"><i class="halflings-icon chevron-up"></i></a><a href="#" class="btn-close">
                            <i class="halflings-icon remove"></i></a>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="typeahead">
                </label>
                <div class="controls">
                </div>
            </div>
            <div class="box-content">
                <asp:GridView DataKeyNames="Id" CssClass="table" GridLines="None" 
                    ID="GridView1" runat="server" EnableModelValidation="True"
                    AllowPaging="True" onrowcommand="GridView1_RowCommand">
                    <Columns>
                        <asp:ButtonField CommandName="myDelete" Text="Delete">
                            <ControlStyle CssClass="btn btn-primary" />
                        </asp:ButtonField>
                       
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <!--/span-->
        <div class="box span6">
            <div class="box-header" data-original-title="">
                <h2>
                    <i class="halflings-icon edit"></i><span class="break"></span>Add Start After Mapping</h2>
                <div class="box-icon">
                    <a href="#" class="btn-setting"><i class="halflings-icon wrench"></i></a><a href="#"
                        class="btn-minimize"><i class="halflings-icon chevron-up"></i></a><a href="#" class="btn-close">
                            <i class="halflings-icon remove"></i></a>
                </div>
            </div>
            <div class="box-content">
                <div class="form-horizontal">
                    <fieldset>
                        <div class="control-group">
                            <label class="control-label" for="typeahead">
                                Recursive Sheet
                            </label>
                            <div class="controls">
                                <asp:DropDownList ID="ddlSheet" CssClass="input-xlarge" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlSheet_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        
                          <div class="control-group">
							  <label class="control-label" for="typeahead">Group </label>
							  <div class="controls">
                                  <asp:DropDownList ID="ddlGroup" CssClass="input-xlarge" runat="server">
                                  </asp:DropDownList>
								
                                
							  </div>
							</div>
                        
                        <div class="form-actions">
                            <asp:Button ID="btnSubmit" CssClass="btn btn-primary" runat="server" Text="Submit"
                                OnClick="btnSubmit_Click" />
                        </div>
                    </fieldset>
                </div>
            </div>
            <!--/span-->
        </div>
    </div>
</asp:Content>

