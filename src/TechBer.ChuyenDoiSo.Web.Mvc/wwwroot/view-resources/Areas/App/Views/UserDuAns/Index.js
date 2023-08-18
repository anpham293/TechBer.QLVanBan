(function () {
    $(function () {

        var _$userDuAnsTable = $('#UserDuAnsTable');
        var _userDuAnsService = abp.services.app.userDuAns;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QLVB.UserDuAn';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.UserDuAns.Create'),
            edit: abp.auth.hasPermission('Pages.UserDuAns.Edit'),
            'delete': abp.auth.hasPermission('Pages.UserDuAns.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/UserDuAns/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/UserDuAns/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditUserDuAnModal'
        });       

		 var _viewUserDuAnModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/UserDuAns/ViewuserDuAnModal',
            modalClass: 'ViewUserDuAnModal'
        });

		        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
		        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$userDuAnsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _userDuAnsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#UserDuAnsTableFilter').val(),
					minTrangThaiFilter: $('#MinTrangThaiFilterId').val(),
					maxTrangThaiFilter: $('#MaxTrangThaiFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val(),
					duAnNameFilter: $('#DuAnNameFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
						{
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewUserDuAnModal.open({ id: data.record.userDuAn.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.userDuAn.id });                                
                            }
                        },
                        {
                            text: app.localize('History'),
                            visible: function () {
                                return entityHistoryIsEnabled();
                            },
                            action: function (data) {
                                _entityTypeHistoryModal.open({
                                    entityTypeFullName: _entityTypeFullName,
                                    entityId: data.record.userDuAn.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteUserDuAn(data.record.userDuAn);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "userDuAn.trangThai",
						 name: "trangThai"   
					},
					{
						targets: 2,
						 data: "userName" ,
						 name: "userFk.name" 
					},
					{
						targets: 3,
						 data: "duAnName" ,
						 name: "duAnFk.name" 
					}
            ]
        });

        function getUserDuAns() {
            dataTable.ajax.reload();
        }

        function deleteUserDuAn(userDuAn) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _userDuAnsService.delete({
                            id: userDuAn.id
                        }).done(function () {
                            getUserDuAns(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

		$('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewUserDuAnButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _userDuAnsService
                .getUserDuAnsToExcel({
				filter : $('#UserDuAnsTableFilter').val(),
					minTrangThaiFilter: $('#MinTrangThaiFilterId').val(),
					maxTrangThaiFilter: $('#MaxTrangThaiFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val(),
					duAnNameFilter: $('#DuAnNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditUserDuAnModalSaved', function () {
            getUserDuAns();
        });

		$('#GetUserDuAnsButton').click(function (e) {
            e.preventDefault();
            getUserDuAns();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getUserDuAns();
		  }
		});
    });
})();