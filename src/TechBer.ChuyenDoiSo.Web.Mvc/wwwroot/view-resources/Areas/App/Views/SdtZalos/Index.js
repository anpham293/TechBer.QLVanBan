(function () {
    $(function () {

        var _$sdtZalosTable = $('#SdtZalosTable');
        var _sdtZalosService = abp.services.app.sdtZalos;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QuanLySdtZalo.SdtZalo';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.SdtZalos.Create'),
            edit: abp.auth.hasPermission('Pages.SdtZalos.Edit'),
            'delete': abp.auth.hasPermission('Pages.SdtZalos.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SdtZalos/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/SdtZalos/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditSdtZaloModal'
        });       

		 var _viewSdtZaloModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/SdtZalos/ViewsdtZaloModal',
            modalClass: 'ViewSdtZaloModal'
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

        var dataTable = _$sdtZalosTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _sdtZalosService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#SdtZalosTableFilter').val(),
					tenFilter: $('#TenFilterId').val(),
					sdtFilter: $('#SdtFilterId').val()
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
                                    _viewSdtZaloModal.open({ id: data.record.sdtZalo.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.sdtZalo.id });                                
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
                                    entityId: data.record.sdtZalo.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteSdtZalo(data.record.sdtZalo);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "sdtZalo.ten",
						 name: "ten"   
					},
					{
						targets: 2,
						 data: "sdtZalo.sdt",
						 name: "sdt"   
					}
            ]
        });

        function getSdtZalos() {
            dataTable.ajax.reload();
        }

        function deleteSdtZalo(sdtZalo) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _sdtZalosService.delete({
                            id: sdtZalo.id
                        }).done(function () {
                            getSdtZalos(true);
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

        $('#CreateNewSdtZaloButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _sdtZalosService
                .getSdtZalosToExcel({
				filter : $('#SdtZalosTableFilter').val(),
					tenFilter: $('#TenFilterId').val(),
					sdtFilter: $('#SdtFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditSdtZaloModalSaved', function () {
            getSdtZalos();
        });

		$('#GetSdtZalosButton').click(function (e) {
            e.preventDefault();
            getSdtZalos();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getSdtZalos();
		  }
		});
    });
})();