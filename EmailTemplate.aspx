<%@ Page Title="" Language="C#" MasterPageFile="~/scheduler.master" AutoEventWireup="true" CodeFile="EmailTemplate.aspx.cs" Inherits="Email" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row-fluid sortable ui-sortable">
				<div class="box span12">
					<div class="box-header" data-original-title="">
						<h2><i class="halflings-icon edit"></i><span class="break"></span>Email Template</h2>
						<div class="box-icon">
							<a href="#" class="btn-setting"><i class="halflings-icon wrench"></i></a>
							<a href="#" class="btn-minimize"><i class="halflings-icon chevron-up"></i></a>
							<a href="#" class="btn-close"><i class="halflings-icon remove"></i></a>
						</div>
					</div>
					<div class="box-content">
						
						  <fieldset>
						      <div class="control-group">
							  <label class="control-label" for="typeahead">Sheet </label>
							  <div class="controls">
                                  <asp:DropDownList ID="ddlSheet" CssClass="input-xlarge" runat="server">
                                  </asp:DropDownList>
								
                                
							  </div>
							</div>

							<div class="control-group">
							  <label class="control-label" for="typeahead">Name </label>
							  <div class="controls">
								<asp:TextBox ID="txtName" CssClass="input-xlarge datepicker hasDatepicker" runat="server"></asp:TextBox>
								
                                
							  </div>
							</div>
							<div class="control-group">
							  <label class="control-label" for="date01">Subject</label>
							  <div class="controls">
                                  <asp:TextBox ID="txtSubjectt" CssClass="input-xlarge datepicker hasDatepicker" runat="server"></asp:TextBox>
								
							  </div>
							</div>

							<div class="control-group">
							  <label class="control-label" for="fileInput">Html File</label>
							  <div class="controls">
								<div class="uploader" id="uniform-fileInput">
                                    <asp:FileUpload ID="FileUpload1" CssClass="input-file uniform_on" runat="server" />
                                  
								</div>
							  </div>
							</div>          
							<div class="control-group hidden-phone">
							  <label class="control-label" for="textarea2">Email Template</label>
							  <div class="controls">
								<div class="cleditorMain" style="width: 500px; height: 250px;">
								     <textarea runat="server" class="cleditor" id="txtMailTemplate" rows="3" style="display: none; width: 500px; height: 197px;"></textarea><iframe frameborder="0" src="javascript:true;" style="width: 500px; height: 197px;"></iframe>
								</div>
							  </div>
							</div>
							<div class="form-actions">
                                <asp:Button ID="btnAdd" class="btn btn-primary" runat="server" Text="Add" 
                                    onclick="btnAdd_Click" />
							 <asp:Button ID="btnUpdate" Visible="false" CssClass="btn btn-primary" runat="server" 
                                      Text ="Update" onclick="btnUpdate_Click" />
							</div>
						  </fieldset>
			

					</div>
				</div><!--/span-->

			</div>
</asp:Content>

