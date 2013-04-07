<%@ Page Title="" Language="C#" MasterPageFile="~/scheduler.master" AutoEventWireup="true" CodeFile="ScheduleRegularSms.aspx.cs" Inherits="ScheduleRegularSms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row-fluid sortable ui-sortable">
				<div class="box span12">
					<div class="box-header" data-original-title="">
						<h2>Sms Shcedule</h2>
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
							  <label class="control-label" for="typeahead">Group </label>
							  <div class="controls">
                                  <asp:DropDownList ID="ddlGroup" CssClass="input-xlarge" runat="server">
                                  </asp:DropDownList>
								
                                
							  </div>
							</div>
							
							<div class="form-actions">
                                <asp:Button ID="btnAdd" class="btn btn-primary" runat="server" Text="Add" 
                                    onclick="btnAdd_Click" />
							 
							</div>
						  </fieldset>
			

					</div>
				</div><!--/span-->

			</div>
</asp:Content>

