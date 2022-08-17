(function () {
    $(function () {

        var _$tinhThanhsTable = $('#TinhThanhsTable');
        var _tinhThanhsService = abp.services.app.tinhThanhs;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QuanLyDanhMuc.TinhThanh';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.TinhThanhs.Create'),
            edit: abp.auth.hasPermission('Pages.TinhThanhs.Edit'),
            'delete': abp.auth.hasPermission('Pages.TinhThanhs.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TinhThanhs/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TinhThanhs/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditTinhThanhModal'
        });       

		 var _viewTinhThanhModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TinhThanhs/ViewtinhThanhModal',
            modalClass: 'ViewTinhThanhModal'
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

        var dataTable = _$tinhThanhsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _tinhThanhsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#TinhThanhsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					maFilter: $('#MaFilterId').val()
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
                                    _viewTinhThanhModal.open({ id: data.record.tinhThanh.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.tinhThanh.id });                                
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
                                    entityId: data.record.tinhThanh.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteTinhThanh(data.record.tinhThanh);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "tinhThanh.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "tinhThanh.ma",
						 name: "ma"   
					}
            ]
        });

        function getTinhThanhs() {
            dataTable.ajax.reload();
        }

        function deleteTinhThanh(tinhThanh) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _tinhThanhsService.delete({
                            id: tinhThanh.id
                        }).done(function () {
                            getTinhThanhs(true);
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

        $('#CreateNewTinhThanhButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _tinhThanhsService
                .getTinhThanhsToExcel({
				filter : $('#TinhThanhsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					maFilter: $('#MaFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditTinhThanhModalSaved', function () {
            getTinhThanhs();
        });

		$('#GetTinhThanhsButton').click(function (e) {
            e.preventDefault();
            getTinhThanhs();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getTinhThanhs();
		  }
		});
    });
})();